#include "stdafx.h"

#define WINSOCKVERSION         0x0101         /* requesting version 1.1     */
#define CMD_CLEAR_SCREEN       1

DWORD WINAPI          worker_sync
(
);

/* Global area */
ULONG                 gulRunning = 0;
DWORD		  		  gdwError = 0;
USHORT                gusPort = 0;
char                  gchAddress[256];
ULONG                 gulCommand = 0;
ULONG                 gulImageId = 0;
void *                gpImage = NULL;
ULONG                 gulImageSize = 0;
ULONG                 gulStopped = 0;

HANDLE ghEvent = NULL;
HANDLE ghThread = NULL;

VOID                   debug
(
	char* msg
)
{
	FILE* fp;

	fp = fopen("c:\\odo.txt", "a+");
	fprintf(fp, "%s\n", msg);
	fclose(fp);
}


/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       getError                                            */
/*                                                                          */
/*      DESCRIPTION:    Returns an error code                               */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      defError .......... default error code         (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/
DWORD                   getError
(
DWORD                 defError
)
{
	DWORD dwRet = WSAGetLastError();
	if (dwRet == 0)
		dwRet = GetLastError();
	if (dwRet == 0)
		dwRet = defError;
	return dwRet;
}


/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       worker                                              */
/*                                                                          */
/*      DESCRIPTION:    Worker for the other three functions.               */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      usPort ............ local network ID           (I)  */
/*                      pszAddress ........ local additional address   (I)  */
/*                      ulCommand ......... see CMD_* constants in *.h (I)  */
/*                      ulImageId ......... numerical image Id or 0    (I)  */
/*                      pImage ............ content of image or NULL   (I)  */
/*                      ulImageSize ....... image size in bytes or 0   (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/
DWORD WINAPI          worker
(
USHORT                usPort,
char *                pszAddress,
ULONG                 ulCommand,
ULONG                 ulImageId,
void *                pImage,
ULONG                 ulImageSize
)
{
	if ((gulRunning != 0) || (gulStopped != 0))                 // send alread in progress, simply do nothing
		return 0;

	gulRunning = 1;                       // sign send in progress
	gusPort = usPort;
	gulCommand = ulCommand;
	gulImageId = ulImageId;
	gulImageSize = ulImageSize;

	strncpy(gchAddress, pszAddress, 250); // copy address

	gpImage = malloc(ulImageSize);        // copy image to local mem
	memcpy(gpImage, pImage, ulImageSize);
	
	SetEvent(ghEvent);                    // wake up worker

	return 0;
}

/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       WorkerThread                                        */
/*                                                                          */
/*      DESCRIPTION:    Send commands asynchronously                        */
/*                                                                          */
/*      RETURN:         see description                                     */
/*                                                                          */
/*      PARAMETER:      see description                                     */
/*                                                                          */
/*--------------------------------------------------------------------------*/
static DWORD WINAPI WorkerThread(LPVOID p)
{
	while (true)
	{
		if (WaitForSingleObject(ghEvent, INFINITE) == WAIT_OBJECT_0)
		{
			if (gulStopped == 0)
			{
				gdwError = worker_sync();                           // send command
				free(gpImage);                                      // cleanup
			}
			
			gulRunning = 0;                                         // sign, ready 

			if (gulStopped != 0)
				return 0;
		}
	}
}


/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       get_last_async_error                                */
/*                                                                          */
/*      DESCRIPTION:    Returns the last error code of the async command.   */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      none                                                */
/*                                                                          */
/*--------------------------------------------------------------------------*/
DWORD WINAPI          get_last_async_error
(
)
{
	return gdwError;
}

/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       worker synchronous                                  */
/*                                                                          */
/*      DESCRIPTION:    Worker for the other three functions.               */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      usPort ............ local network ID           (I)  */
/*                      pszAddress ........ local additional address   (I)  */
/*                      ulCommand ......... see CMD_* constants in *.h (I)  */
/*                      ulImageId ......... numerical image Id or 0    (I)  */
/*                      pImage ............ content of image or NULL   (I)  */
/*                      ulImageSize ....... image size in bytes or 0   (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/

DWORD WINAPI          worker_sync
(
)
{
	WSADATA               WSAData;              /* windows socket information */
	int                   iRet;                 /* function call return value */
	ULONG                 ulIPAddr;             /* target IP address          */
	struct sockaddr_in    Remote;               /* remote partner address     */
	ULONG                 ulSent;               /* nr. of sent bytes          */
	SOCKET                Socket;               /* socket descr. of new conn. */
	ULONG                 ulH;
	DWORD                 dwRet;

	USHORT                usPort = gusPort;
	char *                pszAddress = gchAddress;
	ULONG                 ulCommand = gulCommand;
	ULONG                 ulImageId = gulImageId;
	void *                pImage = gpImage;
	ULONG                 ulImageSize = gulImageSize;
	
	ULONG                 gulRunning = 0;
	DWORD		  		  gdwError = 0;
	USHORT                gusPort = 0;
	char *                gpszAddress = NULL;
	ULONG                 gulCommand = 0;
	ULONG                 gulImageId = 0;
	void *                gpImage = NULL;
	ULONG                 gulImageSize = 0;


	iRet = WSAStartup(WINSOCKVERSION,           /* version 1.1 requested      */
		&WSAData);
	if (iRet)
		return getError(1);

	ulIPAddr = inet_addr(pszAddress);           /* convert net ID to address  */
	if (ulIPAddr == INADDR_NONE)
	{
		dwRet = getError(2);
		WSACleanup();
		return dwRet;
	}

	Socket = socket(PF_INET,                    /* create stream socket       */
		SOCK_STREAM,
		0);
	if (Socket == INVALID_SOCKET)
	{
		dwRet = getError(3);
		WSACleanup();
		return dwRet;
	}

	Remote.sin_family = AF_INET;
	Remote.sin_port = htons(usPort);
	Remote.sin_addr.s_addr = ulIPAddr;

	iRet = connect(Socket,                      /* establish connection       */
		(struct sockaddr *)&Remote,
		sizeof(struct sockaddr_in));
	if (iRet == SOCKET_ERROR)
	{
		dwRet = getError(4);
		WSACleanup();
		return dwRet;
	}

	ulH = htonl(ulCommand);
	iRet = send(Socket,                         /* send command               */
		(char *)&ulH,
		4,
		0);
	if (iRet == SOCKET_ERROR)
	{
		dwRet = getError(5);
		closesocket(Socket);
		WSACleanup();
		return dwRet;
	}

	ulH = htonl(ulImageId);
	iRet = send(Socket,                         /* send image id or 0         */
		(char *)&ulH,
		4,
		0);
	if (iRet == SOCKET_ERROR)
	{
		dwRet = getError(6);
		closesocket(Socket);
		WSACleanup();
		return dwRet;
	}

	ulH = htonl(ulImageSize);
	iRet = send(Socket,                         /* send image size or 0       */
		(char *)&ulH,
		4,
		0);
	if (iRet == SOCKET_ERROR)
	{
		dwRet = getError(7);
		closesocket(Socket);
		WSACleanup();
		return dwRet;
	}

	if (ulCommand == CMD_SEND_IMAGE)
	{
		ulSent = 0;
		while (ulSent < ulImageSize)
		{
			iRet = send(Socket,                       /* send data packet           */
				(char*)pImage + ulSent,
				(int)(ulImageSize - ulSent),
				0);
			if (iRet == SOCKET_ERROR)
			{
				dwRet = getError(8);
				closesocket(Socket);
				WSACleanup();
				return dwRet;
			}
			ulSent += (ULONG)iRet;
		}
	}

	closesocket(Socket);                        /* close main socket          */

	WSACleanup();                               /* do specific cleanup        */

	return 0;
}


/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       send_image                                          */
/*                                                                          */
/*      DESCRIPTION:    Sends the content of an image to the specified      */
/*                      address and port.                                   */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      usPort ............ local network ID           (I)  */
/*                      pszAddress ........ local additional address   (I)  */
/*                      ulImageId ......... numerical image Id         (I)  */
/*                      pImage ............ content of image           (I)  */
/*                      ulImageSize ....... image size in bytes        (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/

DWORD WINAPI            send_image
(
USHORT                usPort,
char *                pszAddress,
ULONG                 ulImageId,
void *                pImage,
ULONG                 ulImageSize
)
{
	return worker(usPort, pszAddress, CMD_SEND_IMAGE, ulImageId, pImage, ulImageSize);
}

/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       test_connection                                     */
/*                                                                          */
/*      DESCRIPTION:    Sends a "test connection" command to the specified  */
/*                      address and port.                                   */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      usPort ............ local network ID           (I)  */
/*                      pszAddress ........ local additional address   (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/

DWORD WINAPI            test_connection
(
USHORT                usPort,
char *                pszAddress
)
{
	return 0;
	//  return worker(usPort, pszAddress, CMD_TEST_CONNECTION, 0, NULL, 0);
}

/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       clear_screen                                        */
/*                                                                          */
/*      DESCRIPTION:    Sends a "clear screen" command to the specified     */
/*                      address and port.                                   */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      usPort ............ local network ID           (I)  */
/*                      pszAddress ........ local additional address   (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/

DWORD WINAPI            clear_screen
(
USHORT                usPort,
char *                pszAddress
)
{
	return worker(usPort, pszAddress, CMD_CLEAR_SCREEN, 0, NULL, 0);
}

/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       clear_screen_ex                                     */
/*                                                                          */
/*      DESCRIPTION:    Sends a "clear screen ex" command to the specified  */
/*                      address and port.                                   */
/*                                                                          */
/*      RETURN:         0 ... ok                                            */
/*                      !=0 . error code (either from WSAGetLastError() or  */
/*                            GetLastError())                               */
/*                                                                          */
/*      PARAMETER:      usPort ............ local network ID           (I)  */
/*                      pszAddress ........ local additional address   (I)  */
/*                      ulImageId ......... numerical image Id or 0    (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/

DWORD WINAPI            clear_screen_ex
(
USHORT                usPort,
char *                pszAddress,
ULONG                 ulImageId
)
{
	return worker(usPort, pszAddress, CMD_CLEAR_SCREEN_EX, ulImageId, NULL, 0);
}

/*--------------------------------------------------------------------------*/
/*                                                                          */
/*      FUNCTION:       DllMain                                             */
/*                                                                          */
/*      DESCRIPTION:    see description                                     */
/*                                                                          */
/*      RETURN:         see description                                     */
/*                                                                          */
/*      PARAMETER:      see description                                     */
/*                                                                          */
/*--------------------------------------------------------------------------*/
BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
	)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		ghEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
		ghThread = CreateThread(NULL, 0, WorkerThread, NULL, 0, NULL);
		break;
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		gulStopped = 1;
		SetEvent(ghEvent);                    // wake up worker

		int iCnt = 0;
		while (gulRunning == 1)
		{
			Sleep(50);
			if (iCnt > 50)
				return TRUE;

			iCnt++;
		}

		break;
	}
	return TRUE;
}

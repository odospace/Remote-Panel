// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the AIDARDSP_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// AIDARDSP_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef AIDARDSP_EXPORTS
#define AIDARDSP_API __declspec(dllexport)
#else
#define AIDARDSP_API __declspec(dllimport)
#endif

/****************************************************************************/
/*      PREPROCESSOR CONSTANTS                                              */
/****************************************************************************/
#define CMD_TEST_CONNECTION    0              /* available commands         */
#define CMD_SEND_IMAGE         2
#define CMD_CLEAR_SCREEN_EX    3

/****************************************************************************/
/*      FUNCTION PROTOTYPING                                                */
/****************************************************************************/
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
/*                      ulImageId ......... numerical image Id (0-9)   (I)  */
/*                      pImage ............ content of image (PNG)     (I)  */
/*                      ulImageSize ....... image size in bytes        (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/
DWORD WINAPI          send_image
(
USHORT                usPort,
char *                pszAddress,
ULONG                 ulImageId,
void *                pImage,
ULONG                 ulImageSize
);

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
DWORD WINAPI          test_connection
(
USHORT                usPort,
char *                pszAddress
);

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
/*                      ulImageId ......... image Id or (0-9)          (I)  */
/*                                                                          */
/*--------------------------------------------------------------------------*/
DWORD WINAPI          clear_screen_ex
(
USHORT                usPort,
char *                pszAddress,
ULONG                 ulImageId
);

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
);
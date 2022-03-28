﻿/*
 * Created by SharpDevelop.
 * User: Joachim
 * Date: 09.03.2015
 * Time: 21:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using Managed.Adb;
using System.IO;
using System.Net.Sockets;
using System.Net;
namespace RemotePanel
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;
		public AdbHelper adbHelper = AdbHelper.Instance;
		public bool doWork = true;
		public int timeout = 5000;
		Thread workerThread = null;
		Thread tcpProxyThread = null;
		public int checkinterval = 30000;
		public int waitTime = 100;
	    public string port = null;
		public string serial = null;
	    public string adbpath = null;		
	    public Boolean verbose = false;
	    public string lastMsg = "";
	    public string lastDebug = "";
		
		#region Initialize icon and menu
		public NotificationIcon()
		{
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			
			notifyIcon.DoubleClick += IconDoubleClick;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
			notifyIcon.Icon = (Icon)resources.GetObject("ico2");
			notifyIcon.ContextMenu = notificationMenu;
			
			string interval = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","checkinterval", "");
        	if ((interval == null) || (interval == ""))
        		interval = "30";			
          	try
        	{
        		checkinterval = Convert.ToInt32(interval)*1000;
        	}
        	catch (Exception ex)
        	{
        		checkinterval = 30000;
        	}			
        	
			string wt = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","waitTime", "");
        	if ((wt == null) || (wt == ""))
        		wt = "100";			
          	try
        	{
        		waitTime = Convert.ToInt32(wt);
        	}
        	catch (Exception ex)
        	{
        		waitTime = 100;
        	}		        	
        	
			port = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","port", "");
        	if ((port == null) || (port == ""))
        		port = "38000";
        	
        	serial = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","serial", "");
        	if (serial == null)
        		serial = "";
        	
        	adbpath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","adbpath", "");
			if ((adbpath == null) || (adbpath == ""))
				adbpath = Path.GetDirectoryName(Application.ExecutablePath)+@"\adb\adb.exe";        	
        	
			string v = "";
        	v = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","verbose", "");
			if ((v == null) || (v == ""))
				verbose = false;
			else
				verbose = true;				
						
             Worker workerObject = new Worker(this);
             workerThread = new Thread(workerObject.DoWork);
             workerThread.IsBackground = true;
             // Start the worker thread.
             workerThread.Start();	

             TcpProxy tcpProxyObject = new TcpProxy(this);
             tcpProxyThread = new Thread(tcpProxyObject.DoWork);
             tcpProxyThread.IsBackground = true;
             // Start the worker thread.
             tcpProxyThread.Start();	
		}
		
		public void ShowMessage(String txt)
		{
			if (!doWork)
				return;
          if (txt.Contains("to locate"))
          		txt=txt+Environment.NewLine+"Open the settings dialog to set the path to adb.exe!";			
			
	        notifyIcon.BalloonTipText = txt;
            notifyIcon.BalloonTipIcon = ToolTipIcon.Error;
            notifyIcon.BalloonTipTitle ="Remote Panel";
            notifyIcon.ShowBalloonTip(15000);	

            lastMsg = txt;
		}

		public void ShowDebug(String txt)
		{
			if (verbose)
			{
		        notifyIcon.BalloonTipText = txt;
	            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
	            notifyIcon.BalloonTipTitle ="Remote Panel";
	            notifyIcon.ShowBalloonTip(5000);	
	            Thread.Sleep(1000);
			}
			lastDebug = txt;
		}
		
		public void ShowInfo(String txt)
		{
			if (!doWork)
				return;
          if (txt.Contains("to locate"))
          		txt=txt+Environment.NewLine+"Open the settings dialog to set the path to adb.exe!";			
			
	        notifyIcon.BalloonTipText = txt;
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle ="Remote Panel";
            notifyIcon.ShowBalloonTip(15000);			
		}		
		
		
		public void StopAdb()
		{
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow = true;
				startInfo.UseShellExecute = false;
				startInfo.FileName = adbpath;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				startInfo.Arguments = "kill-server";
				Process.Start(startInfo);
		}
		
		public List<Device> EnsureRunning()
		{
			List<Device> devices = null;
			try 
			{
		      // using https://madb.codeplex.com/
           	  // for all devices issue adb.exe forward tcp:port tcp:38000
              devices = adbHelper.GetDevices(AndroidDebugBridge.SocketAddress);			  
			}
			catch (Exception ex)  // try to start adb.exe
			{
				// Use ProcessStartInfo class
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow = true;
				startInfo.UseShellExecute = false;
				startInfo.FileName = adbpath;
				startInfo.WindowStyle = ProcessWindowStyle.Hidden;
				startInfo.Arguments = "start-server";
				
				try
				{
					Process exeProcess = Process.Start(startInfo);
					exeProcess.WaitForExit();
				}
				catch (Exception exs)
				{
					throw new Exception(@"Can't start adb.exe. Validate the path within the settings dialog. ("+exs.Message+@")");
				}
				
				Thread.Sleep(5000);  // wait until server starts
				try
				{
					 devices = adbHelper.GetDevices(AndroidDebugBridge.SocketAddress);
				}
				catch (Exception exc)
				{
					// ignore
				}
			}
			
			return devices;
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
				new MenuItem("Check for new device now", IconDoubleClick),				
				new MenuItem("Diagnostic and Help", menuListDevices),
				new MenuItem("Settings", menuAboutClick),
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		#endregion
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
			using (Mutex mtx = new Mutex(true, "RemotePanel", out isFirstInstance)) {
				if (isFirstInstance) {
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
					Application.Run();
					notificationIcon.notifyIcon.Dispose();
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
				}
			} // releases the Mutex
		}
		#endregion
		
		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			Form1 form1 = new Form1(this);
            form1.ShowDialog();       
		}
		
		private void menuExitClick(object sender, EventArgs e)
		{
			doWork = false;
			StopAdb();
			Process.GetCurrentProcess().Kill();
		}
		
		
		public string CreateForward(bool showerror)
		{
			string result = "device with configured serial " + serial + " not found";
			try
			{
				var devices = EnsureRunning();  // start adb.exe
				if (devices == null)
					return "no devices found";
				
				if (devices.Count == 0)
					return "no devices connected";

				foreach (Device device in devices)
				{
					if ((serial == "") || (device.SerialNumber == serial))
					{
						adbHelper.CreateForward(AndroidDebugBridge.SocketAddress, device, Convert.ToInt32(port)+1, 38000);
						string ser = serial;
						if (ser == "")
							ser="<first available>";
						result = "created for device with serial " + ser;
						timeout = checkinterval;  // on success check next time with maxium interval
						break;
					}
				}
            }
			catch (Exception ex)
			{
				if (showerror)
					return ex.Message;
				
				if (!ex.Message.Contains("Invalid device list data") && !ex.Message.Contains("rejected command"))  // temporary error response
				  ShowMessage(ex.Message);
			}
			
			return result;
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			string result = CreateForward(true);
			ShowInfo("Adb forward result: " + result);
		}
		
		private void menuListDevices(object sender, EventArgs e)
		{
			Form2 form2 = new Form2(this);
			form2.ShowDialog();
		}
		#endregion
	}
	
	public class Worker
    {
		NotificationIcon main = null;
		public Worker(NotificationIcon app)
		{
			main = app;
		}
    // This method will be called when the thread is started.
    public void DoWork()
    {
    	while (main.doWork)
    	{
	    	main.CreateForward(false);
	    	if (main.timeout < main.checkinterval)
	    		main.timeout += 1000;  // increase interval until maximum is reached
	    	try
	    	{
	    		Thread.Sleep(main.timeout);
	    	}
	    	catch (Exception ex)
	    	{
	    		// interrupted
	    	}
    	}
     }

   }

	public class TcpProxy
    {
		NotificationIcon main = null;
		public TcpProxy(NotificationIcon app)
		{
			main = app;
		}
    // This method will be called when the thread is started.
	    public void DoWork()
	    {
	    	var inPort = Convert.ToInt32(main.port);
	    	main.ShowDebug("Creating TcpListener.");	

	    	TcpListener server = new TcpListener(IPAddress.Any, inPort);
	    	try
	    	{
	    		server.Start();
	    	}
	    	catch (Exception ex)
	    	{
	    		main.ShowMessage(ex.Message + " - Please restart Remote Panel.");
	    		return;
	    	}			
			
	    	byte[] bytes = new byte[4096];
	    	
	    	while (main.doWork)
	    	{
	    		Socket handler = null;
	    		Socket adb = null;
	    		try
	    		{
	    			Thread.Sleep(main.waitTime);
                	main.ShowDebug("Waiting for connection.");	
                	handler = server.AcceptSocket();
                	handler.ReceiveTimeout = 3000;

                	main.ShowDebug("Something accepted.");
                	adb =  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                	adb.SendTimeout = 3000;
                	adb.Connect(IPAddress.Loopback, inPort+1);
                	
	    			main.ShowDebug("Connected to local ADB.");
                	int total = 0;
                	while (true)
                	{
                		int bytesRec = handler.Receive(bytes);
                		if (bytesRec == 0)
                			break;
                		total += bytesRec;
                		int bytesSend = adb.Send(bytes, 0, bytesRec, SocketFlags.None);
                	}
                	
	    			main.ShowDebug("Bytes send to adb: " + total);
	    			main.lastMsg = "";
	    		}
	    		catch (Exception ex)
	    		{
	    			main.lastMsg = "Main loop: " + ex.Message;
	    			main.ShowDebug("Main loop: " + ex.Message);
	    			Thread.Sleep(5000);
	    		}
	    		finally
	    		{
	    			if (handler != null)
	    			{
	    				try{
		    			   handler.Shutdown(SocketShutdown.Both);
                      	   handler.Close();
	    				}
	    				catch (Exception ex)
	    				{
	    			      //main.message = ex.Message;
	    			      //main.ShowDebug("Closing - " + ex.Message);	    					
	    				}
	    			}
	    			if (adb != null)
	    			{
	    				try{
	    			   		adb.Shutdown(SocketShutdown.Both);
                       		adb.Close();	
	    				}
	    				catch (Exception ex)
	    				{
	    			      //main.message = ex.Message;
	    			      //main.ShowDebug("Closing - " + ex.Message);	    					
	    				}
	    					
	    			}
	    		}
	    	}
	     }
   }	
	
}

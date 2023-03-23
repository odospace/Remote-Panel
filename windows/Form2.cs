/*
 * Created by SharpDevelop.
 * User: Joachim
 * Date: 09.03.2015
 * Time: 22:10
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Managed.Adb;
using System.IO;
namespace RemotePanel
{

	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class Form2 : Form
	{
		NotificationIcon main = null;
		public Form2(NotificationIcon nIcon)
		{
			main = nIcon;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

           	try
           	{
           		// same version as 1.15 !!!
                this.textOutput.AppendText("Remote Panel Version: 1.16" +  Environment.NewLine);      
                
           		main.EnsureRunning();
                int adbVersion = main.adbHelper.GetAdbVersion(AndroidDebugBridge.SocketAddress);
                this.textOutput.AppendText("Adb Version: " + adbVersion + Environment.NewLine);
                
             	var devices = main.adbHelper.GetDevices(AndroidDebugBridge.SocketAddress);
				if (devices.Count != 0)
					this.textOutput.AppendText(Environment.NewLine+"Android devices:"+ Environment.NewLine);             	
				foreach (Device device in devices)
				{
					this.textOutput.AppendText("Model: "+device.Model + " Serial:" + device.SerialNumber + " " + device.AvdName + Environment.NewLine);
				}

				this.textOutput.AppendText(Environment.NewLine);
				
				this.textOutput.AppendText("Last operation: ");
				this.textOutput.AppendText(main.lastDebug);
				this.textOutput.AppendText(Environment.NewLine);

				this.textOutput.AppendText("Last message: ");
				this.textOutput.AppendText(main.lastMsg);
				this.textOutput.AppendText(Environment.NewLine);				
				this.textOutput.AppendText("Adb forward result: ");
					
				string fresult = main.CreateForward(true);
				this.textOutput.AppendText(fresult);
				
           	}
           	catch (Exception ex)
           	{
           		this.textOutput.AppendText(Environment.NewLine+"Error: " + ex.Message);
           	}
			
			String appdir = Path.GetDirectoryName(Application.ExecutablePath);
            String myfile = Path.Combine(appdir, "help.htm");
            webBrowser1.Url = new Uri("file:///" + myfile);           	
			/*
			string adbpath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","adbpath", "adb.exe");
			
            processCaller = new ProcessCaller(this);
            //processCaller.FileName = @"..\..\hello.bat";
            processCaller.FileName = adbpath;
            processCaller.Arguments = command;
            processCaller.StdErrReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.StdOutReceived += new DataReceivedHandler(writeStreamInfo);
            processCaller.Completed += new EventHandler(processCompletedOrCanceled);
            processCaller.Cancelled += new EventHandler(processCompletedOrCanceled);
            processCaller.Failed += new ThreadExceptionEventHandler(processFailed);
            
            this.textOutput.Text = adbpath + " " + command + Environment.NewLine + "Please stand by.." + Environment.NewLine;

            // the following function starts a process and returns immediately,
            // thus allowing the form to stay responsive.
            processCaller.Start();  

			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			*/
		}
		
		/*
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
       
            if (processCaller != null)
            {
                processCaller.Cancel();
            }
            
        }

        /// <summary>
        /// Handles the events of StdErrReceived and StdOutReceived.
        /// </summary>
        /// <remarks>
        /// If stderr were handled in a separate function, it could possibly
        /// be displayed in red in the richText box, but that is beyond 
        /// the scope of this demo.
        /// </remarks>
        private void writeStreamInfo(object sender, DataReceivedEventArgs e)
        {
            this.textOutput.AppendText(e.Text + Environment.NewLine);
        }

        /// <summary>
        /// Handles the events of processCompleted & processCanceled
        /// </summary>
        private void processCompletedOrCanceled(object sender, EventArgs e)
        {
			this.textOutput.AppendText("done." + Environment.NewLine);
        }

        private void processFailed(object sender, ThreadExceptionEventArgs e)
        {
        	this.textOutput.AppendText(e.Exception.ToString() + Environment.NewLine);
        }    
*/    
	}
}

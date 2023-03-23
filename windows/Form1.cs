/*
 * Created by SharpDevelop.
 * User: Joachim
 * Date: 09.03.2015
 * Time: 21:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using Managed.Adb;
using System.IO;

namespace RemotePanel
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class Form1 : Form
	{
		NotificationIcon main = null;
		string oldPath = null;
		string oldPort = null;
		public Form1(NotificationIcon nIcon)
		{
			
			main = nIcon;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			this.checkVerbose.Checked = main.verbose;
			this.textPort.Text = main.port;
			this.textAdbPath.Text = main.adbpath;
			this.comboDevices.Text = main.serial;
			this.textCheck.Text = Convert.ToString(main.checkinterval/1000);
			this.textWaitTime.Text = Convert.ToString(main.waitTime);
			this.oldPath = main.adbpath;
			this.oldPort = main.port;
			try
			{
				var devices=main.EnsureRunning();
				if (devices != null)
				{
					foreach (Device device in devices)
					{
							this.comboDevices.Items.Add(device.SerialNumber);
					}				
				}
			}
			catch (Exception ex)
			{
				// ignore
			}
			
		}
		
		void Form1Load(object sender, EventArgs e)
		{
			
		}

        protected override void OnClosing(CancelEventArgs e) 
        { 
        	base.OnClosing(e); // der "Basis" Code der beim Schließen ausgeführt wird. Kann je nach bedarf weggelassen werden // Anderer Code, den man einbringen will
        	
        	if (this.textPort.Text.Trim()=="")
        		this.textPort.Text = "38000";

        	if (this.textCheck.Text.Trim()=="")
        		this.textCheck.Text = "30";
        	
        	main.verbose = this.checkVerbose.Checked;
        	main.port = this.textPort.Text.Trim();
        	main.serial = this.comboDevices.Text.Trim();

        	string adbpath = this.textAdbPath.Text.Trim();
			if ((adbpath == null) || (adbpath == ""))
				adbpath = Path.GetDirectoryName(Application.ExecutablePath)+@"\adb\adb.exe";          	
        	
			main.adbpath = adbpath;
			
        	try
        	{
        		main.checkinterval = Convert.ToInt32(this.textCheck.Text.Trim())*1000;
        	}
        	catch (Exception ex)
        	{
        		main.checkinterval = 30000;
        	}

        	try
        	{
        		main.waitTime = Convert.ToInt32(this.textWaitTime.Text.Trim());
        	}
        	catch (Exception ex)
        	{
        		main.waitTime = 100;
        	}

        	
        	Registry.SetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","port", this.textPort.Text.Trim());
        	Registry.SetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","adbpath", this.textAdbPath.Text.Trim());
        	Registry.SetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","serial", this.comboDevices.Text.Trim());
        	Registry.SetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","checkinterval", this.textCheck.Text.Trim());
        	Registry.SetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","waitTime", this.textWaitTime.Text.Trim());
        	
        	string v = "";
        	if (this.checkVerbose.Checked)
        		v = "1";
        	Registry.SetValue(@"HKEY_CURRENT_USER\Software\odospace\RemotePanel","verbose", v);
        	        	
        	if (!this.oldPath.Equals(main.adbpath))
        		main.ShowInfo("Path changed, restart windows to apply changes!");
        	if (!this.oldPort.Equals(main.port))
        		main.ShowInfo("Port changed, restart windows to apply changes!");        	
       	}
		
		void Label1Click(object sender, EventArgs e)
		{
			
		}
		
		void ComboDevicesSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Filter = "ADB executable (adb.exe)|adb.exe|All files (*.*)|*.*";
			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{
    			this.textAdbPath.Text = openFileDialog1.FileName;    			
			}
		}
		
		void TextCheckKeyPress(object sender, KeyPressEventArgs e)
		{
		    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		    {
		            e.Handled = true;
		    }			
		}
		void GroupBox1Enter(object sender, EventArgs e)
		{
	
		}
	}
}

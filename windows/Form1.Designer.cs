/*
 * Created by SharpDevelop.
 * User: Joachim
 * Date: 09.03.2015
 * Time: 21:13
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Microsoft.Win32;
using System;
namespace RemotePanel
{
	partial class Form1
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		


		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.textPort = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textAdbPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.comboDevices = new System.Windows.Forms.ComboBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.textCheck = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.checkVerbose = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.textWaitTime = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(4, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Listen port:";
			this.label1.Click += new System.EventHandler(this.Label1Click);
			// 
			// textPort
			// 
			this.textPort.Location = new System.Drawing.Point(93, 6);
			this.textPort.MaxLength = 5;
			this.textPort.Name = "textPort";
			this.textPort.Size = new System.Drawing.Size(57, 20);
			this.textPort.TabIndex = 1;
			this.textPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextPortKeyDown);
			this.textPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextPortKeyPress);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(4, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 2;
			this.label2.Text = "Path to adb.exe:";
			this.label2.Click += new System.EventHandler(this.Label2Click);
			// 
			// textAdbPath
			// 
			this.textAdbPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.textAdbPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.textAdbPath.Location = new System.Drawing.Point(93, 32);
			this.textAdbPath.Name = "textAdbPath";
			this.textAdbPath.Size = new System.Drawing.Size(426, 20);
			this.textAdbPath.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(154, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 23);
			this.label3.TabIndex = 4;
			this.label3.Text = "(default is 38000)";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 61);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 23);
			this.label4.TabIndex = 5;
			this.label4.Text = "Device serial:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(236, 61);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(283, 23);
			this.label5.TabIndex = 7;
			this.label5.Text = "(empty to use the first available device)";
			// 
			// comboDevices
			// 
			this.comboDevices.FormattingEnabled = true;
			this.comboDevices.Location = new System.Drawing.Point(93, 58);
			this.comboDevices.Name = "comboDevices";
			this.comboDevices.Size = new System.Drawing.Size(137, 21);
			this.comboDevices.TabIndex = 8;
			this.comboDevices.SelectedIndexChanged += new System.EventHandler(this.ComboDevicesSelectedIndexChanged);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Title = "Path to adb.exe";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(525, 31);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(30, 22);
			this.button1.TabIndex = 9;
			this.button1.Text = "...";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(4, 89);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 23);
			this.label6.TabIndex = 10;
			this.label6.Text = "Check interval:";
			// 
			// textCheck
			// 
			this.textCheck.Location = new System.Drawing.Point(93, 86);
			this.textCheck.MaxLength = 3;
			this.textCheck.Name = "textCheck";
			this.textCheck.Size = new System.Drawing.Size(57, 20);
			this.textCheck.TabIndex = 11;
			this.textCheck.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextCheckKeyPress);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(154, 89);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(137, 23);
			this.label7.TabIndex = 12;
			this.label7.Text = "(default is 30 seconds)";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(8, 25);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(85, 23);
			this.label8.TabIndex = 13;
			this.label8.Text = "Verbose:";
			this.label8.Click += new System.EventHandler(this.Label8Click);
			// 
			// checkVerbose
			// 
			this.checkVerbose.Location = new System.Drawing.Point(89, 20);
			this.checkVerbose.Name = "checkVerbose";
			this.checkVerbose.Size = new System.Drawing.Size(104, 24);
			this.checkVerbose.TabIndex = 14;
			this.checkVerbose.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.textWaitTime);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.checkVerbose);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Location = new System.Drawing.Point(4, 115);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(551, 85);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Advanced";
			this.groupBox1.Enter += new System.EventHandler(this.GroupBox1Enter);
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(150, 52);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(100, 23);
			this.label10.TabIndex = 17;
			this.label10.Text = "(default is 100 ms)";
			// 
			// textWaitTime
			// 
			this.textWaitTime.Location = new System.Drawing.Point(89, 49);
			this.textWaitTime.MaxLength = 3;
			this.textWaitTime.Name = "textWaitTime";
			this.textWaitTime.Size = new System.Drawing.Size(57, 20);
			this.textWaitTime.TabIndex = 16;
			this.textWaitTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextCheckKeyPress);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(9, 52);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(76, 23);
			this.label9.TabIndex = 15;
			this.label9.Text = "ADB wait time:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(563, 205);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.textCheck);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboDevices);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textAdbPath);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textPort);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "Remote Panel Settings";
			this.Load += new System.EventHandler(this.Form1Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.CheckBox checkVerbose;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textCheck;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ComboBox comboDevices;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textAdbPath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textPort;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textWaitTime;
		private System.Windows.Forms.Label label9;
		
		void Label2Click(object sender, EventArgs e)
		{
			
		}
		
		void TextPortKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			
		}
		
		void TextPortKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{

		    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		    {
		            e.Handled = true;
		    }
		
		}
		
		void Label8Click(object sender, EventArgs e)
		{
			
		}
	}
}

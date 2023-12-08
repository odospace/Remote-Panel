----
>Google will remove old apps from Google Play after 31. August 2023, when not migrated to a newer API version. Because i don't longer actively support Remote Panel, i have moved the source (and installable runtimes) to GitHub. The installable files can be found in the folder runtime.
----

Reuse your old android device to display sensor values of a Windows PC. A WiFi connection is not (!) required, Remote Panel works even when the device is connected to the PC via USB only. However, an SDK for Remote Panel is provided also.

![image](https://github.com/odospace/Remote-Panel/assets/34418472/62dc0bca-a760-4c41-865d-c87d8b948666)
![image](https://github.com/odospace/Remote-Panel/assets/34418472/977e7700-cf87-4ca6-900a-f587f218ad0d)
![image](https://github.com/odospace/Remote-Panel/assets/34418472/90ea74da-5ae9-49f5-8c69-2ea3c1fe8635)
 
Sensor values are provided by the industry-leading system information tool Aida64 which must be purchased separately. Please note that Remote Panel is not affiliated with Aida64 or FinalWire in any way shape or form and no support can be provided by the Aida64 team for this app.
 
Requirements
- Install the Adroid APK from here https://github.com/odospace/Remote-Panel/raw/main/runtime/RemotePanel.apk
- Aida64 version 5.20.3414 or heigher must be installed on the Windows PC.
- Remote Panel (for Windows) version 1.16 must be installed and running, it can be downloaded with the following link https://github.com/odospace/Remote-Panel/raw/main/runtime/RemotePanelSetup.exe 
- Microsoft .Net framework 4.5 must be installed on the Windows PC. This will be done during the installation of Remote Panel (for Windows).
- The android device vendor's drivers must be installed on the Windows PC.
- USB debugging must be enabled on the android device. This is described in detail here. 
 
Enable Aiada64 plugIn
- After installation of Remote Panel (for Windows) Aida64 must be restarted.
- Within Aida64 open the preferences page, navigate to LCD and enable "Odospace". Add items within the LCD items page.
 
Settings
- A long press within the view opens the settings dialog.
 
Troubleshooting
- In general the settings dialog of Remote Panel (for Windows) can be opened from the popup menu of its tray icon.
- Remote Panel (for Windows) uses port 38000 and 38001 for local communication, if you experience problems because another PC program is using one of this ports, change the port number within the Remote Panel (for Windows) settings dialog and within the Aida64 Odospace LCD plugin.
- Remote Panel (for Windows) uses the Android Debug Bridge (adb.exe) for communication. If you experience problems with other Android synchronization programs, try to use another adb.exe file - it can be changed within the settings dialog of Remote Panel (for Windows).
- On default, Remote Panel (for Windows) checks every 30 seconds for new devices, decrease this value within the settings for faster device recognition, increase this value for less CPU usage.
 
Alternative usage
- If an additional PC should send its sensor values to the android device, set the IP address within the Aida64 Odospace LCD plugin to the address of the PC where the android device is connected. For every PC specify a different panel position parameter. Remote Panel (for Windows) must be installed on every PC, however the Remote Panel (for Windows) executable must only be started on the PC where the Android device is connected.
- Remote Panel can be used within a WiFi network also, in such case set the IP address within the Aida64 Odospace PlugIn to the device's address. The port must be set to 38000. The Remote Panel (for Windows) executable can be stopped in this case.
 
Advanced topics
- To automatically start Remote Panel you can use AutoStart.
- To power off the device on PC shut down you can use AutomateIt Pro - use the USB disconnect trigger.
- If the device battery discharge even connected via USB, try to set the CPU speed to a lower level. For example you can use Tickster MOD.
- To power on the device on PC startup the device firmware must be modified (follow this AT YOUR OWN RISK, tested with Cyanogenmod 10.2 on LG P760/P880 and may not work with all devices):
  General
  1. Download the device firmware - it can be the original or custom ROM.
  2. Use Boot/Recovery Repacker to extract the boot.img file of the downloaded ROM.
  3. Within the file init.rc, in the section beginning with "on charger" replace the string "class_start charger" with "trigger boot" or "trigger late-init". Ensure that your editor writes the file in UNIX format.
  4. Use the repacker to compress the boot.img file.
  5. Apply the new firmware.
  Nexus 7 (2012)
  1. Boot in fastboot mode
  2. Issue the following command within a command prompt: "fastboot oem off-mode-charge 0"
  
 

Version 1.14

- Now sensor data of multiple PCs can be displayed on one android device
- The android device can be connected to any PC (not only to the PC where Aida64 is running)
- Requires Aida version 5.20.3414 or higher and Remote Panel (for Windows) version 1.14a
 
Version 1.15
- Fullscreen mode (long press for settings)
- RemotePanel (for windows) version 1.15: Verbose mode for better problem handling, fixes a rare ADB hang when multiple PCs are connected to one android device.
 
Version 1.16
- New option added: Keep screen on (long press for settings)
Version 1.16a
- Fixed Aida64 crash when resuming from sleep

 

Many thanks to the Aida64 team for the great support!

 

Best regards <br/>
Odo
 
 
 
Links
- Google Play: https://play.google.com/store/apps/details?id=com.odospace.remotepanel
- Aida64 Forum: https://forums.aida64.com/topic/2776-display-pc-sensor-values-on-android-device-connected-via-usb-odospace-remote-panel/


Development tools used
- windows:  https://sourceforge.net/projects/sharpdevelop/
- install:  http://www.jrsoftware.org/isdl.php#stable
- aidardsp: Visual Studio
- android:  Eclipse

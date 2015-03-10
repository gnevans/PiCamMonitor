# PiCamMonitor
Windows program to download and view captured frames from a Raspberry Pi's camera.

The program will periodically check for new captured frames on the Pi and download and organise them (so all frames from a single day are kept together) and will delete the original frames from the Pi to make sure it doesn't run out of space.

The program allows you to view these downloaded frames as a movie.
Also allows you to view the live feed direct from the camera.

This should work with any system that captures frames for any movement detected, but was specifically written to work with [Motion](http://www.lavrsen.dk/foswiki/bin/view/Motion/WebHome) running on a Rapberry Pi.

SFTP is used to download the images.

The frames must be saved in the format YYYYMMDD-HHMMSS-nn.jpg.  With Motion, this can be achieved by setting `picture_filename` to `%Y%m%d-%H%M%S-%q` in the .conf file.

It can also monitor a UDP port and immediately react to events from the Pi.
If using Motion, set `on_event_start` to `./event-start.sh`.
Then create a `event-start.sh` with the following contents:
```
#!/bin/bash
# Broadcast motion event start
# (A broadcast address should work, but not for me)
echo -n "picam-event-start" | nc -w1 -u 192.168.2.23 2123
```
Remember to make the script executable and to make sure that any UDP packets for the specified UDP port can get through any firewalls between the Pi and the machine running PiCamMonitor.

## Build instructions
The program was built with Visual Studio 2012 Express edition targeting the .NET 4.0 framework.

It requires the AForge.Video.DirectShow NuGet package.

It also requires [WinSCP](http://winscp.net/eng/index.php) which will have to be manually added to the project.
The required files need to be downloaded from [here](http://winscp.net/download/winscp570automation.zip).
In the root directory of the source tree, create a winscp570automation directory and extract the downloaded files here.
The WinSCP.exe will need copying to the same directory as PiCamMonitor.exe.

## Configuration instructions
You will need to edit PiCamMonitor.exe.config to suit your environment. The following settings are available in this file:

* **PiMotionImagePath** The full path to where the camera saves the images on the Pi.
* **LocalBaseDownloadPath** The full path to where the images will be downloaded to on the Windows machine. 
* **PiFps** The number of images captured per second.  This should be the same as the `framerate` in the Motion .conf file to ensure that they are played back at the same speed as they were captured at.
* **PiUserName** Username to log into the Raspberry Pi using SFTP.
* **PiPassword** Password for above account.
* **PiSshHostKeyFingerprint** Fingerprint of ssh host key on the Pi.  This can be generated on the Pi with `ssh-keygen -l -f /etc/ssh/ssh_host_rsa_key.pub`.
* **PiHost** Host name / IP address of Raspberry Pi.
* **PiStreamUrl** Url on Raspberry Pi to show live camera stream.
* **DownloadInterval** Interval in minutes between checking the Pi for new downloads.
* **DownloadAtStartup** Set to `True` if you want the program to attempt a download as soon as it starts.  If You set the program to start when Windows starts, then this may not be a good idea as it will slow down the booting of Windows and the network may not be fully initialised which can result in errors.
* **AutoStart** If you set this to `True` then the next time the program is run, it will make sure that it will start when Wndows next starts.
* **UseNotificationArea** If this is set to `True`, then the program will run in the Notification area.  Right click on the icon in the Notification area to have options to open the window or to close down the application.  If this setting is `False` then the program will run as a normal program that may be minimised or closed using the Window buttons.
* **PiEventPort** UDP Port number Pi broadcasts events on.
* **EventStartSound** Path to sound file to play when a start event is received from the Pi. 



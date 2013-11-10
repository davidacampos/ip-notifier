ip-notifier
===========

Windows service that polls current external IP to see if it has been updated. If so, it upload its value to an FTP server.

Mostly to be used whenver you have a home server with a dynamic IP, a hosted FTP server (like GoDaddy) and don't want to rely on services like DynDNS.

It's DotNet 4.5 (C#) based and uses Inno Setup 5 for the installer.

Installing
-----------

Just grab the latest setup.exe from /installer and you are good to go.

The installer will ask for several settings including:

* FTP hostname + path + filename (the text file were the IP is going to get uploaded)
* FTP username
* FTP password

Because it's a Windows service, you do not require to "run" the .exe after installing it, it will automatically run; even after reboots.

After installation, settings can be updated manually by updating "ip-notifier.exe.config". A service restart is needed for changes to reflect.

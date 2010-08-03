#!/bin/sh

cd GTKClient/bin/Debug

# startup VNC servers
Xvnc :20 -viewonly -ac -bs  -geometry 1024x768 -depth 16 -desktop sagapresenter -alwaysshared -nocursor &
sleep 2s

# small displays version
x11vnc -viewonly -shared -forever -reflect localhost:5920 -scale 800/1024:nb -rfbport 5921 -nocursor -nopw &

while [ 1 ]; do
	mono GTKClient.exe
done


#!/bin/sh
cd presenterd/bin/Debug
while [ 1 ]; do
	mono presenterd.exe LanSettings.xml
done

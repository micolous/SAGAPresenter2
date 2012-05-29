# SAGAPresenter2 #

Program for running automated presentation displays at LAN parties.

Copyright 2009-2010 Michael Farrell.  Licensed under the AGPL3+.

## Introduction ##

SAGAPresenter2 was a presentation software that was used at StreetGeek between 2009 and 2010.

The original version of the software was called "SAGAPresenter".  This had a monolithic design around a GTK# application.

SAGAPresenter2 improved on this by seperating the view control engine into a seperate daemon over DBus (presenterd), which could then have various clients attached to it (like GTKClient and SP2WebViewer).

## Roadmap ##

* Get building with current Mono, etc.
* Port [CGServer (the PETSCII/C64 display mode](http://micolous.id.au/archives/2009/09/30/retrolan-fun/) from SAGAPresenter1 to a daemon in SAGAPresenter2.
* Make GTKClient work with non-1024x768 displays.
* Remove unused experimental codes, or replace them with something that works.
* Package it.


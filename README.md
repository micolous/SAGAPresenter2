# SAGAPresenter2 #

Program for running automated presentation displays at LAN parties.

Copyright 2009-2010 Michael Farrell.  Licensed under the AGPL3+.

## Introduction ##

SAGAPresenter2 was a presentation software that was used at StreetGeek between 2009 and 2010.

The original version of the software was called "SAGAPresenter".  This had a monolithic design around a GTK# application.

SAGAPresenter2 improved on this by seperating the view control engine into a seperate daemon over DBus (presenterd), which could then have various clients attached to it (like GTKClient and SP2WebViewer).

## Notes ##

While this is a C# application, it will probably not work on Windows.

It depends on DBus bindings for C#, and uses DBus for internal communications heavily.

* `presenterd`: View control engine.  This communicates updates to all clients, sends out events.  It is the backend of the system.  It runs as a DBus service.
* `libpresenterd`: Client library which takes care of some re-written code in C# clients.  Also provides some basic ASP.NET data objects.
* `presenter-test`: Text-mode client which dumps out events sent by `presenterd` for debugging.
* `GTKClient`: Main client application.  Shows a 1024x768 window with the current display.  This is distributed out to clients over VNC and xrdp.
* `SP2WebAdmin`: ASP.NET web application for managing `presenterd`.
* `SP2WebViewer`: ASP.NET web application which prints a list of upcoming events.
* `SP2WebServer`: I believe this was a start on a different way to run the web stuff which was never finished.  I can't remember, this may be nuked in future.
* `pygclient`: Attempt at writing a pygame client.  It failed miserably.
* `packaging`: Packaging MonoDevelop meta-project.
* `scripts`: Common scripts used by the MonoDevelop project.  All AssemblyInfo.cs files are generated dynamically, for example, from hg commits.

## Roadmap ##

* Get building with current Mono, etc.
* Change `create_assembly_info.sh` to use git.
* Port [CGServer (the PETSCII/C64 display mode)](http://micolous.id.au/archives/2009/09/30/retrolan-fun/) from SAGAPresenter1 to a daemon in SAGAPresenter2.
* Make GTKClient work with non-1024x768 displays.
* Remove unused experimental codes, or replace them with something that works.
* Package it.


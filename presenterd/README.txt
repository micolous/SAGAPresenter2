presenterd
==========

presenterd is the backend "server" component of SAGAPresenter2.

It runs as a DBus server for other "clients" which display information.
Typically, you would have something like a GTK# client connected to the
backend, which is itself connected to a VNC X server to distribute to clients.

You would also have a configuration system attached, such as the ASP.NET web
application.
 
This differs greatly from the monolithic architechure of SAGAPresenter1, which
ran as a GTK# application controlling the main interface and running a bunch
of servers attached to it.  This way everything is running as it's own process,
allowing it to be extended as much as you like.


Configuring DBUS for presenterd
===============================

presenterd needs permission to use the system bus.

<!DOCTYPE busconfig PUBLIC "-//freedesktop//DTD D-BUS Bus Configuration 1.0//EN" "http://www.freedesktop.org/standards/dbus/1.0/busconfig.dtd">
<!-- This file should be put in /etc/dbus-1/system.d/sp2.conf -->
<busconfig>
	<policy user="username">
		<allow own="au.id.micolous.sp2"/>
		<allow send_destination="au.id.micolous.sp2" send_interface="au.id.micolous.sp2.presenterd"/>
		<allow send_destination="au.id.micolous.sp2" send_interface="au.id.micolous.sp2.settings"/>
		<allow send_destination="au.id.micolous.sp2" send_interface="org.freedesktop.DBus.Introspectable"/>
	</policy>
</busconfig>

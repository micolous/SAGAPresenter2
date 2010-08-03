using System;
using NDesk.DBus;
using org.freedesktop.DBus;
using GLib;
using SAGAPresenter.libpresenterd;

namespace SAGAPresenter.presenterd
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Settings settings = Settings.LoadSettingsFromFile (args[0]);
							
			//BusG.Init();
			
			Bus bus = ClientHelper.MessageBus;
			string bus_name = "au.id.micolous.sp2";
			ObjectPath path = new ObjectPath ("/presenterd");
			
			if (bus.RequestName (bus_name) == RequestNameReply.PrimaryOwner) {
				Presenterd presenterd = new Presenterd (settings, bus);
				bus.Register (path, (IPresenterd)presenterd);
				
				
				Console.WriteLine("presenterd is running...");
				
				presenterd.StartLoop();
				
				while (true)
					bus.Iterate();
				
			} else {
				// could not claim ownership of the bus
				Console.WriteLine("Error: Could not claim primary ownership of SAGAPresenter2 DBUS object.");
				Console.WriteLine("This is probably because another instance of presenterd is already running, or system policy prohibits it.");
			}
					
		}
	}
}
	
using System;
using Gtk;
using NDesk.DBus;
using SAGAPresenter.libpresenterd;

namespace GTKClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			BusG.Init(ClientHelper.MessageBus);
			Application.Init ();
			
			Settings settings = Settings.Default;
			settings.ThemeName = "WhiteOnBlackLargePrint";
			settings.FontName = "Droid Sans 42";
			
			MainWindow win = new MainWindow ();
			win.Show ();
			
			Application.Run ();
		}
	}
}

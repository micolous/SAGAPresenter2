using System;
using SAGAPresenter.libpresenterd;

namespace presentertest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Test program for presenterd.");
			
			// get current slide
			IPresenterd ip = ClientHelper.Connect ();
			
			Console.WriteLine ("Slide is {0}.", ip.CurrentSlide);
			
			ip.ClockChange += delegate(double time) {
				Console.WriteLine ("Clock changed to {0}", ClientHelper.UnixToDateTime (time));
			};
			
			ip.SlideChange += delegate(Slide newSlide) {
				Console.WriteLine ("Slide changed to {0}", newSlide);
			};
			
			/*
			ip.ActiveSettings.SettingsChanged += delegate() {
				Console.WriteLine ("Settings change event fired.");
			};*/
			
			while (true)
				ClientHelper.IterateBus();
		}
	}
}

using System;
using System.Collections.Generic;
using MiniHttpd;
using SAGAPresenter;

namespace SP2WebServer.HTTPd
{
	public class Index : SimplePageWrapper
	{

		public Index(string name, IDirectory parent, string t) : base(name, parent, t) {
		}
		
		public override Dictionary<string, object> GetPageData (HttpRequest request, IDirectory directory)
		{
			Dictionary<string,object> d = new Dictionary<string,object>();
			d.Add("MarqueeText", MainClass.Sanitise(MainClass.settings.GetMarqueeText()));
			d.Add("NotepadText", MainClass.Sanitise(MainClass.settings.GetNotepadText()));
			d.Add("Timeout", 10);
			d.Add("Fonts", new string[] {"placeholder"});
			d.Add("Font", "placeholder");
			
			d.Add("EventNameFull", MainClass.Sanitise(MainClass.settings.GetEventNameFull()));
			
			/*
			 * d.Add("MarqueeText", Main.Sanitise(mw.MarqueeText));
			d.Add("NotepadText", Main.Sanitise(mw.LanSettings.Notepad));
			d.Add("Timeout", mw.LanSettings.NotepadTimeout);
			d.Add("Fonts", Main.Sanitise(mw.LanSettings.Fonts.ToArray()));
			*/
			//Pango.FontDescription fd = Pango.FontDescription.FromString(mw.LanSettings.NotepadFont);
			
			//d.Add("Font", Main.Sanitise(fd.Family));
			// UGLY HACK ALERT
			// this really should be done properly, however the font size reported in the fontdescription is the wrong
			// unit and no suitable conversion function to points is quickly available.
			//try { 
			//	d.Add("FontSize", int.Parse(mw.LanSettings.NotepadFont.Remove(0,fd.Family.Length+1)));
			//} catch (FormatException) { // some fonts have incorrect family names for some reason, another thing to fix.
				d.Add("FontSize", 30);
			//}
			return d;
		}



	}
}


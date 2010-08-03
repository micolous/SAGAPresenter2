using System;
using System.Collections.Generic;
using MiniHttpd;
using SAGAPresenter;

namespace SP2WebServer.HTTPd
{
	public class MarqueeCommit : SimplePageWrapper
	{

		public MarqueeCommit(string name, IDirectory parent, string t) : base(name, parent, t) {
		}
		
		public override Dictionary<string, object> GetPageData (HttpRequest request, IDirectory directory)
		{
			Dictionary<string,object> d = new Dictionary<string,object>();
			
			// save the data back
			Console.WriteLine("Setting to {0}.", request.Query.Get("marqueeText"));
			MainClass.settings.SetMarqueeText(request.Query.Get("marqueeText"));
			MainClass.settings.FireSettingsChanged();
			d.Add("Redirect", "index.html");
			return d;
		}
	}
}


using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using SAGAPresenter.libpresenterd;
using System.Threading;

namespace SP2WebViewer
{


	public class Global : System.Web.HttpApplication
	{
		//internal static ISettings settings = ClientHelper.ConnectSettings();
		internal static Thread pumpThread;
		
		
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
			pumpThread = new Thread((ThreadStart)delegate() {
				while (true)
					ClientHelper.IterateBus();
			});
			pumpThread.Start();
			
		}

		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_Error (Object sender, EventArgs e)
		{
		}

		protected virtual void Session_End (Object sender, EventArgs e)
		{
		}

		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

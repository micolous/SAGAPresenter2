using System;
using MiniHttpd;
using SAGAPresenter.libpresenterd;
using TemplateMaschine;
using System.Reflection;
using System.Threading;
namespace SP2WebServer
{
	class MainClass
	{
		private static HttpWebServer server;
		internal static Template errorTemplate = new Template("Error.template", Assembly.GetExecutingAssembly()); 
		
		internal static ISettings settings = ClientHelper.ConnectSettings();
		
		
		public static void Main (string[] args)
		{
			Console.WriteLine("Pumping the DBus...");
			new Thread((ThreadStart)DBusIterator).Start();
			
			server = new HttpWebServer(8000);
			VirtualDirectory root = new VirtualDirectory();
			server.Root = root;
			
			root.AddFile(new HTTPd.Index("index.html", root, "Index.template"));
			root.AddFile(new HTTPd.MarqueeCommit("marquee.commit.html", root, "Redirect.template"));
			server.Authenticator = new WebAuthenticator(null);
			server.Start();
			Console.WriteLine("Web server started on port 8000");
		}
		
		
		static void DBusIterator() {
			while (true)
				ClientHelper.IterateBus();
		}
		
		public static String Version {
			get {
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}
		
				
		internal static string Sanitise(string i) {
			i=i.Replace("&", "&amp;");
			i=i.Replace("\"", "&quot;");
			i=i.Replace("<", "&lt;");
			i=i.Replace(">", "&gt;");
			return i;
		}
		
		internal static string[] Sanitise(string[] i) {
			string[] o = new string[i.Length];
			for (uint x=0; x<i.Length; x++) {
				o[x] = Sanitise(i[x]);
			}
			return o;
		}
	}
}

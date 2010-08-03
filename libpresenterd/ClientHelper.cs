
using System;
using NDesk.DBus;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

namespace SAGAPresenter.libpresenterd
{
	public class ClientHelper
	{

		private ClientHelper ()
		{
		}
		
		private static Bus bus = Bus.System;
		
		public static Bus MessageBus { get { return bus; }}
		
		/// <summary>
		/// Connect to the presenterd and return an instance of <see cref="IPresenterd"/> to talk back to the server.
		/// </summary>
		/// <returns>
		/// An <see cref="IPresenterd"/> for talking to the server.
		/// </returns>
		public static IPresenterd Connect() {
			
			string bus_name = "au.id.micolous.sp2";
			ObjectPath path = new ObjectPath("/presenterd");
			
			IPresenterd presenterd = bus.GetObject<IPresenterd>(bus_name, path);
			return presenterd;
		}
		
		public static ISettings ConnectSettings () {
			return ConnectSettings(Connect());
		}
		
		public static ISettings ConnectSettings (IPresenterd presenterd)
		{
			string bus_name = "au.id.micolous.sp2";
			ISettings settings = bus.GetObject<ISettings> (bus_name, presenterd.ActiveSettingsPath);
			return settings;
		}
		
		public static void IterateBus() {
			bus.Iterate();
		}
		
		private static DateTime epoch = new DateTime(1970,1,1,0,0,0,0,DateTimeKind.Utc);
		
		/// <summary>
		/// Converts a time expressed as seconds since the UNIX epoch into a <see cref="System.DateTime"/>.
		/// </summary>
		/// <param name="unix">
		/// A <see cref="System.Double"/> containing the UNIX time being converted.
		/// </param>
		/// <returns>
		/// A <see cref="System.DateTime"/> of the time inputted.
		/// </returns>
		public static DateTime UnixToDateTime(double unix) {
			return epoch.AddSeconds(unix);
		}
		
		/// <summary>
		/// Converts a time expressed as a <see cref="System.DateTime"/> into the number of seconds since the
		/// UNIX epoch.
		/// </summary>
		/// <param name="dt">
		/// A <see cref="DateTime"/> to convert.
		/// </param>
		/// <returns>
		/// A <see cref="System.Double"/> containing the number of seconds since the UNIX epoch.
		/// </returns>
		public static double DateTimeToUnix (DateTime dt)
		{
			return (dt - epoch).TotalSeconds;
		}
		
		public static string TimeFormat (TimeSpan t)
		{
			if (t.TotalHours >= 3.0) {
				return String.Format ("~{0:0}h", t.TotalHours);
			} else if (t.TotalHours >= 1.0) {
				return String.Format ("{0}h {1}m", t.Hours, t.Minutes);
			} else {
				return String.Format ("{0}m", t.Minutes);
			}
		}
		
		public static List<string> CleanupList(List<string> l, bool cleanDupes)
	    {
			// clone list first
			l = new List<string>(l);
			
	        for (int x = 0; x < l.Count; x++)
	        {
	            l[x] = l[x].Trim();
	            if (l[x].Length == 0)
	            {
	                l.RemoveAt(x--);
	            }
	            else if (cleanDupes)
	            {
	                for (int y = 0; y < x; y++)
	                {
	                    if (l[y].Equals(l[x]))
	                    {
	                        l.RemoveAt(x--);
	                        break;
	                    }
	                }
	            }
	        }
	
	        l.TrimExcess();
			
			return l;
	    }
		
		public static string[] NewLineStringToArray(string input) {
			return input.Split('\n');
		}
		
		public static string ArrayToNewLineString(string[] input) {
			StringBuilder sb = new StringBuilder();
			foreach (string item in input) {
				string clean_item = item.Trim();
				if (clean_item.Length >= 1) {
					sb.Append(clean_item);
					sb.Append('\n');
				}
			}
			
			return sb.ToString().Trim();
		}
		
		private static RNGCryptoServiceProvider crng = new RNGCryptoServiceProvider();
		
		public static uint CryptoRNG(uint maximum) {
			return CryptoRNG(0, maximum);
		}
		
		public static uint CryptoRNG(uint minimum, uint maximum) {
			byte[] randomNumber = new byte[8];
			crng.GetNonZeroBytes(randomNumber);
			
			// now cast that into a float
			MemoryStream ms = new MemoryStream(randomNumber);
			BinaryReader br = new BinaryReader(ms);
			
			ulong u = br.ReadUInt64();
			double n = u / (double)ulong.MaxValue;
			
			// now return the number, in the range.
			// zero will never be returned unless we expand our range.
			uint r = (maximum - minimum) + 1;
			return (uint)(((n * r) + minimum) - 1);
		}
		
		public static string EscapeXML(string input) {
			return input.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
		
		}
	}
}


using System;
using System.Xml.Serialization;
using System.IO;
using System.Text;
namespace SAGAPresenter.libpresenterd
{
	/// <summary>
	/// Represents a competition and it's time.
	/// </summary>
	[XmlTypeAttribute()]
	public class Competition : IComparable<Competition>, ICloneable, IEquatable<Competition>
	{
		private DateTime start;
		private DateTime finish;
		private string name = "";
		private string platforms = "";

		/// <summary>
		/// The time the competition will be starting at.
		/// </summary>
		[XmlAttribute("start")]
		public DateTime Start {
			get { return this.start; }
			set { this.start = value; }
		}
		
		/// <summary>
		/// The time the competition will be finishing at.
		/// </summary>
		[XmlAttribute("finish")]
		public DateTime Finish {
			get { return this.finish; }
			set { this.finish = value; }
		}
		
		/// <summary>
		/// The name of the competition.
		/// </summary>
		[XmlAttribute("name")]
		public string Name {
			get { return this.name; }
			set { this.name = value; }
		}
		
		/// <summary>
		/// The code for the platforms that this game runs on.  This is put through the
		/// ConsoleIcons font to be decoded.
		/// </summary>
		[XmlAttribute("platforms")]
		public string Platforms {
			get { return this.platforms; }
			set { this.platforms = value;}
		}
				

		/// <summary>
		/// Is the competition now?
		/// </summary>
		[XmlIgnore()]
		public bool IsNow {
			get { return this.start < DateTime.Now && this.finish > DateTime.Now; }
		}

		/// <summary>
		/// Is the competition in the future?
		/// </summary>
		[XmlIgnore()]
		public bool Upcoming {
			get { return this.start > DateTime.Now && this.finish > DateTime.Now; }
		}
		
		/// <summary>
		/// Has the competition finished?
		/// </summary>
		[XmlIgnore()]
		public bool IsFinished {
			get { return this.finish < DateTime.Now; }
		}

		/// <summary>
		/// The time until the competition starts.
		/// </summary>
		[XmlIgnore()]
		public TimeSpan StartsIn {
			get { return this.start - DateTime.Now; }
		}

		/// <summary>
		/// The time until the competition finishes.
		/// </summary>
		[XmlIgnore()]
		public TimeSpan FinishesIn {
			get { return this.finish - DateTime.Now; }
		}

		/// <summary>
		/// The total time of the competition.
		/// </summary>
		[XmlIgnore()]
		public TimeSpan TotalTime {
			get { return this.finish - this.start; }
		}

		/// <summary>
		/// A fractional amount (0.0 - 1.0) of the competition remaining.
		/// </summary>
		[XmlIgnore()]
		public double FractionRemaining {
			get { return (FinishesIn.TotalSeconds / TotalTime.TotalSeconds); }
		}
		
		[XmlIgnore()]
		public string StartsFinishesInString {
			get {
				if (IsNow) {
					return "ends in " + ClientHelper.TimeFormat(FinishesIn);
				} else {
					return "starts in " + ClientHelper.TimeFormat(StartsIn);
				}
			}
		}
		
		// required for xml deserialisation
		public Competition () {}

		public Competition (DateTime start, DateTime finish, string name, string platforms)
		{
			this.start = start;
			this.finish = finish;
			this.name = name;
			this.platforms = platforms;
		}

		public int CompareTo (Competition other)
		{
			if (this.start > other.start) {
				return 1;
			} else if (this.start < other.start) {
				return -1;
			} else {
				return this.name.CompareTo (other.name);
			}
		}

		public object Clone ()
		{
			Competition c = new Competition (this.start, this.finish, this.name, this.platforms);
			return c;
		}

		public bool Equals (Competition other)
		{
			return this.name.Equals (other.name) && this.start.Equals (other.start) && this.finish.Equals (other.finish);
		}
		
		public override string ToString ()
		{
			return string.Format ("[Competition: Start={0}, Finish={1}, Name={2}, Platforms={3}]", Start, Finish, Name, Platforms);
		}
		
		public SCompetition ToSCompetition ()
		{
			SCompetition sc = new SCompetition ();
			sc.Name = this.name;
			sc.Start = ClientHelper.DateTimeToUnix(this.start);
			sc.Finish = ClientHelper.DateTimeToUnix(this.finish);
			sc.Platforms = this.platforms;
			return sc;
		}
		
		public Competition (SCompetition sc)
		{
			this.name = sc.Name;
			this.start = ClientHelper.UnixToDateTime(sc.Start);
			this.finish =ClientHelper.UnixToDateTime(sc.Finish);
			this.platforms = sc.Platforms;
		}
		
		public static Competition[] SCompetitionsToCompetitions(SCompetition[] arr) {
			Competition[] output = new Competition[arr.Length];
			int x = 0;
			foreach (SCompetition s in arr) {
				output[x++] = new Competition(s);
			}
			
			return output;
		}
		
		[XmlIgnore]
		/// <summary>
		/// This property automatically serialises the contents of the Competition object into a string as XML.
		/// </summary>
		public string XMLData {
			get {
				StringWriter sw = new StringWriter();
				XmlSerializer ser = new XmlSerializer (typeof(Competition));
				ser.Serialize (sw, this);
				sw.Flush();
				return sw.ToString();
			}
		}
		
		/// <summary>
		/// Deserialises XML representation of a Competition object.
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.String"/> containing the Competition data.
		/// </param>
		/// <returns>
		/// A <see cref="Competition"/> of data that was contained in the string.
		/// </returns>
		public static Competition FromXML(string data) {
			StringReader sr = new StringReader(data);
			XmlSerializer ser = new XmlSerializer (typeof(Competition));
			Competition st = (Competition)ser.Deserialize (sr);
			return st;
		}

	}
	
	/// <summary>
	/// Wrapper for Competition structure to allow it to be transported easily over the DBUS.
	/// </summary>
	public struct SCompetition {
		public string Name;
		public double Start;
		public double Finish;
		public string Platforms;
	}
}


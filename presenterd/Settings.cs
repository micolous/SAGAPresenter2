
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;
using SAGAPresenter.libpresenterd;

namespace SAGAPresenter.presenterd
{

	[XmlType()]
	public partial class Settings : /*MarshalByRefObject,*/ ISettings, ICloneable
	{
		public event SettingsChangeDelegate SettingsChanged;
		public event MarqueeChangeDelegate MarqueeChanged;
		
		
		
		public void FireSettingsChanged ()
		{
			SaveSettingsToFile();
			if (SettingsChanged != null)
				SettingsChanged ();
		}
		
		
		
		[XmlIgnore()]
		private string currentFile;
		[XmlIgnore]
		public string CurrentFile { get { return currentFile;}}
		
		public string EventName = "";
		public string EventNameFull = "";
		public string MarqueeText = "Change me!";
		public string ClockTimeFormat = "ddd, HH:mm:ss";
		public string CompetitionTimeFormat = "HH:mm";
		public string NotepadText = string.Empty;
		public DateTime StartTime;
		public DateTime FinishTime;
		
		public List<string> Participants = new List<string>();
		public List<string> Absentees = new List<string>();
		public List<string> Specials = new List<string>();

		#region ISettings implementation
		public double GetStartTime() {
			return ClientHelper.DateTimeToUnix(StartTime);
		}
		
		public void SetStartTime(double value) {
			StartTime = ClientHelper.UnixToDateTime(value);
		}
		
		public double GetFinishTime() {
			return ClientHelper.DateTimeToUnix(FinishTime);
		}
		
		public void SetFinishTime(double value) {
			FinishTime = ClientHelper.UnixToDateTime(value);
		}
		
		public string GetClockTimeFormat() {
			return ClockTimeFormat;
		}
		
		public void SetClockTimeFormat(string value) {
			ClockTimeFormat = value;
		}
		
		public string GetCompetitionTimeFormat() {
			return CompetitionTimeFormat;
		}
		
		public void SetCompetitionTimeFormat(string value) {
			CompetitionTimeFormat = value;
		}
		
		
		public string GetMarqueeText() {
			return MarqueeText;
		}
		
		public void SetMarqueeText(string value) {	
			Console.WriteLine("Request to change marqueeText to {0}.", value);
			if (MarqueeChanged != null)
				MarqueeChanged(value);
			MarqueeText = value;
		}
		
		
		public string GetEventName() {	
			return EventName;
		}
		
		public void SetEventName(string value) {
			EventName = value;
		}
		
		public string GetEventNameFull() {
			return EventNameFull;
		}
		
		public void SetEventNameFull(string value) {
			EventNameFull = value;
		}
		
		public string GetNotepadText() {
			return NotepadText;
		}
		
		public void SetNotepadText(string value) {
			NotepadText = value;
		}
		
		private List<Competition> competitions = new List<Competition> ();

		public List<Competition> Competitions {
			get { return competitions; }
			internal set { competitions = value; }
		}
		
		[XmlIgnore]
		public SCompetition[] CompetitionsS {
			get {
				SCompetition[] o = new SCompetition[competitions.Count];
				int x = 0;
				foreach (Competition c in competitions) {
					o[x++] = c.ToSCompetition ();
				}
				Console.WriteLine ("Returning {0} competitions.", o.Length);
				return o;
			}
		}
		
		public void AddCompetition (SCompetition sc)
		{
			competitions.Add (new Competition(sc));
		}
		public bool DeleteCompetition (SCompetition sc)
		{
			if (competitions.Count > 1) {
				return competitions.Remove (new Competition(sc));
			} else {
				// prevent deleting last competition.
				return false;
			}
		}
		public void ModifyCompetition (uint index, SCompetition sc)
		{
			competitions[(int)index] = new Competition(sc);
		}

		public void ModifyCompetition (SCompetition old_comp, SCompetition new_comp)
		{
			Competition oc = new Competition (old_comp);
			Competition nc = new Competition (new_comp);
			
			int index = competitions.IndexOf (oc);
			competitions[index] = nc;
		}

		private List<SponsorBanner> sponsorBanners = new List<SponsorBanner>();
		
		//[XmlIgnore]
		public List<SponsorBanner> SponsorBanners {
			get {
				return sponsorBanners;
			}
			internal set { sponsorBanners = value; }
		}
		/*
		[Obsolete]
		[XmlElement("SponsorBanner")]
		public SponsorBanner[] SponsorBannersArr {
			get { return sponsorBanners.ToArray (); }
			set {
				if (value != null)
					sponsorBanners = new List<SponsorBanner> (value);
			}
		}
		*/
		
		public string GetParticipants() {
			return ClientHelper.ArrayToNewLineString(Participants.ToArray());
		}
		
		public void SetParticipants(string value) {
			Participants = ClientHelper.CleanupList(new List<string>(ClientHelper.NewLineStringToArray(value)), true);
		}
		
		public string GetAbsentees() {
			return ClientHelper.ArrayToNewLineString(Absentees.ToArray());
		}
		
		public void SetAbsentees(string value) {
			Absentees = ClientHelper.CleanupList(new List<string>(ClientHelper.NewLineStringToArray(value)), true);
		}
		
		public string GetSpecials() {
			return ClientHelper.ArrayToNewLineString(Specials.ToArray());
		}
		
		public void SetSpecials(string value) {
			Specials = ClientHelper.CleanupList(new List<string>(ClientHelper.NewLineStringToArray(value)), false);
		}
		
		#endregion
		
		public static Settings LoadSettingsFromFile (string name)
		{
			TextReader tr = new StreamReader (name);
			XmlSerializer ser = new XmlSerializer (typeof(Settings));
			Settings st = (Settings)ser.Deserialize (tr);
			st.currentFile = name;
			tr.Close ();
			if (st.Competitions.Count == 0) {
				st.Competitions.Add(new Competition(DateTime.Now, DateTime.Now.AddHours(1), "Example", "L"));
			}
			return st;
		}
		
		public void SaveSettingsToFile() {
			this.SaveSettingsToFile(this.currentFile);
		}
		
		public void SaveSettingsToFile (string name)
		{
			TextWriter tw = new StreamWriter (name);
			XmlSerializer ser = new XmlSerializer (typeof(Settings));
			ser.Serialize (tw, this);
			tw.Flush ();
			tw.Close ();
		}

		#region ICloneable implementation
		public object Clone ()
		{
			Settings settings = new Settings ();
			settings.currentFile = this.currentFile;
			settings.EventName = this.EventName;
			settings.EventNameFull = this.EventNameFull;
			return settings;
		}
		#endregion

	}
}

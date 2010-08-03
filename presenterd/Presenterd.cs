
using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using SAGAPresenter.libpresenterd;
using NDesk.DBus;

namespace SAGAPresenter.presenterd
{


	public class Presenterd : IPresenterd
	{
		private Slide currentSlide;
		private uint slideAgeCounter;
		//private Timer slideTimer;
		private uint pausedFor;
		private Settings settings;
		private string lastWinner = string.Empty;

		#region IPresenterd implementation
		
		public Slide CurrentSlide {
			get {
				return currentSlide;
			}
		
		}
		
		public Slide GetCurrentSlide() { return currentSlide; }
		#endregion
		
		
		private Bus bus;
		private ObjectPath settings_path;
		public Presenterd (Settings settings, Bus bus)
		{
			currentSlide = Slide.UpcomingCompetitions;
			pausedFor = 0;
			this.settings = settings;
			this.bus = bus;
		}
		
		internal void StartLoop() {
			new Timer(delegate(object objref) {
					SlideTimerTick();
			}, null, 0, 1000);
		}
		
		/// <summary>
		/// "Ticks" the slide timer, among other things.  We send all our signals from here.
		/// 
		/// Must be fired once per second.
		/// </summary>
		private void SlideTimerTick ()
		{
			if (pausedFor == 0) {
				bool changed = false;
				
				if (CurrentSlide == Slide.Sponsor && slideAgeCounter >= 30) {
					currentSlide = Slide.UpcomingCompetitions; //Slide.Servers;
					changed = true;
				} else if (CurrentSlide == Slide.Servers && slideAgeCounter >= 45) {
					currentSlide = Slide.UpcomingCompetitions;
					changed = true;
				} else if (CurrentSlide == Slide.UpcomingCompetitions && slideAgeCounter >= 45) {
					PickAndSendNewBanner ();
					currentSlide = Slide.Sponsor;
					changed = true;
				} else if (slideAgeCounter == uint.MaxValue) {
					// coming from a paused slide.
					currentSlide = Slide.UpcomingCompetitions;
					changed = true;
					lastWinner = "";
				}
				
				if (changed) {
					slideAgeCounter = 0;
					SlideChangeFire (currentSlide);
				}
			
			
				// finally, age the slide.
				slideAgeCounter++;
			} else {
				// age the pause counter
				pausedFor--;
				if (pausedFor == 0)
					slideAgeCounter = uint.MaxValue;
			}
			
			// update the clock
			ClockChangeFire(DateTime.Now);
		}
		
		public void HideNotepad ()
		{
			HideSpecialPage();
		}

		
		public void HideSpecialPage ()
		{
			slideAgeCounter = uint.MaxValue;
			pausedFor = 0;
		}
		
		public void ShowNotepad (uint duration)
		{
			pausedFor = duration;
			slideAgeCounter = 0;
			currentSlide = Slide.Notepad;
			SlideChangeFire(currentSlide);
		}


			                     
		#region IPresenterd implementation
		
		public event ClockChangeDelegate ClockChange;
		
		private void ClockChangeFire(DateTime time) {
			Console.WriteLine("Fired ClockChange: {0}", time);
			if (ClockChange != null) {
				ClockChange(ClientHelper.DateTimeToUnix(time));
			}
		}

		
		public void LoadSettings ()
		{
			settings = Settings.LoadSettingsFromFile (settings.CurrentFile);
			if (settings_path != null) {
				bus.Unregister (settings_path);
				settings_path = null;
			}
		}
		
		public void SaveSettings ()
		{
			settings.SaveSettingsToFile ();
		}
		
		public ISettings ActiveSettings {
			get { return settings; }
		}
		
		public ObjectPath ActiveSettingsPath {
			get {
				if (settings_path == null) {
					string guid = Guid.NewGuid ().ToString ().Replace("-", "");
					Console.WriteLine ("Registering on {0}", guid);
					settings_path = new ObjectPath("/" + guid);
					bus.Register(settings_path, settings);
				}
				
				return settings_path;
			}
		}
		
		public event SlideChangeDelegate SlideChange;
		
		private void SlideChangeFire (Slide newSlide)
		{
			Console.WriteLine ("Fired SlideChange: {0}", newSlide);
			if (SlideChange != null) {
				SlideChange (newSlide);
			}
		}
		
		public event SponsorChangeDelegate SponsorChange;
		
		private void SponsorChangeFire (string banner_id)
		{
			Console.WriteLine ("Fired SponsorChange: {0}", banner_id);
			if (SponsorChange != null) {
				SponsorChange (banner_id);
			}
		}
		
		private static Random rng = new Random();
		
		private void PickAndSendNewBanner ()
		{
			uint totalWeight = 0;
			foreach (SponsorBanner b in settings.SponsorBanners) {
				totalWeight += b.Weight;
			}
			
			int roll = rng.Next ((int)totalWeight);
			uint index = 0;
			SponsorBanner choice;
			do {
				choice = settings.SponsorBanners[(int)(index++)];
				roll -= ((int)choice.Weight);
				if (index >= settings.SponsorBanners.Count)
					index = 0;
			} while (roll >= 0);
			
			SponsorChangeFire (choice.BannerID);	
		}
		
		public event PickWinnerDelegate PickWinner;
		private void FirePickWinner(string winner) {
			Console.WriteLine("Fired PickWinner: {0}", winner);
			if (PickWinner != null) {
				PickWinner(winner);
			}
		}
		
		public void HideRandomiser ()
		{
			HideSpecialPage();
		}

		public void ShowRandomiser ()
		{
			pausedFor = 300;
			slideAgeCounter = 0;
			currentSlide = Slide.Randomiser;
			SlideChangeFire(currentSlide);
		}
		
		public void SpinRandomiser (PrizeDrawType pdt)
		{
			ShowRandomiser();
			
			// create a list of participants to choose from
			List<string> choices = new List<string>();
			switch (pdt) {
				
			case PrizeDrawType.Major:
				choices.AddRange(settings.Absentees);
				choices.AddRange(settings.Participants);
				break;
				
			case PrizeDrawType.Minor:
				choices.AddRange(settings.Participants);
				break;
				
			case PrizeDrawType.Special:
				choices.AddRange(settings.Specials);
				break;
			}
			
			
			// pick a winner
			lastWinner = choices[(int)ClientHelper.CryptoRNG((uint)choices.Count)];
			FirePickWinner(lastWinner);
			
			// now add to absentee list if minor prize
			if (pdt == PrizeDrawType.Minor) {
				settings.Participants.Remove(lastWinner);
				settings.Absentees.Add(lastWinner);
			}
		}

		public string GetLastWinner ()
		{
			return lastWinner;
		}


		
		#endregion
		
		
	}
}

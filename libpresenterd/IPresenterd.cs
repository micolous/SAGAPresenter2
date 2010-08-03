
using System;
using System.Collections.Generic;
using NDesk.DBus;
using org.freedesktop.DBus;

namespace SAGAPresenter.libpresenterd
{
	public delegate void ClockChangeDelegate(double time);
	public delegate void SlideChangeDelegate(Slide newSlide);
	public delegate void SponsorChangeDelegate(string banner_id);
	public delegate void PickWinnerDelegate(string winner);
	
	/// <summary>
	/// Types of slides that are handled by presenterd.
	/// </summary>
	public enum Slide {
		UpcomingCompetitions,
		Sponsor,		
		Servers,
		Notepad,
		Randomiser,
		IronChef,
		IRC
	}
	
	public enum PrizeDrawType {
		Minor,
		Major,
		Special
	}
	
	
	[Interface("au.id.micolous.sp2.presenterd")]
	public interface IPresenterd
	{

		/// <summary>
		/// Gets the current slide.  If the slide is changing (in ChangeSlide), then it is the old slide.
		/// </summary>
		Slide CurrentSlide { get; }
		/*[Obsolete]
		Slide GetCurrentSlide();*/
		
		/// <summary>
		/// Reload settings from the currently active file.  This will revert
		/// all changes since the last save.
		/// </summary>
		void LoadSettings();
		
		/// <summary>
		/// Save settings to the currently active file.
		/// </summary>
		void SaveSettings();
		
		/// <summary>
		/// Shows the notepad.
		/// </summary>
		/// <param name="duration">
		/// The number of seconds to display the notepad for.
		/// </param>
		void ShowNotepad(uint duration);
		
		/// <summary>
		/// Hides the notepad straight away, and returns to the normal rotation.
		/// </summary>
		void HideNotepad();
		
		/// <summary>
		/// Shows the randomiser page.
		/// </summary>
		void ShowRandomiser();
		
		/// <summary>
		/// Hides the randomiser page.
		/// </summary>
		void HideRandomiser();
		
		/// <summary>
		/// Spins the randomiser.
		/// </summary>
		/// <param name="pdt">
		/// A <see cref="PrizeDrawType"/> to draw.
		/// </param>
		void SpinRandomiser(PrizeDrawType pdt);
		
		/// <summary>
		/// Access the settings and properties associated with this event.  This is a path to where the settings
		/// object is located.
		/// </summary>
		ObjectPath ActiveSettingsPath { get; }
		
		/*[Obsolete]
		ISettings GetActiveSettings();*/
		
		/// <summary>
		/// This event is fired when the clock has changed.  This event is fired about once every second as part of the event loop,
		/// and indicates when the clock should be redrawn.
		/// </summary>
		event ClockChangeDelegate ClockChange;
		
		/// <summary>
		/// This event is fired when the slide has changed.
		/// </summary>
		event SlideChangeDelegate SlideChange;
		
		/// <summary>
		/// Changes the currently displayed banner.  Called before a SlideChange to Sponsor.
		/// </summary>
		event SponsorChangeDelegate SponsorChange;
		
		event PickWinnerDelegate PickWinner;
		
		/// <summary>
		/// Gets the name of the last winner.
		/// </summary>
		/// <returns>
		/// The name of the last winner.
		/// </returns>
		string GetLastWinner();
	}
}

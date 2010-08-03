
using System;
using System.Collections.Generic;
using org.freedesktop.DBus;
using NDesk.DBus;

namespace SAGAPresenter.libpresenterd
{

	public delegate void SettingsChangeDelegate ();
	public delegate void MarqueeChangeDelegate(string newMarquee);

	[Interface("au.id.micolous.sp2.settings")]
	public interface ISettings : ICloneable
	{
		
		/// <summary>
		/// The short name of the event
		/// </summary>
		string GetEventName();
		void SetEventName(string event_name);
		
		/// <summary>
		/// The full name of the event, if the display method does not support graphical logos.
		/// </summary>
		string GetEventNameFull();
		void SetEventNameFull(string event_name_full);
		
		
		/// <summary>
		/// Text to be displayed on the marquee.
		/// </summary>
		string GetMarqueeText();
		void SetMarqueeText(string marquee_text);
		
		
		/// <summary>
		/// Format of the clock to be shown at the top of the screen.  This is a .NET formatting string.
		/// </summary>
		string GetClockTimeFormat();
		void SetClockTimeFormat(string clock_time_format);
		
		
		/// <summary>
		/// The format of the time used for upcoming competitions.  This is a .NET formatting string.
		/// </summary>
		string GetCompetitionTimeFormat();
		void SetCompetitionTimeFormat(string competition_time_format);
		
		/// <summary>
		/// The text to be shown on the Notepad slide.
		/// </summary>
		string GetNotepadText();
		void SetNotepadText(string notepad_text);
		
		double GetStartTime();
		void SetStartTime(double start_time);
		
		double GetFinishTime();
		void SetFinishTime(double finish_time);
	
#region Competitions	
		/// <summary>
		/// A list of competitions in this event.  This is a clone for transport over the DBUS
		/// because of object transport related issues, so modifying this list is pointless.
		/// 
		/// Take a copy of this array as regenerating it is expensive.
		/// </summary>
		SCompetition[] CompetitionsS { get; }
		
		/// <summary>
		/// Adds a new competition to the list.
		/// </summary>
		/// <param name="sc">
		/// A <see cref="SCompetition"/> to add to the list of competitions.
		/// </param>
		void AddCompetition(SCompetition sc);
		
		/// <summary>
		/// Deletes a competition from the list.
		/// </summary>
		/// <param name="sc">
		/// A <see cref="SCompetition"/> for the competition to remove from the list.
		/// </param>
		/// <returns>
		/// true if the item has been removed from the list, false if the item did not exist in the list.
		/// </returns>
		bool DeleteCompetition(SCompetition sc);
		
		/// <summary>
		/// Modifies the competition at the specified index.
		/// </summary>
		/// <param name="index">
		/// The index in the array of the competition to modify
		/// </param>
		/// <param name="sc">
		/// The new information about the competitions.
		/// </param>
		void ModifyCompetition(uint index, SCompetition sc);
		
		/// <summary>
		/// Swaps out an existing competition for a new one.
		/// </summary>
		/// <param name="old_comp">
		/// A <see cref="SCompetition"/> describing the old competition information.
		/// </param>
		/// <param name="new_comp">
		/// A <see cref="SCompetition"/> describing the new competition information.
		/// </param>
		void ModifyCompetition(SCompetition old_comp, SCompetition new_comp);
#endregion
		
		/// <summary>
		/// Event handler to listen for settings changes from other clients.
		/// </summary>
		event SettingsChangeDelegate SettingsChanged;
		
		/// <summary>
		/// Automatically fired when the marquee is updated.
		/// </summary>
		event MarqueeChangeDelegate MarqueeChanged;
		
		/// <summary>
		/// Fire this event when a settings change is completed.  This will signal other clients
		/// to reload the configuration.
		/// </summary>
		void FireSettingsChanged();
		
		/// <summary>
		/// Gets a list of sponsor banner IDs in the system, followed by their weight (probability of appearing).
		/// </summary>
		List<SponsorBanner> SponsorBanners { get; }
		
		// all these are \n seperated lists.
		string GetParticipants();
		void SetParticipants(string value);
		string GetAbsentees();
		void SetAbsentees(string value);
		string GetSpecials();
		void SetSpecials(string value);
	}
}

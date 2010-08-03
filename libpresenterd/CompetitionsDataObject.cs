
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace SAGAPresenter.libpresenterd
{
	/// <summary>
	/// CompetitionsDataObject implements a Data Object for use by data-bound
	/// controls in .NET, such as those used by ASP.NET.
	/// 
	/// It offers a simple way to interface with the competitions list in
	/// libpresenterd.
	/// </summary>
	[DataObject(true)]
	public class CompetitionsDataObject
	{
		ISettings settings;
		public const string Alphabet = "1234567890ABCDEFGIJKLMNOPQRSTUVWXYZ";
		
		public CompetitionsDataObject ()
		{
			settings = ClientHelper.ConnectSettings();
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public Competition[] Select() {
			List<Competition> comps = new List<Competition>(Competition.SCompetitionsToCompetitions(settings.CompetitionsS));
			comps.Sort();
			return comps.ToArray();
		}
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public Competition[] SelectOnlyCurrent() {
			List<Competition> comps = new List<Competition>(Select());
			List<Competition> current_comps = new List<Competition>();
			
			foreach (Competition c in comps) {
				if (!c.IsFinished) {
					current_comps.Add(c);
				}
			}
			
			return current_comps.ToArray();
		}
		
		[DataObjectMethod(DataObjectMethodType.Update, true)]
		public void Update(string Name, string Platforms, DateTime Start, DateTime Finish, string XMLData) {
			// update object
			SCompetition original_comp = Competition.FromXML(XMLData).ToSCompetition();
			SCompetition new_comp = (new Competition(Start, Finish, Name, Platforms)).ToSCompetition();
			settings.ModifyCompetition(original_comp, new_comp);
			settings.FireSettingsChanged();
		}
		
		[DataObjectMethod(DataObjectMethodType.Insert, true)]
		public void Insert(string Name, string Platforms, DateTime Start, DateTime Finish) {
			// insert object
			Competition c = new Competition(Start, Finish, Name, Platforms);
			settings.AddCompetition(c.ToSCompetition());
			settings.FireSettingsChanged();
		}
	
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
		public void Delete(string XMLData) {
			settings.DeleteCompetition(Competition.FromXML(XMLData).ToSCompetition());
			settings.FireSettingsChanged();
			                                            
		}
		
	}
}

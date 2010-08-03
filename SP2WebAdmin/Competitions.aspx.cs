
using System;
using System.Web;
using System.Web.UI;

using SAGAPresenter.libpresenterd;

// these are extra nasties we need.
using System.Web.UI.WebControls;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;

namespace SP2WebAdmin
{


	public partial class Competitions : System.Web.UI.Page
	{
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			
			dvCompetitions.ChangeMode(DetailsViewMode.Insert);
		}
		
		protected void dvCompetitions_ModeChanging (object sender, System.Web.UI.WebControls.DetailsViewModeEventArgs e)
		{
			if (e.NewMode != DetailsViewMode.Insert) {
				e.Cancel = true;
			}
		}
		
		protected void dvCompetitions_ItemInserted (object sender, System.Web.UI.WebControls.DetailsViewInsertedEventArgs e)
		{
			e.KeepInInsertMode = true;
		}
		
		
		
	}
}


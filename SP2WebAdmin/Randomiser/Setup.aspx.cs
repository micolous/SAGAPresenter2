
using System;
using System.Web;
using System.Web.UI;
using SAGAPresenter.libpresenterd;

namespace SP2WebAdmin.Randomiser
{


	public partial class Setup : System.Web.UI.Page
	{
		private ISettings settings;
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);
			
			settings = ClientHelper.ConnectSettings();
			tbParticipants.Text = settings.GetParticipants();
			tbAbsentees.Text = settings.GetAbsentees();
			tbSpecials.Text = settings.GetSpecials();
		}

		protected void btnCommit_Click (object sender, System.EventArgs e)
		{
			settings.SetParticipants(tbParticipants.Text);
			settings.SetAbsentees(tbAbsentees.Text);
			settings.SetSpecials(tbSpecials.Text);
			settings.FireSettingsChanged();
			
			// now regrab the list, because the server will filter
			// out all the dublicates.
			tbParticipants.Text = settings.GetParticipants();
			tbAbsentees.Text = settings.GetAbsentees();
			tbSpecials.Text = settings.GetSpecials();
		}
		
		
	}
}

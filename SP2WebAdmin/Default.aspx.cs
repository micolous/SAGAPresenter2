
using System;
using System.Web;
using System.Web.UI;
using SAGAPresenter.libpresenterd;


namespace SP2WebAdmin
{


	public partial class Default : System.Web.UI.Page
	{
		private IPresenterd presenterd;
		private ISettings settings;

		
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
		}

		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);
			presenterd = ClientHelper.Connect();
			settings = ClientHelper.ConnectSettings(presenterd);
			txtMarquee.Text = settings.GetMarqueeText();
			txtNotepad.Text = settings.GetNotepadText();

		}

		
		public virtual void btnNotepadCommit_Click (object sender, EventArgs args)
		{
			settings.SetNotepadText(txtNotepad.Text);
			settings.FireSettingsChanged();
			presenterd.ShowNotepad(uint.Parse(ddNotepadDuration.SelectedValue));

		}
		protected void btnMarqueeCommit_Click (object sender, System.EventArgs e)
		{
			settings.SetMarqueeText(txtMarquee.Text);
			settings.FireSettingsChanged();
		}
		
		protected void btnNotepadHide_Click (object sender, System.EventArgs e)
		{
			settings.SetNotepadText(txtNotepad.Text);
			settings.FireSettingsChanged();
			presenterd.HideNotepad();
		}
		
		
		
	}
}


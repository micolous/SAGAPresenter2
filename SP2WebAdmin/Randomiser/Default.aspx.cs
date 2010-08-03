
using System;
using System.Web;
using System.Web.UI;
using SAGAPresenter.libpresenterd;

namespace SP2WebAdmin.Randomiser
{


	public partial class Default : System.Web.UI.Page
	{
		private IPresenterd presenterd;
		
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);
			presenterd = ClientHelper.Connect();
		}

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			presenterd.ShowRandomiser();
			lblWinner.Text = presenterd.GetLastWinner();
		}

		protected void btnMinorPrize_Click (object sender, System.EventArgs e)
		{
			presenterd.SpinRandomiser(PrizeDrawType.Minor);
			
			lblWinner.Text = presenterd.GetLastWinner();
		}
		
		protected void btnMajorPrize_Click (object sender, System.EventArgs e)
		{
			presenterd.SpinRandomiser(PrizeDrawType.Major);
			
			lblWinner.Text = presenterd.GetLastWinner();
		}
		
		protected void btnSpecialPrize_Click (object sender, System.EventArgs e)
		{
			presenterd.SpinRandomiser(PrizeDrawType.Special);
			
			lblWinner.Text = presenterd.GetLastWinner();
		}
		
		protected void btnEndRandomiser_Click (object sender, System.EventArgs e)
		{
			presenterd.HideRandomiser();
			
		}
		
		
		
		
		
	}
}

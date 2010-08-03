
using System;
using System.Web;
using System.Web.UI;
using SAGAPresenter.libpresenterd;
using System.Web.UI.WebControls;

namespace SP2WebViewer
{

	public partial class Default : System.Web.UI.Page
	{
		//private IPresenterd presenterd = ClientHelper.Connect();
		private ISettings settings = ClientHelper.ConnectSettings();

		protected void lblEventName_Load (object sender, System.EventArgs e)
		{
			lblEventName.Text = settings.GetEventName();
			lblMarquee.Text = settings.GetMarqueeText();
			gvCompetitions.Sort("Start", SortDirection.Ascending);
			
		}
	
	

	}
}

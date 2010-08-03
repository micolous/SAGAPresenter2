
using System;
using MiniHttpd;
using SAGAPresenter.libpresenterd;

namespace SP2WebServer
{
	public class WebAuthenticator : IAuthenticator
	{
		private ISettings lanSettings;
		
		public WebAuthenticator(ISettings lanSettings)
		{
			this.lanSettings = lanSettings;
		}
		
		public bool Authenticate(string username, string password) {
		
			// just check the password.
			return password == "1234";
		}

	}
}

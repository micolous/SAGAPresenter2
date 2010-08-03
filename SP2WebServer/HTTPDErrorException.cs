using System;
using System.Collections.Generic;

namespace SP2WebServer.HTTPd
{
	
	
	public class HTTPDErrorException : Exception
	{
		private string errorTitle;
		private string errorMessage;
		private Exception inner;
		
		internal HTTPDErrorException(string errorTitle, string errorMessage) : this(errorTitle, errorMessage, null) {}
		internal HTTPDErrorException(string errorTitle, string errorMessage, Exception inner) : base(errorMessage) {
			this.errorMessage = errorMessage;
			this.errorTitle = errorTitle;
			this.inner = inner;
		}
		
		public string GenerateErrorPage() {
			Dictionary<string, object> args = new Dictionary<string,object>();
			args.Add("ErrorTitle", this.errorTitle);
			args.Add("ErrorMessage", this.errorMessage);
			args.Add("Version", MainClass.Version);
			args.Add("GenerationTime", DateTime.Now.ToString("F"));
			if (inner != null) {
				args.Add("Traceback", inner.StackTrace);
			} else {
				args.Add("Traceback", this.StackTrace);
			}
			return MainClass.errorTemplate.Generate(args);
		}
	}
}
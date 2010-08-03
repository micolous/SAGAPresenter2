using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using MiniHttpd;
using TemplateMaschine;
using SAGAPresenter;

namespace SP2WebServer.HTTPd
{
	
	
	public abstract class SimplePageWrapper : IFile
	{
		protected string name;
		protected IDirectory parent;
		protected Template template;

		public SimplePageWrapper(string name, IDirectory parent, string t) {
			this.name = name;
			this.parent = parent;
			template = new Template(t, Assembly.GetExecutingAssembly());
			
		}
		
	
		
		public void OnFileRequested (HttpRequest request, IDirectory directory)
		{
			request.Response.ResponseContent = new MemoryStream();
			StreamWriter writer = new StreamWriter(request.Response.ResponseContent);
			try {
				Dictionary<string, object> d = GetPageData(request, directory);
				d.Add("Version", MainClass.Version);
				d.Add("GenerationTime", DateTime.Now.ToString("F"));
				writer.Write(template.Generate(d));
			} catch (HTTPDErrorException ex) {
				writer.Write(ex.GenerateErrorPage());
			} catch (Exception ex) {
				HTTPDErrorException hex = new HTTPDErrorException(String.Format("Unhandled Exception: {0}", ex.GetType().FullName), String.Format("An exception ({0}) was thrown that was not handled and turned into a HTTPDErrorException properly.  It's message was: {1}", ex.GetType().FullName, ex.Message), ex);
				writer.Write(hex.GenerateErrorPage());
			}
			writer.Flush();
		}
		
		public virtual Dictionary<string, object> GetPageData(HttpRequest request, IDirectory directory) {
			return null;
		}
		
		public string ContentType {
			get { return "text/html"; }
		}
		
		public string Name
        {
            get { return name; }
        }

        public IDirectory Parent
        {
            get { return parent; }
        }

		public void Dispose() {
		}


	}
}


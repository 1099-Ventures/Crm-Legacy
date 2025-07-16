using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Online.Workflow
{
	internal struct HttpResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public string StatusDescription { get; set; }
		public string Data { get; set; }
	}

	internal class HttpHelper
	{
		internal static HttpResponse Get(string url)
		{
			var request = WebRequest.Create(url) as HttpWebRequest;
			using (var response = request.GetResponse() as HttpWebResponse)
			{
				var result = CreateResponse(response);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					using (var sr = new StreamReader(response.GetResponseStream()))
					{
						result.Data = sr.ReadToEnd();
					}
				}

				return result;
			}
		}

		internal static HttpResponse Put(string url, string json)
		{
			var request = WebRequest.Create(url) as HttpWebRequest;
			request.Method = "PUT";
			request.ContentType = "application/json";
			request.Accept = "Accept=application/json";
			request.ContentLength = json.Length;
			request.AllowWriteStreamBuffering = false;
			request.SendChunked = false;

			using (var sw = new StreamWriter(request.GetRequestStream()))
			{
				sw.Write(json);
			}

			var response = request.GetResponse() as HttpWebResponse;
			var result = CreateResponse(response);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				using (var sr = new StreamReader(response.GetResponseStream()))
				{
					result.Data = sr.ReadToEnd();
				}
			}

			return result;
		}

		private static HttpResponse CreateResponse(HttpWebResponse webResponse)
		{
			return new HttpResponse { StatusCode = webResponse.StatusCode, StatusDescription = webResponse.StatusDescription, };
		}
	}
}


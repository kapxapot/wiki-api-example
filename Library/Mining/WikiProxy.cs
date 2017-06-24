using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WikiApiExample.Model;

namespace WikiApiExample.Mining {
	internal class WikiProxy {
		private string httpProxy;

		public WikiProxy(string httpProxy = null) {
			this.httpProxy = httpProxy;
		}

		public IEnumerable<T> Load<T>(string baseUrl, Func<JObject, IEnumerable<T>> process) where T : BaseObject {
			var result = new List<T>();
			var complete = false;
			var parameters = new Dictionary<string, string>();

			while (!complete) {
				var url = AppendParams(baseUrl, parameters);
				var data = GetJson(url);

				try {
					result.AddRange(process(data));

					if (data["continue"] == null) {
						complete = true;
					}
					else {
						parameters.Clear();

						var cont = (JObject)data["continue"];
						foreach (JProperty jp in cont.Properties()) {
							parameters[jp.Name] = (string)jp.Value;
						}
					}
				}
				catch (Exception ex) {
					throw new ApplicationException("Unexpected JSON format", ex);
				}
			}

			return result;
		}

		private string AppendParams(string url, Dictionary<string, string> parameters) {
			if (parameters == null || parameters.Count == 0) {
				return url;
			}

			var sb = new StringBuilder(url);
			foreach (var p in parameters) {
				sb.Append($"&{p.Key}={p.Value}");
			}

			return sb.ToString();
		}

		private JObject GetJson(string url) {
			try {
				using (var wc = new WebClient()) {
					if (!string.IsNullOrWhiteSpace(httpProxy)) {
						wc.Proxy = new WebProxy(httpProxy);
					}

					var raw = wc.DownloadString(url);
					var json = (JObject)JsonConvert.DeserializeObject(raw);

					return json;
				}
			}
			catch (Exception ex) {
				throw new ApplicationException($"Error loading JSON data for url: {url}", ex);
			}
		}
	}
}

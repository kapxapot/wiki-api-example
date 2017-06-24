using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WikiApiExample.Model;

namespace WikiApiExample.Mining {
	internal class Loader {
		private ILogger logger;
		private WikiProxy proxy;

		public Loader(ILogger logger = null, string httpProxy = null) {
			this.logger = logger;
			proxy = new WikiProxy(httpProxy);
		}

		public IEnumerable<Page> GetPages(double latitude, double longitude, int radius = 10000, int limit = 50) {
			logger?.Write("Loading...");

			var pages = LoadPages(latitude, longitude, radius, limit);

			logger?.Write($"Loaded {pages.Count()} pages.");

			Parallel.ForEach(pages, page => {
				page.Images = LoadImages(page);

				logger?.Write($"{page}: Loaded {page.Images.Count()} images.");
			});

			return pages;
		}

		private IEnumerable<Page> LoadPages(double latitude, double longitude, int radius, int limit) {
			var latStr = latitude.ToString(CultureInfo.InvariantCulture);
			var lonStr = longitude.ToString(CultureInfo.InvariantCulture);
			var url = $"https://en.wikipedia.org/w/api.php?action=query&list=geosearch&gsradius={radius}&gscoord={latStr}|{lonStr}&gslimit={limit}&format=json";

			return proxy.Load(
				url,
				data => {
					var entries = data["query"]["geosearch"];
					return entries.Select(e => new Page((string)e["pageid"], (string)e["title"]));
				}
			);
		}

		private IEnumerable<Image> LoadImages(Page page, bool chopExtensions = true) {
			var url = $"https://en.wikipedia.org/w/api.php?action=query&prop=images&pageids={page.Id}&format=json";

			return proxy.Load(
				url,
				data => {
					var images = data["query"]["pages"][page.Id]["images"];
					if (images != null) {
						return images.Select(i => new Image((string)i["title"], chopExtensions));
					}
					else {
						return Enumerable.Empty<Image>();
					}
				}
			);
		}
	}
}

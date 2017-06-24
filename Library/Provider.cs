using WikiApiExample.Analysis;
using WikiApiExample.Analysis.Metrics;
using WikiApiExample.Mining;

namespace WikiApiExample {
	public class Provider {
		private ILogger logger;
		private Loader loader;

		public Provider(ILogger logger, string httpProxy = null) {
			this.logger = logger;
			loader = new Loader(logger, httpProxy);
		}

		public Data GetData(double latitude, double longitude, IMetric metric = null) {
			metric = metric ?? new SimilarityMetric();
			var analyzer = new Analyzer(logger, metric);
			var pages = loader.GetPages(latitude, longitude);

			return analyzer.Process(pages);
		}
	}
}

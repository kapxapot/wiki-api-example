using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiApiExample.Analysis.Metrics;
using WikiApiExample.Model;

namespace WikiApiExample.Analysis {
	internal class Analyzer {
		private ILogger logger;
		private IMetric metric;

		public Analyzer(ILogger logger) : this(logger, new SimilarityMetric()) { }

		public Analyzer(ILogger logger, IMetric metric) {
			this.logger = logger;
			this.metric = metric;
		}

		public Data Process(IEnumerable<Page> pages, double logThreshold = 0.8) {
			var data = new Data(pages);

			var titles = data.UniqueTitles;
			var count = titles.Count();

			logger?.Write($"{data.Images.Count()} images total.");
			logger?.Write($"{count} unique titles total.");

			logger?.Write("Comparing...");

			Parallel.For(0, count, i => {
				var first = titles.ElementAt(i);

				Parallel.For(i + 1, count, j => {
					var second = titles.ElementAt(j);
					var match = new Match(first, second, metric);

					data.AddMatch(match);
					data.AddSum(first, match.Score);
					data.AddSum(second, match.Score);

					if (match.Score >= logThreshold) {
						logger?.Write($"[{first}] vs [{second}]: {Math.Round(match.Score * 100, 2)}%");
					}
				}
				);
			}
			);

			return data;
		}
	}
}

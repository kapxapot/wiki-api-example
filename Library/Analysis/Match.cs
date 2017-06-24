using WikiApiExample.Analysis.Metrics;

namespace WikiApiExample.Analysis {
	public class Match {
		internal Match(string first, string second, IMetric metric) {
			First = first;
			Second = second;

			Score = metric.Compare(First, Second);
		}

		public string First { get; private set; }
		public string Second { get; private set; }
		public double Score { get; private set; }
	}
}

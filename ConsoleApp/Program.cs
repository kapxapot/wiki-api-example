using System;
using System.Linq;

namespace WikiApiExample {
	class Program {
		static void Main(string[] args) {
			var latitude = 55.7447;
			var longitude = 37.6728;

			var logger = new Logger();
			var provider = new Provider(logger);

			Action<string> w = (s) => Console.WriteLine(s);
			Action n = () => Console.WriteLine();

			try {
				var data = provider.GetData(latitude, longitude);

				var topMatches = data.GetTopMatches();
				var topSums = data.GetTopSums();

				n();
				w($"Best matches:");

				int index = 1;
				foreach (var match in topMatches) {
					w($"{index++}. [{match.First}] vs [{match.Second}]: {Math.Round(match.Score * 100, 2)}%");
				}

				n();
				w($"Best sums:");

				index = 1;
				foreach (var sum in topSums) {
					w($"{index++}. {sum.Key}: {Math.Round(sum.Value, 2)}");
				}
			}
			catch (Exception ex) {
				w(ex.Message);
			}

			n();
			w("Done.");
			w("Press any key.");

			Console.ReadKey();
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WikiApiExample.Analysis.Metrics {
	/// <summary>
	/// This class implements string comparison algorithm
	/// based on character pair similarity
	/// Source: http://www.catalysoft.com/articles/StrikeAMatch.html
	/// Implementation: https://stackoverflow.com/questions/653157/a-better-similarity-ranking-algorithm-for-variable-length-strings
	/// Modified by Sergey Atroshchenko
	/// </summary>
	internal class SimilarityMetric : IMetric {
		public double Compare(string str1, string str2) {
			var pairs1 = WordLetterPairs(str1.ToLower());
			var pairs2 = WordLetterPairs(str2.ToLower()).ToList();

			var intersection = 0;
			var union = pairs1.Count() + pairs2.Count();

			if (union == 0) {
				return 0;
			}

			foreach (var pair1 in pairs1) {
				for (int j = 0; j < pairs2.Count; j++) {
					if (pair1 == pairs2[j]) {
						intersection++;
						pairs2.RemoveAt(j);

						break;
					}
				}
			}

			return intersection * 2.0 / union;
		}

		private IEnumerable<string> WordLetterPairs(string str) {
			return Regex.Split(str, @"\s")
				.Where(w => !string.IsNullOrWhiteSpace(w))
				.SelectMany(w => LetterPairs(w));
		}

		private IEnumerable<string> LetterPairs(string str) {
			var len = str.Length;
			if (len == 1) {
				yield return str;
			}
			else {
				int numPairs = len - 1;
				for (int i = 0; i < numPairs; i++) {
					yield return str.Substring(i, 2);
				}
			}
		}
	}
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WikiApiExample.Model;

namespace WikiApiExample.Analysis {
	public class Data {
		private ConcurrentBag<Match> matches;
		private ConcurrentDictionary<string, double> sums;

		public Data(IEnumerable<Page> pages) {
			Pages = pages;

			matches = new ConcurrentBag<Match>();
			sums = new ConcurrentDictionary<string, double>();
		}

		public IEnumerable<Page> Pages { get; private set; }
		public IEnumerable<Match> Matches => matches;
		public ConcurrentDictionary<string, double> Sums => sums;

		public IEnumerable<Image> Images => Pages.SelectMany(p => p.Images);
		public IEnumerable<string> Titles => Images.Select(i => i.Title);
		public IEnumerable<string> UniqueTitles => Titles.Distinct();

		internal void AddMatch(Match match) {
			matches.Add(match);
		}

		internal void AddSum(string title, double sum) {
			if (!sums.ContainsKey(title)) {
				sums[title] = 0;
			}

			sums[title] += sum;
		}

		public IEnumerable<Match> GetTopMatches(int limit = 20) =>
			matches
				.OrderByDescending(m => m.Score)
				.Take(limit);

		public IEnumerable<KeyValuePair<string, double>> GetTopSums(int limit = 20) =>
			sums
				.OrderByDescending(s => s.Value)
				.Take(limit);
	}
}

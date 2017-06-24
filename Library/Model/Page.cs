using System.Collections.Generic;

namespace WikiApiExample.Model {
	public class Page : BaseObject {
		public string Id { get; set; }
		public string Title { get; set; }
		public IEnumerable<Image> Images { get; set; }

		public Page(string id, string title) {
			Id = id;
			Title = title;
		}

		public override string ToString() {
			return $"[{Id}] {Title}";
		}
	}
}

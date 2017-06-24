namespace WikiApiExample.Model {
	public class Image : BaseObject {
		private bool chopExtension;
		public string RawTitle { get; set; }

		public Image(string rawTitle, bool chopExtension = true) {
			RawTitle = rawTitle;
			this.chopExtension = chopExtension;
		}

		public string Title {
			get {
				var title = RawTitle;

				if (title.ToLower().StartsWith("file:")) {
					title = title.Substring(5);
				}

				if (chopExtension) {
					var pos = title.LastIndexOf(".");
					title = title.Substring(0, pos);
				}

				return title;
			}
		}

		public override string ToString() {
			return Title;
		}
	}
}

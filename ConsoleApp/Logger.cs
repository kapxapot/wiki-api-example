using System;

namespace WikiApiExample {
	public class Logger : ILogger {
		public void Write(string text) {
			Console.WriteLine(text);
		}
	}
}

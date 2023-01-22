using System;
namespace MemesFinderMessageOrchestrator.Extentions
{
	public static class StringEnxtensions
	{
		public static string LimitTo(this string str, int maxLength) =>
			str.Length > maxLength ? str.Substring(0, maxLength) : str;
	}
}


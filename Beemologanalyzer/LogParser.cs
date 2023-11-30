using System.Text.RegularExpressions;

namespace Beemologanalyzer;

public class LogParser
{
	public (List<TimeSpan>, List<long>, int) ParseJoinDates(string text)
	{
		Regex logDateRe = new Regex(@"\d\d\d\d/\d\d/\d\d", RegexOptions.Multiline);
		Regex joinDateRe = new Regex(@"\d\d:\d\d:\d\d\.\d\d\d[+,-]\d\d\d\d", RegexOptions.Multiline | RegexOptions.IgnoreCase);
		
		var logDateMatch = logDateRe.Match(text);
		if (!logDateMatch.Success)
		{
			throw new Exception("No log date found in the log.");
		}
		string logDate = logDateMatch.Value;

		var matches = joinDateRe.Matches(text).Select(m => m.Value).ToList();
		if (logDate.Length == 0 || matches.Count == 0)
		{
			throw new Exception("No join dates found in log.");
		}

		var parsedJoinDates = new List<DateTimeOffset>();
		var joinDifference = new List<TimeSpan>();
		var zeroDifferenceIndexes = new List<long>();

		for (int idx = 0; idx < matches.Count; idx++)
		{
			DateTimeOffset date;
			if (!DateTimeOffset.TryParseExact($"{logDate}T{matches[idx]}", "yyyy/MM/ddTHH:mm:ss.fffzzz", null, System.Globalization.DateTimeStyles.None, out date))
			{
				throw new Exception($"Could not parse date `{logDate}T{matches[idx]}`");
			}

			date = date.ToUniversalTime();

			if (matches[idx].EndsWith("-0700"))
			{
				date = date.AddHours(7);
			}

			parsedJoinDates.Add(date);

			if (idx == 0)
			{
				continue;
			}

			TimeSpan difference = date - parsedJoinDates[idx - 1];
			if (difference.TotalMilliseconds < 0)
			{
				continue;
			}

			joinDifference.Add(difference);

			if (difference.TotalMilliseconds == 0)
			{
				zeroDifferenceIndexes.Add(idx);
			}
		}

		return (joinDifference, zeroDifferenceIndexes, matches.Count);
	}
}
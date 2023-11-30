using System.Text.RegularExpressions;

namespace Beemologanalyzer;

public class Parser
{
	public static int ParseJoinDates(string input)
	{
		string pattern = @"/(\\d\\d:\\d\\d:\\d\\d\\.\\d\\d\\d[+,-]\\d\\d\\d\\d)/w+";

		foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
			Console.WriteLine("{0}\n{1}\n{2}", match.Value, match.Groups[1].Value, match.Index);

		int length = input.Length;
		Console.WriteLine(length);
		return length;
	}
}
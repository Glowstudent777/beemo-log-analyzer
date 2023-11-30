using Beemologanalyzer;
using System;
using System.IO;

namespace Benches
{
	public class Benchmark
	{
		public static async Task Main(string[] args)
		{
			try
			{
				using (var sr = new StreamReader("log_data.txt"))
				{
					Parser.ParseJoinDates(sr.ReadToEnd());
				}
			}
			catch (IOException e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}
		}
	}
}

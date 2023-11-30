using Beemologanalyzer;
using System;
using System.Diagnostics;
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
					string text = await sr.ReadToEndAsync();
					var parser = new LogParser();

					// Run the parser 100 times and take the average time.
					var sw = new Stopwatch();
					sw.Start();
					for (int i = 0; i < 100; i++)
					{
						var parsed = parser.ParseJoinDates(text);
						Console.WriteLine(parsed.Item1[i]);

						Console.WriteLine($"Iteration {i + 1} complete.");
					}
					sw.Stop();

					Console.WriteLine($"Total Time: {sw.ElapsedMilliseconds}ms\nAverage time: {sw.ElapsedMilliseconds / 100}ms");
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

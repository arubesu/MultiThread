using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Multithreading
{
	class Program
	{
		static void Main(string[] args)
		{
			PrintElapsedTime(SequentialExecution);
			PrintElapsedTime(ParallelExecution);
		}

		private static void PrintElapsedTime(Action action)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			action?.Invoke();

			stopWatch.Stop();
			Console.WriteLine($"\n{action.Method.Name}");
			Console.WriteLine($"Elapsed Time: {stopWatch.ElapsedMilliseconds} ms\n");
		}

		private static void ParallelExecution()
		{
			Parallel.Invoke(() => CookPasta(),
							() => BraisingSauce());
			Serve();
		}

		private static void SequentialExecution()
		{
			CookPasta();
			BraisingSauce();
			Serve();
		}

		private static void Serve()
		{
			Console.WriteLine(
							"Transfer Cooked Pasta to Sauce. \nEnjoy It!.");
		}

		static void CookPasta()
		{
			Console.WriteLine("Cooking the perfect pasta...");
			Thread.Sleep(2000);
			Console.WriteLine("The pasta is already cooked!");
			Console.WriteLine();
		}

		static void BraisingSauce()
		{
			Console.WriteLine("Braising Sauce...");
			Thread.Sleep(1000);
			Console.WriteLine("The Sauce is done!");
			Console.WriteLine();
		}
	}
}

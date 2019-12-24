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

		}

		/// <summary>
		/// Cook in sequential and parallel form then print the elapsed time.
		/// </summary>
		private static void Cook()
		{
			PrintElapsedTime(SequentialExecution);
			PrintElapsedTime(ParallelExecution);
		}

		/// <summary>
		/// Prints the elapsed time from action execution
		/// </summary>
		/// <param name="action">Action to be executed</param>
		private static void PrintElapsedTime(Action action)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			action?.Invoke();

			stopWatch.Stop();
			Console.WriteLine($"\n{action.Method.Name}");
			Console.WriteLine($"Elapsed Time: {stopWatch.ElapsedMilliseconds} ms\n");
		}

		/// <summary>
		/// Cooks in parallel.
		/// </summary>
		private static void ParallelExecution()
		{
			Parallel.Invoke(() => CookPasta(),
							() => BraisingSauce());
			Serve();
		}

		/// <summary>
		/// Cooks sequentially.
		/// </summary>
		private static void SequentialExecution()
		{
			CookPasta();
			BraisingSauce();
			Serve();
		}

		/// <summary>
		/// Serve the meal
		/// </summary>
		private static void Serve()
		{
			Console.WriteLine(
							"Transfer Cooked Pasta to Sauce. \nEnjoy It!.");
		}

		/// <summary>
		/// Cooks Pasta
		/// </summary>
		static void CookPasta()
		{
			Console.WriteLine("Cooking the perfect pasta...");
			Thread.Sleep(2000);
			Console.WriteLine("The pasta is already cooked!");
			Console.WriteLine();
		}

		/// <summary>
		/// Do braised sauce
		/// </summary>
		static void BraisingSauce()
		{
			Console.WriteLine("Braising Sauce...");
			Thread.Sleep(1000);
			Console.WriteLine("The Sauce is done!");
			Console.WriteLine();
		}
	}
}

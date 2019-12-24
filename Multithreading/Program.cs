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

			BreakingParalellFor(80);
		}

		/// <summary>
		/// Breaks the parallel loop from input number
		/// </summary>
		/// <param name="number">Number to break loop</param>
		private static void BreakingParalellFor(int number = -1)
		{
			var loopResult = Parallel.For(0, 100, (int i, ParallelLoopState state) =>
			{
				if (i == number)
				{
					state.Break();
				}

				ProcessValue(i);
			});

			Console.WriteLine($"Success Executed All itens ? {loopResult.IsCompleted}\n");
			Console.WriteLine($"Lowest Break Iteration :  {loopResult.LowestBreakIteration}\n");
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

		#region Parallel For 

		/// <summary>
		/// Execute processing in sequential and parallel and show the results
		/// </summary>
		static void PrintProcessingResults()
		{
			PrintElapsedTime(SequentiallyProcess);
			PrintElapsedTime(ParallelProcess);
		}

		/// <summary>
		/// Process from 0 to 100 (Exclusive) in parallely
		/// </summary>
		static void ParallelProcess() => Parallel.For(0, 100, (i) => ProcessValue(i));

		/// <summary>
		/// Process from 0 to 100 (Exclusive) sequentially
		/// </summary>
		static void SequentiallyProcess()
		{
			for (int i = 0; i < 100; i++)
			{
				ProcessValue(i);
			}
		}

		/// <summary>
		/// Prints when the value starts to be processed and finish its process
		/// </summary>
		/// <param name="value">Value to process</param>
		static void ProcessValue(int value)
		{
			Console.WriteLine($"Starting to process {value} value");
			Thread.Sleep(100);
			Console.WriteLine($"Finishing to process {value} value");
		}

		#endregion

		#region Parallel Invoke

		/// <summary>
		/// Cook in sequential and parallel form then print the elapsed time.
		/// </summary>
		private static void Cook()
		{
			PrintElapsedTime(SequentialExecution);
			PrintElapsedTime(ParallelExecution);
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

		#endregion
	}
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		/// Retrieves the list of Pokemons
		/// </summary>
		/// <returns></returns>
		private static List<Pokemon> GetPokemons() =>
			JsonConvert.DeserializeObject<List<Pokemon>>(GetPokedexJson());

		/// <summary>
		/// Retrives pokedex JSON
		/// </summary>
		/// <returns></returns>
		private static string GetPokedexJson() => File.ReadAllText("../../../../Pokedex.json");

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

		#region Threads

		/// <summary>
		/// Queue work itens in threads 
		/// </summary>
		/// <param name="length">Length</param>
		static void QueueWorkItens(int length)
		{

			for (int i = 0; i < length; i++)
			{
				int itenState = i;
				ThreadPool.QueueUserWorkItem((state)
							=> ExecuteThreadWithParameter(itenState));
			}
		}

		/// <summary>
		/// Execute Thread With Parameter
		/// </summary>
		/// <param name="param">Parameter</param>
		static void ExecuteThreadWithParameter(object param)
		{
			ShowThreadInformation(Thread.CurrentThread);
			Console.WriteLine("Execution Start: {0}", param);
			Thread.Sleep(1000);
			Console.WriteLine("Execution End: {0}", param);
		}

		/// <summary>
		/// Print Thread information
		/// </summary>
		/// <param name="thread">Thread</param>
		static void ShowThreadInformation(Thread thread)
		{
			Console.WriteLine();
			Console.WriteLine($"Name: {thread.Name}");
			Console.WriteLine($"CurrentCulture: {thread.CurrentCulture}");
			Console.WriteLine($"Priority: {thread.Priority}");
			Console.WriteLine($"ExecutionContext: {thread.ExecutionContext}");
			Console.WriteLine($"Is Background? {thread.IsBackground}");
			Console.WriteLine($"Is Thread in Pool Thread? {thread.IsThreadPoolThread}");
			Console.WriteLine();
		}

		#endregion

		#region Waiting tasks

		/// <summary>
		/// Process tasks continuously
		/// </summary>
		private static void ContinuousTask()
		{
			Task task = Task.Run(() => Hello());

			//Execute task if not on faulted
			task.ContinueWith((previousTask) => World(),
				TaskContinuationOptions.NotOnFaulted);

			//This task only is executed on faulted
			task.ContinueWith((previousTask) => Error(previousTask),
				TaskContinuationOptions.OnlyOnFaulted);
		}

		/// <summary>
		/// Say World
		/// </summary>
		private static void World() => Console.WriteLine("World!");

		/// <summary>
		/// Say Hello
		/// </summary>
		private static void Hello()
		{
			Console.WriteLine("Hello");
			throw new ApplicationException("Ops! An error ocourred!");
		}

		/// <summary>
		/// Prints all innerExceptions from task
		/// </summary>
		/// <param name="task">Task</param>
		private static void Error(Task task)
		{
			var exceptions = task.Exception.InnerExceptions;

			foreach (var exception in exceptions)
				Console.WriteLine(exception);
		}


		/// <summary>
		/// Process the race waiting all tasks
		/// </summary>
		private static void WaitingAllTasksFinish()
		{
			PrintThreadCount();

			Task[] tasks = new Task[10];

			for (int i = 0; i < 10; i++)
			{
				//Keeps the runner number in local variable to fix running condition
				// If don't keep this value the i variable starts with 10 value
				int runnerNumber = i;

				Task task = Task.Run(() => Run(runnerNumber));
				tasks[i] = task;
			}

			//Wait all tasks finish
			Task.WaitAll(tasks);

			PrintThreadCount();
		}

		/// <summary>
		/// Prints the thread count
		/// </summary>
		private static void PrintThreadCount()
		{
			Console.WriteLine("Threads Count:");
			Console.WriteLine(Process.GetCurrentProcess().Threads.Count);
		}


		/// <summary>
		/// Process the runner run
		/// </summary>
		/// <param name="runnerNumber">The Runner number</param>
		public static void Run(int runnerNumber)
		{
			Console.WriteLine("Runner {0} run", runnerNumber);

			Thread.Sleep(1000);
			Console.WriteLine("Corredor {0} finished", runnerNumber);
		}

		#endregion

		#region Parallel Linq

		/// <summary>
		/// Print the pokemon names with forced parallelism
		/// </summary>
		/// <param name="degree">Degree of parallelism</param>
		private static void PrintPokemonNamesWithForcedParallelism(int degree = 4)
		{
			var pokemons =
										from p in
											GetPokemons()
											.AsParallel()
											.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
											.WithDegreeOfParallelism(degree)
										select new
										{
											Id = p.Id,
											Name = p.Name.English
										};

			foreach (var pokemon in pokemons)
				Console.WriteLine($"Pokemon : {pokemon.Name}");
		}

		/// <summary>
		/// Print the pokemon English Names
		/// </summary>
		private static void PrintPokemonNames()
		{
			var pokemons =
							from p in GetPokemons()
							select new
							{
								Id = p.Id,
								Name = p.Name.English
							};

			foreach (var pokemon in pokemons)
				Console.WriteLine($"Pokemon : {pokemon.Name}");
		}

		#endregion

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

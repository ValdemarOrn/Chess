using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MagicBitboard.Performance
{
	class Program
	{
		static Dictionary<ulong, ulong> Map;
		static int idx;
		static int Bits;
		static bool Running;
		static DateTime StartTime;
		static bool Success;
		static Dictionary<int, Tuple<int, ulong>> MagicNumbers = new Dictionary<int, Tuple<int, ulong>>();
		static string Logfile;

		static unsafe void Main(string[] args)
		{
			CalculateMagic();
		}

		public static void CalculateMagic()
		{
			Logfile = "Log-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
			int timeoutSeconds = 10;

			Console.Write("Rook or Bishop: ");
			string line = Console.ReadLine();
			bool rook = (line.ToLower().Trim() == "rook");

			Console.Write("Position to start at: ");
			line = Console.ReadLine();
			idx = Convert.ToInt32(line);

			Console.Write("# of positions to search: ");
			line = Console.ReadLine();
			int idxMax = Convert.ToInt32(line) + idx;

			Console.Write("Time for each iteration (seconds): ");
			line = Console.ReadLine();
			timeoutSeconds = Convert.ToInt32(line);

			Console.Write("Number of worker threads: ");
			line = Console.ReadLine();
			int numberOfThreads = Convert.ToInt32(line);

			Bits = 14;

			while (true)
			{
				if (idx >= idxMax)
					break;

				if (rook)
				{
					var perms = BitboardRook.GetPermutations(idx);
					Map = new Dictionary<ulong, ulong>();

					foreach (var perm in perms)
						Map[perm] = BitboardRook.GetMoves(perm, idx);
				}
				else
				{
					var perms = BitboardBishop.GetPermutations(idx);
					Map = new Dictionary<ulong, ulong>();

					foreach (var perm in perms)
						Map[perm] = BitboardBishop.GetMoves(perm, idx);
				}

				int cardinalCount = Map.Select(x => x.Value).Distinct().Count();

				Log("Position " + idx + " : " + Map.Count + "/" + cardinalCount + ". Searching for " + Bits + " bit index");

				Running = true;
				Success = false;
				StartTime = DateTime.Now;

				for (int i = 0; i < numberOfThreads; i++)
					new Thread(new ThreadStart(Find)).Start();

				while (Running)
				{
					if ((DateTime.Now - StartTime).TotalSeconds > timeoutSeconds)
						Running = false;

					Thread.Sleep(10);
				}

				Thread.Sleep(100);



				if (Success)
				{
					Log("New best match for postition " + idx + ": " + MagicNumbers[idx].Item1 + " bits - " + MagicNumbers[idx].Item2);
					Bits--;
				}
				else
				{
					Log("Search aborted");
					Bits = 14;
					idx++;
				}
			}

			Log("-----------------------------");
			foreach (var kvp in MagicNumbers)
				Log("" + kvp.Key + ": " + kvp.Value.Item1 + " bits: " + kvp.Value.Item2);
			Log("-----------------------------");
			Log("All numbers found");
			Console.ReadLine();
		}

		private static void Log(string p)
		{
			Console.WriteLine(p);
			System.IO.File.AppendAllText(Logfile, p + "\n");
		}

		public static void Find()
		{
			var start = DateTime.Now;
			ulong val = MagicBitboard.FindMagic(Map, Bits, 0, ref Running);
			if (val != 0)
			{
				Success = true;
				lock (MagicNumbers)
				{
					MagicNumbers[idx] = new Tuple<int, ulong>(Bits, val);
				}
			}
			Running = false;
		}
	}
}

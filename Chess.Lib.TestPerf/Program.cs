using Chess.Lib.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.TestPerf
{
	unsafe class Program
	{
		static void Main(string[] args)
		{
			Manager.InitLibrary();

			//TestSearchCount();
			PerftTest();
		}

		private static void PerftTest()
		{
			var b = Board.Create();
			Board.Init(b, 1);

			for (int depth = 1; depth <= 5; depth++)
			{
				var start = DateTime.Now;
				var count = Perft.Search(b, depth);
				var seconds = (DateTime.Now - start).TotalSeconds;
				Console.WriteLine("Perft(" + depth + "): " + count + ", Time: " + String.Format("{0:0.000}", seconds));
			}
		}

		private static void TestSearchCount()
		{
			var t = new SearchTest();
			t.TestSearchNodeCount();
		}
	}
}

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
			//PerftTestSuite.RunTests();
			PerftTestSuite.EnableDebugOutput = false;
			//PerftTestSuite.TestPosition(new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 1, 0));
			//PerftTestSuite.TestPosition(new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 2, 0));
			//PerftTestSuite.TestPosition(new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 3, 0));
			//PerftTestSuite.TestPosition(new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 4, 0));
			//PerftTestSuite.TestPosition(new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 5, 0));
			PerftTestSuite.RunTests();
			//Console.ReadLine();
		}

		private static void TestSearchCount()
		{
			var t = new SearchTest();
			t.TestSearchNodeCount();
		}
	}
}

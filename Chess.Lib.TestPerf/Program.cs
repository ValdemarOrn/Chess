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
			//PerftTestSuite.TestPosition(PerftTestSuite.Positions[1]);
			PerftTestSuite.RunTests();
			Console.ReadLine();
		}

		private static void TestSearchCount()
		{
			var t = new SearchTest();
			t.TestSearchNodeCount();
		}
	}
}

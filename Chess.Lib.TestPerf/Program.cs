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

			PerftTestSuite.RunTests();
			//BasePerftTests.RunBasePerft();

			//TestSearch();
			//SearchTest.GenericSearchTest("r1b1k2r/2q1bppp/p2ppn2/1p4P1/3NPP2/2N2Q2/PPP4P/2KR1B1R b kq", 10);

			Console.ReadLine();
		}

		private static void TestSearch()
		{
			var t = new SearchTest();
			t.TestSearches();
		}
	}
}

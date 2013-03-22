using Chess.Lib.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.TestPerf
{
	unsafe class Program
	{
		static Program()
		{
			Manager.InitLibrary();
		}

		static void Main(string[] args)
		{
			PerftTestSuite.RunTests();
		}

		private static void TestSearch()
		{
			var t = new SearchTest();
			t.TestSearches();
		}
	}
}

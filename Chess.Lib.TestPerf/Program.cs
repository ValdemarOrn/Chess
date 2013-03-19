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

			var t = new SearchTest();
			t.TestSearches();
		}

		private static void TestSearchCount()
		{
			var t = new SearchTest();
			t.TestSearch6();
		}
	}
}

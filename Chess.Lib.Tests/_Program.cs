using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("Chess.Lib.TestPerf")]

namespace Chess.Lib.Tests
{
	class Program
	{
		public static void Main(string[] args)
		{
			Manager.InitLibrary();

			var t = new SearchTest();
			t.TestSearches();
		}
	}
}

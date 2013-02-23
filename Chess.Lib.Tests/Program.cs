using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.Tests
{
	class Program
	{
		public static void Main()
		{
			Manager.InitLibrary();
			var t = new BoardTests();
			t.TestMakeUnmakePromotion();
		}
	}
}

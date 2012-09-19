using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Bitboard.Tests
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var tt = new BitboardTests();
			tt.TestForwardBit();
		}
	}
}

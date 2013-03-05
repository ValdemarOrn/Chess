using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class OpeningBookTest
	{
		[TestMethod]
		public void TestDecompress()
		{
			new OpeningBook().Load(@"c:\openingBook.txt.gz");
		}

		[TestMethod]
		public void TestLookup()
		{
			var book = new OpeningBook();
			book.Load(@"c:\openingBook.txt.gz");
			var moves = book.FindMoves();
			var move = book.SelectMove(moves, Board.COLOR_WHITE);
		}
	}
}

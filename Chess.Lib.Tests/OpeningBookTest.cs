using System;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public class OpeningBookTest
	{
		[Test]
		public void TestDecompress()
		{
			new OpeningBook().Load(@"c:\openingBook.txt.gz");
		}

		[Test]
		public void TestLookup()
		{
			var book = new OpeningBook();
			book.Load(@"c:\openingBook.txt.gz");
			var moves = book.FindMoves();
			var move = book.SelectMove(moves, Board.COLOR_WHITE);
		}
	}
}

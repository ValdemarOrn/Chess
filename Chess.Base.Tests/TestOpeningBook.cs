using Chess.Base.PGN;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestOpeningBook
	{
		static List<string> Book;

		[Test]
		public void TestGenerate()
		{
			var files = new List<string>()
			{
				"..\\..\\..\\TestData\\annotatedsetone.pgn",
				"..\\..\\..\\TestData\\BobbyFischer.pgn",
				"..\\..\\..\\TestData\\HumansVsComputers.pgn",
				"..\\..\\..\\TestData\\perle.pgn"
			};

			// just a smoke test
			var lines = OpeningBook.CompileBook(files, 30);
			Book = lines;
			Assert.IsTrue(lines.Count > 10);
			Assert.IsTrue(lines.All(x => x.Length >= 50 && x.Length <= 70));
		}

		[Test]
		public void Test()
		{
			TestGenerate();
			var book = new OpeningBook(Book);
			var moves = book.GetAvailableMoves("Lb");
			var move = book.SelectMove(moves, Color.Black, new MoveFilter()
			{
				ImportanceOfGameCount = 10,
				ImportantOfWinPercentage = 0,
				MaxBlackWinPercent = 100,
				MaxWhiteWinPercent = 100,
				MinBlackWinPercent = 0,
				MinNumberOfGames = 1,
				MinWhiteWinPercent = 0
			});
		}
	}
}

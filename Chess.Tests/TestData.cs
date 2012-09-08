using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestData
	{
		[TestMethod]
		public void TestTextToTile()
		{
			string a = "E1";
			string b = "d5";
			string c = "h8";

			string X = "A0";
			string XX = "A9";
			string XXX = "A12";
			string XXXX = "K4";

			int tile = 0;

			tile = Data.TextToTile(a);
			Assert.AreEqual(4, tile);

			tile = Data.TextToTile(b);
			Assert.AreEqual(35, tile);

			tile = Data.TextToTile(c);
			Assert.AreEqual(63, tile);

			bool exception = false;
			try
			{
				tile = Data.TextToTile(X);
			}
			catch (Exception)
			{
				exception = true;
			}
			Assert.IsTrue(exception);

			try
			{
				tile = Data.TextToTile(XX);
			}
			catch (Exception)
			{
				exception = true;
			}
			Assert.IsTrue(exception);

			try
			{
				tile = Data.TextToTile(XXX);
			}
			catch (Exception)
			{
				exception = true;
			}
			Assert.IsTrue(exception);

			try
			{
				tile = Data.TextToTile(XXXX);
			}
			catch (Exception)
			{
				exception = true;
			}
			Assert.IsTrue(exception);
		}

		[TestMethod]
		public void TestTileToText()
		{
			Assert.AreEqual("e1", Data.TileToText(4));
			Assert.AreEqual("d5", Data.TileToText(35));
			Assert.AreEqual("h8", Data.TileToText(63));
		}

		[TestMethod]
		public void TestFEN1()
		{ 
			var b1 = new Board(true);

			var str = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq";
			var b2 = Data.FENtoBoard(str);
			
			bool equal = b1.State.SequenceEqual(b2.State);
			Assert.IsTrue(equal);
		}

		[TestMethod]
		public void TestFENCastle1()
		{
			var b1 = new Board(true);

			var str = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq";
			var b2 = Data.FENtoBoard(str);

			Assert.IsTrue(b2.CastleQueensideWhite);
			Assert.IsTrue(b2.CastleKingsideWhite);

			Assert.IsTrue(b2.CastleQueensideBlack);
			Assert.IsTrue(b2.CastleKingsideBlack);
		}

		[TestMethod]
		public void TestFENCastle2()
		{
			var b1 = new Board(true);

			var str = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w Kq";
			var b2 = Data.FENtoBoard(str);

			Assert.IsFalse(b2.CastleQueensideWhite);
			Assert.IsTrue(b2.CastleKingsideWhite);

			Assert.IsTrue(b2.CastleQueensideBlack);
			Assert.IsFalse(b2.CastleKingsideBlack);
		}

		[TestMethod]
		public void TestFENCastleNone()
		{
			var b1 = new Board(true);

			var str = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w -";
			var b2 = Data.FENtoBoard(str);

			Assert.IsFalse(b2.CastleQueensideWhite);
			Assert.IsFalse(b2.CastleKingsideWhite);

			Assert.IsFalse(b2.CastleQueensideBlack);
			Assert.IsFalse(b2.CastleKingsideBlack);
		}

		[TestMethod]
		public void TestFENTurnWhite()
		{
			var b1 = new Board(true);

			var str = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR W Kq";
			var b2 = Data.FENtoBoard(str);

			Assert.AreEqual(Colors.White, b2.Turn);
		}

		[TestMethod]
		public void TestFENTurnBlack()
		{
			var b1 = new Board(true);

			var str = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b Kq";
			var b2 = Data.FENtoBoard(str);

			Assert.AreEqual(Colors.Black, b2.Turn);
		}
	}
}

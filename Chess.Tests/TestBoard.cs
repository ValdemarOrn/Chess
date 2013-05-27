using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestBoard
	{
		[TestMethod]
		public void TestFindByColor()
		{
			var b = new Board(true);


			var c = b.FindByColor(Colors.White);
			Assert.AreEqual(16, c.Count);

			// all white are in the first 2 rows
			Assert.IsFalse(c.Any(x => x >= 16));


			c = b.FindByColor(Colors.Black);
			Assert.AreEqual(16, c.Count);

			// all black are in the upper 2 rows
			Assert.IsFalse(c.Any(x => x < 48));
		}

		[TestMethod]
		public void TestFindByPiece()
		{
			var b = new Board(true);

			var c = b.FindByPiece(Pieces.Pawn);
			Assert.AreEqual(16, c.Count);

			c = b.FindByPiece(Pieces.Rook);
			Assert.AreEqual(4, c.Count);
			Assert.IsTrue(c.Contains(0));
			Assert.IsTrue(c.Contains(7));
			Assert.IsTrue(c.Contains(56));
			Assert.IsTrue(c.Contains(63));
		}

		[TestMethod]
		public void TestFindByPieceAndColor()
		{
			var b = new Board(true);

			var c = b.FindByPieceAndColor(Pieces.Pawn | Colors.White);
			Assert.AreEqual(8, c.Count);

			c = b.FindByPieceAndColor(Pieces.Bishop | Colors.Black);
			Assert.AreEqual(2, c.Count);
			Assert.IsTrue(c.Contains(58));
			Assert.IsTrue(c.Contains(61));

			c = b.FindByPieceAndColor(Pieces.Queen | Colors.White);
			Assert.AreEqual(1, c.Count);
			Assert.IsTrue(c.Contains(3));
		}

		[TestMethod]
		public void TestKingLocation()
		{
			var b = new Board(false);
			b.State[16] = Pieces.King | Colors.White;
			b.State[53] = Pieces.King | Colors.Black;

			Assert.AreEqual(16, b.KingLocation(Colors.White));
			Assert.AreEqual(53, b.KingLocation(Colors.Black));
		}

		[TestMethod]
		public void TestCastlingNoPieces()
		{
			var b = new Board(false);
			b.AllowCastlingAll(); // Can't do this without any pieces on the board
			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);
		}

		[TestMethod]
		public void TestCastlingOK()
		{
			var b = new Board(false);
			b.State[0] = Pieces.Rook | Colors.White;
			b.State[7] = Pieces.Rook | Colors.White;
			b.State[4] = Pieces.King | Colors.White;

			b.State[56] = Pieces.Rook | Colors.Black;
			b.State[63] = Pieces.Rook | Colors.Black;
			b.State[60] = Pieces.King | Colors.Black;

			b.AllowCastlingAll();
			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}

		[TestMethod]
		public void TestCastlingNoQueensideWhite()
		{
			var b = new Board(false);
			b.State[0] = Pieces.Rook | Colors.White;
			b.State[7] = Pieces.Rook | Colors.White;
			b.State[4] = Pieces.King | Colors.White;

			b.State[56] = Pieces.Rook | Colors.Black;
			b.State[63] = Pieces.Rook | Colors.Black;
			b.State[60] = Pieces.King | Colors.Black;

			b.AllowCastlingAll();

			b.PlayerTurn = Colors.White;
			b.Move(0, 1);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Colors.White;
			b.Move(1, 0);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);
		}

		[TestMethod]
		public void TestCastlingNoKingsideWhite()
		{
			var b = new Board(false);
			b.State[0] = Pieces.Rook | Colors.White;
			b.State[7] = Pieces.Rook | Colors.White;
			b.State[4] = Pieces.King | Colors.White;

			b.State[56] = Pieces.Rook | Colors.Black;
			b.State[63] = Pieces.Rook | Colors.Black;
			b.State[60] = Pieces.King | Colors.Black;

			b.AllowCastlingAll();

			b.PlayerTurn = Colors.White;
			b.Move(7, 6);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Colors.White;
			b.Move(6, 7);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}


		[TestMethod]
		public void TestCastlingNoQueensideBlack()
		{
			var b = new Board(false);
			b.State[0] = Pieces.Rook | Colors.White;
			b.State[7] = Pieces.Rook | Colors.White;
			b.State[4] = Pieces.King | Colors.White;

			b.State[56] = Pieces.Rook | Colors.Black;
			b.State[63] = Pieces.Rook | Colors.Black;
			b.State[60] = Pieces.King | Colors.Black;

			b.AllowCastlingAll();

			b.PlayerTurn = Colors.Black;
			b.Move(56, 57);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Colors.Black;
			b.Move(57, 56);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}

		[TestMethod]
		public void TestCastlingNoKingsideBlack()
		{
			var b = new Board(false);
			b.State[0] = Pieces.Rook | Colors.White;
			b.State[7] = Pieces.Rook | Colors.White;
			b.State[4] = Pieces.King | Colors.White;

			b.State[56] = Pieces.Rook | Colors.Black;
			b.State[63] = Pieces.Rook | Colors.Black;
			b.State[60] = Pieces.King | Colors.Black;

			b.AllowCastlingAll();

			b.PlayerTurn = Colors.Black;
			b.Move(63, 62);

			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Colors.Black;
			b.Move(62, 63);

			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}

		[TestMethod]
		public void TestPromoteOK1()
		{
			//Todo: Test Promote()
		}

		[TestMethod]
		public void TestPromoteOK2()
		{
			
		}

		[TestMethod]
		public void TestPromoteFail()
		{
			
		}

		[TestMethod]
		public void TestPromoteIllegalPiece()
		{
			
		}

		[TestMethod]
		public void TestMove1()
		{
			//Todo: Test Move by ref()
		}
	}
}

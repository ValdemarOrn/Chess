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


			var c = b.FindByColor(Color.White);
			Assert.AreEqual(16, c.Count);

			// all white are in the first 2 rows
			Assert.IsFalse(c.Any(x => x >= 16));


			c = b.FindByColor(Color.Black);
			Assert.AreEqual(16, c.Count);

			// all black are in the upper 2 rows
			Assert.IsFalse(c.Any(x => x < 48));
		}

		[TestMethod]
		public void TestFindByPiece()
		{
			var b = new Board(true);

			var c = b.FindByPiece(Piece.Pawn);
			Assert.AreEqual(16, c.Count);

			c = b.FindByPiece(Piece.Rook);
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

			var c = b.FindByPieceAndColor(Piece.Pawn, Color.White);
			Assert.AreEqual(8, c.Count);

			c = b.FindByPieceAndColor(Piece.Bishop, Color.Black);
			Assert.AreEqual(2, c.Count);
			Assert.IsTrue(c.Contains(58));
			Assert.IsTrue(c.Contains(61));

			c = b.FindByPieceAndColor(Piece.Queen, Color.White);
			Assert.AreEqual(1, c.Count);
			Assert.IsTrue(c.Contains(3));
		}

		[TestMethod]
		public void TestKingLocation()
		{
			var b = new Board(false);
			b.State[16] = Colors.Val(Piece.King, Color.White);
			b.State[53] = Colors.Val(Piece.King, Color.Black);

			Assert.AreEqual(16, b.KingLocation(Color.White));
			Assert.AreEqual(53, b.KingLocation(Color.Black));
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
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

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
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.White;
			b.Move(0, 1);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.White;
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
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.White;
			b.Move(7, 6);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.White;
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
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.Black;
			b.Move(56, 57);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.Black;
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
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.Black;
			b.Move(63, 62);

			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.Black;
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

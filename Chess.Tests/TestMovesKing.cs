using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestMovesKing
	{
		[TestMethod]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.King, Color.White);

			var moves = Moves.GetMoves(b, pos);

			Assert.AreEqual(8, moves.Length);

			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 9));

			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos - 1));

			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		[TestMethod]
		public void TestSameColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.King, Color.White);

			b.State[pos + 7] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 8] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 9] = Colors.Val(Piece.Pawn, Color.White);

			b.State[pos + 1] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 1] = Colors.Val(Piece.Pawn, Color.White);

			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 8] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.White);
			
			var moves = Moves.GetMoves(b, pos);

			Assert.AreEqual(0, moves.Length);
		}

		[TestMethod]
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.King, Color.White);

			b.State[pos + 7] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 8] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 9] = Colors.Val(Piece.Pawn, Color.Black);

			b.State[pos + 1] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 1] = Colors.Val(Piece.Pawn, Color.Black);

			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 8] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.Black);

			var moves = Moves.GetMoves(b, pos);

			Assert.AreEqual(8, moves.Length);

			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 9));

			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos - 1));

			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		// ------------------------------------

		[TestMethod]
		public void TestCastleWhiteKingsideOK()
		{
			var b = new Board();
			b.InitBoard();
			b.State[5] = 0;
			b.State[6] = 0;
			Assert.IsTrue(b.CanCastleKWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(5));
			Assert.IsTrue(moves.Contains(6));
		}

		[TestMethod]
		public void TestCastleWhiteKingsideBlocked()
		{
			var b = new Board();
			b.InitBoard();
			b.State[5] = 0;
			Assert.IsTrue(b.CanCastleKWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(5));
		}

		[TestMethod]
		public void TestCastleWhiteKingsideRookMoved()
		{
			var b = new Board();
			b.InitBoard();
			b.State[5] = 0;
			b.State[6] = 0;

			bool ok = b.Move(7, 6);
			Assert.IsTrue(ok);
			b.PlayerTurn = Color.White;
			ok = b.Move(6, 7);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CanCastleKWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(5));
		}


		// ---------------------------

		[TestMethod]
		public void TestCastleWhiteQueensideOK()
		{
			var b = new Board();
			b.InitBoard();
			b.State[1] = 0;
			b.State[2] = 0;
			b.State[3] = 0;
			Assert.IsTrue(b.CanCastleQWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(3));
			Assert.IsTrue(moves.Contains(2));
		}

		[TestMethod]
		public void TestCastleWhiteQueensideBlocked()
		{
			var b = new Board();
			b.InitBoard();
			b.State[3] = 0;
			b.State[1] = 0;
			Assert.IsTrue(b.CanCastleQWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(3));
		}

		[TestMethod]
		public void TestCastleWhiteQueensideRookMoved()
		{
			var b = new Board();
			b.InitBoard();
			b.State[3] = 0;
			b.State[2] = 0;
			b.State[1] = 0;
			bool ok = b.Move(0, 1);
			Assert.IsTrue(ok);
			b.PlayerTurn = Color.White;
			ok = b.Move(1, 0);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CanCastleQWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(3));
		}

		// ------------------------------------

		[TestMethod]
		public void TestCastleBlackKingsideOK()
		{
			var b = new Board();
			b.InitBoard();
			b.State[61] = 0;
			b.State[62] = 0;
			Assert.IsTrue(b.CanCastleKBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(61));
			Assert.IsTrue(moves.Contains(62));
		}

		[TestMethod]
		public void TestCastleBlackKingsideBlocked()
		{
			var b = new Board();
			b.InitBoard();
			b.State[61] = 0;
			Assert.IsTrue(b.CanCastleKBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(61));
		}

		[TestMethod]
		public void TestCastleBlackKingsideRookMoved()
		{
			var b = new Board();
			b.InitBoard();
			b.State[61] = 0;
			b.State[62] = 0;
			b.PlayerTurn = Color.Black;
			bool ok = b.Move(63, 62);
			Assert.IsTrue(ok);
			b.PlayerTurn = Color.Black;
			b.Move(62, 63);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CanCastleKBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(61));
		}


		// ---------------------------

		[TestMethod]
		public void TestCastleBlackQueensideOK()
		{
			var b = new Board();
			b.InitBoard();
			b.State[59] = 0;
			b.State[58] = 0;
			b.State[57] = 0;
			Assert.IsTrue(b.CanCastleQBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(59));
			Assert.IsTrue(moves.Contains(58));
		}

		[TestMethod]
		public void TestCastleBlackQueensideBlocked()
		{
			var b = new Board();
			b.InitBoard();
			b.State[59] = 0;
			b.State[57] = 0;
			Assert.IsTrue(b.CanCastleQBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(59));
		}

		[TestMethod]
		public void TestCastleBlackQueensideRookMoved()
		{
			var b = new Board();
			b.InitBoard();
			b.State[59] = 0;
			b.State[58] = 0;
			b.State[57] = 0;
			b.PlayerTurn = Color.Black;
			bool ok = b.Move(56, 57);
			Assert.IsTrue(ok);
			b.PlayerTurn = Color.Black;
			ok = b.Move(57, 58);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CanCastleQBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(59));
		}
	}
}

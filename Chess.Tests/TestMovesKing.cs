using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
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
			b.State[pos] = Pieces.King | Chess.Colors.White;

			var moves = Moves.GetMoves(b, pos);

			Assert.AreEqual(8, moves.Count);

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
			b.State[pos] = Pieces.King | Chess.Colors.White;

			b.State[pos + 7] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 8] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 9] = Pieces.Pawn | Chess.Colors.White;

			b.State[pos + 1] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 1] = Pieces.Pawn | Chess.Colors.White;

			b.State[pos - 7] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 8] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 9] = Pieces.Pawn | Chess.Colors.White;
			
			var moves = Moves.GetMoves(b, pos);

			Assert.AreEqual(0, moves.Count);
		}

		[TestMethod]
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.King | Chess.Colors.White;

			b.State[pos + 7] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos + 8] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos + 9] = Pieces.Pawn | Chess.Colors.Black;

			b.State[pos + 1] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 1] = Pieces.Pawn | Chess.Colors.Black;

			b.State[pos - 7] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 8] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 9] = Pieces.Pawn | Chess.Colors.Black;

			var moves = Moves.GetMoves(b, pos);

			Assert.AreEqual(8, moves.Count);

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
			Assert.IsTrue(b.CastleKingsideWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(5));
			Assert.IsTrue(moves.Contains(6));
		}

		[TestMethod]
		public void TestCastleWhiteKingsideBlocked()
		{
			var b = new Board();
			b.InitBoard();
			b.State[5] = 0;
			Assert.IsTrue(b.CastleKingsideWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Count);
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
			b.PlayerTurn = Colors.White;
			ok = b.Move(6, 7);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CastleKingsideWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Count);
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
			Assert.IsTrue(b.CastleQueensideWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(2, moves.Count);
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
			Assert.IsTrue(b.CastleQueensideWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Count);
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
			b.PlayerTurn = Colors.White;
			ok = b.Move(1, 0);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CastleQueensideWhite);

			var moves = Moves.GetValidMoves(b, 4);
			Assert.AreEqual(1, moves.Count);
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
			Assert.IsTrue(b.CastleKingsideBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(61));
			Assert.IsTrue(moves.Contains(62));
		}

		[TestMethod]
		public void TestCastleBlackKingsideBlocked()
		{
			var b = new Board();
			b.InitBoard();
			b.State[61] = 0;
			Assert.IsTrue(b.CastleKingsideBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(61));
		}

		[TestMethod]
		public void TestCastleBlackKingsideRookMoved()
		{
			var b = new Board();
			b.InitBoard();
			b.State[61] = 0;
			b.State[62] = 0;
			b.PlayerTurn = Colors.Black;
			bool ok = b.Move(63, 62);
			Assert.IsTrue(ok);
			b.PlayerTurn = Colors.Black;
			b.Move(62, 63);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CastleKingsideBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Count);
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
			Assert.IsTrue(b.CastleQueensideBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(2, moves.Count);
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
			Assert.IsTrue(b.CastleQueensideBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Count);
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
			b.PlayerTurn = Colors.Black;
			bool ok = b.Move(56, 57);
			Assert.IsTrue(ok);
			b.PlayerTurn = Colors.Black;
			ok = b.Move(57, 58);
			Assert.IsTrue(ok);

			Assert.IsFalse(b.CastleQueensideBlack);

			var moves = Moves.GetValidMoves(b, 60);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(59));
		}
	}
}

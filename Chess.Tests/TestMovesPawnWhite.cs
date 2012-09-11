using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestMovesPawnWhite
	{
		[TestMethod]
		public void Test1_1()
		{
			// test free space, starting rank
			var b = new Board();
			b.State[12] = Pieces.Pawn | Chess.Colors.White;
			var moves = Moves.GetMoves(b, 12);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(12 + 8));
			Assert.IsTrue(moves.Contains(12 + 16));
		}

		[TestMethod]
		public void Test1_2()
		{
			// test free space, piece has moved forward
			var b = new Board();
			b.State[20] = Pieces.Pawn | Chess.Colors.White;
			var moves = Moves.GetMoves(b, 20);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(28));
		}

		[TestMethod]
		public void Test2()
		{
			// Test edge of board
			var b = new Board();
			byte pos = 6 * 8 + 2;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 8));
		}

		[TestMethod]
		public void Test3()
		{
			// Test obstacle far, same color
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 16] = Pieces.Pawn | Chess.Colors.White;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 8));
		}

		[TestMethod]
		public void Test4()
		{
			// Test obstacle far, opposite color
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 16] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 8));
		}

		[TestMethod]
		public void Test5()
		{
			// Test obstacle near, same color
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 8] = Pieces.Pawn | Chess.Colors.White;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(0, moves.Count);
		}

		[TestMethod]
		public void Test6()
		{
			// Test obstacle near, opposite color
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 8] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(0, moves.Count);
		}

		[TestMethod]
		public void TestCaptureLeft()
		{
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 7] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(3, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 16));
		}

		[TestMethod]
		public void TestCaptureRight()
		{
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 9] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(3, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 9));
			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 16));
		}

		[TestMethod]
		public void TestCaptureSameColor()
		{
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 7] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 9] = Pieces.Pawn | Chess.Colors.White;
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(2, moves.Count);
		}

		[TestMethod]
		public void TestEnPassantLeft()
		{
			var b = new Board();
			b.PlayerTurn = Colors.Black;

			int posWhite = Notation.TextToTile("e5");
			int posBlack = Notation.TextToTile("d7");

			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;
			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;

			b.Move(posBlack, posBlack - 16);

			var moves = Moves.GetMoves(b, posWhite);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("d6")));
		}

		[TestMethod]
		public void TestEnPassantRight()
		{
			var b = new Board();
			b.PlayerTurn = Colors.Black;

			int posWhite = Notation.TextToTile("e5");
			int posBlack = Notation.TextToTile("f7");

			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;
			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;

			b.Move(posBlack, posBlack - 16);

			var moves = Moves.GetMoves(b, posWhite);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("f6")));
		}
	}
}

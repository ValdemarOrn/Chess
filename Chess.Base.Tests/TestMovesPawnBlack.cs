using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestMovesPawnBlack
	{
		[Test]
		public void Test1_1()
		{
			// test free space
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 16));
		}

		[Test]
		public void Test1_2()
		{
			// test free space
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 5 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 8));
		}

		[Test]
		public void Test2()
		{
			// Test edge of board
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 8));
		}

		[Test]
		public void Test3()
		{
			// Test obstacle far, same color
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 16] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 8));
		}

		[Test]
		public void Test4()
		{
			// Test obstacle far, opposite color
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 16] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 8));
		}

		[Test]
		public void Test5()
		{
			// Test obstacle near, same color
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 8] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(0, moves.Length);
		}

		[Test]
		public void Test6()
		{
			// Test obstacle near, opposite color
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 8] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(0, moves.Length);
		}

		[Test]
		public void TestCaptureLeft()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(3, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 9));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 16));
		}

		[Test]
		public void TestCaptureRight()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(3, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 16));
		}

		[Test]
		public void TestCaptureSameColor()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(2, moves.Length);
		}

		[Test]
		public void TestEnPassantLeft()
		{
			var b = new Board();
			b.PlayerTurn = Color.White;

			int posBlack = Notation.TextToTile("e4");
			int posWhite = Notation.TextToTile("d2");

			b.State[posBlack] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[posWhite] = Colors.Val(Piece.Pawn, Color.White);

			b.Move(posWhite, posWhite + 16);

			var moves = Moves.GetMoves(b, posBlack);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("d3")));
		}

		[Test]
		public void TestEnPassantRight()
		{
			var b = new Board();
			b.PlayerTurn = Color.White;

			int posBlack = Notation.TextToTile("e4");
			int posWhite = Notation.TextToTile("f2");

			b.State[posBlack] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[posWhite] = Colors.Val(Piece.Pawn, Color.White);

			b.Move(posWhite, posWhite + 16);

			var moves = Moves.GetMoves(b, posBlack);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("f3")));
		}
	}
}

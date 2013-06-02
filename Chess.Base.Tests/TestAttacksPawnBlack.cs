using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestAttacksPawnBlack
	{
		[Test]
		public void Test1()
		{
			// test free space
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		[Test]
		public void Test2()
		{
			// Test left edge of board
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 24;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 7));

			// test right edge of board
			b = new Board();
			b.PlayerTurn = Color.Black;
			pos = 24 + 7;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		[Test]
		public void TestCaptureLeft()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 9));
			Assert.IsTrue(moves.Contains(pos - 7));
		}

		[Test]
		public void TestCaptureRight()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 9));
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
			var moves = Attacks.GetAttacks(b, pos);
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

			var moves = Attacks.GetAttacks(b, posBlack);
			Assert.AreEqual(3, moves.Length);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("d4")));
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

			var moves = Attacks.GetAttacks(b, posBlack);
			Assert.AreEqual(3, moves.Length);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("f4")));
		}
	}
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestAttacksPawnBlack
	{
		[TestMethod]
		public void Test1()
		{
			// test free space
			var b = new Board();
			b.PlayerTurn = Colors.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		[TestMethod]
		public void Test2()
		{
			// Test left edge of board
			var b = new Board();
			b.PlayerTurn = Colors.Black;
			int pos = 24;
			b.State[pos] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(pos - 7));

			// test right edge of board
			b = new Board();
			b.PlayerTurn = Colors.Black;
			pos = 24 + 7;
			b.State[pos] = Pieces.Pawn | Chess.Colors.Black;
			moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(1, moves.Count);
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		[TestMethod]
		public void TestCaptureLeft()
		{
			var b = new Board();
			b.PlayerTurn = Colors.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 9] = Pieces.Pawn | Chess.Colors.White;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(pos - 9));
			Assert.IsTrue(moves.Contains(pos - 7));
		}

		[TestMethod]
		public void TestCaptureRight()
		{
			var b = new Board();
			b.PlayerTurn = Colors.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 7] = Pieces.Pawn | Chess.Colors.White;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Count);
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 9));
		}

		[TestMethod]
		public void TestCaptureSameColor()
		{
			var b = new Board();
			b.PlayerTurn = Colors.Black;
			int pos = 6 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 7] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 9] = Pieces.Pawn | Chess.Colors.Black;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Count);
		}

		[TestMethod]
		public void TestEnPassantLeft()
		{
			var b = new Board();
			b.PlayerTurn = Colors.White;

			int posBlack = Notation.TextToTile("e4");
			int posWhite = Notation.TextToTile("d2");

			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;
			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;

			b.Move(posWhite, posWhite + 16);

			var moves = Attacks.GetAttacks(b, posBlack);
			Assert.AreEqual(3, moves.Count);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("d4")));
		}

		[TestMethod]
		public void TestEnPassantRight()
		{
			var b = new Board();
			b.PlayerTurn = Colors.White;

			int posBlack = Notation.TextToTile("e4");
			int posWhite = Notation.TextToTile("f2");

			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;
			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;

			b.Move(posWhite, posWhite + 16);

			var moves = Attacks.GetAttacks(b, posBlack);
			Assert.AreEqual(3, moves.Count);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("f4")));
		}
	}
}

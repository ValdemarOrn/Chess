using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestAttacksPawnWhite
	{
		[TestMethod]
		public void Test1()
		{
			// test free space
			var b = new Board();
			b.State[12] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, 12);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(12 + 7));
			Assert.IsTrue(moves.Contains(12 + 9));
		}

		[TestMethod]
		public void Test2()
		{
			// Test left edge of board
			var b = new Board();
			byte pos = 3 * 8;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos + 9));

			// Test right edge of board
			b = new Board();
			pos = 3 * 8 + 7;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(1, moves.Length);
			Assert.IsTrue(moves.Contains(pos + 7));
		}

		[TestMethod]
		public void TestCaptureLeft()
		{
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 7] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 9));
		}

		[TestMethod]
		public void TestCaptureRight()
		{
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 9] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains(pos + 9));
			Assert.IsTrue(moves.Contains(pos + 7));
		}

		[TestMethod]
		public void TestCaptureSameColor()
		{
			var b = new Board();
			byte pos = 12;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 7] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 9] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
		}

		[TestMethod]
		public void TestEnPassantLeft()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;

			int posWhite = Notation.TextToTile("e5");
			int posBlack = Notation.TextToTile("d7");

			b.State[posWhite] = Colors.Val(Piece.Pawn, Color.White);
			b.State[posBlack] = Colors.Val(Piece.Pawn, Color.Black);

			b.Move(posBlack, posBlack - 16);

			var moves = Attacks.GetAttacks(b, posWhite);
			Assert.AreEqual(3, moves.Length);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("d5")));
		}

		[TestMethod]
		public void TestEnPassantRight()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;

			int posWhite = Notation.TextToTile("e5");
			int posBlack = Notation.TextToTile("f7");

			b.State[posWhite] = Colors.Val(Piece.Pawn, Color.White);
			b.State[posBlack] = Colors.Val(Piece.Pawn, Color.Black);

			b.Move(posBlack, posBlack - 16);

			var moves = Attacks.GetAttacks(b, posWhite);
			Assert.AreEqual(3, moves.Length);
			Assert.IsTrue(moves.Contains(Notation.TextToTile("f5")));
		}
	}
}

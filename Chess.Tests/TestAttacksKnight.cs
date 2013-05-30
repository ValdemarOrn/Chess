using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestAttacksKnight
	{
		[TestMethod]
		public void TestKnight1()
		{
			// test free space
			var b = new Board();
			byte pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos + 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
		}

		[TestMethod]
		public void TestKnightCapture()
		{
			var b = new Board();
			byte pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			b.State[pos - 16 + 1] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos + 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
		}

		[TestMethod]
		public void TestKnightCaptureSameColor()
		{
			var b = new Board();
			byte pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			b.State[pos - 16 + 1] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos + 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
		}

		[TestMethod]
		public void TestKnightNoMoves()
		{
			var b = new Board();
			byte pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			b.State[pos + 16 - 1] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 16 + 1] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 8 - 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 8 + 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 8 - 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 8 + 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 16 - 1] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 16 + 1] = Colors.Val(Piece.Pawn, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
		}

		[TestMethod]
		public void TestKnightKillAll()
		{
			var b = new Board();
			byte pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			b.State[pos + 16 - 1] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 16 + 1] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 8 - 2] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 8 + 2] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 8 - 2] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 8 + 2] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 16 - 1] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 16 + 1] = Colors.Val(Piece.Pawn, Color.Black);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
		}

		[TestMethod]
		public void TestKnightCorner1()
		{
			// upper left
			var b = new Board();
			byte pos = 7 * 8;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
		}

		[TestMethod]
		public void TestKnightCorner2()
		{
			// 7 + 1
			var b = new Board();
			byte pos = 6 * 8 + 1;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(4, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
			Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
			Assert.IsTrue(moves.Contains((byte)(pos + 10)));
			Assert.IsTrue(moves.Contains((byte)(pos - 17)));
		}

		[TestMethod]
		public void TestKnightCorner3()
		{
			// lower right
			var b = new Board();
			byte pos = 7;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(2, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
		}

		[TestMethod]
		public void TestKnightCorner4()
		{
			// 1 + 6
			var b = new Board();
			byte pos = 8 + 6;
			b.State[pos] = Colors.Val(Piece.Knight, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(4, moves.Length);
			Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
			Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
			Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
		}
	}
}

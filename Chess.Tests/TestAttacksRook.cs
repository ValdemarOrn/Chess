using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestAttacksRook
	{
		[TestMethod]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.Rook | Chess.Colors.White;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(14, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos + 2));
			Assert.IsTrue(moves.Contains(pos + 3));
			Assert.IsTrue(moves.Contains(pos - 1));
			Assert.IsTrue(moves.Contains(pos - 2));
			Assert.IsTrue(moves.Contains(pos - 3));
			Assert.IsTrue(moves.Contains(pos - 4));

			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 16));
			Assert.IsTrue(moves.Contains(pos + 24));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 16));
			Assert.IsTrue(moves.Contains(pos - 24));
			Assert.IsTrue(moves.Contains(pos - 32));
		}

		[TestMethod]
		public void TestSameColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.Rook | Chess.Colors.White;
			b.State[pos + 2] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 2] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 16] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 16] = Pieces.Pawn | Chess.Colors.White;

			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos - 1));
			Assert.IsTrue(moves.Contains(pos + 2));
			Assert.IsTrue(moves.Contains(pos - 2));

			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos + 16));
			Assert.IsTrue(moves.Contains(pos - 16));
		}

		[TestMethod]
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.Rook | Chess.Colors.White;
			b.State[pos + 2] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 2] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos + 16] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 16] = Pieces.Pawn | Chess.Colors.Black;

			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Count);
			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos - 1));
			Assert.IsTrue(moves.Contains(pos + 2));
			Assert.IsTrue(moves.Contains(pos - 2));

			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos + 16));
			Assert.IsTrue(moves.Contains(pos - 16));
		}


	}
}

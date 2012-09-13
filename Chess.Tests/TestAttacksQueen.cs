using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestAttacksQueen
	{
		[TestMethod]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.Queen | Chess.Colors.White;
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(13 + 14, moves.Length);

			// bishop moves
			Assert.IsTrue(moves.Contains(pos + 9));
			Assert.IsTrue(moves.Contains(pos + 18));
			Assert.IsTrue(moves.Contains(pos + 27));
			Assert.IsTrue(moves.Contains(pos - 9));
			Assert.IsTrue(moves.Contains(pos - 18));
			Assert.IsTrue(moves.Contains(pos - 27));
			Assert.IsTrue(moves.Contains(pos - 36));

			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 14));
			Assert.IsTrue(moves.Contains(pos + 21));
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 14));
			Assert.IsTrue(moves.Contains(pos - 21));

			// rook moves
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
			b.State[pos] = Pieces.Queen | Chess.Colors.White;
			b.State[pos + 14] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 16] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 18] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos + 2] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 2] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 14] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 16] = Pieces.Pawn | Chess.Colors.White;
			b.State[pos - 18] = Pieces.Pawn | Chess.Colors.White;

			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(16, moves.Length);

			Assert.IsTrue(moves.Contains(pos + 14));
			Assert.IsTrue(moves.Contains(pos + 16));
			Assert.IsTrue(moves.Contains(pos + 18));

			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 9));

			Assert.IsTrue(moves.Contains(pos + 2));
			Assert.IsTrue(moves.Contains(pos - 2));

			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos - 1));

			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 9));

			Assert.IsTrue(moves.Contains(pos - 14));
			Assert.IsTrue(moves.Contains(pos - 16));
			Assert.IsTrue(moves.Contains(pos - 18));
		}

		[TestMethod]
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.Queen | Chess.Colors.White;
			b.State[pos + 14] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos + 16] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos + 18] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos + 2] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 2] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 14] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 16] = Pieces.Pawn | Chess.Colors.Black;
			b.State[pos - 18] = Pieces.Pawn | Chess.Colors.Black;

			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(16, moves.Length);

			Assert.IsTrue(moves.Contains(pos + 14));
			Assert.IsTrue(moves.Contains(pos + 16));
			Assert.IsTrue(moves.Contains(pos + 18));

			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 8));
			Assert.IsTrue(moves.Contains(pos + 9));

			Assert.IsTrue(moves.Contains(pos + 2));
			Assert.IsTrue(moves.Contains(pos - 2));

			Assert.IsTrue(moves.Contains(pos + 1));
			Assert.IsTrue(moves.Contains(pos - 1));

			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 8));
			Assert.IsTrue(moves.Contains(pos - 9));

			Assert.IsTrue(moves.Contains(pos - 14));
			Assert.IsTrue(moves.Contains(pos - 16));
			Assert.IsTrue(moves.Contains(pos - 18));
		}


	}
}

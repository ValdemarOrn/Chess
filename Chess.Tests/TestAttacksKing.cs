using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestAttacksKing
	{
		[TestMethod]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.King | Colors.White;

			var moves = Attacks.GetAttacks(b, pos);

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
			b.State[pos] = Pieces.King | Colors.White;

			b.State[pos + 7] = Pieces.Pawn | Colors.White;
			b.State[pos + 8] = Pieces.Pawn | Colors.White;
			b.State[pos + 9] = Pieces.Pawn | Colors.White;

			b.State[pos + 1] = Pieces.Pawn | Colors.White;
			b.State[pos - 1] = Pieces.Pawn | Colors.White;

			b.State[pos - 7] = Pieces.Pawn | Colors.White;
			b.State[pos - 8] = Pieces.Pawn | Colors.White;
			b.State[pos - 9] = Pieces.Pawn | Colors.White;

			var moves = Attacks.GetAttacks(b, pos);

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
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Pieces.King | Colors.White;

			b.State[pos + 7] = Pieces.Pawn | Colors.Black;
			b.State[pos + 8] = Pieces.Pawn | Colors.Black;
			b.State[pos + 9] = Pieces.Pawn | Colors.Black;

			b.State[pos + 1] = Pieces.Pawn | Colors.Black;
			b.State[pos - 1] = Pieces.Pawn | Colors.Black;

			b.State[pos - 7] = Pieces.Pawn | Colors.Black;
			b.State[pos - 8] = Pieces.Pawn | Colors.Black;
			b.State[pos - 9] = Pieces.Pawn | Colors.Black;

			var moves = Attacks.GetAttacks(b, pos);

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


	}
}

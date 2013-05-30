using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
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
			b.State[pos] = Colors.Val(Pieces.Rook, Color.White);
			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(14, moves.Length);
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
			b.State[pos] = Colors.Val(Pieces.Rook, Color.White);
			b.State[pos + 2] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[pos - 2] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[pos + 16] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[pos - 16] = Colors.Val(Pieces.Pawn, Color.White);

			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
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
			b.State[pos] = Colors.Val(Pieces.Rook, Color.White);
			b.State[pos + 2] = Colors.Val(Pieces.Pawn, Color.Black);
			b.State[pos - 2] = Colors.Val(Pieces.Pawn, Color.Black);
			b.State[pos + 16] = Colors.Val(Pieces.Pawn, Color.Black);
			b.State[pos - 16] = Colors.Val(Pieces.Pawn, Color.Black);

			var moves = Attacks.GetAttacks(b, pos);
			Assert.AreEqual(8, moves.Length);
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

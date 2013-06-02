using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestAttacksKing
	{
		[Test]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.King, Color.White);

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

		[Test]
		public void TestSameColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.King, Color.White);

			b.State[pos + 7] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 8] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 9] = Colors.Val(Piece.Pawn, Color.White);

			b.State[pos + 1] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 1] = Colors.Val(Piece.Pawn, Color.White);

			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 8] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.White);

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

		[Test]
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.King, Color.White);

			b.State[pos + 7] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 8] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 9] = Colors.Val(Piece.Pawn, Color.Black);

			b.State[pos + 1] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 1] = Colors.Val(Piece.Pawn, Color.Black);

			b.State[pos - 7] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 8] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 9] = Colors.Val(Piece.Pawn, Color.Black);

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

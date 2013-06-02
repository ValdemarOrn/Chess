using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestMovesQueen
	{
		[Test]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Queen, Color.White);
			var moves = Moves.GetMoves(b, pos);
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

		[Test]
		public void TestSameColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Queen, Color.White);
			b.State[pos + 14] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 16] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 18] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos + 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 14] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 16] = Colors.Val(Piece.Pawn, Color.White);
			b.State[pos - 18] = Colors.Val(Piece.Pawn, Color.White);

			var moves = Moves.GetMoves(b, pos);
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
			b.State[pos] = Colors.Val(Piece.Queen, Color.White);
			b.State[pos + 14] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 16] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 18] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos + 2] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 2] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 14] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 16] = Colors.Val(Piece.Pawn, Color.Black);
			b.State[pos - 18] = Colors.Val(Piece.Pawn, Color.Black);

			var moves = Moves.GetMoves(b, pos);
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

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestPromote
	{
		[Test]
		public void TestPromoteWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Piece.Queen, b.GetPiece(pos));
		}

		[Test]
		public void TestPromoteBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Piece.Queen, b.GetPiece(pos));
		}

		[Test]
		public void TestPromoteNotAtEdgeWhite()
		{
			var b = new Board();
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Pawn, b.GetPiece(pos));
		}

		[Test]
		public void TestPromoteNotAtEdgeBlack()
		{
			var b = new Board();
			int pos = 1 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Pawn, b.GetPiece(pos));
		}

		[Test]
		public void TestPromoteNonPawnWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Rook, Color.White);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Rook, b.GetPiece(pos));
		}

		[Test]
		public void TestPromoteNonPawnBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Colors.Val(Piece.Bishop, Color.Black);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Bishop, b.GetPiece(pos));
		}
	}
}

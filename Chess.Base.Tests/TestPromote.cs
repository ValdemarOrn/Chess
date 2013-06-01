using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestPromote
	{
		[TestMethod]
		public void TestPromoteWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Piece.Queen, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Piece.Queen, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNotAtEdgeWhite()
		{
			var b = new Board();
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Pawn, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNotAtEdgeBlack()
		{
			var b = new Board();
			int pos = 1 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.Black);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Pawn, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNonPawnWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Colors.Val(Piece.Rook, Color.White);
			bool success = b.Promote(pos, Piece.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Piece.Rook, b.GetPiece(pos));
		}

		[TestMethod]
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

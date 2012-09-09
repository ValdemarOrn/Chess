using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestPromote
	{
		[TestMethod]
		public void TestPromoteWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Colors.White;
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Pieces.Queen, b.Piece(pos));
		}

		[TestMethod]
		public void TestPromoteBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Pieces.Pawn | Colors.Black;
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Pieces.Queen, b.Piece(pos));
		}

		[TestMethod]
		public void TestPromoteNotAtEdgeWhite()
		{
			var b = new Board();
			int pos = 6 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Colors.White;
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Pawn, b.Piece(pos));
		}

		[TestMethod]
		public void TestPromoteNotAtEdgeBlack()
		{
			var b = new Board();
			int pos = 1 * 8 + 4;
			b.State[pos] = Pieces.Pawn | Colors.Black;
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Pawn, b.Piece(pos));
		}

		[TestMethod]
		public void TestPromoteNonPawnWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Pieces.Rook | Colors.White;
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Rook, b.Piece(pos));
		}

		[TestMethod]
		public void TestPromoteNonPawnBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Pieces.Bishop | Colors.Black;
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Bishop, b.Piece(pos));
		}
	}
}

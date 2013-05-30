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
			b.State[pos] = Colors.Val(Pieces.Pawn, Color.White);
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Pieces.Queen, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Colors.Val(Pieces.Pawn, Color.Black);
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsTrue(success);
			Assert.AreEqual(Pieces.Queen, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNotAtEdgeWhite()
		{
			var b = new Board();
			int pos = 6 * 8 + 4;
			b.State[pos] = Colors.Val(Pieces.Pawn, Color.White);
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Pawn, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNotAtEdgeBlack()
		{
			var b = new Board();
			int pos = 1 * 8 + 4;
			b.State[pos] = Colors.Val(Pieces.Pawn, Color.Black);
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Pawn, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNonPawnWhite()
		{
			var b = new Board();
			int pos = 7 * 8 + 4;
			b.State[pos] = Colors.Val(Pieces.Rook, Color.White);
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Rook, b.GetPiece(pos));
		}

		[TestMethod]
		public void TestPromoteNonPawnBlack()
		{
			var b = new Board();
			int pos = 0 + 4;
			b.State[pos] = Colors.Val(Pieces.Bishop, Color.Black);
			bool success = b.Promote(pos, Pieces.Queen);

			Assert.IsFalse(success);
			Assert.AreEqual(Pieces.Bishop, b.GetPiece(pos));
		}
	}
}

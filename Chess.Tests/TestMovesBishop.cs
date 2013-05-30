using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestMovesBishop
	{
		[TestMethod]
		public void TestFree()
		{
			// test free space
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Pieces.Bishop, Color.White);
			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(13, moves.Length);

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
		}

		[TestMethod]
		public void TestCorner()
		{
			// test free space
			var b = new Board();
			int pos = 7;
			b.State[pos] = Colors.Val(Pieces.Bishop, Color.White);
			var moves = Moves.GetMoves(b, pos);
			
		}

		[TestMethod]
		public void TestSameColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Pieces.Bishop, Color.White);
			b.State[pos + 18] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[pos - 18] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[pos + 14] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[pos - 14] = Colors.Val(Pieces.Pawn, Color.White);

			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(4, moves.Length);
			Assert.IsTrue(moves.Contains(pos + 9));
			Assert.IsTrue(moves.Contains(pos + 7));

			Assert.IsTrue(moves.Contains(pos - 9));
			Assert.IsTrue(moves.Contains(pos - 7));
		}

		[TestMethod]
		public void TestOppositeColor()
		{
			var b = new Board();
			int pos = 4 * 8 + 4;
			b.State[pos] = Colors.Val(Pieces.Bishop, Color.White);
			b.State[pos + 18] = Colors.Val(Pieces.Pawn, Color.Black);
			b.State[pos - 18] = Colors.Val(Pieces.Pawn, Color.Black);
			b.State[pos + 14] = Colors.Val(Pieces.Pawn, Color.Black);
			b.State[pos - 14] = Colors.Val(Pieces.Pawn, Color.Black);

			var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(8, moves.Length);
			Assert.IsTrue(moves.Contains(pos + 9));
			Assert.IsTrue(moves.Contains(pos + 7));
			Assert.IsTrue(moves.Contains(pos + 18));
			Assert.IsTrue(moves.Contains(pos + 14));

			Assert.IsTrue(moves.Contains(pos - 9));
			Assert.IsTrue(moves.Contains(pos - 7));
			Assert.IsTrue(moves.Contains(pos - 18));
			Assert.IsTrue(moves.Contains(pos - 14));
		}


	}
}

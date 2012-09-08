using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestValidMove
	{
		[TestMethod]
		public void TestMoveCausesCheck()
		{
			var b = new Board();
			b.Turn = Colors.Black;
			int pos = 5*8 + 4;

			b.State[7*8 + 4] = Pieces.King | Colors.Black;
			b.State[pos] = Pieces.Rook | Colors.Black;

			b.State[0*8 + 4] = Pieces.Queen| Colors.White;
			b.State[7] = Pieces.King | Colors.White;

			bool check = Check.IsChecked(b, Colors.Black);
			Assert.IsFalse(check);

			bool causesCheck;

			causesCheck = Check.MoveSelfChecks(b, pos, pos + 8);
			Assert.IsFalse(causesCheck);

			causesCheck = Check.MoveSelfChecks(b, pos, pos -8);
			Assert.IsFalse(causesCheck);

			causesCheck = Check.MoveSelfChecks(b, pos, pos - 1);
			Assert.IsTrue(causesCheck);

			causesCheck = Check.MoveSelfChecks(b, pos, pos - 3);
			Assert.IsTrue(causesCheck);
		}

		[TestMethod]
		public void TestValidMoves()
		{
			var b = new Board();
			b.Turn = Colors.Black;
			int pos = 5 * 8 + 4;

			b.State[pos + 16] = Pieces.King | Colors.Black;
			b.State[pos] = Pieces.Rook | Colors.Black;

			b.State[pos - 24] = Pieces.Queen | Colors.White;
			b.State[7] = Pieces.King | Colors.White;

			bool check = Check.IsChecked(b, Colors.Black);
			Assert.IsFalse(check);

			// rook is covering the king
			var validMoves = Moves.GetValidMoves(b, pos);
			Assert.AreEqual(4, validMoves.Count);

			var allMoves = Moves.GetMoves(b, pos);
			Assert.AreEqual(4 + 7, allMoves.Count);
		}
	}
}

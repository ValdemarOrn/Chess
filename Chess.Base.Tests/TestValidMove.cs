using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestValidMove
	{
		[Test]
		public void TestMoveCausesCheck()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 5*8 + 4;

			b.State[7*8 + 4] = Colors.Val(Piece.King, Color.Black);
			b.State[pos] = Colors.Val(Piece.Rook, Color.Black);

			b.State[0 * 8 + 4] = Colors.Val(Piece.Queen, Color.White);
			b.State[7] = Colors.Val(Piece.King, Color.White);

			bool check = b.IsChecked(Color.Black);
			Assert.IsFalse(check);

			bool causesCheck;

			causesCheck = b.MoveSelfChecks(pos, pos + 8);
			Assert.IsFalse(causesCheck);

			causesCheck = b.MoveSelfChecks(pos, pos - 8);
			Assert.IsFalse(causesCheck);

			causesCheck = b.MoveSelfChecks(pos, pos - 1);
			Assert.IsTrue(causesCheck);

			causesCheck = b.MoveSelfChecks(pos, pos - 3);
			Assert.IsTrue(causesCheck);
		}

		[Test]
		public void TestValidMoves()
		{
			var b = new Board();
			b.PlayerTurn = Color.Black;
			int pos = 5 * 8 + 4;

			b.State[pos + 16] = Colors.Val(Piece.King, Color.Black);
			b.State[pos] = Colors.Val(Piece.Rook, Color.Black);

			b.State[pos - 24] = Colors.Val(Piece.Queen, Color.White);
			b.State[7] = Colors.Val(Piece.King, Color.White);

			bool check = b.IsChecked(Color.Black);
			Assert.IsFalse(check);

			// rook is covering the king
			var validMoves = Moves.GetValidMoves(b, pos);
			Assert.AreEqual(4, validMoves.Length);

			var allMoves = Moves.GetMoves(b, pos);
			Assert.AreEqual(4 + 7, allMoves.Length);
		}
	}
}

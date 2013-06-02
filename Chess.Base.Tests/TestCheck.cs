using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestCheck
	{
		[Test]
		public void TestAttacksSinglePawn()
		{
			var b = new Board();
			int pos = 12;
			b.State[pos] = Colors.Val(Piece.Pawn, Color.White);
			var attacks = b.GetAttackBoard(Color.White);

			Assert.AreEqual(2, attacks.Count(x => x != 0));
			Assert.AreEqual(1, attacks[12 + 7]);
			Assert.AreEqual(1, attacks[12 + 9]);
		}

		[Test]
		public void TestAttacksTwoRooks1()
		{
			var b = new Board();
			b.State[8] = Colors.Val(Piece.Rook, Color.White);
			b.State[8 + 7] = Colors.Val(Piece.Rook, Color.White);
			var attacks = b.GetAttackBoard(Color.White);

			Assert.AreEqual(6, attacks.Count(x => x == 2));
			Assert.AreEqual(1, attacks[16]);
			Assert.AreEqual(1, attacks[16 + 7]);
			Assert.AreEqual(1, attacks[7*8]);
			Assert.AreEqual(1, attacks[7*8 + 7]);
			Assert.AreEqual(1, attacks[0]);
			Assert.AreEqual(1, attacks[7]);

			Assert.AreEqual(2, attacks[8 + 4]);
		}

		[Test]
		public void TestAttacksTwoRooks2()
		{
			var b = new Board();
			b.State[8 + 3] = Colors.Val(Piece.Rook, Color.White);
			b.State[8*6 + 6] = Colors.Val(Piece.Rook, Color.White);

			// pawns at the attack intersections. Rooks should still 
			// be able to attack those areas but not move to them
			b.State[8 + 6] = Colors.Val(Piece.Pawn, Color.White);
			b.State[8 * 6 + 3] = Colors.Val(Piece.Pawn, Color.White);

			var attacks = b.GetAttackBoard(Color.White);

			Assert.AreEqual(2, attacks.Count(x => x == 2));
			
			// location of rooks, has no attacks
			Assert.AreEqual(0, attacks[8+3]);
			Assert.AreEqual(0, attacks[8*6+6]);

			Assert.AreEqual(1, attacks[16+3]);
			Assert.AreEqual(1, attacks[8+5]);

			Assert.AreEqual(2, attacks[8+6]);
			Assert.AreEqual(2, attacks[8*6+3]);
		}

		[Test]
		public void TestCheckOneRook()
		{
			var b = new Board();
			b.State[8 * 6 + 7] = Colors.Val(Piece.King, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.Black);

			bool check = b.IsChecked(Color.White);

			Assert.IsTrue(check);
		}

		[Test]
		public void TestCheckMateNeverTwoRooks()
		{
			var b = new Board();
			b.State[6] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[8 * 6 + 5] = Colors.Val(Piece.King, Color.Black);

			bool check = b.IsCheckMate(Color.Black);

			Assert.IsFalse(check);
		}

		[Test]
		public void TestCheckMateNoTwoRooks()
		{
			var b = new Board();
			b.State[6] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[8 * 6 + 6] = Colors.Val(Piece.King, Color.Black);
			b.PlayerTurn = Color.Black;

			bool check = b.IsCheckMate(Color.Black);

			Assert.IsFalse(check);
		}

		[Test]
		public void TestCheckMateYesTwoRooks()
		{
			var b = new Board();
			b.State[6] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[7*8 + 7] = Colors.Val(Piece.King, Color.Black);
			b.PlayerTurn = Color.Black;

			bool check = b.IsCheckMate(Color.Black);

			Assert.IsTrue(check);
		}

		[Test]
		public void TestCheckMateNoTwoRooksOneQueen()
		{
			// should be able to move the queen in front
			var b = new Board();
			b.State[6] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[7*8 + 7] = Colors.Val(Piece.King, Color.Black);
			b.State[6*8 + 0] = Colors.Val(Piece.Queen, Color.Black);
			b.PlayerTurn = Color.Black;

			bool check = b.IsCheckMate(Color.Black);

			Assert.IsFalse(check);
		}

		[Test]
		public void TestStalemateIsChecked()
		{
			// king can't kill the queen, pawn protects
			var b = new Board();
			b.State[7 * 8] = Colors.Val(Piece.King, Color.Black);
			b.State[6 * 8 + 1] = Colors.Val(Piece.Queen, Color.White);
			b.State[5 * 8 + 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[7] = Colors.Val(Piece.King, Color.White);
			b.PlayerTurn = Color.Black;

			bool check = b.IsChecked(Color.Black);
			Assert.IsTrue(check);

			bool stalemate = b.IsStalemate(Color.Black);
			Assert.IsFalse(stalemate);
		}

		[Test]
		public void TestStalemateFalse()
		{
			// king can kill the rook
			var b = new Board();
			b.State[7*8] = Colors.Val(Piece.King, Color.Black);
			b.State[6*8+1] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.King, Color.White);
			b.PlayerTurn = Color.Black;

			bool check = b.IsChecked(Color.Black);
			Assert.IsFalse(check);

			bool stalemate = b.IsStalemate(Color.Black);
			Assert.IsFalse(stalemate);
		}

		[Test]
		public void TestStalemateTrue()
		{
			// king can't kill the rook, pawn protects
			var b = new Board();
			b.State[7 * 8] = Colors.Val(Piece.King, Color.Black);
			b.State[6 * 8 + 1] = Colors.Val(Piece.Rook, Color.White);
			b.State[5 * 8 + 2] = Colors.Val(Piece.Pawn, Color.White);
			b.State[7] = Colors.Val(Piece.King, Color.White);
			b.PlayerTurn = Color.Black;

			bool check = b.IsChecked(Color.Black);
			Assert.IsFalse(check);

			bool stalemate = b.IsStalemate(Color.Black);
			Assert.IsTrue(stalemate);
		}


	}
}

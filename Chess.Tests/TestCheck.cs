using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestCheck
	{
		[TestMethod]
		public void TestAttacksSinglePawn()
		{
			var b = new Board();
			int pos = 12;
			b.State[pos] = Colors.Val(Pieces.Pawn, Color.White);
			var attacks = Check.GetAttackBoard(b, Color.White);

			Assert.AreEqual(2, attacks.Count(x => x != 0));
			Assert.AreEqual(1, attacks[12 + 7]);
			Assert.AreEqual(1, attacks[12 + 9]);
		}

		[TestMethod]
		public void TestAttacksTwoRooks1()
		{
			var b = new Board();
			b.State[8] = Colors.Val(Pieces.Rook, Color.White);
			b.State[8 + 7] = Colors.Val(Pieces.Rook, Color.White);
			var attacks = Check.GetAttackBoard(b, Color.White);

			Assert.AreEqual(6, attacks.Count(x => x == 2));
			Assert.AreEqual(1, attacks[16]);
			Assert.AreEqual(1, attacks[16 + 7]);
			Assert.AreEqual(1, attacks[7*8]);
			Assert.AreEqual(1, attacks[7*8 + 7]);
			Assert.AreEqual(1, attacks[0]);
			Assert.AreEqual(1, attacks[7]);

			Assert.AreEqual(2, attacks[8 + 4]);
		}

		[TestMethod]
		public void TestAttacksTwoRooks2()
		{
			var b = new Board();
			b.State[8 + 3] = Colors.Val(Pieces.Rook, Color.White);
			b.State[8*6 + 6] = Colors.Val(Pieces.Rook, Color.White);

			// pawns at the attack intersections. Rooks should still 
			// be able to attack those areas but not move to them
			b.State[8 + 6] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[8 * 6 + 3] = Colors.Val(Pieces.Pawn, Color.White);

			var attacks = Check.GetAttackBoard(b, Color.White);

			Assert.AreEqual(2, attacks.Count(x => x == 2));
			
			// location of rooks, has no attacks
			Assert.AreEqual(0, attacks[8+3]);
			Assert.AreEqual(0, attacks[8*6+6]);

			Assert.AreEqual(1, attacks[16+3]);
			Assert.AreEqual(1, attacks[8+5]);

			Assert.AreEqual(2, attacks[8+6]);
			Assert.AreEqual(2, attacks[8*6+3]);
		}

		[TestMethod]
		public void TestCheckOneRook()
		{
			var b = new Board();
			b.State[8 * 6 + 7] = Colors.Val(Pieces.King, Color.White);
			b.State[7] = Colors.Val(Pieces.Rook, Color.Black);
			
			bool check = Check.IsChecked(b, Color.White);

			Assert.IsTrue(check);
		}

		[TestMethod]
		public void TestCheckMateNeverTwoRooks()
		{
			var b = new Board();
			b.State[6] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7] = Colors.Val(Pieces.Rook, Color.White);
			b.State[8 * 6 + 5] = Colors.Val(Pieces.King, Color.Black);

			bool check = Check.IsCheckMate(b, Color.Black);

			Assert.IsFalse(check);
		}

		[TestMethod]
		public void TestCheckMateNoTwoRooks()
		{
			var b = new Board();
			b.State[6] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7] = Colors.Val(Pieces.Rook, Color.White);
			b.State[8 * 6 + 6] = Colors.Val(Pieces.King, Color.Black);
			b.PlayerTurn = Color.Black;

			bool check = Check.IsCheckMate(b, Color.Black);

			Assert.IsFalse(check);
		}

		[TestMethod]
		public void TestCheckMateYesTwoRooks()
		{
			var b = new Board();
			b.State[6] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7*8 + 7] = Colors.Val(Pieces.King, Color.Black);
			b.PlayerTurn = Color.Black;

			bool check = Check.IsCheckMate(b, Color.Black);

			Assert.IsTrue(check);
		}

		[TestMethod]
		public void TestCheckMateNoTwoRooksOneQueen()
		{
			// should be able to move the queen in front
			var b = new Board();
			b.State[6] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7*8 + 7] = Colors.Val(Pieces.King, Color.Black);
			b.State[6*8 + 0] = Colors.Val(Pieces.Queen, Color.Black);
			b.PlayerTurn = Color.Black;

			bool check = Check.IsCheckMate(b, Color.Black);

			Assert.IsFalse(check);
		}

		[TestMethod]
		public void TestStalemateIsChecked()
		{
			// king can't kill the queen, pawn protects
			var b = new Board();
			b.State[7 * 8] = Colors.Val(Pieces.King, Color.Black);
			b.State[6 * 8 + 1] = Colors.Val(Pieces.Queen, Color.White);
			b.State[5 * 8 + 2] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[7] = Colors.Val(Pieces.King, Color.White);
			b.PlayerTurn = Color.Black;

			bool check = Check.IsChecked(b, Color.Black);
			Assert.IsTrue(check);

			bool stalemate = Check.IsStalemate(b, Color.Black);
			Assert.IsFalse(stalemate);
		}

		[TestMethod]
		public void TestStalemateFalse()
		{
			// king can kill the rook
			var b = new Board();
			b.State[7*8] = Colors.Val(Pieces.King, Color.Black);
			b.State[6*8+1] = Colors.Val(Pieces.Rook, Color.White);
			b.State[7] = Colors.Val(Pieces.King, Color.White);
			b.PlayerTurn = Color.Black;

			bool check = Check.IsChecked(b, Color.Black);
			Assert.IsFalse(check);

			bool stalemate = Check.IsStalemate(b, Color.Black);
			Assert.IsFalse(stalemate);
		}

		[TestMethod]
		public void TestStalemateTrue()
		{
			// king can't kill the rook, pawn protects
			var b = new Board();
			b.State[7 * 8] = Colors.Val(Pieces.King, Color.Black);
			b.State[6 * 8 + 1] = Colors.Val(Pieces.Rook, Color.White);
			b.State[5 * 8 + 2] = Colors.Val(Pieces.Pawn, Color.White);
			b.State[7] = Colors.Val(Pieces.King, Color.White);
			b.PlayerTurn = Color.Black;

			bool check = Check.IsChecked(b, Color.Black);
			Assert.IsFalse(check);

			bool stalemate = Check.IsStalemate(b, Color.Black);
			Assert.IsTrue(stalemate);
		}


	}
}

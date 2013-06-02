using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestBoard
	{
		[Test]
		public void TestFindByColor()
		{
			var b = new Board(true);


			var c = b.FindByColor(Color.White);
			Assert.AreEqual(16, c.Count);

			// all white are in the first 2 rows
			Assert.IsFalse(c.Any(x => x >= 16));


			c = b.FindByColor(Color.Black);
			Assert.AreEqual(16, c.Count);

			// all black are in the upper 2 rows
			Assert.IsFalse(c.Any(x => x < 48));
		}

		[Test]
		public void TestFindByPiece()
		{
			var b = new Board(true);

			var c = b.FindByPiece(Piece.Pawn);
			Assert.AreEqual(16, c.Count);

			c = b.FindByPiece(Piece.Rook);
			Assert.AreEqual(4, c.Count);
			Assert.IsTrue(c.Contains(0));
			Assert.IsTrue(c.Contains(7));
			Assert.IsTrue(c.Contains(56));
			Assert.IsTrue(c.Contains(63));
		}

		[Test]
		public void TestFindByPieceAndColor()
		{
			var b = new Board(true);

			var c = b.FindByPieceAndColor(Piece.Pawn, Color.White);
			Assert.AreEqual(8, c.Count);

			c = b.FindByPieceAndColor(Piece.Bishop, Color.Black);
			Assert.AreEqual(2, c.Count);
			Assert.IsTrue(c.Contains(58));
			Assert.IsTrue(c.Contains(61));

			c = b.FindByPieceAndColor(Piece.Queen, Color.White);
			Assert.AreEqual(1, c.Count);
			Assert.IsTrue(c.Contains(3));
		}

		[Test]
		public void TestKingLocation()
		{
			var b = new Board(false);
			b.State[16] = Colors.Val(Piece.King, Color.White);
			b.State[53] = Colors.Val(Piece.King, Color.Black);

			Assert.AreEqual(16, b.KingLocation(Color.White));
			Assert.AreEqual(53, b.KingLocation(Color.Black));
		}

		[Test]
		public void TestCastlingNoPieces()
		{
			var b = new Board(false);
			b.AllowCastlingAll(); // Can't do this without any pieces on the board
			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);
		}

		[Test]
		public void TestCastlingOK()
		{
			var b = new Board(false);
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();
			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}

		[Test]
		public void TestCastlingNoQueensideWhite()
		{
			var b = new Board(false);
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.White;
			b.Move(0, 1);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.White;
			b.Move(1, 0);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsFalse(b.CanCastleQWhite);
		}

		[Test]
		public void TestCastlingNoKingsideWhite()
		{
			var b = new Board(false);
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.White;
			b.Move(7, 6);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.White;
			b.Move(6, 7);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsFalse(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}


		[Test]
		public void TestCastlingNoQueensideBlack()
		{
			var b = new Board(false);
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.Black;
			b.Move(56, 57);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.Black;
			b.Move(57, 56);

			Assert.IsTrue(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsFalse(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}

		[Test]
		public void TestCastlingNoKingsideBlack()
		{
			var b = new Board(false);
			b.State[0] = Colors.Val(Piece.Rook, Color.White);
			b.State[7] = Colors.Val(Piece.Rook, Color.White);
			b.State[4] = Colors.Val(Piece.King, Color.White);

			b.State[56] = Colors.Val(Piece.Rook, Color.Black);
			b.State[63] = Colors.Val(Piece.Rook, Color.Black);
			b.State[60] = Colors.Val(Piece.King, Color.Black);

			b.AllowCastlingAll();

			b.PlayerTurn = Color.Black;
			b.Move(63, 62);

			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);

			// test that we can't just move back
			b.PlayerTurn = Color.Black;
			b.Move(62, 63);

			Assert.IsFalse(b.CanCastleKBlack);
			Assert.IsTrue(b.CanCastleKWhite);
			Assert.IsTrue(b.CanCastleQBlack);
			Assert.IsTrue(b.CanCastleQWhite);
		}

		[Test]
		public void TestPromoteOK1()
		{
			//Todo: Test Promote()
		}

		[Test]
		public void TestPromoteOK2()
		{
			
		}

		[Test]
		public void TestPromoteFail()
		{
			
		}

		[Test]
		public void TestPromoteIllegalPiece()
		{
			
		}

		[Test]
		public void TestMove1()
		{
			//Todo: Test Move by ref()
		}
	}
}

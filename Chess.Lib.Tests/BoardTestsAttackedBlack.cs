using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class BoardTestsAttackedBlack
	{
		[TestMethod]
		public unsafe void TestIsAttackedBlack1()
		{
			// Pawn 1
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 29, Board.PIECE_PAWN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack1x()
		{
			// Pawn 1 - wrong side
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 45, Board.PIECE_PAWN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack2()
		{
			// Pawn 2
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 28, Board.PIECE_PAWN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack3()
		{
			// Pawn 3
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 38, Board.PIECE_PAWN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack4()
		{
			// Knight 1
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 30, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack5()
		{
			// Knight 2
			var b = Board.Create();
			Board.SetPiece(b, 39, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 22, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 39, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack6()
		{
			// Knight 3
			var b = Board.Create();
			Board.SetPiece(b, 39, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 30, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 39, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack7()
		{
			// Knight 4
			var b = Board.Create();
			Board.SetPiece(b, 32, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 42, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 32, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack8()
		{
			// Bishop 1
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 63, Board.PIECE_BISHOP, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack9()
		{
			// Bishop 2
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 45, Board.PIECE_BISHOP, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack10()
		{
			// Bishop 3
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 22, Board.PIECE_BISHOP, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack11()
		{
			// Bishop 4 - attacking blank
			var b = Board.Create();
			Board.SetPiece(b, 22, Board.PIECE_BISHOP, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 4, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack12()
		{
			// Bishop 5
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 35, Board.PIECE_BISHOP, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack13()
		{
			// Rook 1
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 37, Board.PIECE_ROOK, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack14()
		{
			// Rook 2
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 32, Board.PIECE_ROOK, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack15()
		{
			// Rook 3
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 18, Board.PIECE_ROOK, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack16()
		{
			// Rook 4
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 4, Board.PIECE_ROOK, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack17()
		{
			// Rook 5
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 44, Board.PIECE_ROOK, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack18()
		{
			// Rook 6 - blocked
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 4, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 20, Board.PIECE_PAWN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack19()
		{
			// Queen 1
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 44, Board.PIECE_QUEEN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack20()
		{
			// Queen 2
			var b = Board.Create();
			Board.SetPiece(b, 36, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 15, Board.PIECE_QUEEN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 36, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack21()
		{
			// Queen 3
			var b = Board.Create();
			Board.SetPiece(b, 7, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 0, Board.PIECE_QUEEN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 7, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack22()
		{
			// Queen 4
			var b = Board.Create();
			Board.SetPiece(b, 7, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 42, Board.PIECE_QUEEN, Board.COLOR_WHITE);
			var isAttacked = Board.IsAttacked(b, 7, Board.COLOR_WHITE);
			Assert.IsTrue(isAttacked);
		}

		[TestMethod]
		public unsafe void TestIsAttackedBlack23()
		{
			// Queen 5
			var b = Board.Create();
			Board.SetPiece(b, 7, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 42, Board.PIECE_QUEEN, Board.COLOR_WHITE);
			Board.SetPiece(b, 21, Board.PIECE_PAWN, Board.COLOR_BLACK);
			var isAttacked = Board.IsAttacked(b, 7, Board.COLOR_WHITE);
			Assert.IsFalse(isAttacked);
		}
	}
}

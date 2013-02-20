using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class BoardTests
	{
		[TestMethod]
		public unsafe void TestBoardXY()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			Assert.AreEqual(5, Board.Board_X(5));
			Assert.AreEqual(5, Board.Board_X(5+8));
			Assert.AreEqual(5, Board.Board_X(5+16));
			Assert.AreEqual(3, Board.Board_X(8*7 + 3));

			Assert.AreEqual(7, Board.Board_Y(7 * 8 + 3));
			Assert.AreEqual(1, Board.Board_Y(1 * 8 + 4));
			Assert.AreEqual(3, Board.Board_Y(3 * 8 + 6));
			Assert.AreEqual(4, Board.Board_Y(4 * 8 + 0));
			Assert.AreEqual(7, Board.Board_Y(7 * 8 + 7));
		}

		[TestMethod]
		public unsafe void TestBoardColorWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_PAWNS]), 5);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_PAWNS]), 47);

			b->Boards[Board.BOARD_WHITE] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_WHITE]), 5);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_WHITE]), 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_WHITE]), 47);


			Assert.AreEqual(Colors.White, Board.Board_Color(b, 5));
			Assert.AreEqual(Colors.White, Board.Board_Color(b, 7));
			Assert.AreEqual(Colors.White, Board.Board_Color(b, 47));

			Assert.AreEqual(0, Board.Board_Color(b, 3));
			Assert.AreEqual(0, Board.Board_Color(b, 8));
			Assert.AreEqual(0, Board.Board_Color(b, 46));
		}

		[TestMethod]
		public unsafe void TestBoardColorBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_PAWNS]), 5);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_PAWNS]), 47);

			b->Boards[Board.BOARD_BLACK] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_BLACK]), 5);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_BLACK]), 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Bitboard_Set((b->Boards[Board.BOARD_BLACK]), 47);


			Assert.AreEqual(Colors.Black, Board.Board_Color(b, 5));
			Assert.AreEqual(Colors.Black, Board.Board_Color(b, 7));
			Assert.AreEqual(Colors.Black, Board.Board_Color(b, 47));

			Assert.AreEqual(0, Board.Board_Color(b, 3));
			Assert.AreEqual(0, Board.Board_Color(b, 8));
			Assert.AreEqual(0, Board.Board_Color(b, 46));
		}

		// Todo: Test Board_Init
		// Todo: Test Board_Copy
		// Todo: Test Board_Make
		// Todo: Test Board_Unmake
		// Todo: Test Board_Promote
		// Todo: Test Board_CheckCastling
		// Todo: Test Board_AllowCastlingAll
	}
}

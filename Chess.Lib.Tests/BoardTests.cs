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
			BoardStruct* b = (BoardStruct*)Board.Create();
			Assert.AreEqual(5, Board.X(5));
			Assert.AreEqual(5, Board.X(5+8));
			Assert.AreEqual(5, Board.X(5+16));
			Assert.AreEqual(3, Board.X(8*7 + 3));

			Assert.AreEqual(7, Board.Y(7 * 8 + 3));
			Assert.AreEqual(1, Board.Y(1 * 8 + 4));
			Assert.AreEqual(3, Board.Y(3 * 8 + 6));
			Assert.AreEqual(4, Board.Y(4 * 8 + 0));
			Assert.AreEqual(7, Board.Y(7 * 8 + 7));
		}

		[TestMethod]
		public unsafe void TestBoardColorWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 5);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 47);

			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 5);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 47);


			Assert.AreEqual(Colors.White, Board.Color(b, 5));
			Assert.AreEqual(Colors.White, Board.Color(b, 7));
			Assert.AreEqual(Colors.White, Board.Color(b, 47));

			Assert.AreEqual(0, Board.Color(b, 3));
			Assert.AreEqual(0, Board.Color(b, 8));
			Assert.AreEqual(0, Board.Color(b, 46));
		}

		[TestMethod]
		public unsafe void TestBoardColorBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 5);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 47);

			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 5);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 47);


			Assert.AreEqual(Colors.Black, Board.Color(b, 5));
			Assert.AreEqual(Colors.Black, Board.Color(b, 7));
			Assert.AreEqual(Colors.Black, Board.Color(b, 47));

			Assert.AreEqual(0, Board.Color(b, 3));
			Assert.AreEqual(0, Board.Color(b, 8));
			Assert.AreEqual(0, Board.Color(b, 46));
		}

		[TestMethod]
		public unsafe void TestBoardInit()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 0);

			byte allCastle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			Assert.AreEqual((ulong)0, b->Boards[Board.BOARD_WHITE]);
			Assert.AreEqual((ulong)0, b->Boards[Board.BOARD_BLACK]);
			Assert.AreEqual(allCastle, b->Castle);
		}

		[TestMethod]
		public unsafe void TestBoardInitPieces()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 1);

			byte allCastle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			Assert.AreEqual((ulong)0xFFFF, b->Boards[Board.BOARD_WHITE]);
			Assert.AreEqual((ulong)0xFFFF000000000000, b->Boards[Board.BOARD_BLACK]);
			Assert.AreEqual((ulong)0xFF00000000FF00, b->Boards[Board.BOARD_PAWNS]);
			Assert.AreEqual((ulong)0x1000000000000010, b->Boards[Board.BOARD_KINGS]);
			Assert.AreEqual(allCastle, b->Castle);
		}

		[TestMethod]
		public unsafe void TestBoardAttacks()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 13, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 50, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			Board.SetPiece(b, 8, Board.PIECE_PAWN, Board.COLOR_WHITE);

			Board.SetPiece(b, 46, Board.PIECE_PAWN, Board.COLOR_BLACK);

			new Moves(); // load moves

			var whiteMap = Board.AttackMap(b, Board.COLOR_WHITE);
			var blackMap = Board.AttackMap(b, Board.COLOR_BLACK);
			
			Assert.AreEqual((ulong)0x725470, whiteMap);
			Assert.AreEqual((ulong)0xE0A0EA000000000, blackMap);
		}

		// Todo: Test Board_Copy
		// Todo: Test Board_Make
		// Todo: Test Board_Unmake
		// Todo: Test Board_Promote
		// Todo: Test Board_CheckCastling
		// Todo: Test Board_AllowCastlingAll
	}
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class MovePawnBlackTests
	{
		// --------------- Simple moves, single piece on board ---------------

		[TestMethod]
		public unsafe void TestBlackPawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 53);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 53);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 53);

			Assert.AreEqual((ulong)0x202000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 35);

			Assert.AreEqual((ulong)0x8000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 15);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 15);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 15);

			Assert.AreEqual((ulong)0x80, moves);
		}

		// --------------- capture moves, two pieces on board ---------------

		[TestMethod]
		public unsafe void TestBlackPawnCapture1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 53);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 53);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 44);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 44);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 53);

			Assert.AreEqual((ulong)0x302000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 35 - 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 9);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 35 - 9);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 35);

			Assert.AreEqual((ulong)0x1C000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 35 - 7);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 35);

			Assert.AreEqual((ulong)0x18000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture4()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 15);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 15);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 6);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 6);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 7);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 15);

			Assert.AreEqual((ulong)0x40, moves);
		}

		// --------------- non-capture moves, same color ---------------

		[TestMethod]
		public unsafe void TestBlackPawnSame1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 53);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 53);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 46);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 46);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 53);

			Assert.AreEqual((ulong)0x202000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35 - 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 9);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35 - 9);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 35);

			Assert.AreEqual((ulong)0x8000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35 - 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 9);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 35 - 9);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 35);

			Assert.AreEqual((ulong)0xC000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame4()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 8);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 8);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 1);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 1);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 8);

			Assert.AreEqual((ulong)0x1, moves);
		}

		[TestMethod]
		public unsafe void TestBlackBlockedWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 42);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 42);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 34);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 34);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 42);

			Assert.AreEqual((ulong)0x0, moves);
		}

		[TestMethod]
		public unsafe void TestBlackBlockedBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 42);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 42);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 34);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 34);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 42);

			Assert.AreEqual((ulong)0x0, moves);
		}

		[TestMethod]
		public unsafe void TestBlackEnPassantMoveRight()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 25, Board.PIECE_PAWN, Board.COLOR_BLACK);
			Board.SetPiece(b, 26, Board.PIECE_PAWN, Board.COLOR_WHITE);
			b->EnPassantTile = 18;

			ulong moves = Moves.GetMoves(b, 25);

			// tile 17,18
			Assert.AreEqual((ulong)0x60000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackEnPassantMoveLeft()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 30, Board.PIECE_PAWN, Board.COLOR_BLACK);
			Board.SetPiece(b, 29, Board.PIECE_PAWN, Board.COLOR_WHITE);
			b->EnPassantTile = 21;

			ulong moves = Moves.GetMoves(b, 30);

			// tile 17,18
			Assert.AreEqual((ulong)0x600000, moves);
		}
	}
}

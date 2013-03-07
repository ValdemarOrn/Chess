using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class MovePawnWhiteTests
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			//Pawn.Load();
		}

		// --------------- Simple moves, single piece on board ---------------

		[TestMethod]
		public unsafe void TestWhitePawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 8);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 8);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 8);

			Assert.AreEqual((ulong)0x1010000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnBlocker1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 8, Board.PIECE_PAWN, Board.COLOR_WHITE);
			
			ulong moves = Moves.GetMoves(b, 8);
			Assert.AreEqual((ulong)0x1010000, moves);

			Board.SetPiece(b, 16, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			moves = Moves.GetMoves(b, 8);
			Assert.AreEqual((ulong)0x0, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);

			Assert.AreEqual((ulong)0x800000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 55);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 55);

			Assert.AreEqual((ulong)0x8000000000000000, moves);
		}

		// --------------- capture moves, two pieces on board ---------------

		[TestMethod]
		public unsafe void TestWhitePawnCapture1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 8);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 8);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 17);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 17);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 8);

			Assert.AreEqual((ulong)0x1030000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnCapture2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 27 + 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 9);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 27 + 9);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);

			Assert.AreEqual((ulong)0x1C00000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnCapture3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 27 + 7);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);

			Assert.AreEqual((ulong)0xC00000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnCapture4()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 55);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55 + 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 55 + 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55 + 8);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 55 + 8);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 55);

			Assert.AreEqual((ulong)0x4000000000000000, moves);
		}

		// --------------- non-capture moves, same color ---------------

		[TestMethod]
		public unsafe void TestWhitePawnSame1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 8);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 8);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 17);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 17);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 8);

			Assert.AreEqual((ulong)0x1010000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSame2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27 + 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 9);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27 + 9);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);

			Assert.AreEqual((ulong)0x800000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSame3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27 + 7);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27 + 9);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 27 + 9);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);

			Assert.AreEqual((ulong)0x1800000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSame4()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 55);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55 + 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 55 + 7);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 55);

			Assert.AreEqual((ulong)0x8000000000000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteBlockedBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 20);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 20);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 28);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 28);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 20);

			Assert.AreEqual((ulong)0x0, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteBlockedWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 20);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 20);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 28);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 28);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 20);

			Assert.AreEqual((ulong)0x0, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteEnPassantMoveRight()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 33, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 34, Board.PIECE_PAWN, Board.COLOR_BLACK);
			b->EnPassantTile = 42;

			ulong moves = Moves.GetMoves(b, 33);

			// tile 41, 42
			Assert.AreEqual((ulong)0x60000000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteEnPassantMoveLeft()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 38, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 37, Board.PIECE_PAWN, Board.COLOR_BLACK);
			b->EnPassantTile = 45;

			ulong moves = Moves.GetMoves(b, 38);

			// tile 45, 46
			Assert.AreEqual((ulong)0x600000000000, moves);
		}
	}
}

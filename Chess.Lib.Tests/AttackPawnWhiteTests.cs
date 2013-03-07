using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class AttackPawnWhiteTests
	{
		// --------------- Simple moves, single piece on board ---------------

		[TestMethod]
		public unsafe void TestWhitePawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 8);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 8);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 8);

			Assert.AreEqual((ulong)0x20000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 55);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 55);

			Assert.AreEqual((ulong)0x4000000000000000, moves);
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

			ulong moves = Moves.GetAttacks(b, 8);

			Assert.AreEqual((ulong)0x20000, moves);
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

			ulong moves = Moves.GetAttacks(b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
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

			ulong moves = Moves.GetAttacks(b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
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

			ulong moves = Moves.GetAttacks(b, 55);

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

			ulong moves = Moves.GetAttacks(b, 8);

			Assert.AreEqual((ulong)0x20000, moves);
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

			ulong moves = Moves.GetAttacks(b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
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

			ulong moves = Moves.GetAttacks(b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
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

			ulong moves = Moves.GetAttacks(b, 55);

			Assert.AreEqual((ulong)0x4000000000000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteEnPassantAttack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 37, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 36, Board.PIECE_PAWN, Board.COLOR_BLACK);
			b->EnPassantTile = 44;

			ulong attacks = Moves.GetAttacks(b, 37);
			Assert.AreEqual((ulong)0x500000000000, attacks);

			ulong moves = Moves.GetMoves(b, 37);
			Assert.AreEqual((ulong)0x300000000000, moves);
		}
	}
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public class AttackPawnBlackTests
	{
		// --------------- Simple moves, single piece on board ---------------

		[Test]
		public unsafe void TestBlackPawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 53);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 53);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 53);

			Assert.AreEqual((ulong)0x500000000000, moves);
		}

		[Test]
		public unsafe void TestBlackPawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[Test]
		public unsafe void TestBlackPawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 8);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 8);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 8);

			Assert.AreEqual((ulong)0x2, moves);
		}

		// --------------- capture moves, two pieces on board ---------------

		[Test]
		public unsafe void TestBlackPawnCapture1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 55);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 55);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 46);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 46);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 55);

			Assert.AreEqual((ulong)0x400000000000, moves);
		}

		[Test]
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

			ulong moves = Moves.GetAttacks(b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[Test]
		public unsafe void TestBlackPawnCapture3()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 35);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35 - 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 35 - 7);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[Test]
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

			ulong moves = Moves.GetAttacks(b, 15);

			Assert.AreEqual((ulong)0x40, moves);
		}

		// --------------- non-capture moves, same color ---------------

		[Test]
		public unsafe void TestBlackPawnSame1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 48);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 48);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 41);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 41);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 48);

			Assert.AreEqual((ulong)0x20000000000, moves);
		}

		[Test]
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

			ulong moves = Moves.GetAttacks(b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[Test]
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

			ulong moves = Moves.GetAttacks(b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[Test]
		public unsafe void TestBlackPawnSame4()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 15);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 15);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 6);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 6);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetAttacks(b, 15);

			Assert.AreEqual((ulong)0x40, moves);
		}

		[Test]
		public unsafe void TestBlackEnPassantAttack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 28, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 29, Board.PIECE_PAWN, Board.COLOR_BLACK);
			b->EnPassantTile = 20;

			ulong attacks = Moves.GetAttacks(b, 29);
			Assert.AreEqual((ulong)0x500000, attacks);

			ulong moves = Moves.GetMoves(b, 29);
			Assert.AreEqual((ulong)0x300000, moves);
		}
	}
}

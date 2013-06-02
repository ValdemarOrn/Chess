using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Chess.Lib.MoveClasses;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public class KnightTest
	{
		[Test]
		public unsafe void TestKnightMoves1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);
			Assert.AreEqual((ulong)0x142200221400, moves);
		}

		[Test]
		public unsafe void TestKnightMoves2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 14);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 14);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 14);
			Assert.AreEqual((ulong)0xA0100010, moves);
		}

		[Test]
		public unsafe void TestKnightMoves1WhiteBlocking()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);

			// set 2 white pawns tha block moves
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 17);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 17);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 44);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 44);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 27);
			Assert.AreEqual((ulong)0x42200201400, moves);

			// white can still "attack" those squares
			ulong attacks = Moves.GetAttacks(b, 27);
			Assert.AreEqual((ulong)0x142200221400, attacks);
		}

		[Test]
		public unsafe void TestKnightMoves2BWhiteBlocking()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 14);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 14);

			// set 2 white pawns tha block moves
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 20);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 20);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 29);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 29);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 14);
			Assert.AreEqual((ulong)0x80000010, moves);

			ulong attacks = Moves.GetAttacks(b, 14);
			Assert.AreEqual((ulong)0xA0100010, attacks);
		}

		[Test]
		public unsafe void TestKnightMoves1BlockOpponent()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 27);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 27);

			// set 2 black pawns, they don't block moves
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 17);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 17);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 44);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 44);
			Board.GenerateTileMap(b);

			// attacks and moves should be equal
			ulong moves = Moves.GetMoves(b, 27);
			Assert.AreEqual((ulong)0x142200221400, moves);

			// white can still "attack" those squares
			ulong attacks = Moves.GetAttacks(b, 27);
			Assert.AreEqual((ulong)0x142200221400, attacks);
		}

		[Test]
		public unsafe void TestKnightMoves2BlockOpponent()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 14);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 14);

			// set 2 black pawns, they don't block moves
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 20);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 20);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 29);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 29);
			Board.GenerateTileMap(b);

			// attacks and moves should be equal
			ulong moves = Moves.GetMoves(b, 14);
			Assert.AreEqual((ulong)0xA0100010, moves);

			ulong attacks = Moves.GetAttacks(b, 14);
			Assert.AreEqual((ulong)0xA0100010, attacks);
		}

		[Test]
		public unsafe void TestKnightBlack1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 41);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 41);

			// set 1 black pawn, 1 white pawn
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 51);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 51);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 35);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 35);
			Board.GenerateTileMap(b);

			ulong moves = Moves.GetMoves(b, 41);
			Assert.AreEqual((ulong)0x500000805000000, moves);

			ulong attacks = Moves.GetAttacks(b, 41);
			Assert.AreEqual((ulong)0x508000805000000, attacks);
		}

	}
}

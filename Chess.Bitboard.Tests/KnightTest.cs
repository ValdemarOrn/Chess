using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chess.Lib.MoveClasses;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class KnightTest
	{
		[TestMethod]
		public unsafe void TestKnightMoves1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 27);
			b->White = Bitboard.Bitboard_Set((b->White), 27);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 27);
			Assert.AreEqual((ulong)0x142200221400, moves);
		}

		[TestMethod]
		public unsafe void TestKnightMoves2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 14);
			b->White = Bitboard.Bitboard_Set((b->White), 14);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 14);
			Assert.AreEqual((ulong)0xA0100010, moves);
		}

		[TestMethod]
		public unsafe void TestKnightMoves1WhiteBlocking()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 27);
			b->White = Bitboard.Bitboard_Set((b->White), 27);

			// set 2 white pawns tha block moves
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 17);
			b->White = Bitboard.Bitboard_Set((b->White), 17);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 44);
			b->White = Bitboard.Bitboard_Set((b->White), 44);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 27);
			Assert.AreEqual((ulong)0x42200201400, moves);

			// white can still "attack" those squares
			ulong attacks = Moves.Moves_GetAttacks((IntPtr)b, 27);
			Assert.AreEqual((ulong)0x142200221400, attacks);
		}

		[TestMethod]
		public unsafe void TestKnightMoves2BWhiteBlocking()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 14);
			b->White = Bitboard.Bitboard_Set((b->White), 14);

			// set 2 white pawns tha block moves
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 20);
			b->White = Bitboard.Bitboard_Set((b->White), 20);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 29);
			b->White = Bitboard.Bitboard_Set((b->White), 29);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 14);
			Assert.AreEqual((ulong)0x80000010, moves);

			ulong attacks = Moves.Moves_GetAttacks((IntPtr)b, 14);
			Assert.AreEqual((ulong)0xA0100010, attacks);
		}

		[TestMethod]
		public unsafe void TestKnightMoves1BlockOpponent()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 27);
			b->White = Bitboard.Bitboard_Set((b->White), 27);

			// set 2 black pawns, they don't block moves
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 17);
			b->Black = Bitboard.Bitboard_Set((b->Black), 17);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 44);
			b->Black = Bitboard.Bitboard_Set((b->Black), 44);

			// attacks and moves should be equal
			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 27);
			Assert.AreEqual((ulong)0x142200221400, moves);

			// white can still "attack" those squares
			ulong attacks = Moves.Moves_GetAttacks((IntPtr)b, 27);
			Assert.AreEqual((ulong)0x142200221400, attacks);
		}

		[TestMethod]
		public unsafe void TestKnightMoves2BlockOpponent()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 14);
			b->White = Bitboard.Bitboard_Set((b->White), 14);

			// set 2 black pawns, they don't block moves
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 20);
			b->Black = Bitboard.Bitboard_Set((b->Black), 20);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 29);
			b->Black = Bitboard.Bitboard_Set((b->Black), 29);

			// attacks and moves should be equal
			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 14);
			Assert.AreEqual((ulong)0xA0100010, moves);

			ulong attacks = Moves.Moves_GetAttacks((IntPtr)b, 14);
			Assert.AreEqual((ulong)0xA0100010, attacks);
		}

		[TestMethod]
		public unsafe void TestKnightBlack1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Knights = Bitboard.Bitboard_Set((b->Knights), 41);
			b->Black = Bitboard.Bitboard_Set((b->Black), 41);

			// set 1 black pawn, 1 white pawn
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 51);
			b->Black = Bitboard.Bitboard_Set((b->Black), 51);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 35);
			b->White = Bitboard.Bitboard_Set((b->White), 35);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 41);
			Assert.AreEqual((ulong)0x500000805000000, moves);

			ulong attacks = Moves.Moves_GetAttacks((IntPtr)b, 41);
			Assert.AreEqual((ulong)0x508000805000000, attacks);
		}

	}
}

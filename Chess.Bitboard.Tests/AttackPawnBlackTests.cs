using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class AttackPawnBlackTests
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			//Pawn.Load();
		}

		// --------------- Simple moves, single piece on board ---------------

		[TestMethod]
		public unsafe void TestBlackPawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 53);
			b->Black = Bitboard.Set((b->Black), 53);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 53);

			Assert.AreEqual((ulong)0x500000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 8);
			b->Black = Bitboard.Set((b->Black), 8);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 8);

			Assert.AreEqual((ulong)0x2, moves);
		}

		// --------------- capture moves, two pieces on board ---------------

		[TestMethod]
		public unsafe void TestBlackPawnCapture1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 55);
			b->Black = Bitboard.Set((b->Black), 55);

			b->Pawns = Bitboard.Set((b->Pawns), 46);
			b->White = Bitboard.Set((b->White), 46);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 55);

			Assert.AreEqual((ulong)0x400000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 7);
			b->White = Bitboard.Set((b->White), 35 - 7);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 9);
			b->White = Bitboard.Set((b->White), 35 - 9);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 7);
			b->White = Bitboard.Set((b->White), 35 - 7);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture4()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 15);
			b->Black = Bitboard.Set((b->Black), 15);

			b->Pawns = Bitboard.Set((b->Pawns), 6);
			b->White = Bitboard.Set((b->White), 6);

			b->Pawns = Bitboard.Set((b->Pawns), 7);
			b->White = Bitboard.Set((b->White), 7);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 15);

			Assert.AreEqual((ulong)0x40, moves);
		}

		// --------------- non-capture moves, same color ---------------

		[TestMethod]
		public unsafe void TestBlackPawnSame1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 48);
			b->Black = Bitboard.Set((b->Black), 48);

			b->Pawns = Bitboard.Set((b->Pawns), 41);
			b->Black = Bitboard.Set((b->Black), 41);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 48);

			Assert.AreEqual((ulong)0x20000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 7);
			b->Black = Bitboard.Set((b->Black), 35 - 7);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 9);
			b->Black = Bitboard.Set((b->Black), 35 - 9);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 7);
			b->Black = Bitboard.Set((b->Black), 35 - 7);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 9);
			b->White = Bitboard.Set((b->White), 35 - 9);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x14000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame4()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 15);
			b->Black = Bitboard.Set((b->Black), 15);

			b->Pawns = Bitboard.Set((b->Pawns), 6);
			b->Black = Bitboard.Set((b->Black), 6);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 15);

			Assert.AreEqual((ulong)0x40, moves);
		}

		// Todo: Test En passant Bitboard attacks Black
	}
}

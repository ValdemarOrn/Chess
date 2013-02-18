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
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			//Pawn.Load();
		}

		// --------------- Simple moves, single piece on board ---------------

		[TestMethod]
		public unsafe void TestWhitePawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 8);
			b->White = Bitboard.Set((b->White), 8);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 8);

			Assert.AreEqual((ulong)0x20000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 27);
			b->White = Bitboard.Set((b->White), 27);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 55);
			b->White = Bitboard.Set((b->White), 55);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 55);

			Assert.AreEqual((ulong)0x4000000000000000, moves);
		}

		// --------------- capture moves, two pieces on board ---------------

		[TestMethod]
		public unsafe void TestWhitePawnCapture1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 8);
			b->White = Bitboard.Set((b->White), 8);

			b->Pawns = Bitboard.Set((b->Pawns), 17);
			b->Black = Bitboard.Set((b->Black), 17);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 8);

			Assert.AreEqual((ulong)0x20000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnCapture2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 27);
			b->White = Bitboard.Set((b->White), 27);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 7);
			b->Black = Bitboard.Set((b->Black), 27 + 7);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 9);
			b->Black = Bitboard.Set((b->Black), 27 + 9);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnCapture3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 27);
			b->White = Bitboard.Set((b->White), 27);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 7);
			b->Black = Bitboard.Set((b->Black), 27 + 7);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnCapture4()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 55);
			b->White = Bitboard.Set((b->White), 55);

			b->Pawns = Bitboard.Set((b->Pawns), 55 + 7);
			b->Black = Bitboard.Set((b->Black), 55 + 7);

			b->Pawns = Bitboard.Set((b->Pawns), 55 + 8);
			b->Black = Bitboard.Set((b->Black), 55 + 8);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 55);

			Assert.AreEqual((ulong)0x4000000000000000, moves);
		}

		// --------------- non-capture moves, same color ---------------

		[TestMethod]
		public unsafe void TestWhitePawnSame1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 8);
			b->White = Bitboard.Set((b->White), 8);

			b->Pawns = Bitboard.Set((b->Pawns), 17);
			b->White = Bitboard.Set((b->White), 17);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 8);

			Assert.AreEqual((ulong)0x20000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSame2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 27);
			b->White = Bitboard.Set((b->White), 27);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 7);
			b->White = Bitboard.Set((b->White), 27 + 7);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 9);
			b->White = Bitboard.Set((b->White), 27 + 9);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSame3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 27);
			b->White = Bitboard.Set((b->White), 27);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 7);
			b->White = Bitboard.Set((b->White), 27 + 7);

			b->Pawns = Bitboard.Set((b->Pawns), 27 + 9);
			b->Black = Bitboard.Set((b->Black), 27 + 9);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 27);

			Assert.AreEqual((ulong)0x1400000000, moves);
		}

		[TestMethod]
		public unsafe void TestWhitePawnSame4()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 55);
			b->White = Bitboard.Set((b->White), 55);

			b->Pawns = Bitboard.Set((b->Pawns), 55 + 7);
			b->White = Bitboard.Set((b->White), 55 + 7);

			ulong moves = Moves.Moves_GetAttacks((IntPtr)b, 55);

			Assert.AreEqual((ulong)0x4000000000000000, moves);
		}

		// Todo: Test En passant Bitboard attacks white
	}
}

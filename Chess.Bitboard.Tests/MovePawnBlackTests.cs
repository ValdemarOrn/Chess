using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Bitboard.Tests
{
	[TestClass]
	public class MovePawnBlackTests
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			Pawn.Load();
		}

		// --------------- Simple moves, single piece on board ---------------

		[TestMethod]
		public unsafe void TestBlackPawnSingle1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 53);
			b->Black = Bitboard.Set((b->Black), 53);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 53);

			Assert.AreEqual((ulong)0x202000000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSingle2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x8000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSingle3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 15);
			b->Black = Bitboard.Set((b->Black), 15);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 15);

			Assert.AreEqual((ulong)0x80, moves);
		}

		// --------------- capture moves, two pieces on board ---------------

		[TestMethod]
		public unsafe void TestBlackPawnCapture1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 53);
			b->Black = Bitboard.Set((b->Black), 53);

			b->Pawns = Bitboard.Set((b->Pawns), 44);
			b->White = Bitboard.Set((b->White), 44);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 53);

			Assert.AreEqual((ulong)0x302000000000, moves);
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

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x1C000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnCapture3()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 35);
			b->Black = Bitboard.Set((b->Black), 35);

			b->Pawns = Bitboard.Set((b->Pawns), 35 - 7);
			b->White = Bitboard.Set((b->White), 35 - 7);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x18000000, moves);
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

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 15);

			Assert.AreEqual((ulong)0x40, moves);
		}

		// --------------- non-capture moves, same color ---------------

		[TestMethod]
		public unsafe void TestBlackPawnSame1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 53);
			b->Black = Bitboard.Set((b->Black), 53);

			b->Pawns = Bitboard.Set((b->Pawns), 46);
			b->Black = Bitboard.Set((b->Black), 46);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 53);

			Assert.AreEqual((ulong)0x202000000000, moves);
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

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 35);

			Assert.AreEqual((ulong)0x8000000, moves);
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

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 35);

			Assert.AreEqual((ulong)0xC000000, moves);
		}

		[TestMethod]
		public unsafe void TestBlackPawnSame4()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 8);
			b->Black = Bitboard.Set((b->Black), 8);

			b->Pawns = Bitboard.Set((b->Pawns), 1);
			b->Black = Bitboard.Set((b->Black), 1);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 8);

			Assert.AreEqual((ulong)0x1, moves);
		}

		[TestMethod]
		public unsafe void TestBlackBlockedWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 42);
			b->Black = Bitboard.Set((b->Black), 42);

			b->Pawns = Bitboard.Set((b->Pawns), 34);
			b->White = Bitboard.Set((b->White), 34);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 42);

			Assert.AreEqual((ulong)0x0, moves);
		}

		[TestMethod]
		public unsafe void TestBlackBlockedBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Set((b->Pawns), 42);
			b->Black = Bitboard.Set((b->Black), 42);

			b->Pawns = Bitboard.Set((b->Pawns), 34);
			b->Black = Bitboard.Set((b->Black), 34);

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 42);

			Assert.AreEqual((ulong)0x0, moves);
		}

		// Todo: Test En passant Bitboard moves Black
	}
}

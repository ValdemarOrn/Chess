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
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			Assert.AreEqual(5, Board.Board_X(5));
			Assert.AreEqual(5, Board.Board_X(5+8));
			Assert.AreEqual(5, Board.Board_X(5+16));
			Assert.AreEqual(3, Board.Board_X(8*7 + 3));

			Assert.AreEqual(7, Board.Board_Y(7 * 8 + 3));
			Assert.AreEqual(1, Board.Board_Y(1 * 8 + 4));
			Assert.AreEqual(3, Board.Board_Y(3 * 8 + 6));
			Assert.AreEqual(4, Board.Board_Y(4 * 8 + 0));
			Assert.AreEqual(7, Board.Board_Y(7 * 8 + 7));
		}

		[TestMethod]
		public unsafe void TestBoardColorWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 5);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 7);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 47);

			b->White = Bitboard.Bitboard_Set((b->White), 5);
			b->White = Bitboard.Bitboard_Set((b->White), 7);
			b->White = Bitboard.Bitboard_Set((b->White), 47);


			Assert.AreEqual(Colors.White, Board.Board_Color((IntPtr)b, 5));
			Assert.AreEqual(Colors.White, Board.Board_Color((IntPtr)b, 7));
			Assert.AreEqual(Colors.White, Board.Board_Color((IntPtr)b, 47));

			Assert.AreEqual(0, Board.Board_Color((IntPtr)b, 3));
			Assert.AreEqual(0, Board.Board_Color((IntPtr)b, 8));
			Assert.AreEqual(0, Board.Board_Color((IntPtr)b, 46));
		}

		[TestMethod]
		public unsafe void TestBoardColorBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 5);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 7);
			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 47);

			b->Black = Bitboard.Bitboard_Set((b->Black), 5);
			b->Black = Bitboard.Bitboard_Set((b->Black), 7);
			b->Black = Bitboard.Bitboard_Set((b->Black), 47);


			Assert.AreEqual(Colors.Black, Board.Board_Color((IntPtr)b, 5));
			Assert.AreEqual(Colors.Black, Board.Board_Color((IntPtr)b, 7));
			Assert.AreEqual(Colors.Black, Board.Board_Color((IntPtr)b, 47));

			Assert.AreEqual(0, Board.Board_Color((IntPtr)b, 3));
			Assert.AreEqual(0, Board.Board_Color((IntPtr)b, 8));
			Assert.AreEqual(0, Board.Board_Color((IntPtr)b, 46));
		}
	}
}

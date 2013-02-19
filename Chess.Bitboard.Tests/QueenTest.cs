using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Chess.Lib.MoveClasses;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class QueenTest
	{
		[TestMethod]
		public void TestQueenMovesAll()
		{
			for (int i = 0; i < 64; i++)
			{
				var b = new Chess.Board(false);
				b.State[i] = Pieces.Queen | Chess.Colors.White;
				var movesBasic = Chess.Moves.GetMoves(b, i);
				movesBasic = movesBasic.OrderBy(x => x).ToArray();

				var movesFast = Queen.Queen_Read(i, 0);
				var list = Bitboard.Bitboard_BitList(movesFast);
				list = list.OrderBy(x => x).ToArray();

				Assert.AreEqual(movesBasic.Length, list.Length);
				for (int j = 0; j < movesBasic.Length; j++)
					Assert.AreEqual((int)movesBasic[j], (int)list[j]);
			}
		}

		[TestMethod]
		public unsafe void TestWhiteQueen1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Queens = Bitboard.Bitboard_Set((b->Queens), 28);
			b->White = Bitboard.Bitboard_Set((b->White), 28);

			var moves = Moves.Moves_GetMoves((IntPtr)b, 28);
			Assert.AreEqual((ulong)0x11925438EF385492, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteQueen2()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Queens = Bitboard.Bitboard_Set((b->Queens), 7);
			b->White = Bitboard.Bitboard_Set((b->White), 7);

			b->Knights = Bitboard.Bitboard_Set((b->Knights), 4);
			b->White = Bitboard.Bitboard_Set((b->White), 4);

			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 21);
			b->White = Bitboard.Bitboard_Set((b->White), 21);

			b->Pawns = Bitboard.Bitboard_Set((b->Pawns), 31);
			b->White = Bitboard.Bitboard_Set((b->White), 31);

			var moves = Moves.Moves_GetMoves((IntPtr)b, 7);
			Assert.AreEqual((ulong)0x80C060, moves);
		}
	}
}
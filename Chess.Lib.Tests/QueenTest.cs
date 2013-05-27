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
				var b = new Chess.Base.Board(false);
				b.State[i] = Chess.Base.Pieces.Queen | Chess.Base.Colors.White;
				var movesBasic = Chess.Base.Moves.GetMoves(b, i);
				movesBasic = movesBasic.OrderBy(x => x).ToArray();

				var movesFast = Queen.Read(i, 0);
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
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_QUEENS] = Bitboard.Set((b->Boards[Board.BOARD_QUEENS]), 28);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 28);
			Board.GenerateTileMap(b);

			var moves = Moves.GetMoves(b, 28);
			Assert.AreEqual((ulong)0x11925438EF385492, moves);
		}

		[TestMethod]
		public unsafe void TestWhiteQueen2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_QUEENS] = Bitboard.Set((b->Boards[Board.BOARD_QUEENS]), 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 7);

			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 4);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 4);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 21);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 21);

			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 31);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 31);
			Board.GenerateTileMap(b);

			var moves = Moves.GetMoves(b, 7);
			Assert.AreEqual((ulong)0x80C060, moves);
		}
	}
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class KingTest
	{
		[TestMethod]
		public unsafe void WhiteKingTest1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 4);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 4);

			ulong movesKing = MoveClasses.King.Read(4);
			Assert.AreEqual((ulong)0x3828, movesKing);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.GetMoves(b, 4);
			Assert.AreEqual((ulong)0x386C, moves);

			// disallow castling
			b->Castle = 0;

			// standard king moves plus castling moves
			moves = Moves.GetMoves(b, 4);
			Assert.AreEqual((ulong)0x3828, moves);
		}

		[TestMethod]
		public unsafe void WhiteCastleKingsideOnly()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 4);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 4);

			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 2);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 2);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.GetMoves(b, 4);
			Assert.AreEqual((ulong)0x3868, moves);			
		}

		[TestMethod]
		public unsafe void WhiteCastleQueensideOnly()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 4);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 4);

			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 6);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 6);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.GetMoves(b, 4);
			Assert.AreEqual((ulong)0x382C, moves);
		}

		[TestMethod]
		public unsafe void BlackKingTest1()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 60);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 60);

			ulong movesKing = MoveClasses.King.Read(60);
			Assert.AreEqual((ulong)0x2838000000000000, movesKing);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.GetMoves(b, 60);
			Assert.AreEqual((ulong)0x6C38000000000000, moves);

			// disallow castling
			b->Castle = 0;

			// standard king moves plus castling moves
			moves = Moves.GetMoves(b, 60);
			Assert.AreEqual((ulong)0x2838000000000000, moves);
		}

		[TestMethod]
		public unsafe void BlackCastleKingsideOnly()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 60);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 60);

			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 59);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 59);

			// standard king moves plus castling moves
			ulong moves = Moves.GetMoves(b, 60);
			Assert.AreEqual((ulong)0x6038000000000000, moves);
		}

		[TestMethod]
		public unsafe void BlackCastleQueensideOnly()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 60);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 60);

			b->Boards[Board.BOARD_KNIGHTS] = Bitboard.Set((b->Boards[Board.BOARD_KNIGHTS]), 61);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 61);

			// standard king moves plus castling moves
			ulong moves = Moves.GetMoves(b, 60);
			Assert.AreEqual((ulong)0xC38000000000000, moves);
		}

		[TestMethod]
		public unsafe void SwitchKings()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();

			// kings on the wrong side, should NOT allow castling

			//white king at 60
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 60);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 60);

			//black king at 4
			b->Boards[Board.BOARD_KINGS] = Bitboard.Set((b->Boards[Board.BOARD_KINGS]), 4);
			b->Boards[Board.BOARD_BLACK]= Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 4);

			// We have disabled all castling, this should prevent any castling moves
			b->Castle = 0;

			ulong moves = Moves.GetMoves(b, 4);
			Assert.AreEqual((ulong)0x3828, moves);

			moves = Moves.GetMoves(b, 60);
			Assert.AreEqual((ulong)0x2838000000000000, moves);
		}
	}
}

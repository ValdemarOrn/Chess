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
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Kings = Bitboard.Bitboard_Set((b->Kings), 4);
			b->White = Bitboard.Bitboard_Set((b->White), 4);

			ulong movesKing = MoveClasses.King.King_Read(4);
			Assert.AreEqual((ulong)0x3828, movesKing);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 4);
			Assert.AreEqual((ulong)0x386C, moves);

			// disallow castling
			b->Castle = 0;

			// standard king moves plus castling moves
			moves = Moves.Moves_GetMoves((IntPtr)b, 4);
			Assert.AreEqual((ulong)0x3828, moves);
		}

		[TestMethod]
		public unsafe void WhiteCastleKingsideOnly()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Kings = Bitboard.Bitboard_Set((b->Kings), 4);
			b->White = Bitboard.Bitboard_Set((b->White), 4);

			b->Knights = Bitboard.Bitboard_Set((b->Knights), 2);
			b->White = Bitboard.Bitboard_Set((b->White), 2);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 4);
			Assert.AreEqual((ulong)0x3868, moves);			
		}

		[TestMethod]
		public unsafe void BlackKingTest1()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Kings = Bitboard.Bitboard_Set((b->Kings), 60);
			b->Black = Bitboard.Bitboard_Set((b->Black), 60);

			ulong movesKing = MoveClasses.King.King_Read(60);
			Assert.AreEqual((ulong)0x2838000000000000, movesKing);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 60);
			Assert.AreEqual((ulong)0x6C38000000000000, moves);

			// disallow castling
			b->Castle = 0;

			// standard king moves plus castling moves
			moves = Moves.Moves_GetMoves((IntPtr)b, 60);
			Assert.AreEqual((ulong)0x2838000000000000, moves);
		}

		[TestMethod]
		public unsafe void BlackCastleKingsideOnly()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();
			b->Kings = Bitboard.Bitboard_Set((b->Kings), 60);
			b->Black = Bitboard.Bitboard_Set((b->Black), 60);

			b->Knights = Bitboard.Bitboard_Set((b->Knights), 59);
			b->Black = Bitboard.Bitboard_Set((b->Black), 59);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			// standard king moves plus castling moves
			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 60);
			Assert.AreEqual((ulong)0x6038000000000000, moves);
		}

		[TestMethod]
		public unsafe void SwitchKings()
		{
			BoardStruct* b = (BoardStruct*)Board.Board_Create();

			// kings on the wrong side, should NOT allow castling

			//white king at 60
			b->Kings = Bitboard.Bitboard_Set((b->Kings), 60);
			b->White = Bitboard.Bitboard_Set((b->White), 60);

			//black king at 4
			b->Kings = Bitboard.Bitboard_Set((b->Kings), 4);
			b->Black= Bitboard.Bitboard_Set((b->Black), 4);

			// allow all castling
			b->Castle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			ulong moves = Moves.Moves_GetMoves((IntPtr)b, 4);
			Assert.AreEqual((ulong)0x3828, moves);

			moves = Moves.Moves_GetMoves((IntPtr)b, 60);
			Assert.AreEqual((ulong)0x2838000000000000, moves);
		}
	}
}

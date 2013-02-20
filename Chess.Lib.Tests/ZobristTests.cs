using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class ZobristTests
	{
		[TestMethod]
		public void TestInit()
		{
			Zobrist.Zobrist_Init();
			var tile23 = Zobrist.Zobrist_Read(Board.BOARD_WHITE | Board.BOARD_PAWNS, 23);
		}

		[TestMethod]
		public unsafe void TestHashWrong1()
		{
			Zobrist.Zobrist_Init();
			var b = Board.Board_Create();
			Board.Board_SetPiece(b, 10, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.Board_SetPiece(b, 34, Board.PIECE_KING, Board.COLOR_BLACK);
			var hash = Zobrist.Zobrist_Calculate(b);

			var hash2 = Zobrist.Zobrist_Read(Zobrist.Zobrist_Index_Read(Board.PIECE_PAWN | Board.COLOR_WHITE), 10);
			hash2 = hash2 ^ Zobrist.Zobrist_Read(Zobrist.Zobrist_Index_Read(Board.PIECE_KING | Board.COLOR_BLACK), 34);

			Assert.AreNotEqual(hash2, hash);
		}

		[TestMethod]
		public unsafe void TestHashOK1()
		{
			Zobrist.Zobrist_Init();
			var b = Board.Board_Create();
			Board.Board_SetPiece(b, 10, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.Board_SetPiece(b, 34, Board.PIECE_KING, Board.COLOR_BLACK);
			var hash = Zobrist.Zobrist_Calculate(b);

			var hash2 = Zobrist.Zobrist_Read(Zobrist.Zobrist_Index_Read(Board.PIECE_PAWN | Board.COLOR_WHITE), 10);
			hash2 = hash2 ^ Zobrist.Zobrist_Read(Zobrist.Zobrist_Index_Read(Board.PIECE_KING | Board.COLOR_BLACK), 34);

			hash2 = hash2 ^ Zobrist.Zobrist_Read(Zobrist.ZOBRIST_SIDE, Board.COLOR_WHITE);
			hash2 = hash2 ^ Zobrist.Zobrist_Read(Zobrist.ZOBRIST_CASTLING, Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ);
			hash2 = hash2 ^ Zobrist.Zobrist_Read(Zobrist.ZOBRIST_ENPASSANT, 0);

			Assert.AreEqual(hash2, hash);
		}
	}
}

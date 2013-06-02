using System;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public class ZobristTests
	{
		[Test]
		public void TestInit()
		{
			Zobrist.Init();
			var tile23 = Zobrist.Read(Board.BOARD_WHITE | Board.BOARD_PAWNS, 23);
		}

		[Test]
		public unsafe void TestHashWrong1()
		{
			Zobrist.Init();
			var b = Board.Create();
			Board.SetPiece(b, 10, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 34, Board.PIECE_KING, Board.COLOR_BLACK);
			var hash = Zobrist.Calculate(b);

			var hash2 = Zobrist.Read(Zobrist.IndexRead(Board.PIECE_PAWN | Board.COLOR_WHITE), 10);
			hash2 = hash2 ^ Zobrist.Read(Zobrist.IndexRead(Board.PIECE_KING | Board.COLOR_BLACK), 34);

			Assert.AreNotEqual(hash2, hash);
		}

		[Test]
		public unsafe void TestHashOK1()
		{
			Zobrist.Init();
			var b = Board.Create();
			Board.SetPiece(b, 10, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 34, Board.PIECE_KING, Board.COLOR_BLACK);
			var hash = Zobrist.Calculate(b);

			var hash2 = Zobrist.Read(Zobrist.IndexRead(Board.PIECE_PAWN | Board.COLOR_WHITE), 10);
			hash2 = hash2 ^ Zobrist.Read(Zobrist.IndexRead(Board.PIECE_KING | Board.COLOR_BLACK), 34);

			hash2 = hash2 ^ Zobrist.Read(Zobrist.ZOBRIST_SIDE, Board.COLOR_WHITE);
			hash2 = hash2 ^ Zobrist.Read(Zobrist.ZOBRIST_CASTLING, Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ);
			hash2 = hash2 ^ Zobrist.Read(Zobrist.ZOBRIST_ENPASSANT, 0);

			Assert.AreEqual(hash2, hash);
		}

		[Test]
		public unsafe void TestHashBoardSet()
		{
			Zobrist.Init();
			var b = Board.Create();
			Board.SetPiece(b, 10, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 34, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 12, Board.PIECE_QUEEN, Board.COLOR_WHITE);
			Board.SetPiece(b, 61, Board.PIECE_ROOK, Board.COLOR_BLACK);
			var hash = Zobrist.Calculate(b);

			Assert.AreEqual(hash, b->Hash);
		}

		[Test]
		public unsafe void TestHashBoardUnset()
		{
			Zobrist.Init();
			var b = Board.Create();
			Board.Init(b, 1);

			Board.ClearPiece(b, 10);
			Board.ClearPiece(b, 2);
			Board.ClearPiece(b, 62);
			Board.ClearPiece(b, 57);
			Board.ClearPiece(b, 53);
			Board.ClearPiece(b, 30); // empty area

			var hash = Zobrist.Calculate(b);

			Assert.AreEqual(hash, b->Hash);
		}
	}
}

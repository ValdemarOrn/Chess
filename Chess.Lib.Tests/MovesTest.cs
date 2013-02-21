using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class MovesTest
	{
		[TestMethod]
		public unsafe void TestGetCastlingType()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			var type = Moves.GetCastlingType(b, 4, 2);
			Assert.AreEqual(Board.CASTLE_WQ, type);

			type = Moves.GetCastlingType(b, 4, 6);
			Assert.AreEqual(Board.CASTLE_WK, type);

			type = Moves.GetCastlingType(b, 60, 58);
			Assert.AreEqual(Board.CASTLE_BQ, type);

			type = Moves.GetCastlingType(b, 60, 62);
			Assert.AreEqual(Board.CASTLE_BK, type);
		}

		[TestMethod]
		public unsafe void TestGetCastlingTypeWithRook()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_ROOK, Board.COLOR_WHITE);

			var type = Moves.GetCastlingType(b, 4, 2);
			Assert.AreEqual(0, type);

			type = Moves.GetCastlingType(b, 4, 6);
			Assert.AreEqual(0, type);

			type = Moves.GetCastlingType(b, 60, 58);
			Assert.AreEqual(0, type);

			type = Moves.GetCastlingType(b, 60, 62);
			Assert.AreEqual(0, type);
		}

		// test  GetAvailableCastlingTypes(BoardStruct* board, int color);

		[TestMethod]
		public unsafe void TestGetAvailableCastlingTypesAll()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ | Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ | Board.CASTLE_BK, blackCastle);
		}


		[TestMethod]
		public unsafe void TestGetAvailableCastlingTypesWhiteQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 8, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ | Board.CASTLE_BK, blackCastle);
		}

		[TestMethod]
		public unsafe void TestGetAvailableCastlingTypesWhiteKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 15, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ | Board.CASTLE_BK, blackCastle);
		}

		[TestMethod]
		public unsafe void TestGetAvailableCastlingTypesBlackQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 48, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ | Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BK, blackCastle);
		}

		[TestMethod]
		public unsafe void TestGetAvailableCastlingTypesBlackKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 55, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ | Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ, blackCastle);
		}


		// test  GetCastlingMoves(BoardStruct* board, int color);

		[TestMethod]
		public unsafe void TestGetCastlingMovesAll()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x44, whiteCastleBB);
			Assert.AreEqual((ulong)0x4400000000000000, blackCastleBB);
		}


		[TestMethod]
		public unsafe void TestGetCastlingMovesWhiteQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 8, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x40, whiteCastleBB);
			Assert.AreEqual((ulong)0x4400000000000000, blackCastleBB);
		}

		[TestMethod]
		public unsafe void TesGetCastlingMovesWhiteKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 15, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x4, whiteCastleBB);
			Assert.AreEqual((ulong)0x4400000000000000, blackCastleBB);
		}

		[TestMethod]
		public unsafe void TestGetCastlingMovesBlackQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 48, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x44, whiteCastleBB);
			Assert.AreEqual((ulong)0x4000000000000000, blackCastleBB);
		}

		[TestMethod]
		public unsafe void TestGetCastlingMovesBlackKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 55, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x44, whiteCastleBB);
			Assert.AreEqual((ulong)0x400000000000000, blackCastleBB);
		}

		// Todo: Test Moves_GetEnPassantVictimTile()
	}
}

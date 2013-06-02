using System;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public class MovesTest
	{
		[Test]
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

		[Test]
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

		[Test]
		public unsafe void TestGetAvailableCastlingTypesAll()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ | Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ | Board.CASTLE_BK, blackCastle);
		}


		[Test]
		public unsafe void TestGetAvailableCastlingTypesWhiteQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 8, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ | Board.CASTLE_BK, blackCastle);
		}

		[Test]
		public unsafe void TestGetAvailableCastlingTypesWhiteKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 15, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ | Board.CASTLE_BK, blackCastle);
		}

		[Test]
		public unsafe void TestGetAvailableCastlingTypesBlackQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 48, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ | Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BK, blackCastle);
		}

		[Test]
		public unsafe void TestGetAvailableCastlingTypesBlackKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 55, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_WHITE);
			var blackCastle = Moves.GetAvailableCastlingTypes(b, Board.COLOR_BLACK);

			Assert.AreEqual(Board.CASTLE_WQ | Board.CASTLE_WK, whiteCastle);
			Assert.AreEqual(Board.CASTLE_BQ, blackCastle);
		}


		// test  GetCastlingMoves(BoardStruct* board, int color);

		[Test]
		public unsafe void TestGetCastlingMovesAll()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x44, whiteCastleBB);
			Assert.AreEqual((ulong)0x4400000000000000, blackCastleBB);
		}


		[Test]
		public unsafe void TestGetCastlingMovesWhiteQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 8, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x40, whiteCastleBB);
			Assert.AreEqual((ulong)0x4400000000000000, blackCastleBB);
		}

		[Test]
		public unsafe void TesGetCastlingMovesWhiteKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 15, Board.PIECE_KNIGHT, Board.COLOR_BLACK);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x4, whiteCastleBB);
			Assert.AreEqual((ulong)0x4400000000000000, blackCastleBB);
		}

		[Test]
		public unsafe void TestGetCastlingMovesBlackQueenside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 48, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x44, whiteCastleBB);
			Assert.AreEqual((ulong)0x4000000000000000, blackCastleBB);
		}

		[Test]
		public unsafe void TestGetCastlingMovesBlackKingside()
		{
			var b = Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 55, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);

			var whiteCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_WHITE);
			var blackCastleBB = Moves.GetCastlingMoves(b, Board.COLOR_BLACK);

			Assert.AreEqual((ulong)0x44, whiteCastleBB);
			Assert.AreEqual((ulong)0x400000000000000, blackCastleBB);
		}

		[Test]
		public unsafe void TestGetAllMovesWhiteKing()
		{
			var moveList100 = stackalloc Move[100];
			var b = Board.Create();
			b->Castle = 0;
			b->PlayerTurn = Board.COLOR_WHITE;
			Board.SetPiece(b, 0, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 63, Board.PIECE_KING, Board.COLOR_BLACK);

			int moveCount = Moves.GetAllMoves(b, moveList100);

			Assert.AreEqual(3, moveCount);
			Assert.AreEqual(0, moveList100[0].From);
			Assert.AreEqual(1, moveList100[0].To);
			Assert.AreEqual(0, moveList100[1].From);
			Assert.AreEqual(8, moveList100[1].To);
			Assert.AreEqual(0, moveList100[2].From);
			Assert.AreEqual(9, moveList100[2].To);
		}

		[Test]
		public unsafe void TestGetAllMovesKingsPawns()
		{
			var moveList100 = stackalloc Move[100];
			var b = Board.Create();
			b->Castle = 0;
			Board.SetPiece(b, 0, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 9, Board.PIECE_PAWN, Board.COLOR_WHITE);
			Board.SetPiece(b, 63, Board.PIECE_KING, Board.COLOR_BLACK);
			Board.SetPiece(b, 54, Board.PIECE_PAWN, Board.COLOR_BLACK);

			b->PlayerTurn = Board.COLOR_WHITE;

			int moveCount = Moves.GetAllMoves(b, moveList100);
			Assert.AreEqual(4, moveCount);

			// king
			Assert.AreEqual(0, moveList100[0].From);
			Assert.AreEqual(1, moveList100[0].To);
			Assert.AreEqual(0, moveList100[1].From);
			Assert.AreEqual(8, moveList100[1].To);

			// pawn
			Assert.AreEqual(9, moveList100[2].From);
			Assert.AreEqual(17, moveList100[2].To);
			Assert.AreEqual(9, moveList100[3].From);
			Assert.AreEqual(25, moveList100[3].To);

			b->PlayerTurn = Board.COLOR_BLACK;

			moveCount = Moves.GetAllMoves(b, moveList100);
			Assert.AreEqual(4, moveCount);

			// king
			Assert.AreEqual(63, moveList100[2].From);
			Assert.AreEqual(55, moveList100[2].To);
			Assert.AreEqual(63, moveList100[3].From);
			Assert.AreEqual(62, moveList100[3].To);

			// pawn
			Assert.AreEqual(54, moveList100[0].From);
			Assert.AreEqual(38, moveList100[0].To);
			Assert.AreEqual(54, moveList100[1].From);
			Assert.AreEqual(46, moveList100[1].To);
		}

		// Todo: Test Moves_GetEnPassantVictimTile()
	}
}

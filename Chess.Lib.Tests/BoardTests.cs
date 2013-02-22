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
			BoardStruct* b = (BoardStruct*)Board.Create();
			Assert.AreEqual(5, Board.X(5));
			Assert.AreEqual(5, Board.X(5+8));
			Assert.AreEqual(5, Board.X(5+16));
			Assert.AreEqual(3, Board.X(8*7 + 3));

			Assert.AreEqual(7, Board.Y(7 * 8 + 3));
			Assert.AreEqual(1, Board.Y(1 * 8 + 4));
			Assert.AreEqual(3, Board.Y(3 * 8 + 6));
			Assert.AreEqual(4, Board.Y(4 * 8 + 0));
			Assert.AreEqual(7, Board.Y(7 * 8 + 7));
		}

		[TestMethod]
		public unsafe void TestBoardColorWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 5);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 47);

			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 5);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 7);
			b->Boards[Board.BOARD_WHITE] = Bitboard.Set((b->Boards[Board.BOARD_WHITE]), 47);


			Assert.AreEqual(Colors.White, Board.Color(b, 5));
			Assert.AreEqual(Colors.White, Board.Color(b, 7));
			Assert.AreEqual(Colors.White, Board.Color(b, 47));

			Assert.AreEqual(0, Board.Color(b, 3));
			Assert.AreEqual(0, Board.Color(b, 8));
			Assert.AreEqual(0, Board.Color(b, 46));
		}

		[TestMethod]
		public unsafe void TestBoardColorBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 5);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 7);
			b->Boards[Board.BOARD_PAWNS] = Bitboard.Set((b->Boards[Board.BOARD_PAWNS]), 47);

			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 5);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 7);
			b->Boards[Board.BOARD_BLACK] = Bitboard.Set((b->Boards[Board.BOARD_BLACK]), 47);


			Assert.AreEqual(Colors.Black, Board.Color(b, 5));
			Assert.AreEqual(Colors.Black, Board.Color(b, 7));
			Assert.AreEqual(Colors.Black, Board.Color(b, 47));

			Assert.AreEqual(0, Board.Color(b, 3));
			Assert.AreEqual(0, Board.Color(b, 8));
			Assert.AreEqual(0, Board.Color(b, 46));
		}

		[TestMethod]
		public unsafe void TestBoardInit()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 0);

			byte allCastle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			Assert.AreEqual((ulong)0, b->Boards[Board.BOARD_WHITE]);
			Assert.AreEqual((ulong)0, b->Boards[Board.BOARD_BLACK]);
			Assert.AreEqual(allCastle, b->Castle);
		}

		[TestMethod]
		public unsafe void TestBoardInitPieces()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 1);

			byte allCastle = Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ;

			Assert.AreEqual((ulong)0xFFFF, b->Boards[Board.BOARD_WHITE]);
			Assert.AreEqual((ulong)0xFFFF000000000000, b->Boards[Board.BOARD_BLACK]);
			Assert.AreEqual((ulong)0xFF00000000FF00, b->Boards[Board.BOARD_PAWNS]);
			Assert.AreEqual((ulong)0x1000000000000010, b->Boards[Board.BOARD_KINGS]);
			Assert.AreEqual(allCastle, b->Castle);
		}

		[TestMethod]
		public unsafe void TestBoardAttacks()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 13, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 50, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_KNIGHT, Board.COLOR_WHITE);
			Board.SetPiece(b, 8, Board.PIECE_PAWN, Board.COLOR_WHITE);

			Board.SetPiece(b, 46, Board.PIECE_PAWN, Board.COLOR_BLACK);

			new Moves(); // load moves

			var whiteMap = Board.AttackMap(b, Board.COLOR_WHITE);
			var blackMap = Board.AttackMap(b, Board.COLOR_BLACK);
			
			Assert.AreEqual((ulong)0x725470, whiteMap);
			Assert.AreEqual((ulong)0xE0A0EA000000000, blackMap);
		}

		[TestMethod]
		public unsafe void TestGetCastlingAll()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();

			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 7, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 56, Board.PIECE_ROOK, Board.COLOR_BLACK);
			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_BLACK);

			var castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ, castling);
		}

		[TestMethod]
		public unsafe void TestGetCastlingNone()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();

			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			var castling = Board.GetCastling(b);
			Assert.AreEqual(0, castling);
		}

		[TestMethod]
		public unsafe void TestGetCastlingNone2()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();

			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 7, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 56, Board.PIECE_ROOK, Board.COLOR_BLACK);
			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_BLACK);

			b->Castle = 0;

			var castling = Board.GetCastling(b);
			Assert.AreEqual(0, castling);
		}

		[TestMethod]
		public unsafe void TestGetCastlingSingleRooks()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();

			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);


			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_WHITE);
			var castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_WQ, castling);
			Board.ClearPiece(b, 0);

			Board.SetPiece(b, 7, Board.PIECE_ROOK, Board.COLOR_WHITE);
			castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_WK, castling);
			Board.ClearPiece(b, 7);

			Board.SetPiece(b, 56, Board.PIECE_ROOK, Board.COLOR_BLACK);
			castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_BQ, castling);
			Board.ClearPiece(b, 56);

			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_BLACK);
			castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_BK, castling);
			Board.ClearPiece(b, 63);
		}

		[TestMethod]
		public unsafe void TestGetCastlingSingleAllowances()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();

			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 7, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 56, Board.PIECE_ROOK, Board.COLOR_BLACK);
			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_BLACK);

			b->Castle = Board.CASTLE_WQ;
			var castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_WQ, castling);

			b->Castle = Board.CASTLE_WK;
			castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_WK, castling);

			b->Castle = Board.CASTLE_BQ;
			castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_BQ, castling);

			b->Castle = Board.CASTLE_BK;
			castling = Board.GetCastling(b);
			Assert.AreEqual(Board.CASTLE_BK, castling);
		}

		[TestMethod]
		public unsafe void TestCheckStateNone()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_BLACK);

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			var check = Board.GetCheckState(b);
			Assert.AreEqual(0, check);
		}

		[TestMethod]
		public unsafe void TestCheckStateWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_BLACK);

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			var check = Board.GetCheckState(b);
			Assert.AreEqual(1, check);
		}

		[TestMethod]
		public unsafe void TestCheckStateBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_WHITE);

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			var check = Board.GetCheckState(b);
			Assert.AreEqual(1, check);
		}

		[TestMethod]
		public unsafe void TestIsCheckedNone()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_WHITE);
			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_BLACK);

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			var check = Board.IsChecked(b, Board.COLOR_WHITE);
			Assert.AreEqual(0, check);
			check = Board.IsChecked(b, Board.COLOR_BLACK);
			Assert.AreEqual(0, check);
		}

		[TestMethod]
		public unsafe void TestIsCheckedWhite()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 0, Board.PIECE_ROOK, Board.COLOR_BLACK);

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			var check = Board.IsChecked(b, Board.COLOR_WHITE);
			Assert.AreEqual(1, check);
			check = Board.IsChecked(b, Board.COLOR_BLACK);
			Assert.AreEqual(0, check);
		}

		[TestMethod]
		public unsafe void TestIsCheckedBlack()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.SetPiece(b, 4, Board.PIECE_KING, Board.COLOR_WHITE);
			Board.SetPiece(b, 60, Board.PIECE_KING, Board.COLOR_BLACK);

			Board.SetPiece(b, 63, Board.PIECE_ROOK, Board.COLOR_WHITE);

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);

			var check = Board.IsChecked(b, Board.COLOR_WHITE);
			Assert.AreEqual(0, check);
			check = Board.IsChecked(b, Board.COLOR_BLACK);
			Assert.AreEqual(1, check);
		}

		[TestMethod]
		public unsafe void TestMakePawn()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 1);
			Board.Make(b, 12, 28);

			Assert.AreEqual(20, b->EnPassantTile);
			Assert.AreEqual(true, Bitboard.Get(b->Boards[Board.BOARD_PAWNS], 28));
			Assert.AreEqual(false, Bitboard.Get(b->Boards[Board.BOARD_PAWNS], 12));
		}

		[TestMethod]
		public unsafe void TestMakeRooks()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 1);
			Board.Make(b, 6, 21);

			Assert.AreEqual(1, b->CurrentMove);
			Assert.AreEqual(0, b->EnPassantTile);
			Assert.AreEqual(true, Bitboard.Get(b->Boards[Board.BOARD_KNIGHTS], 21));
			Assert.AreEqual(false, Bitboard.Get(b->Boards[Board.BOARD_WHITE], 6));
			Assert.AreEqual(Board.COLOR_BLACK, b->PlayerTurn);
			Board.Make(b, 62, 45);

			Assert.AreEqual(2, b->CurrentMove);
			Assert.AreEqual(2, b->FiftyMoveRulePlies);
			Assert.AreEqual(Board.COLOR_WHITE, b->PlayerTurn);
			Assert.AreEqual(true, Bitboard.Get(b->Boards[Board.BOARD_KNIGHTS], 45));
			Assert.AreEqual(false, Bitboard.Get(b->Boards[Board.BOARD_BLACK], 62));
		}

		[TestMethod]
		public unsafe void TestMakeRooksUnmake()
		{
			BoardStruct* b = (BoardStruct*)Board.Create();
			Board.Init(b, 1);

			Board.Make(b, 6, 21);
			Board.Make(b, 62, 45);

			Assert.AreEqual(2, b->CurrentMove);

			Board.Unmake(b);
			Board.Unmake(b);

			Assert.AreEqual(Zobrist.Calculate(b), b->Hash);
			Assert.AreEqual(Board.AttackMap(b, Board.COLOR_WHITE), b->AttacksWhite);
			Assert.AreEqual(Board.AttackMap(b, Board.COLOR_BLACK), b->AttacksBlack);
			Assert.AreEqual(Board.CASTLE_BK | Board.CASTLE_BQ | Board.CASTLE_WK | Board.CASTLE_WQ, b->Castle);
			Assert.AreEqual(Board.COLOR_WHITE, b->PlayerTurn);
			Assert.AreEqual(0, b->CurrentMove);
		}

		// Todo: Test unmake en passant
		// Todo: Test unmake castling


		// Todo: Test Board_Copy
		// Todo: Test Board_Unmake
		// Todo: Test Board_Promote
		
	}
}

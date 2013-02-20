using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct BoardStruct
	{
		public fixed ulong Boards[8];

		public ulong Hash;
		public uint CurrentMove;

		public byte PlayerTurn;
		public byte EnPassantTile;
		public byte FiftyMoveRulePlies;
		public byte Castle;
		public byte CheckmateState;

		public Move* Moves;
	}

	public class Board
	{
		public const int COLOR_WHITE = 1 << 4;
		public const int COLOR_BLACK = 2 << 4;

		public const int PIECE_PAWN = 2;
		public const int PIECE_KNIGHT = 3;
		public const int PIECE_BISHOP = 4;
		public const int PIECE_ROOK = 5;
		public const int PIECE_QUEEN = 6;
		public const int PIECE_KING = 7;

		public const int CASTLE_WK = 1 << 0;
		public const int CASTLE_WQ = 1 << 1;
		public const int CASTLE_BK = 1 << 2;
		public const int CASTLE_BQ = 1 << 3;

		public const int CHECK_NONE = 0;
		public const int CHECK_CHECK = 1;
		public const int CHECK_MATE = 2;

		public const int BOARD_WHITE = 0;
		public const int BOARD_BLACK = 1;
		public const int BOARD_PAWNS = 2;
		public const int BOARD_KNIGHTS = 3;
		public const int BOARD_BISHOPS = 4;
		public const int BOARD_ROOKS = 5;
		public const int BOARD_QUEENS = 6;
		public const int BOARD_KINGS = 7;

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern BoardStruct* Board_Create();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Board_Delete(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern IntPtr Board_Copy(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Board_Init(BoardStruct* board, int setPieces);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_X(int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Y(int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Board_Color(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Board_Piece(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Board_SetPiece(BoardStruct* board, int square, int pieceType, int color);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Board_ClearPiece(BoardStruct* board, int square);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Board_Make(BoardStruct* board, int from, int to, int verifyLegalMove);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Board_MakeMove(BoardStruct* board, IntPtr move, int verifyLegalMove);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Board_Unmake(BoardStruct* board, IntPtr move, int verifyLegalMove);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Board_Promote(BoardStruct* board, int square, int pieceType);



		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Board_CheckCastling(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Board_Promote(BoardStruct* board);
	}
}

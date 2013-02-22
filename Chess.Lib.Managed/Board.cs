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

		public ulong AttacksWhite;
		public ulong AttacksBlack;
		public ulong Hash;

		public int CurrentMove;
		public byte PlayerTurn;

		public byte EnPassantTile;
		public byte FiftyMoveRulePlies;
		public byte Castle;
		public byte CheckState;

		public MoveHistory* MoveHistory;
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

		public const int BOARD_WHITE = 0;
		public const int BOARD_BLACK = 1;
		public const int BOARD_PAWNS = 2;
		public const int BOARD_KNIGHTS = 3;
		public const int BOARD_BISHOPS = 4;
		public const int BOARD_ROOKS = 5;
		public const int BOARD_QUEENS = 6;
		public const int BOARD_KINGS = 7;

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Create", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern BoardStruct* Create();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Delete", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Delete(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Copy", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern IntPtr Copy(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Init", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Init(BoardStruct* board, int setPieces);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_X", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int X(int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Y", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Y(int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Color", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Color(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Piece", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Piece(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_SetPiece", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void SetPiece(BoardStruct* board, int square, int pieceType, int color);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_ClearPiece", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void ClearPiece(BoardStruct* board, int square);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_AttackMap", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong AttackMap(BoardStruct* board, int color);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Make", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Make(BoardStruct* board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Unmake", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Unmake(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_Promote", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Promote(BoardStruct* board, int square, int pieceType);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_GetCastling", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern byte GetCastling(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_GetCheckState", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern byte GetCheckState(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Board_IsChecked", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int IsChecked(BoardStruct* board, int color);
	}
}

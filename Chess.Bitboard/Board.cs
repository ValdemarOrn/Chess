using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public struct BoardStruct
	{
		public ulong White;
		public ulong Black;

		public ulong Pawns;
		public ulong Knights;
		public ulong Bishops;
		public ulong Rooks;
		public ulong Queens;
		public ulong Kings;

		public uint MoveCount;

		public byte PlayerTurn;
		public byte EnPassantTile;
		public byte FiftyMoveRulePlies;

		public byte Castle;

        public uint CurrentMove;
        public IntPtr Moves;
	}

	public class Board
	{
		public const int COLOR_WHITE = 1 << 4;
		public const int COLOR_BLACK = 2 << 4;

		public const int PIECE_PAWN = 1;
		public const int PIECE_ROOK = 2;
		public const int PIECE_BISHOP = 3;
		public const int PIECE_KNIGHT = 4;
		public const int PIECE_QUEEN = 5;
		public const int PIECE_KING = 6;

		public const int CASTLE_WK = 1 << 1;
		public const int CASTLE_WQ = 1 << 2;
		public const int CASTLE_BK = 1 << 3;
		public const int CASTLE_BQ = 1 << 4;

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr Board_Create();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Board_Delete(IntPtr board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr Board_Copy(IntPtr board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_X(int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Y(int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Color(IntPtr board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Piece(IntPtr board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Board_SetPiece(IntPtr board, int square, int pieceType, int color);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Board_ClearPiece(IntPtr board, int square);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Make(IntPtr board, int from, int to, int verifyLegalMove);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_MakeMove(IntPtr board, IntPtr move, int verifyLegalMove);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Unmake(IntPtr board, IntPtr move, int verifyLegalMove);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Board_Promote(IntPtr board,  int square, int pieceType);



		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Board_CheckCastling(IntPtr board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Board_InitBoard(IntPtr board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Board_Promote(IntPtr board);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Chess.Lib.MoveClasses;

namespace Chess.Lib
{
	public class Moves
	{
        static Moves()
        {
            // initialize the static constructors
            new Pawn();
            new Rook();
            new Knight();
            new Bishop();
            new King();
			new Queen();
        }

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Moves_GetMoves(IntPtr board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Moves_GetAttacks(IntPtr board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Moves_GetMoves(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Moves_GetAttacks(BoardStruct* board, int tile);

		// ------------- Specific Moves --------------

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte Moves_IsCastlingMove(IntPtr board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Moves_GetCastlingMoves(IntPtr board, int from);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Moves_CanPromote(IntPtr board, int square);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Moves_IsCaptureMove(IntPtr board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Moves_IsEnPassantCapture(IntPtr board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Moves_GetEnPassantVictimTile(IntPtr board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Moves_GetEnPassantTile(IntPtr board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Moves_GetEnPassantMove(IntPtr board, int from);

	}
}

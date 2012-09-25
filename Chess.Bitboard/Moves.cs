using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Bitboard
{
	public class Moves
	{
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Moves_GetMoves(IntPtr board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Moves_GetAttacks(IntPtr board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Moves_GetMoves(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Moves_GetAttacks(BoardStruct* board, int tile);

	}
}

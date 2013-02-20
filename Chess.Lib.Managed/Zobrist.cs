using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	public class Zobrist
	{
		public const int ZOBRIST_SIDE = 12;
		public const int ZOBRIST_CASTLING = 13;
		public const int ZOBRIST_ENPASSANT = 14;

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Zobrist_Init();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Zobrist_Calculate(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte Zobrist_Index_Read(int pieceAndColor);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Zobrist_Read(int index, int square);
	}
}

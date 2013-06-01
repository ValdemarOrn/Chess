using Chess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	public class Zobrist
	{
		static Zobrist()
		{
			Init();
		}

		public const int ZOBRIST_SIDE = 12;
		public const int ZOBRIST_CASTLING = 13;
		public const int ZOBRIST_ENPASSANT = 14;

		public static unsafe ulong Calculate(string fenString)
		{
			var bx = Notation.ReadFEN(fenString);
			var b = Helpers.ManagedBoardToNative(bx);
			var hash = Calculate(b);
			return hash;
		}

		[DllImport("Chess.Lib.dll", EntryPoint = "Zobrist_Init", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Init();

		[DllImport("Chess.Lib.dll", EntryPoint = "Zobrist_Calculate", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Calculate(BoardStruct* board);

		[DllImport("Chess.Lib.dll", EntryPoint = "Zobrist_IndexRead", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern byte IndexRead(int pieceAndColor);

		[DllImport("Chess.Lib.dll", EntryPoint = "Zobrist_Read", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Read(int index, int square);
	}
}

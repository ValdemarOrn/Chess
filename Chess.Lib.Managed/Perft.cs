using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	public class Perft
	{
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Perft_Search", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong Search(BoardStruct* board, int depth);
	}
}

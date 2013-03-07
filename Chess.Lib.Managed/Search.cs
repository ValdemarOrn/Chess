using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	public class Search
	{
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Search_SearchPos", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern MoveSmall SearchPos(BoardStruct* board, int searchDepth);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct SearchStats
	{
		public ulong TotalNodeCount;
		public ulong EvalNodeCount;
		public ulong QuiescentNodeCount;
		public ulong NoneNodeCount;

		public ulong CutNodeCount;
		public ulong PVNodeCount;
		public ulong AllNodeCount;

		public ulong BestMoveIndexSum;
		public ulong CutMoveIndexSum;

		public fixed ulong NodesAtPly[Search.Search_PlyMax];
		public fixed ulong MovesAtPly[Search.Search_PlyMax];
	}

	public class Search
	{
		public const int Search_PlyMax = 20;		

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Search_SearchPos", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern MoveSmall SearchPos(BoardStruct* board, int searchDepth);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Search_GetSearchStats", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern SearchStats* GetSearchStats();
	}
}

﻿using System;
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
		public ulong QuiescentNodeCount;
		public ulong EvalNodeCount;

		public ulong CutNodeCount;
		public ulong PVNodeCount;
		public ulong AllNodeCount;

		public ulong QCutNodeCount;
		public ulong QPVNodeCount;
		public ulong QAllNodeCount;

		public ulong HashHitsCount;
		public ulong HashFullHitCount;

		public ulong EvalHits;
		public ulong EvalTotal;

		public ulong PruneDelta;
		public ulong PruneBadCaptures;

		public fixed int BestMoveIndex[100];
		public fixed int CutMoveIndex[100];

		public fixed int QBestMoveIndex[100];
		public fixed int QCutMoveIndex[100];

		public fixed ulong NodesAtPly[Search.Search_PlyMax];
		public fixed ulong MovesAtPly[Search.Search_PlyMax];

		
	}

	public class Search
	{
		public const int Search_PlyMax = 40;		

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Search_SearchPos", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern MoveSmall SearchPos(BoardStruct* board, int searchDepth);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Search_GetSearchStats", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern SearchStats* GetSearchStats();
	}
}
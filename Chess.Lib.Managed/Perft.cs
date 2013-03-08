using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct PerftEntry
	{
		public byte From;
		public byte To;
		public ulong Count;

	}

	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct PerftResults
	{
		public int StartDepth;
		public PerftEntry* Entries;
		public int EntryCount;
		public ulong Total;
	}

	public class Perft
	{
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Perft_Search", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern PerftResults* Search(BoardStruct* board, int depth);
	}
}

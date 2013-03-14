using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct TTableEntry
	{
		public ulong Hash;

		public byte BestMoveFrom;
		public byte BestMoveTo;

		public byte NodeType;
		public byte Depth;

		public int Score;
	}

	public class TTable
	{
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "TTable_GetTableSize", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int GetTableSize();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "TTable_Init", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Init(int sizeMB);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "TTable_Delete", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern void Delete();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "TTable_Read", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern TTableEntry* Read(ulong hash);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "TTable_Insert", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern bool Insert(TTableEntry* entry);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib
{
	public class Bitboard
	{
		public static unsafe ulong Bitboard_Make(params int[] tiles)
		{
			ulong bitboard = 0;
			foreach (var tile in tiles)
				SetRef(ref bitboard, tile);

			return bitboard;
		}

		/// <summary>
		/// Generate a string representation from the bitboard
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static string ToString(ulong val)
		{
			var lines = new string[8];
			for(int i=0; i<8; i++)
				lines[i] = "";

			for(int i=0; i<8; i++)
				for(int j=0; j<8; j++)
					lines[i] += ((val >> (i*8+j)) & 0x01).ToString();

			var output = 
				lines[7] + "\n" + 
				lines[6] + "\n" + 
				lines[5] + "\n" + 
				lines[4] + "\n" + 
				lines[3] + "\n" + 
				lines[2] + "\n" + 
				lines[1] + "\n" + 
				lines[0];

			return output;
		}

		// --------------------- Bitscan Operations ---------------------

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_Unset", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Unset(ulong val, int index);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_Set", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Set(ulong val, int index);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_UnsetRef", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void UnsetRef(ref ulong val, int index);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_SetRef", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetRef(ref ulong val, int index);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_Get", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool Get(ulong val, int index);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_GetRef", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern bool GetRef(ref ulong val, int index);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_ForwardBit", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ForwardBit(ulong value);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_ReverseBit", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int ReverseBit(ulong value);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_PopCount", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int PopCount(ulong value);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Bitboard_BitList", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern int BitListIntPtr(ulong value, IntPtr outputList_s64);

		/// <summary>
		/// Returns a list containing the index of all set bits in a bitboard
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static byte[] Bitboard_BitList(ulong value)
		{
			unsafe
			{
				byte* list = stackalloc byte[64];
				int count = BitListIntPtr(value, (IntPtr)list);

				var output = new byte[count];
				Marshal.Copy((IntPtr)list, output, 0, count);

				return output;
			}
		}

	}
}

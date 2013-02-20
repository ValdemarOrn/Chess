using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib
{
	public class Bitboard
	{
		public static ulong Bitboard_Unset(ulong val, int index)
		{
			ulong inv = ~(ulong)((ulong)1 << index);
			return val & inv;
		}

		public static ulong Bitboard_Set(ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			return val | mask;
		}

		public static void Bitboard_UnsetRef(ref ulong val, int index)
		{
			ulong inv = ~(ulong)((ulong)1 << index);
			val = val & inv;
		}

		public static void Bitboard_SetRef(ref ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			val = val | mask;
		}

		public static bool Bitboard_Get(ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			return (val & mask) > 0;
		}

		public static ulong Bitboard_Make(params int[] tiles)
		{
			ulong bitboard = 0;
			foreach (var tile in tiles)
				Bitboard_SetRef(ref bitboard, tile);

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

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_ForwardBit(ulong value);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_ReverseBit(ulong value);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_PopCount(ulong value);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Bitboard_BitList")]
		public static extern int Bitboard_BitList_IntPtr(ulong value, IntPtr outputList_s64);

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
				int count = Bitboard_BitList_IntPtr(value, (IntPtr)list);

				var output = new byte[count];
				Marshal.Copy((IntPtr)list, output, 0, count);

				return output;
			}
		}

	}
}

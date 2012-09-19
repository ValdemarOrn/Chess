using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MagicBitboard
{
	public class Bitboard
	{
		public static ulong Unset(ulong val, int index)
		{
			ulong inv = ~(ulong)((ulong)1 << index);
			return val & inv;
		}

		public static ulong Set(ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			return val | mask;
		}

		public static void Unset(ref ulong val, int index)
		{
			ulong inv = ~(ulong)((ulong)1 << index);
			val = val & inv;
		}

		public static void Set(ref ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			val = val | mask;
		}

		public static bool Get(ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			return (val & mask) > 0;
		}

		public static ulong Make(params int[] tiles)
		{
			ulong bitboard = 0;
			foreach (var tile in tiles)
				Set(ref bitboard, tile);

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

		[DllImport("..\\..\\..\\FastOps\\x64\\Debug\\FastOps.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_ForwardBit(ulong value);

		[DllImport("..\\..\\..\\FastOps\\x64\\Debug\\FastOps.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_ReverseBit(ulong value);

		[DllImport("..\\..\\..\\FastOps\\x64\\Debug\\FastOps.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_PopCount(ulong value);

		[DllImport("..\\..\\..\\FastOps\\x64\\Debug\\FastOps.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern int Bitboard_TryMany();

		

	}
}

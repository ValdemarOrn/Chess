using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public static bool Get(ulong val, int index)
		{
			ulong mask = ((ulong)1 << index);
			return (val & mask) > 0;
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


        [System.Runtime.InteropServices.DllImport("..\\..\\..\\Release\\FastOps.dll", 
            SetLastError = true, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern uint FirstBit(ulong value);

        [System.Runtime.InteropServices.DllImport("..\\..\\..\\Release\\FastOps.dll",
            SetLastError = true, CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern uint TryMany();

	}
}

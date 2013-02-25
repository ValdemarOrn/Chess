using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.EGTB
{
	class Zob
	{
		public static ulong[,] Keys = new ulong[16, 64];
		public static byte[] Index = new byte[256];

		public static void Load()
		{
			for (int i = 0; i < 256; i++)
				Index[i] = Zobrist.IndexRead(i);

			for (int i = 0; i < 16; i++)
				for (int j = 0; j < 64; j++)
					Keys[i,j] = Zobrist.Read(i, j);
		}
	}
}

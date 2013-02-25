using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Chess.Lib.ZobristGen
{
	class Program
	{
		static void Main(string[] args)
		{
			var rand = new RNGCryptoServiceProvider();
			var bytes = new byte[8];

			ulong[] vals = new ulong[16 * 64];

			for (int i = 0; i < vals.Length; i++)
			{
				rand.GetBytes(bytes);
				vals[i] = GetULong(bytes);
			}

			StringBuilder data = new StringBuilder();
			data.AppendLine("uint64_t Zobrist_Keys[16][64] = {");

			for (int x = 0; x < 16; x++)
			{
				data.AppendLine("\t{");
				for (int y = 0; y < 64; y++)
				{
					var val = (x == 15) ? (ulong)0 : vals[x * 64 + y];
					data.AppendLine(String.Format("\t\t0x{0:X},", val));
				}
				data.AppendLine("\t},");
			}

			data.AppendLine("};");

			var output = data.ToString();
			Console.Write(output);
		}

		private static ulong GetULong(byte[] bytes)
		{
			ulong val = 0;
			val |= ((ulong)bytes[0] << 0);
			val |= ((ulong)bytes[1] << 8);
			val |= ((ulong)bytes[2] << 16);
			val |= ((ulong)bytes[3] << 24);
			val |= ((ulong)bytes[4] << 32);
			val |= ((ulong)bytes[5] << 40);
			val |= ((ulong)bytes[6] << 48);
			val |= ((ulong)bytes[7] << 56);
			return val;
		}
	}
}

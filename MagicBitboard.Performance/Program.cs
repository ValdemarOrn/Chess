using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicBitboard.Performance
{
	class Program
	{
		static void Main(string[] args)
		{
			for (int idx = 0; idx < 64; idx++)
			{
				//int idx = 0;

				var perms = Bitboard.GetRookPermutations(idx);
				var map = new Dictionary<ulong, ulong>();

				foreach (var perm in perms)
				{
					map[perm] = Bitboard.GetRookMoves(perm, idx);
				}

				int count = map.Select(x => x.Value).Distinct().Count();

				// --------------------------------
				int bits = 14; // 2^14
				Console.WriteLine("Testing at " + bits + " bits = " + (Math.Pow(2, bits) / 1024) + " kilobytes per position");
				var start = DateTime.Now;

				Bitboard.FindMagic(map, bits);

				Console.WriteLine("Time: " + (DateTime.Now - start).TotalMilliseconds + " ms");

			}
			Console.ReadLine();
		}
	}
}

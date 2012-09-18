using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicBitboard
{
	public class Bitboard
	{
		public static ulong[] RookVectors;

		static Bitboard()
		{
			GenRookVectors();
		}

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

		static void GenRookVectors()
		{
			RookVectors = new ulong[64];

			for (int i = 0; i < 64; i++)
			{
				ulong Move = 0;

				int x = i % 8;
				int y = i >> 3; // i/8

				// mark all in x direction, from file 2-7
				for (int k = 1; k < 7; k++)
					Move |= ((ulong)1) << (y * 8 + k);

				// mark all in y direction, from rank 2-7
				for (int k = 1; k < 7; k++)
					Move |= ((ulong)1) << (k * 8 + x);

				Move = Unset(Move, i);

				RookVectors[i] = Move;
			}
		}

		/// <summary>
		/// Gets all permutations possible for a rook in that position.
		/// checks all possible variations of blockers 
		/// (between 1024-4096 possibilities, depending on rook position)
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static List<ulong> GetRookPermutations(int pos)
		{
			var variations = new List<ulong>();

			var vector = RookVectors[pos];
			var str = ToString(vector);
			List<int> bitlist = new List<int>();
			for (int i = 0; i < 64; i++)
			{
				if (Get(vector, i))
					bitlist.Add(i);
			}

			// scroll through all permutations from 0...max
			int max = 1 << bitlist.Count;
			for (int val = 0; val < max; val++)
			{
				ulong permutation = 0;

				// set bits in the variation
				for(int b=0; b < bitlist.Count; b++)
				{
					if (Get((ulong)val, b))
						permutation = Set(permutation, bitlist[b]);
					else
						permutation = Unset(permutation, bitlist[b]);
				}

				variations.Add(permutation);
			}

			return variations;
		}

		public static ulong GetRookMoves(ulong permutation, int index)
		{
			ulong moves = 0;
			int target = 0;

			// Move up
			target = index + 8;
			while (target < 64)
			{
				moves = Set(moves, target);
				if (Get(permutation, target)) // check for blockers
					break;
				target += 8;
			}

			// Move down
			target = index - 8;
			while (target >= 0)
			{
				moves = Set(moves, target);
				if (Get(permutation, target)) // check for blockers
					break;
				target -= 8;
			}

			// Move right
			target = index + 1;
			while (Board.X(target) > Board.X(index))
			{
				moves = Set(moves, target);
				if (Get(permutation, target)) // check for blockers
					break;
				target++;
			}

			// Move left
			target = index - 1;
			while (Board.X(target) < Board.X(index))
			{
				moves = Set(moves, target);
				if (Get(permutation, target)) // check for blockers
					break;
				target--;
			}

			return moves;
		}

		/// <summary>
		/// Brute forces the magic constant the provides perfect hashing for all dictionary keys to values
		/// </summary>
		/// <param name="?"></param>
		/// <returns></returns>
		public static ulong FindMagic(Dictionary<ulong, ulong> map, int keyBits)
		{
			ulong iterations = 0;

			ulong size = (ulong)(1 << keyBits);
			var r = new Random(0);
			byte[] buf = new byte[8];

			ulong[] table = new ulong[size];
			ulong magic = 0;

			while(true)
			{
				// clear table
				for (int i = 0; i < (int)size; i++)
					table[i] = 0;

				iterations++;

				r.NextBytes(buf);
				magic = BitConverter.ToUInt64(buf, 0);
				bool success = true;

				foreach (var kvp in map)
				{
					int idx = (int)((kvp.Key * magic) >> (64 - keyBits));
					if (table[idx] == 0 || table[idx] == kvp.Value)
						table[idx] = kvp.Value;
					else
					{
						success = false;
						break;
					}
				}

				if (success)
					break;
			}

			Console.WriteLine("Completed in " + iterations + " iterations");
			return magic;
		}
	}
}

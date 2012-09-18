using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicBitboard
{
	public sealed class BitboardRook
	{
		public static ulong[] RookVectors;

		static BitboardRook()
		{
			RookVectors = GetRookVectors();
		}

		/// <summary>
		/// creates move vectors with no blockers on the board for all 64 positions of the rook
		/// </summary>
		static ulong[] GetRookVectors()
		{
			var vectors = new ulong[64];

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

				Move = Bitboard.Unset(Move, i);

				vectors[i] = Move;
			}

			return vectors;
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
			var str = Bitboard.ToString(vector);
			List<int> bitlist = new List<int>();
			for (int i = 0; i < 64; i++)
			{
				if (Bitboard.Get(vector, i))
					bitlist.Add(i);
			}

			// scroll through all permutations from 0...max
			int max = 1 << bitlist.Count;
			for (int val = 0; val < max; val++)
			{
				ulong permutation = 0;

				// set bits in the variation
				for (int b = 0; b < bitlist.Count; b++)
				{
					if (Bitboard.Get((ulong)val, b))
						permutation = Bitboard.Set(permutation, bitlist[b]);
					else
						permutation = Bitboard.Unset(permutation, bitlist[b]);
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
				moves = Bitboard.Set(moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target += 8;
			}

			// Move down
			target = index - 8;
			while (target >= 0)
			{
				moves = Bitboard.Set(moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target -= 8;
			}

			// Move right
			target = index + 1;
			while (Board.X(target) > Board.X(index))
			{
				moves = Bitboard.Set(moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target++;
			}

			// Move left
			target = index - 1;
			while (Board.X(target) < Board.X(index))
			{
				moves = Bitboard.Set(moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target--;
			}

			return moves;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Chess.Bitboard.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Chess.Bitboard.Calculate")]

namespace Chess.Bitboard
{
	public sealed class Rook
	{
		internal static ulong[] RookVectors;

		static Rook()
		{
			RookVectors = GetRookVectors();
		}

		/// <summary>
		/// This operation generates all permutations and their corresponding moves
		/// and calls Rook_SetupTables in unmanaged code
		/// </summary>
		public static void Load()
		{
			Rook_SetupTables();

			for (int i = 0; i < 64; i++)
			{
				Rook_LoadVector(i, RookVectors[i]);

				var perms = Rook.GetPermutations(i);
				var map = new Dictionary<ulong, ulong>();

				foreach (var perm in perms)
				{
					var move = Rook.GetMoves(perm, i);
					int err = Rook_Load(i, perm, move);
					if (err != 0)
						throw new Exception("Table is corrupt");
				}
			}
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

				Bitboard.Unset(ref Move, i);

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
		internal static List<ulong> GetPermutations(int pos)
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
						Bitboard.Set(ref permutation, bitlist[b]);
					else
						Bitboard.Unset(ref permutation, bitlist[b]);
				}

				variations.Add(permutation);
			}

			return variations;
		}

		/// <summary>
		/// Convert the bitboard permutation into a valid attack board
		/// </summary>
		/// <param name="permutation"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		internal static ulong GetMoves(ulong permutation, int index)
		{
			ulong moves = 0;
			int target = 0;

			// Move up
			target = index + 8;
			while (target < 64)
			{
				Bitboard.Set(ref moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target += 8;
			}

			// Move down
			target = index - 8;
			while (target >= 0)
			{
				Bitboard.Set(ref moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target -= 8;
			}

			// Move right
			target = index + 1;
			while (Chess.Board.X(target) > Chess.Board.X(index))
			{
				Bitboard.Set(ref moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target++;
			}

			// Move left
			target = index - 1;
			while (Chess.Board.X(target) < Chess.Board.X(index))
			{
				Bitboard.Set(ref moves, target);
				if (Bitboard.Get(permutation, target)) // check for blockers
					break;
				target--;
			}

			return moves;
		}

		
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void Rook_SetupTables();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern int Rook_Load(int pos, ulong permutation, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern int Rook_LoadVector(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Rook_Read(int pos, ulong occupancy);

	}
}

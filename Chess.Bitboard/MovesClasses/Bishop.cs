using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib.MoveClasses
{
	public sealed class Bishop
	{
		public static ulong[] BishopVectors;

		static Bishop()
		{
			BishopVectors = GetBishopVectors();
		}

		/// <summary>
		/// This operation generates all permutations and their corresponding moves
		/// and calls Bishop_SetupTables in unmanaged code
		/// </summary>
		public static void Load()
		{
			Bishop_SetupTables();

			for (int i = 0; i < 64; i++)
			{
				Bishop_LoadVector(i, BishopVectors[i]);

				var perms = Bishop.GetPermutations(i);
				var map = new Dictionary<ulong, ulong>();

				foreach (var perm in perms)
				{
					var move = Bishop.GetMoves(perm, i);
					int err = Bishop_Load(i, perm, move);
					if (err != 0)
						throw new Exception("Table is corrupt");
				}
			}
		}

		/// <summary>
		/// creates move vectors with no blockers on the board for all 64 positions of the bishop
		/// </summary>
		static ulong[] GetBishopVectors()
		{
			var vectors = new ulong[64];

			for (int i = 0; i < 64; i++)
			{
				ulong Move = 0;

				int x = i % 8;
				int y = i >> 3; // i/8

				int target = 0;

				// NOTE: We don't traverse x 0 and 7, or y 0 and 7

				// mark up right
				target = i;
				while (true)
				{
					target += 9;
					if (target < 56 && Chess.Board.X(target) < 7 && Chess.Board.X(target) > Chess.Board.X(i))
						Bitboard.Set(ref Move, target);
					else
						break;
				}

				// mark up left
				target = i;
				while (true)
				{
					target += 7;
					if (target < 56 && Chess.Board.X(target) > 0 && Chess.Board.X(target) < Chess.Board.X(i))
						Bitboard.Set(ref Move, target);
					else
						break;
				}

				// mark down right
				target = i;
				while (true)
				{
					target -= 7;
					if (target > 7 && Chess.Board.X(target) < 7 && Chess.Board.X(target) > Chess.Board.X(i))
						Bitboard.Set(ref Move, target);
					else
						break;
				}

				// mark down left
				target = i;
				while (true)
				{
					target -= 9;
					if (target > 7 && Chess.Board.X(target) > 0 && Chess.Board.X(target) < Chess.Board.X(i))
						Bitboard.Set(ref Move, target);
					else
						break;
				}

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

			var vector = BishopVectors[pos];
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

			// Move up right
			target = index + 9;
			while (true)
			{
				if (target < 64 && Chess.Board.X(target) > Chess.Board.X(index))
					Bitboard.Set(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target += 9;
			}

			// Move up left
			target = index + 7;
			while (target >= 0)
			{
				if (target < 64 && Chess.Board.X(target) < Chess.Board.X(index))
					Bitboard.Set(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target += 7;
			}

			// Move down right
			target = index - 7;
			while (Chess.Board.X(target) > Chess.Board.X(index))
			{
				if (target >= 0 && Chess.Board.X(target) > Chess.Board.X(index))
					Bitboard.Set(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target -= 7;
			}

			// Move down left
			target = index - 9;
			while (Chess.Board.X(target) < Chess.Board.X(index))
			{
				if (target < 64 && Chess.Board.X(target) < Chess.Board.X(index))
					Bitboard.Set(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target -= 9;
			}

			return moves;
		}

		
		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void Bishop_SetupTables();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern int Bishop_Load(int pos, ulong permutation, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern int Bishop_LoadVector(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Bishop_Read(int pos, ulong occupancy);

	}
}

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
			Load();
		}

		/// <summary>
		/// This operation generates all permutations and their corresponding moves
		/// and calls Bishop_SetupTables in unmanaged code
		/// </summary>
		public static void Load()
		{
			SetupTables();

			for (int i = 0; i < 64; i++)
			{
				LoadVector(i, BishopVectors[i]);

				var perms = Bishop.GetPermutations(i);
				var map = new Dictionary<ulong, ulong>();

				foreach (var perm in perms)
				{
					var move = Bishop.GetMoves(perm, i);
					int err = Load(i, perm, move);
					if (err != 0)
						throw new Exception("Table is corrupt");
				}
				
			}
		}

		/// <summary>
		/// creates move vectors with no blockers on the board for all 64 positions of the bishop
		/// </summary>
		static unsafe ulong[] GetBishopVectors()
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
					if (target < 56 && Chess.Base.Board.X(target) < 7 && Chess.Base.Board.X(target) > Chess.Base.Board.X(i))
						Bitboard.SetRef(ref Move, target);
					else
						break;
				}

				// mark up left
				target = i;
				while (true)
				{
					target += 7;
					if (target < 56 && Chess.Base.Board.X(target) > 0 && Chess.Base.Board.X(target) < Chess.Base.Board.X(i))
						Bitboard.SetRef(ref Move, target);
					else
						break;
				}

				// mark down right
				target = i;
				while (true)
				{
					target -= 7;
					if (target > 7 && Chess.Base.Board.X(target) < 7 && Chess.Base.Board.X(target) > Chess.Base.Board.X(i))
						Bitboard.SetRef(ref Move, target);
					else
						break;
				}

				// mark down left
				target = i;
				while (true)
				{
					target -= 9;
					if (target > 7 && Chess.Base.Board.X(target) > 0 && Chess.Base.Board.X(target) < Chess.Base.Board.X(i))
						Bitboard.SetRef(ref Move, target);
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
		internal static unsafe List<ulong> GetPermutations(int pos)
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
						Bitboard.SetRef(ref permutation, bitlist[b]);
					else
						Bitboard.UnsetRef(ref permutation, bitlist[b]);
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
		internal static unsafe ulong GetMoves(ulong permutation, int index)
		{
			ulong moves = 0;
			int target = 0;

			// Move up right
			target = index;
			while (true)
			{
				if (target < 64 && (Chess.Base.Board.X(target) > Chess.Base.Board.X(index) || target == index))
					Bitboard.SetRef(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target += 9;
			}

			// Move up left
			target = index;
			while (true)
			{
				if (target < 64 && (Chess.Base.Board.X(target) < Chess.Base.Board.X(index) || target == index))
					Bitboard.SetRef(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target += 7;
			}

			// Move down right
			target = index;
			while (true)
			{
				if (target >= 0 && (Chess.Base.Board.X(target) > Chess.Base.Board.X(index) || target == index))
					Bitboard.SetRef(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target -= 7;
			}

			// Move down left
			target = index;
			while (true)
			{
				if (target >= 0 && (Chess.Base.Board.X(target) <= Chess.Base.Board.X(index) || target == index))
					Bitboard.SetRef(ref moves, target);
				else
					break;

				if (Bitboard.Get(permutation, target)) // check for blockers
					break;

				target -= 9;
			}

			Bitboard.UnsetRef(ref moves, index);

			return moves;
		}


		[DllImport("Chess.Lib.dll", EntryPoint = "Bishop_SetupTables", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void SetupTables();

		[DllImport("Chess.Lib.dll", EntryPoint = "Bishop_Load", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern int Load(int pos, ulong permutation, ulong moveBoard);

		[DllImport("Chess.Lib.dll", EntryPoint = "Bishop_LoadVector", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern int LoadVector(int pos, ulong moveBoard);

		[DllImport("Chess.Lib.dll", EntryPoint = "Bishop_Read", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Read(int pos, ulong occupancy);

	}
}

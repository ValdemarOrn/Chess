using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib.MoveClasses
{
	public sealed class Knight
	{
		/// <summary>
		/// This operation calculates attacks for all pawn positions and loads them into unmanaged code
        /// Afterwards the Knight_Read operation can be used to query for possible moves
		/// </summary>
        static Knight()
		{
			for (int i = 0; i < 64; i++)
			{
				var moves = GetMoves(i);
				Load(i, moves);
			}
		}

        /// <summary>
        /// Returns all possible attacks for a knight from the given square.
        /// Uses the (slow) C# move generator to create the bitboard moves.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		static ulong GetMoves(int index)
		{
			var b = new Chess.Board();
			b.State[index] = Pieces.Knight | Colors.White;
			var moves = Chess.Moves.GetMoves(b, index);

			ulong output = 0;
			foreach (var move in moves)
				Bitboard.SetRef(ref output, move);

			return output;
		}

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Knight_Load", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void Load(int pos, ulong moveBoard);

		// ---------------------------------------------

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Knight_Read", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Read(int pos);


	}
}

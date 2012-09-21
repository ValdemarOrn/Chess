using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Bitboard
{
	public sealed class King
	{
		/// <summary>
		/// This operation calculates moves and attacks for all pawn positions and loads them into unmanaged code
		/// </summary>
		public static void Load()
		{
			for (int i = 0; i < 64; i++)
			{
				var moves = GetMoves(i);
				King_Load(i, moves);
			}
		}

		static ulong GetMoves(int index)
		{
			// I use the old move generator to create the bitboard moves

			var b = new Board();
			b.State[index] = Pieces.King | Colors.White;
			var moves = Moves.GetMoves(b, index);

			ulong output = 0;
			foreach (var move in moves)
				Bitboard.Set(ref output, move);

			return output;
		}

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void King_Load(int pos, ulong moveBoard);

		// ---------------------------------------------

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong King_Read(int pos);


	}
}

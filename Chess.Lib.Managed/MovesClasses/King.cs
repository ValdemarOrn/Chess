using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib.MoveClasses
{
	public sealed class King
	{
		/// <summary>
		/// This operation calculates moves and attacks for all pawn positions and loads them into unmanaged code
		/// </summary>
        static King()
		{
			for (int i = 0; i < 64; i++)
			{
				var moves = GetMoves(i);
				Load(i, moves);
			}
		}

		static unsafe ulong GetMoves(int index)
		{
			// I use the old move generator to create the bitboard moves

			var b = new Chess.Base.Board();
			b.CanCastleKBlack = false;
			b.CanCastleKWhite = false;
			b.CanCastleQBlack = false;
			b.CanCastleQWhite = false;
			b.State[index] = Chess.Base.Colors.Val(Chess.Base.Pieces.King, Chess.Base.Color.White);
			var moves = Chess.Base.Moves.GetMoves(b, index);

			ulong output = 0;
			foreach (var move in moves)
				Bitboard.SetRef(ref output, move);

			return output;
		}

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "King_Load", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void Load(int pos, ulong moveBoard);

		// ---------------------------------------------

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "King_Read", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Read(int pos);


	}
}

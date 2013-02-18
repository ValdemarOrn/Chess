using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Chess.Lib.MoveClasses
{
	public sealed class Pawn
	{
		/// <summary>
		/// This operation calculates moves and attacks for all pawn positions and loads them into unmanaged code
		/// </summary>
        static Pawn()
		{
			for (int i = 0; i < 64; i++)
			{
				var moveW = GetMovesWhite(i);
				var moveB = GetMovesBlack(i);
				var attackW = GetAttacksWhite(i);
				var attackB = GetAttacksBlack(i);

				Pawn_LoadWhiteMove(i, moveW);
				Pawn_LoadBlackMove(i, moveB);
				Pawn_LoadWhiteAttack(i, attackW);
				Pawn_LoadBlackAttack(i, attackB);
			}
		}

		static ulong GetMovesWhite(int index)
		{
			ulong moves = 0;
			int x = Chess.Board.X(index);
			int y = Chess.Board.Y(index);

			if(y == 0 || y == 7) // cannot ever happen
			{
				return (ulong)0;
			}

			if(y == 1)
			{
				Bitboard.Set(ref moves, index + 8);
				Bitboard.Set(ref moves, index + 16);
			}
			else
			{
				Bitboard.Set(ref moves, index + 8);
			}

			return moves;
		}

		static ulong GetMovesBlack(int index)
		{
			ulong moves = 0;
			int x = Chess.Board.X(index);
			int y = Chess.Board.Y(index);

			if (y == 0 || y == 7) // cannot ever happen
			{
				return (ulong)0;
			}

			if (y == 6)
			{
				Bitboard.Set(ref moves, index - 8);
				Bitboard.Set(ref moves, index - 16);
			}
			else
			{
				Bitboard.Set(ref moves, index - 8);
			}

			return moves;
		}

		static ulong GetAttacksWhite(int index)
		{
			ulong moves = 0;
			int x = Chess.Board.X(index);
			int y = Chess.Board.Y(index);

			if (y == 0 || y == 7) // cannot ever happen
			{
				return (ulong)0;
			}

			if (x == 0)
			{
				Bitboard.Set(ref moves, index + 9);
			}
			else if (x == 7)
			{
				Bitboard.Set(ref moves, index + 7);
			}
			else
			{
				Bitboard.Set(ref moves, index + 7);
				Bitboard.Set(ref moves, index + 9);
			}

			return moves;
		}

		static ulong GetAttacksBlack(int index)
		{
			ulong moves = 0;
			int x = Chess.Board.X(index);
			int y = Chess.Board.Y(index);

			if (y == 0 || y == 7) // cannot ever happen
			{
				return (ulong)0;
			}

			if (x == 0)
			{
				Bitboard.Set(ref moves, index - 7);
			}
			else if (x == 7)
			{
				Bitboard.Set(ref moves, index - 9);
			}
			else
			{
				Bitboard.Set(ref moves, index - 7);
				Bitboard.Set(ref moves, index - 9);
			}

			return moves;
		}

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void Pawn_LoadWhiteMove(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void Pawn_LoadBlackMove(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void Pawn_LoadWhiteAttack(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		static extern void Pawn_LoadBlackAttack(int pos, ulong moveBoard);

		// ---------------------------------------------

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Pawn_ReadWhiteMove(int pos);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Pawn_ReadBlackMove(int pos);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Pawn_ReadWhiteAttack(int pos);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Pawn_ReadBlackAttack(int pos);

		
	}
}

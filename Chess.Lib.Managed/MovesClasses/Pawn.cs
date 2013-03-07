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

				LoadWhiteMove(i, moveW);
				LoadBlackMove(i, moveB);
				LoadWhiteAttack(i, attackW);
				LoadBlackAttack(i, attackB);
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
				Bitboard.SetRef(ref moves, index + 8);
				Bitboard.SetRef(ref moves, index + 16);
			}
			else
			{
				Bitboard.SetRef(ref moves, index + 8);
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
				Bitboard.SetRef(ref moves, index - 8);
				Bitboard.SetRef(ref moves, index - 16);
			}
			else
			{
				Bitboard.SetRef(ref moves, index - 8);
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
				Bitboard.SetRef(ref moves, index + 9);
			}
			else if (x == 7)
			{
				Bitboard.SetRef(ref moves, index + 7);
			}
			else
			{
				Bitboard.SetRef(ref moves, index + 7);
				Bitboard.SetRef(ref moves, index + 9);
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
				Bitboard.SetRef(ref moves, index - 7);
			}
			else if (x == 7)
			{
				Bitboard.SetRef(ref moves, index - 9);
			}
			else
			{
				Bitboard.SetRef(ref moves, index - 7);
				Bitboard.SetRef(ref moves, index - 9);
			}

			return moves;
		}

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_LoadWhiteMove", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void LoadWhiteMove(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_LoadBlackMove", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void LoadBlackMove(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_LoadWhiteAttack", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void LoadWhiteAttack(int pos, ulong moveBoard);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_LoadBlackAttack", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		static extern void LoadBlackAttack(int pos, ulong moveBoard);

		// ---------------------------------------------

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_ReadWhiteMove", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ReadWhiteMove(int pos);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_ReadBlackMove", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ReadBlackMove(int pos);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_ReadWhiteAttack", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ReadWhiteAttack(int pos);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Pawn_ReadBlackAttack", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ReadBlackAttack(int pos);

		
	}
}

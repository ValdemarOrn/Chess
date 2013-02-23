using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Chess.Lib.MoveClasses;

namespace Chess.Lib
{
	public class Moves
	{
		static Moves()
		{
			Init();
		}

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_Init", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Init();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetMoves", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong GetMoves(BoardStruct* board, int tile);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetAttacks", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong GetAttacks(BoardStruct* board, int tile);

		// ------------- Specific Moves --------------


		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetCastlingType", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern byte GetCastlingType(BoardStruct* board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetAvailableCastlingTypes", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern byte GetAvailableCastlingTypes(BoardStruct* board, int color);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetCastlingMoves", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong GetCastlingMoves(BoardStruct* board, int color);


		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_IsCaptureMove", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int IsCaptureMove(BoardStruct* board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_IsEnPassantCapture", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int IsEnPassantCapture(BoardStruct* board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetEnPassantVictimTile", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int GetEnPassantVictimTile(BoardStruct* board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetEnPassantTile", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int GetEnPassantTile(BoardStruct* board, int from, int to);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Moves_GetEnPassantMove", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern ulong GetEnPassantMove(BoardStruct* board, int from);

	}
}

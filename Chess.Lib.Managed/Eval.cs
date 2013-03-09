using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
	public unsafe struct EvalStats
	{
		public int Material;
		public int PositionalBonus;
		public int MobilityBonus;
		public int UndefendedPenalty;
		public int DoublePawnPenalty;
		public int IsolatedPawnPenalty;
		public int TempoBonus;
	}

	public class Eval
	{
		static Eval()
		{
			Init();
		}

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Eval_Init", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Init();

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Eval_Evaluate", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Evaluate(BoardStruct* board);

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "Eval_GetEvalStats", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern EvalStats* GetEvalStats(int color);
	}
}

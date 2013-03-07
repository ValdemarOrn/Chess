using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
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
	}
}

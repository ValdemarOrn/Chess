﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	public class SEE
	{
		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "SEE_Square", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Square(BoardStruct* board, int square);

		[DllImport("C:\\Src\\_Tree\\Applications\\Chess\\Chess.Lib\\x64\\bin\\Chess.Lib.dll", EntryPoint = "SEE_Capture", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static unsafe extern int Capture(BoardStruct* board, int from, int to);
	}
}
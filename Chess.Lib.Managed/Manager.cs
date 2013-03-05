using Chess.Lib.MoveClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib
{
	public class Manager
	{
		public static void InitLibrary()
		{
			new Eval();
			new Moves();
			new Zobrist();

			new Pawn();
			new Rook();
			new Knight();
			new Bishop();
			new King();
			new Queen();

			cb = Callback;
			var ptr = Marshal.GetFunctionPointerForDelegate(cb);
			Manager.SetCallback(ptr);
		}

		delegate void CallbackDelegate(IntPtr data);

		static CallbackDelegate cb;

		static unsafe void Callback(IntPtr data)
		{
			byte* b = (byte*)data;
			int strLen = 0;
			while (*b != 0)
			{
				strLen++;
				b++;
			}

			var str = Marshal.PtrToStringAnsi(data, strLen);
			Console.Write(str);
		}

		[DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", EntryPoint = "Manager_SetCallback", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetCallback(IntPtr callbackPtr);
	}
}

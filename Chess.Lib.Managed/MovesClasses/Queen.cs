using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Chess.Lib.MoveClasses
{
	public sealed class Queen
	{
		static Queen()
		{
			new Rook();
			new Bishop();
		}

		[DllImport("Chess.Lib.dll", EntryPoint = "Queen_Read", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong Read(int pos, ulong occupancy);


	}
}

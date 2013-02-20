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

        [DllImport("..\\..\\..\\Chess.Lib\\x64\\Debug\\Chess.Lib.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong Queen_Read(int pos, ulong occupancy);


    }
}

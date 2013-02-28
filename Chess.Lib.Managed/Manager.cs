using Chess.Lib.MoveClasses;
using System;
using System.Collections.Generic;
using System.Linq;
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
		}
	}
}

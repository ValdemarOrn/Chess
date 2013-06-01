using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public enum Castling
	{
		None = 0,

		KingsideWhite = 1,
		QueensideWhite = 2,
		KingsideBlack = 4,
		QueensideBlack = 8
	}
}

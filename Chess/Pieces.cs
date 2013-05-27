using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public sealed class Pieces
	{
		public const int Pawn = 2;
		public const int Knight = 3;
		public const int Bishop = 4;
		public const int Rook = 5;
		public const int Queen = 6;
		public const int King = 7;

		public static int Get(int piece)
		{
			return piece & 0x0F;
		}

		public static string ToString(int piece)
		{
			switch (piece)
			{
				case Pawn:
					return "Pawn";
				case Rook:
					return "Rook";
				case Bishop:
					return "Bishop";
				case Knight:
					return "Knight";
				case Queen:
					return "Queen";
				case King:
					return "King";
				default:
					return "";
			}
		}
	}
}

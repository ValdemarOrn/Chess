using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public enum Piece
	{
		None = 0,

		Pawn = 2,
		Knight = 3,
		Bishop = 4,
		Rook = 5,
		Queen = 6,
		King = 7
	}

	public static class Pieces
	{
		public static Piece Get(int piece)
		{
			return (Piece)(piece & 0x0F);
		}

		public static string GetLetter(this Piece piece)
		{
			switch (piece)
			{
				case Piece.Pawn:
					return "";
				case Piece.Rook:
					return "R";
				case Piece.Bishop:
					return "B";
				case Piece.Knight:
					return "N";
				case Piece.Queen:
					return "Q";
				case Piece.King:
					return "K";
				default:
					return "";
			}
		}

		public static string ToString(this Piece piece)
		{
			switch (piece)
			{
				case Piece.Pawn:
					return "Pawn";
				case Piece.Rook:
					return "Rook";
				case Piece.Bishop:
					return "Bishop";
				case Piece.Knight:
					return "Knight";
				case Piece.Queen:
					return "Queen";
				case Piece.King:
					return "King";
				default:
					return "";
			}
		}
	}
}

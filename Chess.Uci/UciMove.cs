using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public enum UciPiece
	{
		None = 0,

		Pawn = 2,
		Knight = 3,
		Bishop = 4,
		Rook = 5,
		Queen = 6,
		King = 7
	}

	public class UciMove
	{
		public int From;
		public int To;
		public UciPiece Promotion;

		public UciMove()
		{ }

		public UciMove(int from, int to, UciPiece promotion = UciPiece.None)
		{
			From = from;
			To = to;
			Promotion = promotion;
		}

		public static UciMove FromString(string move)
		{
			if (move == null || String.IsNullOrWhiteSpace(move))
				return null;

			move = move.ToLower();

			int fx, fy, tx, ty = 0;
			var promo = UciPiece.None;

			fx = move[0] - 'a';
			fy = move[1] - '1';

			tx = move[2] - 'a';
			ty = move[3] - '1';

			if (move.Length > 4)
			{
				if (move[4] == 'n')
					promo = UciPiece.Knight;
				else if (move[4] == 'b')
					promo = UciPiece.Bishop;
				else if (move[4] == 'r')
					promo = UciPiece.Rook;
				else if (move[4] == 'q')
					promo = UciPiece.Queen;
			}

			return new UciMove(fx + fy * 8, tx + ty * 8, promo);
		}

		public override string ToString()
		{ 
			int fromX = From % 8;
			int fromY = From / 8;

			int toX = To % 8;
			int toY = To / 8;

			var output = ((char)('a' + fromX)).ToString() + ((char)('1' + fromY)).ToString();
			output += ((char)('a' + toX)).ToString() + ((char)('1' + toY)).ToString();

			if (Promotion == 0)
				return output;
			else if (Promotion == UciPiece.Knight)
				output += "n";
			else if (Promotion == UciPiece.Bishop)
				output += "b";
			else if (Promotion == UciPiece.Rook)
				output += "r";
			else if (Promotion == UciPiece.Queen)
				output += "q";

			return output;
		}
	}
}

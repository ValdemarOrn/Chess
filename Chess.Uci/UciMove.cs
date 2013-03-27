using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public class UciMove
	{
		public int From;
		public int To;
		public int Promotion;

		public UciMove()
		{ }

		public UciMove(int from, int to, int promotion = 0)
		{
			From = from;
			To = to;
			Promotion = promotion;
		}

		public static UciMove FromString(string move)
		{
			if (move == null)
				return null;

			move = move.ToLower();

			int fx, fy, tx, ty, promo = 0;

			fx = move[0] - 'a';
			fy = move[1] - '1';

			tx = move[2] - 'a';
			ty = move[3] - '1';

			if (move.Length > 4)
			{
				if (move[4] == 'n')
					promo = 2;
				else if (move[4] == 'b')
					promo = 3;
				else if (move[4] == 'r')
					promo = 4;
				else if (move[4] == 'q')
					promo = 5;
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
			else if (Promotion == 2)
				output += "n";
			else if (Promotion == 3)
				output += "b";
			else if (Promotion == 4)
				output += "r";
			else if (Promotion == 5)
				output += "q";

			return output;
		}
	}
}

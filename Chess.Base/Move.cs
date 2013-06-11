using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public class Move
	{
		public int From;
		public int To;
		public Piece Promotion;

		public Move()
		{
			From = 0;
			To = 0;
			Promotion = Piece.None;
		}

		public Move(int from, int to, Piece promotion = Piece.None)
		{
			From = from;
			To = to;
			Promotion = promotion;
		}

		public Move Copy()
		{
			return new Move(From, To, Promotion);
		}

		public override string ToString()
		{
			string output = Notation.TileToText(From) + Notation.TileToText(To);
			if (Promotion != Piece.None)
				output += "=" + Promotion.GetLetter();

			return output;
		}

		public override bool Equals(object obj)
		{
			if(!(obj is Move))
				return false;
			
			var m = obj as Move;
			return From == m.From && To == m.To && Promotion == m.Promotion;
		}

		public override int GetHashCode()
		{
			return From + (To << 6) + ((int)Promotion << 12);
		}
	}
}

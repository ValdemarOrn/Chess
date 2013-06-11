using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class PGNMove : IPGNElement
	{
		public PGNTokenType Type { get { return PGNTokenType.Move; } }

		public Piece Piece;
		public Color Color;
		public int MoveNumber;

		public int From;
		public int To;
		public bool Capture;
		public bool Check;
		public bool Mate;
		public Piece Promotion;

		public override string ToString()
		{
			string output = Notation.GetPieceLetter(Piece) + Notation.TileToText(From) + Notation.TileToText(To);
			if (Promotion != Piece.None)
				output += "=" + Promotion.GetLetter();

			return output;
		}
	}
}

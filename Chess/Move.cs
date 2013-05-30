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
			return Notation.TileToText(From) + Notation.TileToText(To) + Promotion.GetLetter();
		}
	}

	public class MoveData
	{
		public int From;
		public int To;
		public int MoveCount;
		public Color Color;
		public int Capture;
		public int CaptureTile; // only differs from "To" in en passant attacks
		public Piece Promotion;
		public bool Check;
		public bool Mate;
		public bool Queenside;
		public bool Kingside;
		

		public MoveData(
			int from,
			int to,
			int moveCount = 0,
			Color color = 0,
			int capture = 0,
			int captureTile = 0,
			Piece promotion = 0,
			bool check = false,
			bool mate = false,
			bool queenside = false,
			bool kingside = false)
		{
			From = from;
			To = to;
			MoveCount = moveCount;
			Color = color;
			Capture = capture;
			CaptureTile = captureTile;
			Promotion = promotion;
			Check = check;
			Mate = mate;
			Queenside = queenside;
			Kingside = kingside;
		}
	}
}

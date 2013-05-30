using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public struct Move
	{
		public int From;
		public int To;
		public int MoveCount;
		public Color Color;
		public int Capture;
		public int CaptureTile; // only differs from "To" in en passant attacks
		public int Promotion;
		public bool Check;
		public bool Mate;
		public bool Queenside;
		public bool Kingside;
		

		public Move(
			int from,
			int to,
			int moveCount = 0,
			Color color = 0,
			int capture = 0,
			int captureTile = 0,
			int promotion = 0,
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

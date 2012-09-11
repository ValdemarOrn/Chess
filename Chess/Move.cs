using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public struct Move
	{
		public int From, To, MoveCount, Color;
		public bool Capture;
		public int Promotion;
		public bool Check;
		public bool Mate;
		public bool Queenside;
		public bool Kingside;

		public Move(
			int from,
			int to,
			int turn = 0,
			int color = 0,
			bool capture = false,
			int promotion = 0,
			bool check = false,
			bool mate = false,
			bool queenside = false,
			bool kingside = false)
		{
			From = from;
			To = to;
			MoveCount = turn;
			Color = color;
			Capture = capture;
			Promotion = promotion;
			Check = check;
			Mate = mate;
			Queenside = queenside;
			Kingside = kingside;
		}
	}
}

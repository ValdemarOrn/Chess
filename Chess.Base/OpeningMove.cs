using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public class OpeningMove
	{
		public Move Move;

		public int WhiteWins;
		public int BlackWins;
		public int Tie;
		public int Total;

		public double WhiteWinPercent { get { return WhiteWins * 100 / (double)Total; } }
		public double BlackWinPercent { get { return BlackWins * 100 / (double)Total; } }
	}
}

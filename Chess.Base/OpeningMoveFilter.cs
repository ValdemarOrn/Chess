using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public class OpeningBookFilter
	{
		public int MinNumberOfGames;

		/// <summary>
		/// Set mimimum win % when playing as white
		/// </summary>
		public double MinWhiteWinPercent;

		/// <summary>
		/// Set mimimum win % when playing as black
		/// </summary>
		public double MinBlackWinPercent;

		/// <summary>
		/// Set maximum win % of white when playing as Black
		/// </summary>
		public double MaxWhiteWinPercent;

		/// <summary>
		/// Set maximum win % of black when playing as White
		/// </summary>
		public double MaxBlackWinPercent;

		public double ImportanceOfGameCount;
		public double ImportantOfWinPercentage;
	}
}

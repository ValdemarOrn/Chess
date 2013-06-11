using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public enum GameResultsType
	{
		Tie = 0, 
		WhiteWins = 1,
		BlackWins = 2,
		Unresolved = 3
	}

	public class GameResults : IPGNElement
	{
		public PGNTokenType Type { get { return PGNTokenType.Results; } }

		public GameResultsType Results { get; private set; }

		public GameResults(string results)
		{
			if (results == "1-0")
				Results = GameResultsType.WhiteWins;
			else if (results == "0-1")
				Results = GameResultsType.BlackWins;
			else if (results == "1/2-1/2" || results == "1/2")
				Results = GameResultsType.Tie;
			else if (results == "*")
				Results = GameResultsType.Unresolved;
			else
				throw new Exception("Illegal game result: " + results);
		}

		public override string ToString()
		{
			if (Results == GameResultsType.WhiteWins)
				return "1-0";
			if (Results == GameResultsType.BlackWins)
				return "0-1";
			if (Results == GameResultsType.Tie)
				return "1/2-1/2";
			if (Results == GameResultsType.Unresolved)
				return "*";
			
			return "";
		}
	}
}

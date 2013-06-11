using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class PGNParserResults
	{
		public List<PGNGame> Games;
		public List<PGNParserError> Errors;

		public PGNParserResults()
		{
			Games = new List<PGNGame>();
			Errors = new List<PGNParserError>();
		}
	}
}

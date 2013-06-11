using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class GameComment : IPGNElement
	{
		public PGNTokenType Type { get { return PGNTokenType.Comment; } }
		public string Comment { get; private set; }

		public GameComment(string comment)
		{
			Comment = comment;	
		}

		public override string ToString()
		{
			return "{ " + Comment + " }";
		}
	}
}

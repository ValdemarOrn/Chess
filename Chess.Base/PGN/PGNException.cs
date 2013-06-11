using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public enum ParserSource
	{
		Tokenizer,
		Parser
	}

	public class PGNException : Exception
	{
		public ParserSource SourceComponent;
		public PGNToken PgnToken;
		public string Token;
		public int Index;

		public PGNException(string message, int index, string token, ParserSource source) 
			: base(message)
		{
			SourceComponent = source;
			Index = index;
			Token = token;
		}

		public PGNException(string message, int index, string token, ParserSource source, Exception innerException)
			: base(message, innerException)
		{
			SourceComponent = source;
			Index = index;
			Token = token;
		}

		public PGNException(string message, PGNToken token, ParserSource source = ParserSource.Parser)
			: base(message)
		{
			SourceComponent = source;
			Index = token.Index;
			PgnToken = token;
		}

		public PGNException(string message, PGNToken token, ParserSource source, Exception innerException)
			: base(message, innerException)
		{
			SourceComponent = source;
			Index = token.Index;
			PgnToken = token;
		}
	}
}

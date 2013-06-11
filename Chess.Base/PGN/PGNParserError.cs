using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class PGNParserError
	{
		public int GameNumber;
		public string Value;

		public int Index;
		public int Line;
		public int Col;

		public ParserSource Source;
		public string Message;
		public Exception Exception;

		public override string ToString()
		{
			var fields = typeof(PGNParserError).GetFields();
			var sb = new StringBuilder();
			foreach (var field in fields)
			{
				sb.Append(field.Name);
				sb.Append(": ");
				sb.Append(field.GetValue(this).ToString());
			}
			return sb.ToString();
		}
	}
}

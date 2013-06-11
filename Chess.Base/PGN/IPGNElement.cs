using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public interface IPGNElement
	{
		PGNTokenType Type { get; }
	}
}

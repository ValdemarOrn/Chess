using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public enum PGNTokenType
	{
		None = 0,

		Comment,
		MoveNumber,
		Move,
		VariationStart,
		VariationEnd,
		Ellipsis,
		Annotation,
		Results,
		End
	}
}

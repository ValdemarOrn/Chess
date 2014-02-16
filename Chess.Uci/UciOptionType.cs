using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	/// <summary>
	/// UCI Option Types
	/// </summary>
	[Serializable]
	public enum UciOptionType
	{
		Check,
		Spin,
		Combo,
		Button,
		String
	}
}

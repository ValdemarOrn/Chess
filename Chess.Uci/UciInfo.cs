using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public enum UciInfo
	{
		Depth,
		SelDepth,
		Time,
		Nodes,
		PV,
		MultiPV,
		Score,
		CurrMove,
		CurrMoveNumber,
		HashFull,
		NPS,
		TBHits,
		SBHits,
		CPULoad,
		String,
		Refutation,
		CurrLine
	}
}

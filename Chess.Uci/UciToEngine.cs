using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public enum UciToEngine
	{
		Uci,
		Debug,
		IsReady,
		SetOption,
		Register,
		UciNewGame,
		Position,
		Go,
		Stop,
		PonderHit,
		Quit
	}
}

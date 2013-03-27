using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public interface IUciGui
	{
		void ID(string name, string author);
		void UciOk();
		void ReadyOk();
		void BestMove(UciMove bestMove, UciMove ponderMove);
		void CopyProtection(bool protectionIsOk);
		void Registration(bool registrationIsOk);
		void Info(Dictionary<UciInfo, string> infoValues);
		void Option(string name, UciOptionType type, object defaultValue, object min, object max, List<object> values);
	}
}

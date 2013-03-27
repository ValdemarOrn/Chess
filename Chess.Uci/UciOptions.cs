using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	/// <summary>
	/// Standard UCI options
	/// </summary>
	public static class UciOptions
	{
		public const string Hash                  = "Hash";
		public const string NalimovPath           = "NalimovPath";
		public const string NalimovCache          = "NalimovCache";
		public const string Ponder                = "Ponder";
		public const string OwnBook               = "OwnBook";
		public const string MultiPV               = "MultiPV";
		public const string UCI_ShowCurrLine      = "UCI_ShowCurrLine";
		public const string UCI_ShowRefutations   = "UCI_ShowRefutations";
		public const string UCI_LimitStrength     = "UCI_LimitStrength";
		public const string UCI_Elo               = "UCI_Elo";
		public const string UCI_AnalyseMode       = "UCI_AnalyseMode";
		public const string UCI_Opponent          = "UCI_Opponent";
		public const string UCI_EngineAbout       = "UCI_EngineAbout";
		public const string UCI_ShredderbasesPath = "UCI_ShredderbasesPath";
		public const string UCI_SetPositionValue  = "UCI_SetPositionValue";
	}
}

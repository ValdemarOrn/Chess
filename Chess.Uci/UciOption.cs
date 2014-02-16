using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	[Serializable]
	public class UciOption
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public UciOptionType Type { get; set; }
		public object DefaultValue { get; set; }
		public int? Min { get; set; }
		public int? Max { get; set; }
		public List<object> Options { get; set; }

		public UciOption()
		{
			Options = new List<object>();
		}
	}
}

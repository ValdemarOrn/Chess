using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public class UciOption
	{
		public string Name { get; set; } 
		public UciOptionType Type { get; set; }
		public object DefaultValue { get; set; }
		public int? Min { get; set; }
		public int? Max { get; set; }
		public List<object> Values { get; set; }

		public UciOption()
		{
			Values = new List<object>();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public class CommandParser
	{
		/// <summary>
		/// Extract all the key-value elements from a UCI command
		/// </summary>
		/// <param name="commandString"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		public static Dictionary<string, string> GetElements(string commandString, params string[] keys)
		{
			var output = new Dictionary<string, string>();
			keys = keys.Select(x => x.ToLower().Trim()).ToArray();

			var indexes = GetIndexes(commandString, keys.ToList());
			indexes = indexes.OrderBy(x => x.Item1).ToList();

			// add end-of-line index
			indexes.Add(new Tuple<int, string>(commandString.Length, null));

			for(int i = 0; i < indexes.Count - 1; i++)
			{
				var str = commandString.Substring(indexes[i].Item1, indexes[i + 1].Item1 - indexes[i].Item1);
				str = str.Replace(indexes[i].Item2, "").Trim();
				output[indexes[i].Item2] = str;
			}

			return output;
		}

		public static List<string> GetMultipleElements(string commandString, string key)
		{
			var output = new List<string>();

			var indexes = GetIndexes(commandString, new List<string>(){ key });
			indexes = indexes.OrderBy(x => x.Item1).ToList();

			// add end-of-line index
			indexes.Add(new Tuple<int, string>(commandString.Length, null));

			for (int i = 0; i < indexes.Count - 1; i++)
			{
				var str = commandString.Substring(indexes[i].Item1, indexes[i + 1].Item1 - indexes[i].Item1);
				str = str.Replace(indexes[i].Item2, "").Trim();
				output.Add(str);
			}

			return output;
		}

		private static List<Tuple<int, string>> GetIndexes(string commandString, List<string> keys)
		{
			var indexes = new List<Tuple<int, string>>();
			int idx = 0;
			while (true)
			{
				var res = keys
					.Select(x => new KeyValuePair<string, int>(x, commandString.IndexOf(x, idx)))
					.Where(x => x.Value > -1);

				if(res.Count() == 0)
					break;

				var min = res.Min(x => x.Value);
				var kvp = res.First(x => x.Value == min);
				idx = kvp.Value + kvp.Key.Length;
				indexes.Add(new Tuple<int, string>(kvp.Value, kvp.Key));
			}

			return indexes;
		}
	}
}

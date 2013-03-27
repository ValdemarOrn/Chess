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

		private static List<Tuple<int, string>> GetIndexes(string commandString, List<string> keys)
		{
			var indexes = new List<Tuple<int, string>>();
			while (true)
			{
				int i = 999999999;
				string k = null;

				foreach (var key in keys)
				{
					var idx = commandString.IndexOf(key);

					if (idx != -1 && idx < i)
					{
						i = idx;
						k = key;
					}
				}

				if (i == 999999999 || k == null)
					break;

				keys.Remove(k);
				indexes.Add(new Tuple<int, string>(i, k));
			}

			return indexes;
		}
	}
}

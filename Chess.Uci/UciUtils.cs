using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public static class UciUtils
	{
		/// <summary>
		/// Convert a string to a UciToEngine enum
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static UciToEngine? GetUciCommand(string command)
		{
			if (command == null || String.IsNullOrWhiteSpace(command))
				return null;

			command = command.ToLower().Trim();

			var enumStrings = Enum.GetNames(typeof(UciToEngine));

			foreach (var e in enumStrings)
			{
				if (e.ToLower() == command)
					return (UciToEngine)Enum.Parse(typeof(UciToEngine), e);
			}

			return null;
		}


		public static bool ParseBool(string p)
		{
			if (p == null)
				return false;

			p = p.ToLower();
			return (p == "1" || p == "true" || p == "yes" || p == "on");
		}
	}
}

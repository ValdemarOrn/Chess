using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public static class UciUtils
	{
		/*/// <summary>
		/// Convert a string to a UciToEngine enum
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static UciToEngine? GetEngineCommand(string command)
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

		/// <summary>
		/// Convert a string to a UciToGui enum
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public static UciToGui? GetGuiCommand(string command)
		{
			if (command == null || String.IsNullOrWhiteSpace(command))
				return null;

			command = command.ToLower().Trim();

			var enumStrings = Enum.GetNames(typeof(UciToGui));

			foreach (var e in enumStrings)
			{
				if (e.ToLower() == command)
					return (UciToGui)Enum.Parse(typeof(UciToGui), e);
			}

			return null;
		}*/

		/// <summary>
		/// T must be enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static Nullable<T> GetEnum<T>(string value) where T : struct, IConvertible
		{
			if (value == null || String.IsNullOrWhiteSpace(value))
				return null;

			var enumStrings = Enum.GetNames(typeof(T));

			foreach (var e in enumStrings)
			{
				if (e.ToLower() == value)
					return (T)Enum.Parse(typeof(T), e);
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

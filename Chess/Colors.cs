using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public sealed class Colors
	{
		public const int White = 1 << 4;
		public const int Black = 2 << 4;

		public static int Get(int piece)
		{
			return piece & 0xF0;
		}

		/// <summary>
		/// Get the opposite color. Return zero if value is not a valid color
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static int Opposite(int color)
		{
			if (color == White)
				return Black;
			if (color == Black)
				return White;

			return 0;
		}

		public static string ToString(int color)
		{
			if (color == White)
				return "White";
			if (color == Black)
				return "Black";

			return "";
		}
	}
}

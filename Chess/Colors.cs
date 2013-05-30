using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public enum Color
	{
		None = 0,

		White = 1 << 4,
		Black = 2 << 4
	}

	public sealed class Colors
	{
		public static Color Get(int piece)
		{
			return (Color)(piece & 0xF0);
		}

		/// <summary>
		/// Get the opposite color. Return zero if value is not a valid color
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		public static Color Opposite(Color color)
		{
			if (color == Color.White)
				return Color.Black;
			if (color == Color.Black)
				return Color.White;

			return Color.None;
		}

		public static byte Val(Color color, int piece)
		{
			return (byte)((int)color | piece);
		}

		public static byte Val(int piece, Color color)
		{
			return (byte)((int)color | piece);
		}

		public static byte Val(Color color, Piece piece)
		{
			return (byte)((int)color | (int)piece);
		}

		public static byte Val(Piece piece, Color color)
		{
			return (byte)((int)color | (int)piece);
		}

		public static string ToString(Color color)
		{
			if (color == Color.White)
				return "White";
			if (color == Color.Black)
				return "Black";

			return "";
		}
	}
}

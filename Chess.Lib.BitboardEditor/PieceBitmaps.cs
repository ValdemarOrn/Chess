using Chess.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Chess.Lib.BitboardEditor
{
	public class PieceBitmaps
	{
		public static Bitmap WhiteBishop;
		public static Bitmap WhiteKing;
		public static Bitmap WhiteKnight;
		public static Bitmap WhitePawn;
		public static Bitmap WhiteQueen;
		public static Bitmap WhiteRook;

		public static Bitmap BlackBishop;
		public static Bitmap BlackKing;
		public static Bitmap BlackKnight;
		public static Bitmap BlackPawn;
		public static Bitmap BlackQueen;
		public static Bitmap BlackRook;

		// dir = @"C:\Src\_Tree\Applications\Chess\Pieces\";
		static string _dir;
		public static string Dir
		{
			get { return _dir; }
			set
			{
				_dir = value;
				LoadBitmaps();
			}
		}

		static void LoadBitmaps()
		{
			WhiteBishop = new Bitmap(Dir + "white\\bishop.png");
			WhiteKing = new Bitmap(Dir + "white\\king.png");
			WhiteKnight = new Bitmap(Dir + "white\\knight.png");
			WhitePawn = new Bitmap(Dir + "white\\pawn.png");
			WhiteQueen = new Bitmap(Dir + "white\\queen.png");
			WhiteRook = new Bitmap(Dir + "white\\rook.png");

			BlackBishop = new Bitmap(Dir + "black\\bishop.png");
			BlackKing = new Bitmap(Dir + "black\\king.png");
			BlackKnight = new Bitmap(Dir + "black\\knight.png");
			BlackPawn = new Bitmap(Dir + "black\\pawn.png");
			BlackQueen = new Bitmap(Dir + "black\\queen.png");
			BlackRook = new Bitmap(Dir + "black\\rook.png");
		}

		public static Bitmap GetBitmap(int piece)
		{
			Chess.Base.Color color = Colors.Get(piece);
			int type = Pieces.Get(piece);

			if (color == Chess.Base.Color.White)
			{
				switch (type)
				{
					case Pieces.Bishop:
						return WhiteBishop;
					case Pieces.King:
						return WhiteKing;
					case Pieces.Knight:
						return WhiteKnight;
					case Pieces.Pawn:
						return WhitePawn;
					case Pieces.Queen:
						return WhiteQueen;
					case Pieces.Rook:
						return WhiteRook;
				}
			}
			if (color == Chess.Base.Color.Black)
			{
				switch (type)
				{
					case Pieces.Bishop:
						return BlackBishop;
					case Pieces.King:
						return BlackKing;
					case Pieces.Knight:
						return BlackKnight;
					case Pieces.Pawn:
						return BlackPawn;
					case Pieces.Queen:
						return BlackQueen;
					case Pieces.Rook:
						return BlackRook;
				}
			}

			return null;
		}
	}
}

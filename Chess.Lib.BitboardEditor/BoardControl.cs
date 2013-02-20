using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chess.Lib.BitboardEditor
{
	public class BoardControl : UserControl
	{
		int TileSize = 50;
		int DotSize = 20;
		Brush BrushWhite;
		Brush BrushBlack;

		ulong _state;
		public ulong State
		{
			get { return _state; }
			set { _state = value; RefreshBuffer(); }
		}

		public Action UpdateCallback;

		public BoardControl()
		{
			this.DoubleBuffered = true;
			State = (ulong)0;

			BrushWhite = Brushes.White;
			BrushBlack = Brushes.LightGray;

			CreateTiles();

		}

		Bitmap Tiles = null;
		Bitmap Buffer = null;

		void CreateTiles()
		{
			Tiles = new Bitmap(50 * 8, 50 * 8);
			var g = Graphics.FromImage(Tiles);

			// draw tiles
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var b = ((x + y) % 2 == 0) ? BrushWhite : BrushBlack;
					g.FillRectangle(b, x * TileSize, y * TileSize, TileSize, TileSize);
				}
			}

			Buffer = new Bitmap(50 * 8, 50 * 8);
		}

		void RefreshBuffer()
		{
			if (Tiles == null || Buffer == null)
				return;

			var g = Graphics.FromImage(Buffer);
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			g.DrawImageUnscaled(Tiles, 0, 0);

			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					int index = x + (7 - y) * 8;
					if (Bitboard.Get(State, index))
						g.FillEllipse(Brushes.Black, (x + 0.5f) * TileSize - DotSize / 2, (y + 0.5f) * TileSize - DotSize / 2, DotSize, DotSize);
				}
			}

			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImageUnscaled(Buffer, 0, 0);
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			int x = (e.X / TileSize);
			int y = 7 - (e.Y / TileSize);

			int index = x + y * 8;

			if (Bitboard.Get(State, index))
				State = Bitboard.Unset(State, index);
			else
				State = Bitboard.Set(State, index);

			UpdateCallback();
		}
	}
}

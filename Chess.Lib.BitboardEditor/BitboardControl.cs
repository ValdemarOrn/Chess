using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chess.Lib.BitboardEditor
{
	public class BitboardControl : UserControl
	{
		int TileSize = 50;
		int DotSize = 20;
		Brush BrushWhite;
		Brush BrushBlack;

		bool _labeledTiles;
		public bool LabeledTiles
		{
			get { return _labeledTiles; }
			set
			{
				_labeledTiles = value;
				CreateTiles();
				RefreshBuffer();
			}
		}

		ulong _bitState;
		public ulong BitState
		{
			get { return _bitState; }
			set { _bitState = value; RefreshBuffer(); }
		}

		public Action UpdateCallback;

		public BitboardControl()
		{
			this.DoubleBuffered = true;
			BitState = (ulong)0;

			BrushWhite = Brushes.White;
			BrushBlack = Brushes.LightGray;

			CreateTiles();

		}

		Bitmap Tiles = null;
		Bitmap Buffer = null;

		public void CreateTiles()
		{
			Tiles = new Bitmap(50 * 8, 50 * 8);
			var g = Graphics.FromImage(Tiles);

			// draw tiles
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var b = ((x + y) % 2 == 0) ? BrushWhite : BrushBlack;
					var bx = ((x + y) % 2 == 0) ? BrushBlack : BrushWhite;

					int xs = x * TileSize;
					int ys = y * TileSize;
					g.FillRectangle(b, xs, ys, TileSize, TileSize);
					
					if(LabeledTiles)
					{
						int num = x + (7 - y) * 8;
						g.DrawString(num.ToString(), new Font("Arial", 12), bx, xs + 2, ys + 2);
					}
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

			try
			{
				for (int x = 0; x < 8; x++)
				{
					for (int y = 0; y < 8; y++)
					{
						int index = x + (7 - y) * 8;

						if (Bitboard.Get(BitState, index))
							g.FillEllipse(Brushes.Black, (x + 0.5f) * TileSize - DotSize / 2, (y + 0.5f) * TileSize - DotSize / 2, DotSize, DotSize);
					}
				}
			}
			catch (Exception) { }

			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImageUnscaled(Buffer, 0, 0);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			try
			{
				Console.WriteLine("Bang");
				int x = (e.X / TileSize);
				int y = 7 - (e.Y / TileSize);

				int index = x + y * 8;

				if (Bitboard.Get(BitState, index))
					BitState = Bitboard.Unset(BitState, index);
				else
					BitState = Bitboard.Set(BitState, index);

				UpdateCallback();
			}
			catch (Exception)
			{ }
		}
	}
}

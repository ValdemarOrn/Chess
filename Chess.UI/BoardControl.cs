using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chess.UI
{
	public class BoardControl : UserControl
	{
		int TileSize = 50;
		Brush BrushWhite;
		Brush BrushBlack;
		Brush BrushSelected;
		Brush BrushTarget;

		public Board Board;
		public int SelectedTile;

		public Action GameUpdatedCallback;

		public BoardControl()
		{
			Board = null;
			SelectedTile = -1;

			BrushWhite = Brushes.White;
			BrushBlack = Brushes.LightGray;
			BrushSelected = new SolidBrush(Color.FromArgb(80, Color.Lime));
			BrushTarget = new SolidBrush(Color.FromArgb(90, Color.SteelBlue));

		}

		public BoardControl(Board board) : this()
		{
			Board = board;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;

			// draw tiles
			for(int x=0; x<8; x++)
			{
				for(int y=0; y<8; y++)
				{
					var b = ((x + y) % 2 == 0) ? BrushWhite : BrushBlack;
					g.FillRectangle(b, x * TileSize, y * TileSize, TileSize, TileSize);
				}
			}

			// draw selected tile
			if (SelectedTile != -1)
			{
				int x = Board.X(SelectedTile);
				int y = Board.Y(SelectedTile);
				g.FillRectangle(BrushSelected, x * TileSize, (7-y) * TileSize, TileSize, TileSize);

				// Draw tiles that can be moved to
				var moves = Moves.GetValidMoves(Board, SelectedTile);
				foreach (var move in moves)
				{
					int xx = Board.X(move);
					int yy = Board.Y(move);
					g.FillRectangle(BrushTarget, xx * TileSize, (7 - yy) * TileSize, TileSize, TileSize);
				}
			}

			

			if (Board == null)
				return;

			// draw pieces
			for (int i = 0; i < 64; i++)
			{
				if (Board.State[i] == 0)
					continue;

				Bitmap map = PieceBitmaps.GetBitmap(Board.State[i]);
				int x = Board.X(i);
				int y = Board.Y(i);
				g.DrawImage(map, 2.5f + x * TileSize, 2.5f + (7-y) * TileSize, 45.0f, 45.0f);
			}
			
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			if (Board == null)
				return;

			int x = (e.X / TileSize);
			int y = 7 - (e.Y / TileSize);

			int tile = x + y * 8;

			// Check if we are moving a piece
			if (SelectedTile != -1)
			{
				var moves = Moves.GetValidMoves(Board, SelectedTile);
				if (moves.Contains(tile))
				{
					bool moved = Board.Move(SelectedTile, tile, true);
					if (moved)
					{
						SelectedTile = -1;
						GameUpdatedCallback();
						Refresh();
						return;
					}
				}
			}

			if (Board.State[tile] == 0)
				SelectedTile = -1;
			else if (SelectedTile == tile)
				SelectedTile = -1;
			else if (Board.Color(tile) == Board.PlayerTurn)
				SelectedTile = tile;
			else
				SelectedTile = -1;

			base.OnMouseClick(e);
			Invalidate();
		}
	}
}

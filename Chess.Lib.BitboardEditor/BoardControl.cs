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
		Brush BrushWhite;
		Brush BrushBlack;
		Brush BrushSelected;
		Brush BrushMove;
		int[] MoveArray;

		public IntPtr BoardStruct;

		public int SelectedTile { get; set; }

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

		int[] _state;
		public int[] State
		{
			get { return _state; }
			set
			{
				if (value == null || value.Length != 64)
					return;

				_state = value;
				RefreshBuffer();
			}
		}

		public Action UpdateCallback;

		public BoardControl()
		{
			this.DoubleBuffered = true;
			State = new int[64];

			BrushWhite = Brushes.White;
			BrushBlack = Brushes.LightGray;
			BrushSelected = new SolidBrush(Color.FromArgb(40, 20, 50, 255));
			BrushMove = new SolidBrush(Color.FromArgb(40, 200, 30, 30));

			CreateTiles();

		}

		Bitmap Tiles = null;
		Bitmap Buffer = null;

		public void CreateTiles()
		{
			try
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

						if (LabeledTiles)
						{
							int num = x + (7 - y) * 8;
							g.DrawString(Notation.TileToText(num).ToUpper(), new Font("Arial", 12), bx, xs + 2, ys + 2);
						}
					}
				}

				Buffer = new Bitmap(50 * 8, 50 * 8);
			}
			catch (Exception) { }
		}

		void RefreshBuffer()
		{
			if (Tiles == null || Buffer == null)
				return;

			var g = Graphics.FromImage(Buffer);
			g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

			g.DrawImageUnscaled(Tiles, 0, 0);

			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					int index = x + (7 - y) * 8;

					if (index == SelectedTile)
						g.FillRectangle(BrushSelected, x * TileSize, y * TileSize, TileSize, TileSize);

					if (MoveArray != null && MoveArray.Contains(index))
						g.FillRectangle(BrushMove, x * TileSize, y * TileSize, TileSize, TileSize);

					var piece = State[index];
					if (piece == 0 || (piece & 0xF0) == 0 || (piece & 0x0F) == 0)
						continue;

					Bitmap map = PieceBitmaps.GetBitmap(piece);
					if (map == null)
						continue;

					g.DrawImage(map, 2 + x * TileSize, 2 + y * TileSize, 45, 45);
						
				}
			}

			this.Invalidate();

			if (UpdateCallback != null)
				UpdateCallback();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImageUnscaled(Buffer, 0, 0);
		}

		int[] StateBefore;
		int From;
		int LastIndex;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			try
			{
				StateBefore = (int[])State.Clone();

				int x = (e.X / TileSize);
				int y = 7 - (e.Y / TileSize);

				int index = x + y * 8;

				From = index;
				LastIndex = index;
				Console.WriteLine("Down: From is " + From);
			}
			catch (Exception)
			{ }
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			Console.WriteLine("Up");
			From = -1;
			StateBefore = null;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			try
			{
				if (From == -1 || StateBefore == null)
					return;

				int x = (e.X / TileSize);
				int y = 7 - (e.Y / TileSize);

				if (x < 0 || x >= 8 || y < 0 || y >= 8)
					return;

				int index = x + y * 8;

				if (index == LastIndex)
					return;

				LastIndex = index;
				Console.WriteLine("Moving From " + From + " to " + index);

				State = (int[])StateBefore.Clone();
				var piece = State[From];
				State[From] = 0;
				State[index] = piece;
				RefreshBuffer();
			}
			catch (Exception) { }
		}

		protected override unsafe void OnMouseClick(MouseEventArgs e)
		{
			try
			{
				int x = (e.X / TileSize);
				int y = 7 - (e.Y / TileSize);

				int index = x + y * 8;

				if (index == SelectedTile)
					SelectedTile = -1;
				else
					SelectedTile = index;

				if (SelectedTile != -1 && State[SelectedTile] > 0)
				{
					Move* moveList = stackalloc Move[100];
					ulong moves = Moves.GetMoves((BoardStruct*)BoardStruct, SelectedTile);
					MoveArray = Bitboard.Bitboard_BitList(moves).Select(n => (int)n).ToArray();
				}
				else
					MoveArray = new int[0];

				RefreshBuffer();
			}
			catch (Exception) { }
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			OnMouseClick((MouseEventArgs)e);
		}

		int LastColor;

		public void HandleKeyPress(char key)
		{
			try
			{
				if (SelectedTile == -1)
					return;

				int color = State[SelectedTile] & 0xF0;
				if (color == 0)
					color = LastColor;

				if(key == '1')
				{
					color = (color == Board.COLOR_WHITE) ? Board.COLOR_BLACK : Board.COLOR_WHITE;
					State[SelectedTile] = State[SelectedTile] & 0x0F | color;
				}
				else if (key == '2')
					State[SelectedTile] = color | Board.PIECE_PAWN;
				else if (key == '3')
					State[SelectedTile] = color | Board.PIECE_KNIGHT;
				else if (key == '4')
					State[SelectedTile] = color | Board.PIECE_BISHOP;
				else if (key == '5')
					State[SelectedTile] = color | Board.PIECE_ROOK;
				else if (key == '6')
					State[SelectedTile] = color | Board.PIECE_QUEEN;
				else if (key == '7')
					State[SelectedTile] = color | Board.PIECE_KING;
				else if ((key == '8') || (key == '9') || (key == '0'))
					State[SelectedTile] = 0;

				LastColor = color;
				RefreshBuffer();
			}
			catch (Exception) { }
		}

		
	}
}

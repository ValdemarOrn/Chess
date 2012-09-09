using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess.UI
{
	partial class GameView : Form
	{
		public GameController Ctrl;

		public GameView()
		{
			InitializeComponent();
			PieceBitmaps.Dir = @"C:\Src\_Tree\Applications\Chess\Pieces\";
			Ctrl = new GameController(this);
			boardControl.GameUpdatedCallback = Ctrl.GameUpdated;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Ctrl.InitGame();
		}

		private void boardUI1_MouseClick(object sender, MouseEventArgs e)
		{
			int tile = boardControl.SelectedTile;
			labelSelectedTile.Text = "Selected Tile: " + Notation.TileToText(tile);
		}

		private void buttonSetState_Click(object sender, EventArgs e)
		{
			Ctrl.SetState(textBoxFEN.Text);
		}
	}
}

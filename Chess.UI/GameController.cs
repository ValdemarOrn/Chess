using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.UI
{
	class GameController
	{
		public GameView View;
		public Board Board;

		public GameController(GameView view)
		{
			this.View = view;
			Board = new Board();
		}

		public void InitGame()
		{
			Board.InitBoard();
			View.gameControl.Board = Board;
			View.gameControl.Refresh();
		}

		public void GameUpdated()
		{
			bool check = Check.IsChecked(Board, Board.Turn);
			SetCheck(check);
		}

		public void SetCheck(bool check)
		{
			if (check)
				View.labelChecked.Text = Colors.ToString(Board.Turn) + " is checked";
			else
				View.labelChecked.Text = "";
		}
	}
}

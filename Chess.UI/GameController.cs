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
			View.boardControl.Board = Board;
			Refresh();
		}

		public void SetState(string FEN)
		{
			var b = Notation.FENtoBoard(FEN);
			Board = b;
			View.boardControl.Board = b;
			Refresh();
		}

		public void GameUpdated()
		{
			bool check = Check.IsChecked(Board, Board.PlayerTurn);
			SetCheck(check);
			var score = AI.PositionEvaluator.EvaluatePosition(Board);
			SetScore(score);
		}

		public void Refresh()
		{
			View.boardControl.Refresh();
		}

		public void SetCheck(bool check)
		{
			if (check)
				View.labelChecked.Text = Colors.ToString(Board.PlayerTurn) + " is checked";
			else
				View.labelChecked.Text = "";
		}

		public void SetScore(int score)
		{
			View.labelScore.Text = "Score: " + score;
		}
	}
}

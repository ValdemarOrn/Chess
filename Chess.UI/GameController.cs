using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.AI;

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
			SetScore();
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

		public void SetScore()
		{
			var scores = AI.PositionEvaluator.EvaluatePosition(Board);
			View.labelWhiteScore.Text = scores.Item1.ToString();
			View.labelBlackScore.Text = scores.Item2.ToString();
			decimal diff = ((decimal)(scores.Item1.TotalScore - scores.Item2.TotalScore)) / 1000M;
			View.labelScore.Text = diff.ToString("0.00");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.AI
{
	public class MoveTree
	{
		public Tuple<int, Move> Search(Board board, int currentDepth, int finalDepth)
		{
			bool maximize = board.PlayerTurn == Colors.White;

			var moves = new List<Move>();

			for (int i = 0; i < 64; i++)
			{
				if (board.Color(i) != board.PlayerTurn)
					continue;

				var dest = Moves.GetValidMoves(board, i);
				foreach (var d in dest)
					moves.Add(new Move(i, d));
			}

			Move bestmove = new Move();

			int score = (maximize) ? -199999999 : 199999999;

			for (int i = 0; i < moves.Count; i++)
			{
				var move = moves[i];
				var temp = board.Copy();
				bool valid = temp.Move(ref move, true);
				if (!valid) throw new Exception("Can't make move!");

				int result = 0;
				if (currentDepth >= finalDepth)
					result = PositionEvaluator.ScoreDiff(PositionEvaluator.EvaluatePosition(temp));
				else
					result = Search(temp, currentDepth + 1, finalDepth).Item1;

				if (maximize)
				{
					if (result > score)
					{
						score = result;
						bestmove = move;
					}
				}
				else
				{
					if (result < score)
					{
						score = result;
						bestmove = move;
					}
				}
			}

			return new Tuple<int,Move>(score, bestmove);

		}
	}
}

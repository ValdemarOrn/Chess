using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	/// <summary>
	/// Methods to find out where a piece can legally move
	/// </summary>
	public sealed class Moves
	{
		/// <summary>
		/// Finds all moves for the piece that are legal and valid (do not leave your own king in check)
		/// </summary>
		/// <param name="board"></param>
		/// <param name="square"></param>
		/// <returns></returns>
		public static List<int> GetValidMoves(Board board, int square)
		{
			var output = new List<int>();

			var targets = GetMoves(board, square);
			foreach (var target in targets)
			{
				bool causesCheck = Check.MoveSelfChecks(board, square, target);
				if (!causesCheck)
					output.Add(target);
			}

			return output;
		}

		/// <summary>
		/// Get all moves for the piece at the particular square. Does not take into
		/// account checks, stalemate or whose move it really is.
		/// </summary>
		/// <param name="board"></param>
		/// <param name="square"></param>
		/// <returns></returns>
		public static List<int> GetMoves(Board board, int square)
		{
			var output = new List<int>();

			if (Pieces.Get(board.State[square]) == Pieces.Pawn)
				output = GetPawnMoves(board, square);
			else if (Pieces.Get(board.State[square]) == Pieces.Knight)
				output = GetKnightMoves(board, square);
			else if (Pieces.Get(board.State[square]) == Pieces.Rook)
				output = GetRookMoves(board, square);
			else if (Pieces.Get(board.State[square]) == Pieces.Bishop)
				output = GetBishopMoves(board, square);
			else if (Pieces.Get(board.State[square]) == Pieces.Queen)
				output = GetQueenMoves(board, square);
			else if (Pieces.Get(board.State[square]) == Pieces.King)
				output = GetKingMoves(board, square);

			return output;
		}

		private static List<int> GetPawnMoves(Board board, int square)
		{
			// Todo: En passant

			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);

			var output = new List<int>();

			if(color == Colors.White)
			{
				// move forward
				int target = square + 8;
				if (target < 64 && board.State[target] == 0)
				{
					output.Add(target);

					target = square + 16;
					if (y == 1 && board.State[target] == 0)
						output.Add(target);
				}

				// capture left
				target = square + 7;
				if (x > 0 && y < 7 && board.Color(target) == Colors.Black)
					output.Add(target);

				// capture right
				target = square + 9;
				if (x < 7 && y < 7 && board.Color(target) == Colors.Black)
					output.Add(target);
			}
			else
			{
				int target = square - 8;
				if (target >= 0 && board.State[target] == 0)
				{
					output.Add(target);

					target = square - 16;
					if (y == 6 && board.State[target] == 0)
						output.Add(target);
				}

				// capture left
				target = square - 9;
				if (x > 0 && y > 0 && board.Color(target) == Colors.White)
					output.Add(target);

				// capture right
				target = square - 7;
				if (x < 7 && y > 0 && board.Color(target) == Colors.White)
					output.Add(target);
			}

			return output;
		}

		private static List<int> GetKnightMoves(Board board, int square)
		{
			// -x-x-  -0-1-
			// x---x  2---3
			// --O--  --O--
			// x---x  4---5
			// -x-x-  -6-7-

			var output = new List<int>();
			int target = 0;

			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);

			// 0
			if (x > 0 && y < 6)
			{
				target = square + 15;
				if (board.Color(target) != color)
					output.Add(target);
			}
			// 1
			if (x < 7 && y < 6)
			{
				target = square + 17;
				if (board.Color(target) != color)
					output.Add(target);
			}

			// 2
			if (x > 1 && y < 7)
			{
				target = square + 6;
				if (board.Color(target) != color)
					output.Add(target);
			}
			// 3
			if (x < 6 && y < 7)
			{
				target = square + 10;
				if (board.Color(target) != color)
					output.Add(target);
			}

			// -x-x-  -0-1-
			// x---x  2---3
			// --O--  --O--
			// x---x  4---5
			// -x-x-  -6-7-

			// 4
			if (x > 1 && y > 0)
			{
				target = square - 10;
				if (board.Color(target) != color)
					output.Add(target);
			}
			// 5
			if (x < 6 && y > 0)
			{
				target = square - 6;
				if (board.Color(target) != color)
					output.Add(target);
			}

			// 6
			if (x > 0 && y > 1)
			{
				target = square - 17;
				if (board.Color(target) != color)
					output.Add(target);
			}
			// 7
			if (x < 7 && y > 1)
			{
				target = square - 15;
				if (board.Color(target) != color)
					output.Add(target);
			}

			return output;
		}

		private static List<int> GetRookMoves(Board board, int square)
		{
			var output = new List<int>();
			int color = board.Color(square);
			int target = 0;

			// Move up
			target = square + 8;
			while (target < 64 && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target += 8;
			}

			// Move down
			target = square - 8;
			while (target >= 0 && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target -= 8;
			}

			// Move right
			target = square + 1;
			while (Board.X(target) > Board.X(square) && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target++;
			}

			// Move left
			target = square - 1;
			while (Board.X(target) < Board.X(square) && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target--;
			}

			return output;
		}

		private static List<int> GetBishopMoves(Board board, int square)
		{
			var output = new List<int>();
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);
			int target = 0;

			// Move up right
			target = square + 9;
			while (target < 64 && Board.X(target) > x && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target += 9;
			}

			// Move up left
			target = square + 7;
			while (target < 64 && Board.X(target) < x && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target += 7;
			}

			// Move down right
			target = square - 7;
			while (target >= 0 && Board.X(target) > x && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target -= 7;
			}

			// Move down left
			target = square - 9;
			while (target >= 0 && Board.X(target) < x && board.Color(target) != color)
			{
				output.Add(target);
				if (board.State[target] != 0) // enemy piece
					break;
				target -= 9;
			}

			return output;
		}

		private static List<int> GetQueenMoves(Board board, int square)
		{
			var output = GetRookMoves(board, square);
			output.AddRange(GetBishopMoves(board, square));
			return output;
		}

		private static List<int> GetKingMoves(Board board, int square)
		{
			var output = new List<int>();
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);
			int target = 0;

			if (y < 7)
			{
				target = square + 7;
				if (x > 0 && board.Color(target) != color)
					output.Add(target);

				target = square + 8;
				if (board.Color(target) != color)
					output.Add(target);

				target = square + 9;
				if (x < 7 && board.Color(target) != color)
					output.Add(target);
			}

			target = square - 1;
			if (x > 0 && board.Color(target) != color)
				output.Add(target);

			target = square + 1;
			if (x < 7 && board.Color(target) != color)
				output.Add(target);

			if (y > 0)
			{
				target = square - 9;
				if (x > 0 && board.Color(target) != color)
					output.Add(target);

				target = square - 8;
				if (board.Color(target) != color)
					output.Add(target);

				target = square - 7;
				if (x < 7 && board.Color(target) != color)
					output.Add(target);
			}

			//Todo: Support castle moves for king and rook
			return output;
		}
	}
}

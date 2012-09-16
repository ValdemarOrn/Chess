using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	/// <summary>
	/// Methods to find out where a piece can legally move
	/// </summary>
	public sealed class Attacks
	{
		public static int[] GetAttacks(Board board, int square)
		{
			List<int> output = null;
			int pieceType = Pieces.Get(board.State[square]);

			switch (pieceType)
			{
				case Pieces.Pawn:
					output = GetPawnAttacks(board, square);
					break;
				case Pieces.Knight:
					output = GetKnightAttacks(board, square);
					break;
				case Pieces.Rook:
					output = GetRookAttacks(board, square);
					break;
				case Pieces.Bishop:
					output = GetBishopAttacks(board, square);
					break;
				case Pieces.Queen:
					output = GetQueenAttacks(board, square);
					break;
				case Pieces.King:
					output = GetKingAttacks(board, square);
					break;
			}

			return output.ToArray();
		}

		private static List<int> GetPawnAttacks(Board board, int square)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);

			var output = new List<int>();

			if (color == Colors.White)
			{
				// capture left
				int target = square + 7;
				if (x > 0 && y < 7)
					output.Add(target);

				// capture right
				target = square + 9;
				if (x < 7 && y < 7)
					output.Add(target);

				// en passant left
				target = square + 7;
				if (y == 4 && x > 0 && board.EnPassantTile == target && board.State[target - 8] == (Pieces.Pawn | Colors.Black))
					output.Add(target - 8); // I'm ATTACKING the pawn at target-8 even though I move to target

				// en passant right
				target = square + 9;
				if (y == 4 && x < 7 && board.EnPassantTile == target && board.State[target - 8] == (Pieces.Pawn | Colors.Black))
					output.Add(target - 8); // I'm ATTACKING the pawn at target-8 even though I move to target
			}
			else
			{
				// capture left
				int target = square - 9;
				if (x > 0 && y > 0)
					output.Add(target);

				// capture right
				target = square - 7;
				if (x < 7 && y > 0)
					output.Add(target);

				// en passant left
				target = square - 9;
				if (y == 3 && x > 0 && board.EnPassantTile == target && board.State[target + 8] == (Pieces.Pawn | Colors.White))
					output.Add(target+8); // I'm ATTACKING the pawn at target+8 even though I move to target

				// en passant right
				target = square - 7;
				if (y == 3 && x < 7 && board.EnPassantTile == target && board.State[target + 8] == (Pieces.Pawn | Colors.White))
					output.Add(target + 8); // I'm ATTACKING the pawn at target+8 even though I move to target
			}

			return output;
		}

		private static List<int> GetKnightAttacks(Board board, int square)
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
				output.Add(target);
			}
			// 1
			if (x < 7 && y < 6)
			{
				target = square + 17;
				output.Add(target);
			}

			// 2
			if (x > 1 && y < 7)
			{
				target = square + 6;
				output.Add(target);
			}
			// 3
			if (x < 6 && y < 7)
			{
				target = square + 10;
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
				output.Add(target);
			}
			// 5
			if (x < 6 && y > 0)
			{
				target = square - 6;
				output.Add(target);
			}

			// 6
			if (x > 0 && y > 1)
			{
				target = square - 17;
				output.Add(target);
			}
			// 7
			if (x < 7 && y > 1)
			{
				target = square - 15;
				output.Add(target);
			}

			return output;
		}

		private static List<int> GetRookAttacks(Board board, int square)
		{
			var output = new List<int>();
			int color = board.Color(square);
			int target = 0;

			// Move up
			target = square + 8;
			while (target < 64)
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target += 8;
			}

			// Move down
			target = square - 8;
			while (target >= 0)
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target -= 8;
			}

			// Move right
			target = square + 1;
			while (Board.X(target) > Board.X(square))
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target++;
			}

			// Move left
			target = square - 1;
			while (Board.X(target) < Board.X(square))
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target--;
			}

			return output;
		}

		private static List<int> GetBishopAttacks(Board board, int square)
		{
			var output = new List<int>();
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);
			int target = 0;

			// Move up right
			target = square + 9;
			while (target < 64 && Board.X(target) > x)
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target += 9;
			}

			// Move up left
			target = square + 7;
			while (target < 64 && Board.X(target) < x)
			{
				output.Add(target);
				if (board.State[target] != 0) //  piece
					break;
				target += 7;
			}

			// Move down right
			target = square - 7;
			while (target >= 0 && Board.X(target) > x)
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target -= 7;
			}

			// Move down left
			target = square - 9;
			while (target >= 0 && Board.X(target) < x)
			{
				output.Add(target);
				if (board.State[target] != 0) // piece
					break;
				target -= 9;
			}

			return output;
		}

		private static List<int> GetQueenAttacks(Board board, int square)
		{
			var output = GetRookAttacks(board, square);
			output.AddRange(GetBishopAttacks(board, square));
			return output;
		}

		private static List<int> GetKingAttacks(Board board, int square)
		{
			var output = new List<int>();
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);
			int target = 0;

			if (y < 7)
			{
				target = square + 7;
				if (x > 0)
					output.Add(target);

				target = square + 8;
				output.Add(target);

				target = square + 9;
				if (x < 7)
					output.Add(target);
			}

			target = square - 1;
			if (x > 0)
				output.Add(target);

			target = square + 1;
			if (x < 7)
				output.Add(target);

			if (y > 0)
			{
				target = square - 9;
				if (x > 0)
					output.Add(target);

				target = square - 8;
				output.Add(target);

				target = square - 7;
				if (x < 7)
					output.Add(target);
			}

			return output;
		}
	}
}

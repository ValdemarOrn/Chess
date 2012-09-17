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
			int movecount = 0;
			int[] moves = new int[28];
			int pieceType = Pieces.Get(board.State[square]);

			switch (pieceType)
			{
				case Pieces.Pawn:
					GetPawnAttacks(board, square, moves, ref movecount);
					break;
				case Pieces.Knight:
					GetKnightAttacks(board, square, moves, ref movecount);
					break;
				case Pieces.Rook:
					GetRookAttacks(board, square, moves, ref movecount);
					break;
				case Pieces.Bishop:
					GetBishopAttacks(board, square, moves, ref movecount);
					break;
				case Pieces.Queen:
					GetQueenAttacks(board, square, moves, ref movecount);
					break;
				case Pieces.King:
					GetKingAttacks(board, square, moves, ref movecount);
					break;
			}

			int[] output = new int[movecount];
			for (int i = 0; i < movecount; i++)
				output[i] = moves[i];

			return output;
		}

		private static void GetPawnAttacks(Board board, int square, int[] moves, ref int count)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);

			if (color == Colors.White)
			{
				// capture left
				int target = square + 7;
				if (x > 0 && y < 7)
				{
					moves[count] = target;
					count++;
				}

				// capture right
				target = square + 9;
				if (x < 7 && y < 7)
				{
					moves[count] = target;
					count++;
				}

				// en passant left
				target = square + 7;
				if (y == 4 && x > 0 && board.EnPassantTile == target && board.State[target - 8] == (Pieces.Pawn | Colors.Black))
				{
					moves[count] = target - 8; // I'm ATTACKING the pawn at target-8 even though I move to target
					count++;
				}

				// en passant right
				target = square + 9;
				if (y == 4 && x < 7 && board.EnPassantTile == target && board.State[target - 8] == (Pieces.Pawn | Colors.Black))
				{
					moves[count] = target - 8; // I'm ATTACKING the pawn at target-8 even though I move to target
					count++;
				}
			}
			else
			{
				// capture left
				int target = square - 9;
				if (x > 0 && y > 0)
				{
					moves[count] = target;
					count++;
				}

				// capture right
				target = square - 7;
				if (x < 7 && y > 0)
				{
					moves[count] = target;
					count++;
				}

				// en passant left
				target = square - 9;
				if (y == 3 && x > 0 && board.EnPassantTile == target && board.State[target + 8] == (Pieces.Pawn | Colors.White))
				{
					moves[count] = target+8; // I'm ATTACKING the pawn at target+8 even though I move to target
					count++;
				}

				// en passant right
				target = square - 7;
				if (y == 3 && x < 7 && board.EnPassantTile == target && board.State[target + 8] == (Pieces.Pawn | Colors.White))
				{
					moves[count] = target + 8; // I'm ATTACKING the pawn at target+8 even though I move to target
					count++;
				}
			}
		}

		private static void GetKnightAttacks(Board board, int square, int[] moves, ref int count)
		{
			// -x-x-  -0-1-
			// x---x  2---3
			// --O--  --O--
			// x---x  4---5
			// -x-x-  -6-7-

			int target = 0;

			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);

			// 0
			if (x > 0 && y < 6)
			{
				target = square + 15;
				moves[count] = target;
				count++;
			}
			// 1
			if (x < 7 && y < 6)
			{
				target = square + 17;
				moves[count] = target;
				count++;
			}

			// 2
			if (x > 1 && y < 7)
			{
				target = square + 6;
				moves[count] = target;
				count++;
			}
			// 3
			if (x < 6 && y < 7)
			{
				target = square + 10;
				moves[count] = target;
				count++;
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
				moves[count] = target;
				count++;
			}
			// 5
			if (x < 6 && y > 0)
			{
				target = square - 6;
				moves[count] = target;
				count++;
			}

			// 6
			if (x > 0 && y > 1)
			{
				target = square - 17;
				moves[count] = target;
				count++;
			}
			// 7
			if (x < 7 && y > 1)
			{
				target = square - 15;
				moves[count] = target;
				count++;
			}
		}

		private static void GetRookAttacks(Board board, int square, int[] moves, ref int count)
		{
			int color = board.Color(square);
			int target = 0;

			// Move up
			target = square + 8;
			while (target < 64)
			{
				moves[count] = target; 
				count++;
				if (board.State[target] != 0) // piece
					break;
				target += 8;
			}

			// Move down
			target = square - 8;
			while (target >= 0)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // piece
					break;
				target -= 8;
			}

			// Move right
			target = square + 1;
			while (Board.X(target) > Board.X(square))
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // piece
					break;
				target++;
			}

			// Move left
			target = square - 1;
			while (Board.X(target) < Board.X(square))
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // piece
					break;
				target--;
			}
		}

		private static void GetBishopAttacks(Board board, int square, int[] moves, ref int count)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);
			int target = 0;

			// Move up right
			target = square + 9;
			while (target < 64 && Board.X(target) > x)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // piece
					break;
				target += 9;
			}

			// Move up left
			target = square + 7;
			while (target < 64 && Board.X(target) < x)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) //  piece
					break;
				target += 7;
			}

			// Move down right
			target = square - 7;
			while (target >= 0 && Board.X(target) > x)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // piece
					break;
				target -= 7;
			}

			// Move down left
			target = square - 9;
			while (target >= 0 && Board.X(target) < x)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // piece
					break;
				target -= 9;
			}
		}

		private static void GetQueenAttacks(Board board, int square, int[] moves, ref int count)
		{
			GetRookAttacks(board, square, moves, ref count);
			GetBishopAttacks(board, square, moves, ref count);
		}

		private static void GetKingAttacks(Board board, int square, int[] moves, ref int count)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			int color = board.Color(square);
			int target = 0;

			if (y < 7)
			{
				target = square + 7;
				if (x > 0)
				{
					moves[count] = target;
					count++;
				}

				target = square + 8;
				moves[count] = target;
				count++;

				target = square + 9;
				if (x < 7)
				{
					moves[count] = target;
					count++;
				}
			}

			target = square - 1;
			if (x > 0)
			{
				moves[count] = target;
				count++;
			}

			target = square + 1;
			if (x < 7)
			{
				moves[count] = target;
				count++;
			}

			if (y > 0)
			{
				target = square - 9;
				if (x > 0)
				{
					moves[count] = target;
					count++;
				}

				target = square - 8;
				moves[count] = target;
				count++;

				target = square - 7;
				if (x < 7)
				{
					moves[count] = target;
					count++;
				}
			}
		}
	}
}

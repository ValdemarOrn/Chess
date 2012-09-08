using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public sealed class Check
	{
		/// <summary>
		/// Evaluates how many pieces can attack each tile
		/// </summary>
		/// <param name="board"></param>
		/// <param name="attackerColor"></param>
		/// <returns></returns>
		public static int[] GetAttackBoard(Board board, int attackerColor)
		{
			int color = attackerColor;
			var attackBoard = new int[64];

			// Search for all pieces of the attacker's color
			for (int i = 0; i < 64; i++)
			{
				if (board.Color(i) != color)
					continue;

				// find all the moves this piece can make
				var moves = Attacks.GetAttacks(board, i);

				// mark them on the attack board
				foreach (int move in moves)
					attackBoard[move]++;
			}

			return attackBoard;
		}

		/// <summary>
		/// Checks if the king of the specified color is in check
		/// </summary>
		/// <param name="board"></param>
		/// <param name="kingColor"></param>
		/// <returns></returns>
		public static bool IsChecked(Board board, int kingColor, int kingLocation = -1)
		{
			var attackerColor = (kingColor == Colors.White) ? Colors.Black : Colors.White;
			var attacks = GetAttackBoard(board, attackerColor);
			
			if(kingLocation == -1)
				kingLocation = board.KingLocation(kingColor);

			// king is not in check
			if (attacks[kingLocation] == 0)
				return false;
			else 
				return true;
		}

		/// <summary>
		/// Check if there is a checkmate
		/// </summary>
		/// <param name="board"></param>
		/// <param name="kingColor"></param>
		/// <returns></returns>
		public static bool IsCheckMate(Board board, int kingColor)
		{
			var attackerColor = (kingColor == Colors.White) ? Colors.Black : Colors.White;
			var attacks = GetAttackBoard(board, attackerColor);
			var kingLocation = board.KingLocation(kingColor);

			// king is not in check
			if (attacks[kingLocation] == 0)
				return false;

			// Try all possible moves for the defenders pieces
			// If there are any valid moves, then the defender can break the check.
			for (int i = 0; i < 64; i++)
			{
				if (board.Color(i) != kingColor)
					continue;

				var moves = Moves.GetValidMoves(board, i);
				if (moves.Count > 0)
					return false;
			}

			// King could not get out of check, he has been check mated
			return true;
		}

		/// <summary>
		/// Checks for stale mate, where there are no valid moves
		/// </summary>
		/// <param name="board"></param>
		/// <param name="kingColor"></param>
		/// <returns></returns>
		public static bool IsStalemate(Board board, int kingColor)
		{
			// It's not a stalemate if he's in check
			if (IsChecked(board, kingColor))
				return false;

			// Try all possible moves for the defenders pieces
			// If there are any valid moves, then the defender is not in a stalemate
			// Note: pawns are the only pieces that can be blocked from moving, aside from king
			for (int i = 0; i < 64; i++)
			{
				if (board.Color(i) != kingColor)
					continue;

				var moves = Moves.GetValidMoves(board, i);
				if (moves.Count > 0)
					return false;
			}

			// King could not get out of check, he has been check mated
			return true;
		}

		/// <summary>
		/// Figures out if the specified move puts or keeps your own king in check, which is illegal
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static bool MoveSelfChecks(Board board, int from, int to)
		{
			int colorToMove = board.Color(from);

			board = board.Copy();
			board.Move(from, to);
			bool ok = Check.IsChecked(board, colorToMove);
			return ok;
		}
	}
}

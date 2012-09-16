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
		public const int HasCastled = -1;
		public const int CannotCastle = 0;
		public const int CanCastle = 1;

		public const int CastleKingsideWhite = 1;
		public const int CastleQueensideWhite = 2;
		public const int CastleKingsideBlack = 3;
		public const int CastleQueensideBlack = 4;

		/// <summary>
		/// Checks if the move is a castling move. Returns the type of castling that is
		/// executed. Returns 0 if not a castling move. See Moves.CastleXXX constants in the Moves class
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static int IsCastlingMove(Board board, int from, int to)
		{
			// white kingside
			if (from == 4 && to == 6 && board.State[from] == (Pieces.King | Colors.White))
				return CastleKingsideWhite;

			// white queenside
			if (from == 4 && to == 2 && board.State[from] == (Pieces.King | Colors.White))
				return CastleQueensideWhite;

			// black kingside
			if (from == 60 && to == 62 && board.State[from] == (Pieces.King | Colors.Black))
				return CastleKingsideBlack;

			// black queenside
			if (from == 60 && to == 58 && board.State[from] == (Pieces.King | Colors.Black))
				return CastleQueensideBlack;

			return 0;
		}

		/// <summary>
		/// Checks if the move is an en passant move
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static bool IsEnPassantCapture(Board board, int from, int to)
		{
			var color = board.Color(from);

			if (board.Piece(from) != Pieces.Pawn)
				return false;

			if (color == Colors.White)
			{
				if (to == from + 7 || to == from + 9)
					if (board.EnPassantTile == to)
						return true;
			}

			if (color == Colors.Black)
			{
				if (to == from - 7 || to == from - 9)
					if (board.EnPassantTile == to)
						return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the tile of the pawn that is killed in an en passant capture
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static int EnPassantVictim(Board board, int from, int to)
		{
			var color = board.Color(from);

			if (color == Colors.White)
				return to - 8;

			if (color == Colors.Black)
				return to + 8;

			throw new Exception("Unrecognized color");
		}

		/// <summary>
		/// Checks if the move is pawn moving 2 tiles, and returns the tile at which it can be captured
		/// (between from and to). Returns 0 if the piece can not be captured in an en passant move
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static int EnPassantTile(Board board, int from, int to)
		{
			var color = board.Color(from);
			var piece = board.Piece(from);
			var y = Board.Y(from);

			if (piece != Pieces.Pawn)
				return 0;

			if (color == Colors.White && y == 1 && to == (from + 16))
				return from + 8;

			if (color == Colors.Black && y == 6 && to == (from - 16))
				return from - 8;

			return 0;
		}

		public static bool IsCaptureMove(Board board, int from, int to)
		{
			if (board.State[to] != 0 || IsEnPassantCapture(board, from, to))
				return true;

			return false;
		}

		/// <summary>
		/// Checks if a piece can be promoted
		/// </summary>
		/// <param name="square"></param>
		/// <param name="pieceType"></param>
		/// <returns></returns>
		public static bool CanPromote(Board board, int square)
		{
			// Can only promote pawns
			if (board.Piece(square) != Pieces.Pawn)
				return false;

			int color = board.Color(square);

			if (Board.Y(square) == 7 && color == Colors.White)
				return true;

			if (Board.Y(square) == 0 && color == Colors.Black)
				return true;

			return false;
		}

		/// <summary>
		/// Checks if a rook at the specified square still has the ability to castle
		/// </summary>
		/// <param name="board"></param>
		/// <param name="square"></param>
		/// <returns></returns>
		public static bool CanRookStillCastle(Board board, int square)
		{
			switch(square)
			{
				case(0):
					return board.CanCastleQWhite;
				case (7):
					return board.CanCastleKWhite;
				case (56):
					return board.CanCastleQBlack;
				case (63):
					return board.CanCastleKBlack;
				default:
					return false;
			}
		}

		/// <summary>
		/// Finds all moves for the piece that are legal and valid (do not leave your own king in check)
		/// </summary>
		/// <param name="board"></param>
		/// <param name="square"></param>
		/// <returns></returns>
		public static int[] GetValidMoves(Board board, int square)
		{
			var output = new List<int>();

			var targets = GetMoves(board, square);
			foreach (var target in targets)
			{
				bool causesCheck = Check.MoveSelfChecks(board, square, target);
				if (!causesCheck)
					output.Add(target);
			}

			return output.ToArray();
		}

		/// <summary>
		/// Get all moves for the piece at the particular square. Does not take into
		/// account checks, stalemate or whose move it really is.
		/// </summary>
		/// <param name="board"></param>
		/// <param name="square"></param>
		/// <returns></returns>
		public static int[] GetMoves(Board board, int square)
		{
			List<int> output = null;
			int pieceType = Pieces.Get(board.State[square]);

			switch(pieceType)
			{
				case Pieces.Pawn:
					output = GetPawnMoves(board, square);
					break;
				case Pieces.Knight:
					output = GetKnightMoves(board, square);
					break;
				case Pieces.Rook:
					output = GetRookMoves(board, square);
					break;
				case Pieces.Bishop:
					output = GetBishopMoves(board, square);
					break;
				case Pieces.Queen:
					output = GetQueenMoves(board, square);
					break;
				case Pieces.King:
					output = GetKingMoves(board, square);
					break;
			}

			return output.ToArray();
		}

		private static List<int> GetPawnMoves(Board board, int square)
		{
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

				// en passant left
				target = square + 7;
				if (y == 4 && x > 0 && board.EnPassantTile == target && board.State[target - 8] == (Pieces.Pawn | Colors.Black))
					output.Add(target);

				// en passant right
				target = square + 9;
				if (y == 4 && x < 7 && board.EnPassantTile == target && board.State[target - 8] == (Pieces.Pawn | Colors.Black))
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

				// en passant left
				target = square - 9;
				if (y == 3 && x > 0 && board.EnPassantTile == target && board.State[target + 8] == (Pieces.Pawn | Colors.White))
					output.Add(target);

				// en passant right
				target = square - 7;
				if (y == 3 && x < 7 && board.EnPassantTile == target && board.State[target + 8] == (Pieces.Pawn | Colors.White))
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

			// castling
			if (color == Colors.White && square == 4)
			{
				if (board.CanCastleKWhite && board.State[5] == 0 && board.State[6] == 0)
					output.Add(6);
				if (board.CanCastleQWhite && board.State[3] == 0 && board.State[2] == 0 && board.State[1] == 0)
					output.Add(2);
			}
			else if (color == Colors.Black && square == 60)
			{
				if (board.CanCastleKBlack && board.State[61] == 0 && board.State[62] == 0)
					output.Add(62);
				if (board.CanCastleQBlack && board.State[59] == 0 && board.State[58] == 0 && board.State[57] == 0)
					output.Add(58);
			}

			return output;
		}
	}
}

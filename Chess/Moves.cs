using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	/// <summary>
	/// Methods to find out where a piece can legally move
	/// </summary>
	public sealed class Moves
	{
		/// <summary>
		/// Checks if the move is a castling move. Returns the type of castling that is
		/// executed. Returns 0 if not a castling move. See Moves.CastleXXX constants in the Moves class
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public static Castling IsCastlingMove(Board board, int from, int to)
		{
			// white kingside
			if (from == 4 && to == 6 && board.State[from] == ((int)Piece.King | (int)Color.White))
				return Castling.KingsideWhite;

			// white queenside
			if (from == 4 && to == 2 && board.State[from] == ((int)Piece.King | (int)Color.White))
				return Castling.QueensideWhite;

			// black kingside
			if (from == 60 && to == 62 && board.State[from] == ((int)Piece.King | (int)Color.Black))
				return Castling.KingsideBlack;

			// black queenside
			if (from == 60 && to == 58 && board.State[from] == ((int)Piece.King | (int)Color.Black))
				return Castling.QueensideBlack;

			return Castling.None;
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
			return (to != 0) && (board.EnPassantTile == to) && (board.GetPiece(from) == Piece.Pawn);
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
			var color = board.GetColor(from);

			if (color == Color.White)
				return to - 8;

			if (color == Color.Black)
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
			var color = board.GetColor(from);
			var piece = board.GetPiece(from);
			var y = Board.Y(from);

			if (piece != Piece.Pawn)
				return 0;

			if (color == Color.White && y == 1 && to == (from + 16))
				return from + 8;

			if (color == Color.Black && y == 6 && to == (from - 16))
				return from - 8;

			return 0;
		}

		public static bool IsCaptureMove(Board board, int from, int to)
		{
			if (board.State[to] != 0 || to == board.EnPassantTile)
				return true;

			return false;
		}

		/// <summary>
		/// Checks if a piece on the board can be promoted
		/// </summary>
		/// <param name="square"></param>
		/// <param name="pieceType"></param>
		/// <returns></returns>
		public static bool CanPromote(Board board, int square)
		{
			// Can only promote pawns
			if (board.GetPiece(square) != Piece.Pawn)
				return false;

			Color color = board.GetColor(square);

			if (Board.Y(square) == 7 && color == Color.White)
				return true;

			if (Board.Y(square) == 0 && color == Color.Black)
				return true;

			return false;
		}

		/// <summary>
		/// Checks if a piece can be promoted
		/// </summary>
		/// <param name="square"></param>
		/// <param name="pieceType"></param>
		/// <returns></returns>
		public static bool CanPromote(int square, Color color, Piece piece)
		{
			int y = Board.Y(square);
			return (piece == Piece.Pawn) && ((y == 7 && color == Color.White) || (y == 0 && color == Color.Black));
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
		/// Returns all valid moves for the current player
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public static Move[] GetMoves(Board board)
		{
			var moves = new List<Move>();
			var player = board.PlayerTurn;
			for(int i = 0; i < 64; i++)
			{
				if (board.GetColor(i) != player)
					continue;

				Piece piece = board.GetPiece(i);

				var dest = GetMoves(board, i);
				for (int j = 0; j < dest.Length; j++)
				{
					var move = new Move();
					move.From = i;
					move.To = dest[j];
					moves.Add(move);

					if(CanPromote(move.To, player, piece))
					{
						var move2 = move.Copy();
						var move3 = move.Copy();
						var move4 = move.Copy();

						move.Promotion = Piece.Knight;
						move2.Promotion = Piece.Bishop;
						move3.Promotion = Piece.Rook;
						move4.Promotion = Piece.Queen;
						
						moves.Add(move2);
						moves.Add(move3);
						moves.Add(move4);
					}
				}
			}

			return moves.ToArray();
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
				bool causesCheck = board.MoveSelfChecks(square, target);
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
			int movecount = 0;
			int[] moves = new int[28];
			Piece pieceType = Pieces.Get(board.State[square]);

			switch(pieceType)
			{
				case Piece.Pawn:
					GetPawnMoves(board, square, moves, ref movecount);
					break;
				case Piece.Knight:
					GetKnightMoves(board, square, moves, ref movecount);
					break;
				case Piece.Rook:
					GetRookMoves(board, square, moves, ref movecount);
					break;
				case Piece.Bishop:
					GetBishopMoves(board, square, moves, ref movecount);
					break;
				case Piece.Queen:
					GetQueenMoves(board, square, moves, ref movecount);
					break;
				case Piece.King:
					GetKingMoves(board, square, moves, ref movecount);
					break;
			}

			int[] output = new int[movecount];
			for (int i = 0; i < movecount; i++)
				output[i] = moves[i];

			return output;
		}

		private static void GetPawnMoves(Board board, int square, int[] moves, ref int count)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			Color color = board.GetColor(square);

			if (y == 0 || y == 7) // cannot ever happen
				return;

			if(color == Color.White)
			{
				// move forward
				int target = square + 8;
				if (target < 64 && board.State[target] == 0)
				{
					moves[count] = target;
					count++;

					target = square + 16;
					if (y == 1 && board.State[target] == 0)
					{
						moves[count] = target;
						count++;
					}
				}

				// capture left
				target = square + 7;
				if (x > 0 && y < 7 && board.GetColor(target) == Color.Black)
				{
					moves[count] = target;
					count++;
				}

				// capture right
				target = square + 9;
				if (x < 7 && y < 7 && board.GetColor(target) == Color.Black)
				{
					moves[count] = target;
					count++;
				}

				// en passant left
				target = square + 7;
				if (y == 4 && x > 0 && board.EnPassantTile == target && board.State[target - 8] == ((int)Piece.Pawn | (int)Color.Black))
				{
					moves[count] = target;
					count++;
				}

				// en passant right
				target = square + 9;
				if (y == 4 && x < 7 && board.EnPassantTile == target && board.State[target - 8] == ((int)Piece.Pawn | (int)Color.Black))
				{
					moves[count] = target;
					count++;
				}
			}
			else
			{
				int target = square - 8;
				if (target >= 0 && board.State[target] == 0)
				{
					moves[count] = target;
					count++;

					target = square - 16;
					if (y == 6 && board.State[target] == 0)
					{
						moves[count] = target;
						count++;
					}
				}

				// capture left
				target = square - 9;
				if (x > 0 && y > 0 && board.GetColor(target) == Color.White)
				{
					moves[count] = target;
					count++;
				}

				// capture right
				target = square - 7;
				if (x < 7 && y > 0 && board.GetColor(target) == Color.White)
				{
					moves[count] = target;
					count++;
				}

				// en passant left
				target = square - 9;
				if (y == 3 && x > 0 && board.EnPassantTile == target && board.State[target + 8] == Colors.Val(Piece.Pawn, Color.White))
				{
					moves[count] = target;
					count++;
				}

				// en passant right
				target = square - 7;
				if (y == 3 && x < 7 && board.EnPassantTile == target && board.State[target + 8] == Colors.Val(Piece.Pawn, Color.White))
				{
					moves[count] = target;
					count++;
				}
			}
		}

		private static void GetKnightMoves(Board board, int square, int[] moves, ref int count)
		{
			// -x-x-  -0-1-
			// x---x  2---3
			// --O--  --O--
			// x---x  4---5
			// -x-x-  -6-7-

			int target = 0;

			int x = Board.X(square);
			int y = Board.Y(square);
			Color color = board.GetColor(square);

			// 0
			if (x > 0 && y < 6)
			{
				target = square + 15;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}
			// 1
			if (x < 7 && y < 6)
			{
				target = square + 17;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}

			// 2
			if (x > 1 && y < 7)
			{
				target = square + 6;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}
			// 3
			if (x < 6 && y < 7)
			{
				target = square + 10;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
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
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}
			// 5
			if (x < 6 && y > 0)
			{
				target = square - 6;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}

			// 6
			if (x > 0 && y > 1)
			{
				target = square - 17;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}
			// 7
			if (x < 7 && y > 1)
			{
				target = square - 15;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}
		}

		private static void GetRookMoves(Board board, int square, int[] moves, ref int count)
		{
			Color color = board.GetColor(square);
			int target = 0;

			// Move up
			target = square + 8;
			while (target < 64 && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target += 8;
			}

			// Move down
			target = square - 8;
			while (target >= 0 && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target -= 8;
			}

			// Move right
			target = square + 1;
			while (Board.X(target) > Board.X(square) && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target++;
			}

			// Move left
			target = square - 1;
			while (Board.X(target) < Board.X(square) && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target--;
			}
		}

		private static void GetBishopMoves(Board board, int square, int[] moves, ref int count)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			Color color = board.GetColor(square);
			int target = 0;

			// Move up right
			target = square + 9;
			while (target < 64 && Board.X(target) > x && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target += 9;
			}

			// Move up left
			target = square + 7;
			while (target < 64 && Board.X(target) < x && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target += 7;
			}

			// Move down right
			target = square - 7;
			while (target >= 0 && Board.X(target) > x && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target -= 7;
			}

			// Move down left
			target = square - 9;
			while (target >= 0 && Board.X(target) < x && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
				if (board.State[target] != 0) // enemy piece
					break;
				target -= 9;
			}

		}

		private static void GetQueenMoves(Board board, int square, int[] moves, ref int count)
		{
			GetRookMoves(board, square, moves, ref count);
			GetBishopMoves(board, square, moves, ref count);
		}

		private static void GetKingMoves(Board board, int square, int[] moves, ref int count)
		{
			int x = Board.X(square);
			int y = Board.Y(square);
			Color color = board.GetColor(square);
			int target = 0;

			if (y < 7)
			{
				target = square + 7;
				if (x > 0 && board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}

				target = square + 8;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}

				target = square + 9;
				if (x < 7 && board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}

			target = square - 1;
			if (x > 0 && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
			}

			target = square + 1;
			if (x < 7 && board.GetColor(target) != color)
			{
				moves[count] = target;
				count++;
			}

			if (y > 0)
			{
				target = square - 9;
				if (x > 0 && board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}

				target = square - 8;
				if (board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}

				target = square - 7;
				if (x < 7 && board.GetColor(target) != color)
				{
					moves[count] = target;
					count++;
				}
			}

			// castling
			// no quares between rook and king can be occupied, the king cannot pass through any checked attacked squares
			if (color == Color.White && square == 4)
			{
				if (board.CanCastleKWhite && board.State[5] == 0 && board.State[6] == 0)
				{
					var attacks = board.GetAttackBoard(Color.Black);
					if (attacks[4] == 0 && attacks[5] == 0 && attacks[6] == 0)
					{
						moves[count] = 6;
						count++;
					}
				}
				if (board.CanCastleQWhite && board.State[3] == 0 && board.State[2] == 0 && board.State[1] == 0)
				{
					var attacks = board.GetAttackBoard(Color.Black);
					if (attacks[2] == 0 && attacks[3] == 0 && attacks[4] == 0)
					{
						moves[count] = 2;
						count++;
					}
				}
			}
			else if (color == Color.Black && square == 60)
			{
				if (board.CanCastleKBlack && board.State[61] == 0 && board.State[62] == 0)
				{
					var attacks = board.GetAttackBoard(Color.White);
					if (attacks[60] == 0 && attacks[61] == 0 && attacks[62] == 0)
					{
						moves[count] = 62;
						count++;
					}
				}
				if (board.CanCastleQBlack && board.State[59] == 0 && board.State[58] == 0 && board.State[57] == 0)
				{
					var attacks = board.GetAttackBoard(Color.White);
					if (attacks[58] == 0 && attacks[59] == 0 && attacks[60] == 0)
					{
						moves[count] = 58;
						count++;
					}
				}
			}
		}
	}
}

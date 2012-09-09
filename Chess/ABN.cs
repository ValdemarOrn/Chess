using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public class ABN
	{
		/// <summary>
		/// Read Algebraic notation and return a list of all moves made
		/// </summary>
		/// <param name="initBoard"></param>
		/// <param name="ANString"></param>
		/// <returns></returns>
		public static List<Move> ABNToMoves(Board initBoard, string ANString)
		{
			var board = initBoard;
			var moves = new List<Move>();

			var tokens = ANString.Split(' ').Select(x => x.Trim()).ToList();
			int turn = 0;
			int i = 0;

			try
			{
				for (i = 0; i < tokens.Count; i++)
				{
					string token = tokens[i];

					// ---------------------- Parse the turn ----------------------
					if (token.Contains('.'))
					{
						int t = Convert.ToInt32(token.Replace(".", ""));
						if (t < turn)
							throw new Exception("Turn was lower than expected");
						if (t == turn && moves.Count(x => x.Turn == t) != 1) // checks if we already have white's move
							throw new Exception("Turn was lower than expected");
						if (t > turn + 1)
							throw new Exception("Missed turn " + (turn + 1));
						turn = t;
						continue;
					}

					// ---------------------- Parse the move ----------------------
					Move move = GetMove(board, token, board.Turn);
					move.Turn = turn;
					moves.Add(move);
					board.Move(move.From, move.To, true);
				}
			}
			catch (Exception e)
			{
				throw new Exception("Error parsing algebraic notation at token " + i, e);
			}

			return moves;
		}

		private static Move GetMove(Board board, string token, int color)
		{
			// Process captures
			bool capture = token.Contains('x');
			token = token.Replace("x", "");

			// find the piece
			int piece = GetPiece(token[0]);

			// remove the piece identifier from the string
			if (piece != Pieces.Pawn)
				token = token.Substring(1);

			// Is check
			bool check = token.Contains('+');
			token = token.Replace("+", "");

			// Is mate
			bool mate = token.Contains('#');
			token = token.Replace("#", "");

			// Is castle
			bool queenside = token.Contains("O-O-O");
			token = token.Replace("O-O-O", "");

			bool kingside = token.Contains("O-O");
			token = token.Replace("O-O", "");

			// Return move if castled

			if (kingside && color == Colors.White)
			{
				if (board.CastleKingsideWhite && board.State[4] == (Pieces.King | Colors.White) && board.State[5] == 0 && board.State[6] == 0 && board.State[7] == (Pieces.Rook | Colors.White))
					return new Move(4, 6, 0, color, false, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Kingside castling for white should not be allowed at this point");
			}
			if (queenside && color == Colors.White)
			{
				if (board.CastleQueensideWhite && board.State[4] == (Pieces.King | Colors.White) && board.State[3] == 0 && board.State[2] == 0 && board.State[1] == 0 && board.State[0] == (Pieces.Rook | Colors.White))
					return new Move(4, 2, 0, color, false, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Queenside castling for white should not be allowed at this point");
			}

			if (kingside && color == Colors.Black)
			{
				if (board.CastleKingsideBlack && board.State[60] == (Pieces.King | Colors.Black) && board.State[61] == 0 && board.State[62] == 0 && board.State[63] == (Pieces.Rook | Colors.Black))
					return new Move(60, 62, 0, color, false, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Kingside castling for black should not be allowed at this point");
			}
			if (queenside && color == Colors.Black)
			{
				if (board.CastleQueensideBlack && board.State[60] == (Pieces.King | Colors.Black) && board.State[59] == 0 && board.State[58] == 0 && board.State[57] == 0 && board.State[56] == (Pieces.Rook | Colors.Black))
					return new Move(60, 58, 0, color, false, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Queenside castling for black should not be allowed at this point");
			}


			// promotion
			char promotedTo = (char)0;
			bool promotion = token.Contains('=');
			if (promotion)
			{
				promotedTo = token[token.IndexOf('=') + 1];
				// remove promotion from string
				token = token.Substring(0, token.IndexOf('=')) + token.Substring(token.IndexOf('=') + 1);
			}

			// find the target
			int target = 0;
			if (!kingside && !queenside)
			{
				string targetString = token.Substring(token.Length - 2);
				target = Notation.TextToTile(targetString);
				token = token.Substring(0, token.Length - 2);
			}

			// find the hints
			Tuple<int, int> hint = GetHint(token);

			// figure out which piece is moving
			int from = -1;
			var pieces = board.FindByPieceAndColor(color | piece);
			var possible = new List<int>();
			foreach (var p in pieces)
			{
				var moves = Moves.GetValidMoves(board, p);
				if (moves.Contains(target))
					possible.Add(p);
			}

			if(possible.Count == 0)
				throw new Exception("No piece has a valid move to tile " + Notation.TileToText(target));
			
			if (possible.Count > 1)
			{
				// filter possible pieces by using the hints
				if (hint.Item1 != -1)
					possible = possible.Where(k => Board.X(k) == hint.Item1).ToList();
				if (hint.Item2 != -1)
					possible = possible.Where(k => Board.Y(k) == hint.Item2).ToList();
			}

			if (possible.Count != 1)
				throw new Exception("No piece has a valid move to tile " + Notation.TileToText(target));

			from = possible[0];

			return new Move(from, target, 0, color, capture, GetPiece(promotedTo), check, mate, queenside, kingside);
		}

		private static Tuple<int, int> GetHint(string token)
		{
			int x = -1;
			int y = -1;

			if (token.Length > 0)
			{
				char hint = token[0];
				if (hint >= 'a' && hint <= 'h')
					x = (hint - 'a');
				if (hint >= '1' && hint <= '8')
					y = (hint - '1');
			}

			if (token.Length > 1)
			{
				char hint = token[1];
				if (hint >= 'a' && hint <= 'h')
					x = (hint - 'a');
				if (hint >= '1' && hint <= '8')
					y = (hint - '1');
			}

			return new Tuple<int, int>(x, y);
		}

		private static int GetPiece(char p)
		{
			if (p == (char)0)
				return 0;

			switch (p)
			{
				case 'N':
					return Pieces.Knight;
				case 'B':
					return Pieces.Bishop;
				case 'K':
					return Pieces.King;
				case 'Q':
					return Pieces.Queen;
				case 'R':
					return Pieces.Rook;
				default:
					return Pieces.Pawn;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chess.Base.PGN
{
	public class PGNToken
	{
		public PGNTokenType TokenType;
		public string Value;
		public int Index;

		public PGNToken()
		{
			
		}

		public PGNToken(PGNTokenType type, string value, int index)
		{
			TokenType = type;
			Value = value;
			Index = index;
		}

		public override string ToString()
		{
			return TokenType.ToString() + " " + Value;
		}

		public int GetMoveNumber()
		{
			int val = 0;
			bool parsed = Int32.TryParse(Regex.Replace(Value, @"[\.]", ""), out val);
			return (parsed) ? val : -1;
		}

		/// <summary>
		/// Returns information about the move.
		/// Is not guaranteed to return the "From" or "Color" field.
		/// Includes:
		///		Piece
		///		To
		///		Promotion
		///		Check
		///		Mate
		/// </summary>
		/// <returns></returns>
		public PGNMove GetMove(Board board)
		{
			var val = Value.Trim();

			var move = new PGNMove();
			move.Check = val.Contains('+') || val.Contains('#');
			move.Mate = val.Contains('#');
			move.Capture = val.Contains('x');
			move.Color = board.PlayerTurn;
			move.MoveNumber = board.MoveCount;

			val = Regex.Replace(val, @"[x\.\+\#]", "");
			move.Piece = Notation.GetPiece(val);
			move.Promotion = Notation.GetPromotion(val);

			// remove piece type and promotion
			if(move.Piece != Piece.Pawn)
				val = val.Substring(1);
			if(move.Promotion != Piece.None)
			{
				val = val.Replace("=", "");
				val = val.Substring(0, val.Length - 1);
			}

			if(val == "O-O")
			{
				move.From = (move.Color == Color.White) ? 4 : 60;
				move.To = (move.Color == Color.White) ? 6 : 62;
				move.Piece = Piece.King;
			}
			else if (val == "O-O-O")
			{
				move.From = (move.Color == Color.White) ? 4 : 60;
				move.To = (move.Color == Color.White) ? 2 : 58;
				move.Piece = Piece.King;
			}
			else
			{
				move.To = Notation.TextToTile(val.Substring(val.Length - 2));
				int fromX = -1;
				int fromY = -1;

				// check if there is additional information in the move, e.g. "From" tile
				if (val.Length == 4)
				{
					move.From = Notation.TextToTile(val.Substring(0, 2));
				}
				else if (val.Length == 3)
				{
					char ch = val[0];
					if (Char.IsLetter(ch))
						fromX = Notation.FileToInt(ch);
					else
						fromY = Notation.RankToInt(ch);
				}

				move.From = GetMovingPiece(board, move, fromX, fromY);
			}

			return move;
		}

		private int GetMovingPiece(Board board, PGNMove move, int fromX, int fromY)
		{
			// figure out which piece is moving
			int from = -1;
			var pieces = board.FindByPieceAndColor(move.Piece, board.PlayerTurn);
			var possible = new List<int>();
			foreach (var p in pieces)
			{
				var moves = Moves.GetValidMoves(board, p);
				if (moves.Contains(move.To))
					possible.Add(p);
			}

			if (possible.Count == 0)
				throw new Exception("No piece has a valid move to tile " + Notation.TileToText(move.To));

			if (possible.Count > 1)
			{
				// filter possible pieces by using the hints
				if (fromX != -1)
					possible = possible.Where(k => Board.X(k) == fromX).ToList();
				if (fromY != -1)
					possible = possible.Where(k => Board.Y(k) == fromY).ToList();
			}

			if (possible.Count != 1)
				throw new Exception("No piece has a valid move to tile " + Notation.TileToText(move.To));

			from = possible[0];
			return from;
		}
	}
}

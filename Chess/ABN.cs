using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chess
{
	public class ABN
	{
		/// <summary>
		/// Strip out comments and annotations in PGN files, make them suitable to parsing by ABNToMoves
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string[] StripPGN(string input, int maxGames = -1)
		{
			Console.WriteLine("Replacing...");
			input = input.Replace("\r", " ").Replace("\n", " ");
			Console.WriteLine("Replacing...");
			input = input.Replace("  ", " ");
			Console.WriteLine("Replacing...");
			input = input.Replace("  ", " ");
			Console.WriteLine("Done Replacing...");

			List<StringBuilder> gameBuilders = new List<StringBuilder>();

			StringBuilder game = null;
			int commentLevel = 0;

			char commentChar = '\0';
			char uncommentChar = '\0'; // the character at which the ignore ends

			// sets to true when we encounter the first move data for the game
			// used to separate games in files that contain many games
			bool gameStarting = false;
			int gameNumber = 0;

			for(int i = 0; i < input.Length; i++)
			{
				char x = input[i];
				if (x == '[' && !gameStarting && commentLevel == 0)
				{
					gameStarting = true;
					Console.WriteLine("Game #" + gameNumber);
					gameNumber++;
					if (maxGames > 0 && gameNumber > maxGames)
						break;
				}

				if(commentChar == '\0' || commentChar == x)
				{
					if (x == '[')
					{
						commentChar = '[';
						uncommentChar = ']';
						commentLevel++;
						continue;
					}

					if (x == '(')
					{
						commentChar = '(';
						uncommentChar = ')';
						commentLevel++;
						continue;
					}

					if (x == '{')
					{
						commentChar = '{';
						uncommentChar = '}';
						commentLevel++;
						continue;
					}
				}
				else if(uncommentChar == x)
				{
					commentLevel--;
					if (commentLevel == 0)
						commentChar = '\0';

					continue;
				}

				if (commentLevel > 0)
					continue;

				if(gameStarting)
				{
					if (Char.IsWhiteSpace(x))
						continue; // Game can't start with whitespace

					gameStarting = false;
					game = new StringBuilder();
					gameBuilders.Add(game);
				}

				game.Append(x);
			}

			Console.WriteLine("Finished parsing stage 1");
			string[] output = gameBuilders.Select(x => x.ToString()).ToArray();
			Console.WriteLine("Finished convertin stringbuilders to strings");

			System.Threading.Tasks.Parallel.ForEach(output.Select((x, n) => n), i =>
			{
				Console.WriteLine("Regex for Game #" + i);
				output[i] = Regex.Replace(output[i], @"[0-9]+(\.){2,4}", "");
				output[i] = Regex.Replace(output[i], @"(\$)[0-9]{0,3}", "");
				output[i] = Regex.Replace(output[i], @"[\!\?]", "");
				output[i] = Regex.Replace(output[i], @"( )+", " ");
			});

			return output;
		}

		/// <summary>
		/// Read Algebraic notation and return a list of all moves made
		/// </summary>
		/// <param name="initBoard"></param>
		/// <param name="ANString"></param>
		/// <returns></returns>
		public static List<Move> ABNToMoves(Board initBoard, string ANString)
		{
			ANString = ANString.Replace(".", ". ");
			ANString = ANString.Replace("  ", " ");

			var board = initBoard.Copy();
			var moves = new List<Move>();

			var tokens = ANString.Split(' ').Select(x => x.Trim()).Where(x => !String.IsNullOrWhiteSpace(x)).ToList();
			int MoveCount = 0;
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
						if (t < MoveCount)
							throw new Exception("Move count was lower than expected");
						if (t == MoveCount && moves.Count(x => x.MoveCount == t) != 1) // checks if we already have white's move
							throw new Exception("Move count was lower than expected");
						if (t > MoveCount + 1)
							throw new Exception("Missed move number " + (MoveCount + 1));
						MoveCount = t;
						continue;
					}

					// ---------------------- Parse final score ----------------------
					if (i == tokens.Count - 1 && (token.Contains("1-0") || token.Contains("0-1") || token.Contains("1/2")))
						continue;

					// ---------------------- Parse the move ----------------------
					Move move = GetMove(board, token, board.PlayerTurn);
					move.MoveCount = MoveCount;
					moves.Add(move);
					bool legal = board.Move(move.From, move.To, true);
					if (!legal)
						throw new Exception("Illegal move at token " + i + ": " + token + " - Move #" + move.MoveCount);

					// check for promotions
					if (move.Promotion != 0)
					{
						if (Moves.CanPromote(board, move.To))
						{
							bool promoted = board.Promote(move.To, move.Promotion);
							if(!promoted)
								throw new Exception("Unable to promote piece");
						}
						else
							throw new Exception("Unable to promote piece");
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception("Error parsing algebraic notation at token " + i + ", during move #" + MoveCount + " : " + tokens[i], e);
			}

			return moves;
		}

		private static Move GetMove(Board board, string token, int color)
		{
			// Process captures - Done after we find target
			int capture = 0;
			int captureTile = -1;
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
				if (board.CanCastleKWhite && board.State[4] == (Pieces.King | Colors.White) && board.State[5] == 0 && board.State[6] == 0 && board.State[7] == (Pieces.Rook | Colors.White))
					return new Move(4, 6, 0, color, capture, captureTile, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Kingside castling for white should not be allowed at this point");
			}
			if (queenside && color == Colors.White)
			{
				if (board.CanCastleQWhite && board.State[4] == (Pieces.King | Colors.White) && board.State[3] == 0 && board.State[2] == 0 && board.State[1] == 0 && board.State[0] == (Pieces.Rook | Colors.White))
					return new Move(4, 2, 0, color, capture, captureTile, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Queenside castling for white should not be allowed at this point");
			}

			if (kingside && color == Colors.Black)
			{
				if (board.CanCastleKBlack && board.State[60] == (Pieces.King | Colors.Black) && board.State[61] == 0 && board.State[62] == 0 && board.State[63] == (Pieces.Rook | Colors.Black))
					return new Move(60, 62, 0, color, capture, captureTile, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Kingside castling for black should not be allowed at this point");
			}
			if (queenside && color == Colors.Black)
			{
				if (board.CanCastleQBlack && board.State[60] == (Pieces.King | Colors.Black) && board.State[59] == 0 && board.State[58] == 0 && board.State[57] == 0 && board.State[56] == (Pieces.Rook | Colors.Black))
					return new Move(60, 58, 0, color, capture, captureTile, 0, check, mate, queenside, kingside);
				else
					throw new Exception("Queenside castling for black should not be allowed at this point");
			}


			// promotion
			int promotion = 0;

			if (piece == Pieces.Pawn)
			{
				// I don't use the = sign
				token = token.Replace("=", "");

				// check if there are any piece specifiers in the string
				if (token.Contains("Q"))
					promotion = Pieces.Queen;
				if (token.Contains("N"))
					promotion = Pieces.Knight;
				if (token.Contains("B"))
					promotion = Pieces.Bishop;
				if (token.Contains("R"))
					promotion = Pieces.Rook;

				// Remove promotion specifier from string
				token = token.Replace("Q", "");
				token = token.Replace("N", "");
				token = token.Replace("B", "");
				token = token.Replace("R", "");
			}

			// find the target
			int to = 0;
			if (!kingside && !queenside)
			{
				string targetString = token.Substring(token.Length - 2);
				to = Notation.TextToTile(targetString);
				token = token.Substring(0, token.Length - 2);
			}

			capture = board.State[to];
			captureTile = to;


			// find the hints
			Tuple<int, int> hint = GetHint(token);

			// figure out which piece is moving
			int from = -1;
			var pieces = board.FindByPieceAndColor(color | piece);
			var possible = new List<int>();
			foreach (var p in pieces)
			{
				var moves = Moves.GetValidMoves(board, p);
				if (moves.Contains(to))
					possible.Add(p);
			}

			if(possible.Count == 0)
				throw new Exception("No piece has a valid move to tile " + Notation.TileToText(to));
			
			if (possible.Count > 1)
			{
				// filter possible pieces by using the hints
				if (hint.Item1 != -1)
					possible = possible.Where(k => Board.X(k) == hint.Item1).ToList();
				if (hint.Item2 != -1)
					possible = possible.Where(k => Board.Y(k) == hint.Item2).ToList();
			}

			if (possible.Count != 1)
				throw new Exception("No piece has a valid move to tile " + Notation.TileToText(to));

			from = possible[0];

			// Check if this is an en passant attack, then change the attackTile
			if (Moves.IsEnPassantCapture(board, from, to))
			{
				captureTile = Moves.EnPassantVictim(board, from, to);
				capture = board.State[captureTile];
			}

			return new Move(from, to, 0, color, capture, captureTile, promotion, check, mate, queenside, kingside);
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

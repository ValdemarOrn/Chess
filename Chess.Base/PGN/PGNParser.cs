using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Base.PGN
{
	

	public class PGNParser
	{
		public static readonly char[] ControlChars = { '(', ')', '[', ']', '{', '}' };
		public static readonly string DataPadding = "\n\n\n\n";

		public PGNParser()
		{
			
		}

		/// <summary>
		/// Reads the contents of a PGN file, returning a list of games
		/// and a list of exceptions for games that have errors.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="skip"></param>
		/// <param name="gameHandler"></param>
		/// <param name="errorHandler"></param>
		/// <returns></returns>
		public PGNParserResults ParsePGN(
			string data, 
			int skip = 1,
			Action<PGNGame> gameHandler = null, 
			Action<PGNParserError> errorHandler = null)
		{
			var sb = new StringBuilder();
			var output = new PGNParserResults();

			// newlines simplify boundary checks and help the parser detect start and end
			// of game data
			sb.Append(DataPadding);
			sb.Append(data);
			sb.Append(DataPadding);
			sb.Replace("\r", "");
			data = sb.ToString();
			var indexes = FindGameIndexes(data);

			for (int i = 0; i < indexes.Count; i++)
				output.Games.Add(null);

			Parallel.For(0, indexes.Count, (i) =>
			{
				if (i % skip == 0)
				{
					try
					{
						var idx = indexes[i];
						var game = ParseGame(data, idx);
						output.Games[i] = game;
						if (gameHandler != null)
							gameHandler(game);
					}
					catch (PGNException e)
					{
						var err = new PGNParserError();
						err.GameNumber = i + 1;
						err.Col = GetCol(data, e.Index);
						err.Line = GetRow(data, e.Index) - DataPadding.Length;
						err.Exception = e;
						err.Index = e.Index;
						err.Message = e.Message + ((e.InnerException != null) ? e.InnerException.Message : "");
						err.Source = e.SourceComponent;
						err.Value = (e.PgnToken != null) ? (e.PgnToken.TokenType.ToString() + ": " + e.PgnToken.Value) : e.Token;
						output.Errors.Add(err);

						if (errorHandler != null)
							errorHandler(err);
					}
					catch (Exception ex)
					{
						var err = new PGNParserError();
						err.GameNumber = i + 1;
						err.Exception = ex;
						err.Message = ex.Message;

						Console.WriteLine(err.ToString());
						output.Errors.Add(err);

						if (errorHandler != null)
							errorHandler(err);
					}
				}
			});

			return output;
		}

		/// <summary>
		/// returns a list of indexes where PGN Tags start
		/// Used to split a large text file into individual games
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public List<int> FindGameIndexes(string data)
		{
			var output = new List<int>();
			for(int i = 0; i < data.Length - 2; i++)
			{
				if (data[i] == '\n' && data[i + 1] == '\n' && data[i + 2] == '[')
					output.Add(i + 2);
			}
			return output;
		}

		/// <summary>
		/// parses a single PGN game.
		/// Can also parse a move list in algebraic notation.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		public PGNGame ParseGame(string data, int i = 0)
		{
			// make sure there is padding at the end.
			// When batch processing games this has already been done. Only needs to be
			// done when parsing single games
			if (data[data.Length - 1] != '\n' || data[data.Length - 2] != '\n')
				data += DataPadding;

			// Lexer
			var tags = ParseTags(data, ref i);
			var tokens = Tokenize(data, ref i);

			// Parser
			var iToken = 0;
			var position = new Board(true);
			var variation = GetVariation(tokens, ref iToken, position);
			var game = new PGNGame(tags, variation);
			return game;
		}

		private Dictionary<string, string> ParseTags(string data, ref int i)
		{
			var output = new Dictionary<string, string>();
			while(i < data.Length)
			{
				var line = GetLine(data, i);

				if (line.Trim().Length == 0)
					i++;
				else if(!line.StartsWith("["))
					break;
				else
				{
					int len = line.Length + 1;

					if (line[0] != '[' || line[line.Length - 1] != ']')
						throw new PGNException("Invalid Tag line", i, line, ParserSource.Parser);

					line = line.Substring(1, line.Length - 2);
					var idx = line.IndexOf(' ');
					if (idx == -1)
						throw new PGNException("Invalid Tag line", i, line, ParserSource.Parser);

					var key = line.Substring(0, idx);
					var value = line.Substring(idx + 1);
					if(value.Length >= 2 && value[0] == '"')
						value = value.Substring(1, value.Length - 2);

					output[key] = value;

					i += len; // add one for newline character
				}
			}

			return output;
		}

		#region Tokenizer

		private List<PGNToken> Tokenize(string data, ref int i)
		{
			var tokens = new List<PGNToken>();
			var iBefore = i;
			PGNToken token = new PGNToken();

			while(i < data.Length)
			{
				try
				{
					iBefore = i;
					token = GetToken(data, ref i);
				}
				catch(Exception e)
				{
					throw new PGNException("Invalid token", i, GetWord(data, i), ParserSource.Tokenizer, e);
				}

				if(i == iBefore || token.TokenType == PGNTokenType.None)
				{
					throw new PGNException("No token was detected", i, GetWord(data, i), ParserSource.Tokenizer);
				}

				tokens.Add(token);
				if (token.TokenType == PGNTokenType.End)
					break;
			}

			return tokens;
		}

		private PGNToken GetToken(string data, ref int i)
		{
			while (Char.IsWhiteSpace(data[i]))
			{
				if (data[i] == '\n' && data[i + 1] == '\n')
				{
					i += 2;
					return new PGNToken(PGNTokenType.End, null, i - 2);
				}

				i++;
				if (i >= data.Length)
				{
					return new PGNToken(PGNTokenType.End, null, i - 1);
				}
			}

			var word = GetWord(data, i);
			int index = i;

			if (data[i] == '(' || data[i] == '[')
			{
				var text = data[i].ToString();
				i++;
				return new PGNToken(PGNTokenType.VariationStart, text, index);
			}
			else if (data[i] == ')' || data[i] == ']')
			{
				var text = data[i].ToString();
				i++;
				return new PGNToken(PGNTokenType.VariationEnd, text, index);
			}
			else if (data[i] == '$' || data[i] == '!' || data[i] == '?')
			{
				i += word.Length;
				return new PGNToken(PGNTokenType.Annotation, word, index);
			}
			else if (data[i] == '{')
			{
				int start = i;
				while (data[i] != '}')
					i++;

				i++;
				string text = data.Substring(start, i - start);
				text = text.Replace("\n", "").Trim();
				text = text.Substring(1, text.Length - 2);
				return new PGNToken(PGNTokenType.Comment, text, index);
			}
			else if (data[i] == '.' && data[i + 1] == '.')
			{
				int start = i;
				while (data[i] == '.')
					i++;

				var text = data.Substring(start, i - start);
				return new PGNToken(PGNTokenType.Ellipsis, text, index);
			}
			else if (word == "1-0" || word == "1/2-1/2" || word == "0-1" || word == "*" || word == "1/2")
			{
				i += word.Length;
				return new PGNToken(PGNTokenType.Results, word, index);
			}
			else if (word == "O-O" || word == "O-O-O")
			{
				i += word.Length;
				return new PGNToken(PGNTokenType.Move, word, index);
			}
			else if (Char.IsNumber(data[i]))
			{
				int start = i;
				while (Char.IsNumber(data[i]))
					i++;

				string number = data.Substring(start, i - start);

				if (data[i] == '.')
					number += ".";

				// consume the dot unless it's an ellipsis.
				if (data[i + 1] != '.')
					i++;

				return new PGNToken(PGNTokenType.MoveNumber, number, index);
			}
			else if (Char.IsLetter(data[i]))
			{
				int start = i;
				while (!Char.IsWhiteSpace(data[i]) && data[i] != '!' && data[i] != '?' && !ControlChars.Contains(data[i]))
					i++;

				var text = data.Substring(start, i - start);
				return new PGNToken(PGNTokenType.Move, text, index);
			}
			else
			{
				throw new PGNException("Invalid token", i, GetWord(data, i), ParserSource.Tokenizer);
			}
		}

		private static int GetCol(string data, int position)
		{
			int i = 0;
			if (data[i] == '\n')
				return 0;

			while (data[position - i] != '\n')
				i++;

			return i;
		}

		private static int GetRow(string data, int i)
		{
			return data.Substring(0, i).Count(x => x == '\n') + 1;
		}

		private static string GetWord(string data, int i)
		{
			int wordStart = i;
			int w = i;
			while (w < data.Length && !Char.IsWhiteSpace(data[w]) && !ControlChars.Contains(data[w]))
				w++;

			var word = data.Substring(wordStart, w - wordStart);
			return word;
		}

		private static string GetLine(string data, int i)
		{
			int lineStart = i;
			int w = i;
			while (w < data.Length && data[w] != '\n')
				w++;

			var line = data.Substring(lineStart, w - lineStart);
			return line;
		}

		#endregion

		#region Compiler

		private GameVariation GetVariation(List<PGNToken> tokens, ref int i, Board position)
		{
			var positions = new List<Board>() { position };
			var variation = new GameVariation();
			var token = tokens[i];

			try
			{
				for (; i < tokens.Count; i++)
				{
					if (tokens[i].TokenType == PGNTokenType.End || tokens[i].TokenType == PGNTokenType.VariationEnd)
						break;

					var newPos = ParseToken(variation, tokens, ref i, positions, position);
					if(newPos != position)
					{
						positions.Add(newPos);
						position = newPos;
					}
				}
			}
			catch(Exception e)
			{
				throw new PGNException("Error parsing token", token, ParserSource.Parser, e);
			}

			return variation;
		}

		private Board ParseToken(
			GameVariation variation, 
			List<PGNToken> tokens, 
			ref int i, 
			List<Board> positions, 
			Board position)
		{
			var token = tokens[i];

			switch (token.TokenType)
			{
				case (PGNTokenType.Annotation):
					variation.Elements.Add(new GameAnnotation(token.Value));
					break;
				case (PGNTokenType.Comment):
					variation.Elements.Add(new GameComment(token.Value));
					break;
				case (PGNTokenType.Ellipsis):
					// Assert its blacks turn
					if (position.PlayerTurn != Color.Black)
						throw new PGNException("Encountered unexpected token. It should be blacks turn to play", token);
					break;
				case (PGNTokenType.Move):
					position = position.Copy();
					var move = MakeMove(token, position);
					variation.Elements.Add(move);
					break;
				case (PGNTokenType.MoveNumber):
					if (position.MoveCount > token.GetMoveNumber())
						throw new PGNException("Unexpected move number", token);
					break;
				case (PGNTokenType.Results):
					variation.Elements.Add(new GameResults(token.Value));
					break;
				case (PGNTokenType.VariationStart):
					var startpos = GetStartPosition(tokens, ref i, positions);
					var newVariation = GetVariation(tokens, ref i, startpos);
					variation.Elements.Add(newVariation);
					break;
			}

			return position;
		}

		private PGNMove MakeMove(PGNToken token, Board position)
		{
			var move = token.GetMove(position);

			if (move.Capture && position.State[move.To] == 0 && position.EnPassantTile == 0)
				throw new PGNException("Move is not a capture", token);

			if (move.Color != position.PlayerTurn)
				throw new PGNException("It is not this players turn to move", token);

			if (move.Color != position.GetColor(move.From))
				throw new PGNException("Moving piece is not of the expected color", token);

			if (move.Piece != position.GetPiece(move.From))
				throw new PGNException("Moving piece is not of the expected type", token);

			var ok = position.Move(move.From, move.To, true);

			if (!ok)
				throw new PGNException("Illegal move", token);

			if (move.Promotion != Piece.None)
			{
				ok = position.Promote(move.To, move.Promotion);
				if(!ok)
					throw new PGNException("Illegal promotion", token);
			}

			if (move.Check && !position.IsChecked(position.PlayerTurn))
				throw new PGNException("Move did not cause a check", token);

			if (move.Mate && !position.IsCheckMate())
				throw new PGNException("Move did not cause a mate", token);

			return move;
		}

		/// <summary>
		/// Finds the position where the current variation starts, 
		/// and return a COPY of that position.
		/// </summary>
		/// <param name="tokens"></param>
		/// <param name="i"></param>
		/// <param name="positions"></param>
		/// <returns></returns>
		private Board GetStartPosition(List<PGNToken> tokens, ref int i, List<Board> positions)
		{
			if (tokens[i].TokenType == PGNTokenType.VariationStart)
				i++;

			if (tokens[i].TokenType == PGNTokenType.Ellipsis)
			{
				i++;
				return positions[positions.Count - 2];
			}

			int j = i;

			while (j < tokens.Count)
			{
				if (tokens[j].TokenType == PGNTokenType.MoveNumber)
				{
					var moveNumber = tokens[j].GetMoveNumber();
					Color playerTurn = Color.None;
					j++;

					if (tokens[j].TokenType == PGNTokenType.Ellipsis)
					{
						playerTurn = Color.Black;
						j++;
					}
					else if (tokens[j].TokenType == PGNTokenType.Move)
						playerTurn = Color.White;
					else
						throw new PGNException("Encountered unexpected token", tokens[i]);

					int k = positions.Count - 1;
					var nextPos = positions[k];
					while (true)
					{
						if (nextPos.MoveCount == moveNumber && nextPos.PlayerTurn == playerTurn)
							break;

						k--;
						if (k < 0)
							throw new PGNException("Invalid variation start", tokens[i]);

						nextPos = positions[k];
					}

					return nextPos.Copy();
				}
				else
					j++;
			}

			throw new PGNException("Malformed variation", tokens[i]);
		}

		#endregion


	}
}

using Chess.Base.PGN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Chess.Base
{
	public class OpeningBook
	{
		/// <summary>
		/// Compiles an opening book out of one or more PGN input files.
		/// </summary>
		/// <param name="pgnFileNames">list of files to parse</param>
		/// <param name="moveCount">how many moves to store in the book, e.g. First 20 moves</param>
		/// <param name="skip">
		/// only use every n-th game, e.g. 2 = use every other game, 3 = use every third game.
		/// Useful for limiting the output size if the input PGN is huge
		/// </param>
		/// <param name="callback">callback to take action for every game that is parsed</param>
		/// <param name="errorHandler">callback to handle errors</param>
		/// <returns>List of all parsed games, Encoded</returns>
		public static List<string> CompileBook(
			List<string> pgnFileNames, 
			int moveCount = 20,
			int skip = 1,
			Action<string, string> callback = null,
			Action<string, PGNParserError> errorHandler = null)
		{
			int halfMoves = moveCount * 2; // +1 to round up
			var output = new List<string>();

			foreach(var file in pgnFileNames)
			{
				try
				{
					Action<PGNParserError> errHandler = x => { errorHandler(file, x); };
					Action<PGNGame> gameHandler = x =>
					{
						var encoded = GameEncoder.EncodeGame(x, false);
						if (encoded.Length > halfMoves)
						{
							// make sure we don't cut off the promotion, in case tha last move is a promotion
							if (encoded[halfMoves] == '*')
								encoded = encoded.Substring(0, halfMoves + 2);
							else
								encoded = encoded.Substring(0, halfMoves);
						}

						string results = "-" + ((int)x.Results.Results).ToString();
						var line = encoded + results;
						output.Add(line);
						if(callback != null)
							callback(file, line);
					};

					var data = File.ReadAllText(file);
					var parser = new PGNParser();
					var games = parser.ParsePGN(data, skip, gameHandler, errHandler);
				}
				catch(Exception e)
				{
					if(errorHandler != null)
						errorHandler(file, new PGNParserError(){ Exception = e, Message = "Unable to parse input file" });
				}
			}

			output = output.Where(x => !String.IsNullOrWhiteSpace(x)).OrderBy(x => x).ToList();
			return output;
		}

		public List<string> OpeningLines { get; private set; }

		public OpeningBook(List<string> openingLines)
		{
			OpeningLines = openingLines;
		}

		public List<OpeningMove> GetAvailableMoves(string currentLine)
		{
			var len = currentLine.Length;
			var a = OpeningLines.BinarySearch(x => x.Substring(0, len).CompareTo(currentLine));
			var b = OpeningLines.BinarySearchMax(x => x.Substring(0, len).CompareTo(currentLine));
			
			var lines = new List<string>();
			for (int i = a; i <= b; i++)
				lines.Add(OpeningLines[i]);

			var moves = new Dictionary<Move, OpeningMove>();
			foreach(var line in lines)
			{
				var result = GameEncoder.DecodeResult(line);
				var move = GameEncoder.DecodeMove(line, len);
				OpeningMove m = null;

				if(moves.ContainsKey(move))
				{
					m = moves[move];
				}
				else
				{
					m = new OpeningMove() { Move = move };
					moves[move] = m;
				}

				m.Total++;

				switch(result)
				{
					case(GameResultsType.BlackWins):
						m.BlackWins++;
						break;
					case(GameResultsType.WhiteWins):
						m.WhiteWins++;
						break;
					case(GameResultsType.Tie):
						m.Tie++;
						break;
				}
			}

			return moves.Select(x => x.Value).ToList();
		}

		/// <summary>
		/// Filters the possible moves based and selects one with a weighted random function
		/// </summary>
		/// <param name="openingMoves"></param>
		/// <param name="player"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public Move SelectMove(List<OpeningMove> openingMoves, Color player, OpeningBookFilter filter)
		{
			List<OpeningMove> validMoves = new List<OpeningMove>();
			int total = openingMoves.Sum(x => x.Total);
			OpeningMove selected = null;

			if(player == Color.White)
			{
				validMoves = openingMoves
					.Where(x => x.BlackWinPercent <= filter.MaxBlackWinPercent)
					.Where(x => x.WhiteWinPercent >= filter.MinWhiteWinPercent)
					.Where(x => x.Total >= filter.MinNumberOfGames)
					.ToList();

				selected = WeightedRandom(validMoves,
					x => x.WhiteWinPercent * filter.ImportantOfWinPercentage
					+ x.Total / (double)total * filter.ImportanceOfGameCount);
			}
			else
			{
				validMoves = openingMoves
					.Where(x => x.WhiteWinPercent <= filter.MaxWhiteWinPercent)
					.Where(x => x.BlackWinPercent >= filter.MinBlackWinPercent)
					.Where(x => x.Total >= filter.MinNumberOfGames)
					.ToList();

				selected = WeightedRandom(validMoves,
					x => x.BlackWinPercent * filter.ImportantOfWinPercentage
					+ x.Total / (double)total * filter.ImportanceOfGameCount);
			}

			return selected.Move;
		}

		static RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

		/// <summary>
		/// returns a random object from the list, taking into account its weight
		/// (possibility of it being selected).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="weightFunction"></param>
		/// <returns></returns>
		private T WeightedRandom<T>(List<T> list, Func<T, double> weightFunction)
		{
			var totalWeight = list.Sum(x => weightFunction(x));
			var table = new List<Tuple<uint, uint, T>>();
			uint i = 0;
			foreach(var item in list)
			{
				double ratio = weightFunction(item) / totalWeight;
				uint range = (uint)(ratio * UInt32.MaxValue);
				range = (range > 0) ? range : 1;
				table.Add(new Tuple<uint, uint, T>(i, i + range - 1, item));
				i += range;
			}

			var bytes = new byte[4];
			Rand.GetBytes(bytes);
			var val = BitConverter.ToUInt32(bytes, 0);
			var output = table.Single(x => val >= x.Item1 && val <= x.Item2).Item3;
			return output;
		}
	}
}

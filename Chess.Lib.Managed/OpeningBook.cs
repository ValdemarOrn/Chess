using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Base;

namespace Chess.Lib
{
	public class OpeningMove
	{
		public int From;
		public int To;
		public int SampleCount;

		public int WhiteWins;
		public int BlackWins;
		public int Draws;

		public double WhiteRatio
		{
			get { return WhiteWins / (double)SampleCount; }
		}

		public double BlackRatio
		{
			get { return BlackWins / (double)SampleCount; }
		}

		public double DrawRatio
		{
			get { return Draws / (double)SampleCount; }
		}
	}
	
	public class OpeningBook
	{
		public static void StripPGNs(string inputFile, string outputFile, int maxNumberOfGames = -1)
		{
			var text = System.IO.File.ReadAllText(inputFile);
			Console.WriteLine("Read all text");

			var moveStrings = ABN.StripPGN(text, maxNumberOfGames);
			Console.WriteLine("Stripped PGN");

			System.IO.File.WriteAllLines(outputFile, moveStrings);
		}

		public static string GenerateBook(string[] strippedInputs)
		{
			var moveStrings = strippedInputs;

			int i = 0;
			var splitter = new char[] { ' ' };
			var sb = new StringBuilder();

			var board = new Chess.Base.Board(true);

			foreach(var m in moveStrings)
			{
				try
				{
					Console.WriteLine("Writing game #" + i);
					//var sb = new StringBuilder();

					board.InitBoard();
					var moves = ABN.ABNToMoves(board, m);
					var winLose = m.Split(splitter, StringSplitOptions.RemoveEmptyEntries).Last();

					if (winLose.Contains('1') && winLose.Contains('2'))
						sb.Append("D ");
					else if (winLose.Trim() == "1-0")
						sb.Append("W ");
					else if (winLose.Trim() == "0-1")
						sb.Append("B ");
					else
						throw new Exception("No end result found");

					for (int j = 0; j < 20 && j < moves.Count; j++)
					{
						sb.Append(Notation.TileToText(moves[j].From) + Notation.TileToText(moves[j].To));
						sb.Append(" ");
					}

					sb.Append("\n");
				}
				catch (Exception)
				{
					Console.WriteLine("Exception writing game #" + i);
				}
				i++;
			}

			return sb.ToString();
		}

		public const double MaxOpponentAdvantage = 0.03;
		public const int MinimumSamples = 40;
		public const int MinimumSamplesGreatMove = 20;

		string[][] Book;
		int MovesMade;

		public OpeningBook()
		{
			rand = new Random((int)(DateTime.Now.Ticks % Int32.MaxValue));
		}

		public void Load(string openingBookFilename)
		{
			FileStream fs = File.OpenRead(openingBookFilename);
			
			var gzipStream = new System.IO.Compression.GZipStream(fs, System.IO.Compression.CompressionMode.Decompress);
			var reader = new StreamReader(gzipStream);
			var str = reader.ReadToEnd();
			Book = str.Split('\n').Where((u, i) => i % 10 == 0).Select(x => x.Split(' ').Select(y => y.Trim()).ToArray()).ToArray();
			MovesMade = 0;
		}

		/// <summary>
		/// Filters the opening book by playing the specified move.
		/// Removes all other entires from the book (they are redundant after making the move)
		/// </summary>
		/// <param name="moveFrom"></param>
		/// <param name="moveTo"></param>
		public void FilterBook(int moveFrom, int moveTo)
		{
			string text = Notation.TileToText(moveFrom) + Notation.TileToText(moveTo);
			Book = Book.Where(x => x[MovesMade + 1] == text).ToArray();
			MovesMade++;
		}

		public List<OpeningMove> FindMoves()
		{
			var output = new Dictionary<string, OpeningMove>();

			for(int i = 0; i < Book.Length; i++)
			{
				var line = Book[i];
				string nextMove = line[MovesMade + 1];
				int from = Notation.TextToTile(nextMove.Substring(0, 2));
				int to = Notation.TextToTile(nextMove.Substring(2, 2));

				OpeningMove move = null;

				if (output.ContainsKey(nextMove))
				{
					move = output[nextMove];
				}
				else
				{
					move = new OpeningMove();
					move.From = from;
					move.To = to;
					output[nextMove] = move;
				}

				move.SampleCount++;

				if (line[0] == "D")
					move.Draws++;
				else if (line[0] == "B")
					move.BlackWins++;
				else if (line[0] == "W")
					move.WhiteWins++;
			}

			return output.Select(x => x.Value).ToList();
		}

		Random rand;

		public OpeningMove SelectMove(List<OpeningMove> moves, int color)
		{
			var suitable = new List<OpeningMove>();

			if(color == Board.COLOR_WHITE)
			{
				foreach(var move in moves)
				{
					if (move.WhiteRatio + MaxOpponentAdvantage < move.BlackRatio)
						continue;

					bool isGreatMove = (move.WhiteRatio > move.BlackRatio + 0.1);

					if (isGreatMove && move.SampleCount < MinimumSamplesGreatMove)
						continue;
					else if (move.SampleCount < MinimumSamples)
						continue;

					suitable.Add(move);
				}
			}
			else
			{
				foreach(var move in moves)
				{
					if (move.BlackRatio + MaxOpponentAdvantage < move.WhiteRatio)
						continue;

					bool isGreatMove = (move.BlackRatio > move.WhiteRatio + 0.1);

					if (isGreatMove && move.SampleCount < MinimumSamplesGreatMove)
						continue;
					else if (move.SampleCount < MinimumSamples)
						continue;

					suitable.Add(move);
				}
			}

			// Weight the randomness with samplecount

			var output = WeightedRandom(suitable);

			FilterBook(output.From, output.To);
			return output;
		}

		private OpeningMove WeightedRandom(List<OpeningMove> suitable)
		{
			var weights = new int[suitable.Count];
			var totalWeight = suitable.Sum(x => x.SampleCount);

			for(int i = 0; i < suitable.Count; i++)
			{
				if (i > 0)
					weights[i] = weights[i - 1];

				weights[i] += suitable[i].SampleCount;
			}

			var r = rand.Next(0, totalWeight);
			for (int i = 0; i < suitable.Count; i++)
			{
				if (r < weights[i])
					return suitable[i];
			}

			return suitable[0];
		}
	}
}

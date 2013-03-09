using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.TestPerf
{
	public class Position
	{
		public string FEN;
		public int Depth;
		public ulong ExpectedCount;
		public ulong ActualCount;

		public Position(string fen, int depth, ulong expected)
		{
			FEN = fen;
			Depth = depth;
			ExpectedCount = expected;
		}
	}

	public unsafe class PerftTestSuite
	{
		public static List<Position> Positions;
		public static bool EnableDebugOutput;

		static PerftTestSuite()
		{
			Positions = new List<Position>();
			Positions.Add(new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 5, 4865609));
			Positions.Add(new Position("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -", 4, 4085603));
			Positions.Add(new Position("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -", 6, 11030083));
			Positions.Add(new Position("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1", 5, 15833292));
			Positions.Add(new Position("rnbqkb1r/pp1p1ppp/2p5/4P3/2B5/8/PPP1NnPP/RNBQK2R w KQkq - 0 6", 4, 1761505));
		}

		public static void RunTests()
		{
			foreach (var position in Positions)
			{
				Console.WriteLine("Searching " + position.Depth + " plies");
				Console.WriteLine("Expecting " + position.ExpectedCount);
				Console.WriteLine("Position: " + position.FEN);
				position.ActualCount = TestPosition(position);
			}

			Console.WriteLine("\n-------------------------------\n");

			foreach (var position in Positions)
			{
				if (position.ActualCount != position.ExpectedCount)
				{
					Console.WriteLine("ERROR: Position reported wrong move count. Expected " + position.ExpectedCount + ", found " + position.ActualCount);
					Console.WriteLine("FEN: " + position.FEN);
				}
			}

			int failed = Positions.Where(x => x.ActualCount != x.ExpectedCount).Count();
			int success = Positions.Where(x => x.ActualCount == x.ExpectedCount).Count();

			Console.WriteLine("Tests Passed: " + success);
			Console.WriteLine("Tests Failed: " + failed);
		}

		/// <summary>
		/// Counts all moves from the given position to the required ply count, and returns the actual number of move paths found
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static ulong TestPosition(Position pos)
		{
			string fenString = pos.FEN;
			int depth = pos.Depth;
			ulong expectedCount = pos.ExpectedCount;

			var x = Notation.FENtoBoard(fenString);
			var b = Helpers.ManagedBoardToNative(x);

			var start = DateTime.Now;
			var results = Perft.Search(b, depth);
			var seconds = (DateTime.Now - start).TotalSeconds;
			Console.WriteLine("Perft(" + depth + "): " + (results->Total) + ", Time: " + String.Format("{0:0.000}", seconds));
			Console.WriteLine("");

			if (EnableDebugOutput)
			{
				for (int i = 0; i < results->EntryCount; i++)
				{
					int from = results->Entries[i].From;
					int to = results->Entries[i].To;
					ulong count = results->Entries[i].Count;

					string sfrom = Notation.TileToText(from);
					string sto = Notation.TileToText(to);
					Console.WriteLine(sfrom + " " + sto + " : " + count);
				}
			}

			Board.Delete(b);

			return results->Total;
		}
	}
}

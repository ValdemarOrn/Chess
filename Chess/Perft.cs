using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public class PerftEntry
	{
		public int From;
		public int To;
		public long Count;

		public override string ToString()
		{
			return Notation.TileToText(From) + "-" + Notation.TileToText(To) + "  " + Count;
		}
	}

	public class PerftResults
	{
		public int StartDepth;
		public List<PerftEntry> Entries;
		public long Total;

		public PerftResults()
		{
			Entries = new List<PerftEntry>();
		}
	}

	/// <summary>
	/// A basic Perft search. Used to validate correctness, to search for illegal/wrong move
	/// </summary>
	public class Perft
	{
		public static PerftResults RunPerft(Board boardBase, int depth)
		{
			var p = new Perft(boardBase);
			p.PerftCount(depth);
			return p.Results;
		}

		protected Board Board;
		protected PerftResults Results;
		protected int Depth;

		protected Perft(Board board)
		{
			Board = board;
		}

		long PerftCount(int depth)
		{
			Depth = depth;
			Results = new PerftResults();
			Results.StartDepth = Depth;
			return PerftCount(Board.Copy(), Depth);
		}

		private long PerftCount(Board boardBase, int depth)
		{
			long total = 0;

			// find all the moves
			var moves = Moves.GetMoves(boardBase);
 
			for (int i = 0; i < moves.Length; i++) 
			{
				Move move = moves[i];
				var board = boardBase.Copy();
				bool valid = board.Move(move.From, move.To, true);
				
				if(!valid)
					continue;

				if(move.Promotion > 0)
					board.Promote(move.To, move.Promotion);

				long cnt = (depth == 1) ? 1 : PerftCount(board, depth - 1);
				total += cnt;

				// log data in the top node
				if (Results.StartDepth == depth)
					Results.Entries.Add(new PerftEntry() { Count = cnt, From = move.From, To = move.To });
			}

			if (Results.StartDepth == depth)
				Results.Total = total;

			return total;
		}
	}
}

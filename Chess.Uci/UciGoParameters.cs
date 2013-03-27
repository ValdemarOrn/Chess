using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public class UciGoParameters
	{
		/// <summary>
		/// If not null then the engine should limit it's search to these moves only
		/// </summary>
		public List<UciMove> SearchMoves;

		/// <summary>
		/// start searching in ponder mode
		/// </summary>
		public bool Ponder;

		/// <summary>
		/// white time left, in milliseconds
		/// </summary>
		public long? WhiteTime;

		/// <summary>
		/// Black time left, in milliseconds
		/// </summary>
		public long? BlackTime;

		/// <summary>
		/// White increment time per move, in milliseconds
		/// </summary>
		public long? WhiteInc;

		/// <summary>
		/// Black increment time per move, in milliseconds
		/// </summary>
		public long? BlackInc;

		/// <summary>
		/// Moves left until the next time control window
		/// </summary>
		public int? MovesToGo;

		/// <summary>
		/// Search to a specific depth
		/// </summary>
		public int? Depth;

		/// <summary>
		/// Search maximum number of nodes
		/// </summary>
		public int? Nodes;

		/// <summary>
		/// Search for mate in x moves
		/// </summary>
		public int? Mate;

		/// <summary>
		/// Search for a given time, in milliseconds
		/// </summary>
		public long? MoveTime;

		/// <summary>
		/// Search for an infinite time
		/// </summary>
		public bool Infinite;
	}
}

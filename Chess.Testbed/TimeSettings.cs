using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Testbed
{
	public enum TimeMode
	{
		FixedDepth,
		TimePerMove,
		Infinite,
		NodeCount,
		TimeControl
	}

	/// <summary>
	/// Class that describes time control settings
	/// </summary>
	public class TimeSettings
	{
		public string Name { get; set; }

		public TimeMode TimeModeWhite { get; set; }
		public TimeMode TimeModeBlack { get; set; }

		/// <summary>
		/// If TimeMode = FixedDepth, sets the depth to search for
		/// </summary>
		public int? Depth { get; set; }

		/// <summary>
		/// If TimeMode = NodeCount, sets the number of nodes to search for
		/// </summary>
		public int? NodeCount { get; set; }

		/// <summary>
		/// If TimeMode = TimePerMove, sets the time to search for
		/// </summary>
		public int? TimePerMoveMillis { get; set; }


		/// <summary>
		/// Sets the amount of time each player has at the start
		/// </summary>
		public int? InitialTimeMillis { get; set; }

		/// <summary>
		/// How much to increment player's time after he moves
		/// </summary>
		public int? MoveIncrementMillis { get; set; }

		/// <summary>
		/// Number of milliseconds per time control window.
		/// E.g. 40 moves / 40 minutes => 40*60*1000 = 2400000
		/// </summary>
		public int? TimeControlWindowMillis { get; set; }

		/// <summary>
		/// How many moves per time control window
		/// </summary>
		public int? MovesPerWindow { get; set; }
	}
}

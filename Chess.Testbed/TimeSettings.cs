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
		TimeControl,
		Blitz
	}

	/// <summary>
	/// Class that describes time control settings
	/// </summary>
	public class TimeSettings
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public TimeMode TimeModeHuman { get; set; }
		public TimeMode TimeModeMachine { get; set; }

		/// <summary>
		/// If TimeMode = FixedDepth, sets the depth to search for
		/// </summary>
		public int? Depth { get; set; }

		/// <summary>
		/// If TimeMode = NodeCount, sets the number of nodes to search for
		/// </summary>
		public int? NodeCount { get; set; }

		/// <summary>
		/// If TimeMode = TimePerMove, sets the time to search for, in seconds
		/// </summary>
		public int? TimePerMove { get; set; }


		/// <summary>
		/// Sets the number of seconds each player has at the start
		/// </summary>
		public int? InitialTime { get; set; }

		/// <summary>
		/// How many seconds to increment player's time after he moves
		/// </summary>
		public int? MoveIncrement { get; set; }

		/// <summary>
		/// Number of seconds per time control window.
		/// E.g. 40 moves / 40 minutes => 40*60 = 2400
		/// </summary>
		public int? TimeControlWindow { get; set; }

		/// <summary>
		/// How many moves per time control window
		/// </summary>
		public int? MovesPerWindow { get; set; }
	}
}

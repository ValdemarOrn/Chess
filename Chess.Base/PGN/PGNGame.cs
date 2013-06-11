using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class PGNGame
	{
		public Dictionary<string, string> Tags { get; private set; }
		public GameVariation Game { get; private set; }
		public GameResults Results { get; private set; }

		public PGNGame(Dictionary<string, string> tags, GameVariation game)
		{
			Tags = tags;
			Game = game;
			Results = game.Elements.LastOrDefault(x => x is GameResults) as GameResults;
		}

		public List<PGNMove> GetMainVariation()
		{
			var output = new List<PGNMove>();
			foreach (var item in Game.Elements)
			{
				if (item is PGNMove)
					output.Add(item as PGNMove);
			}
			return output;
		}
	}
}

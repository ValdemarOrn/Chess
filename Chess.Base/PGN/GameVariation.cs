using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class GameVariation : IPGNElement
	{
		public PGNTokenType Type { get { return PGNTokenType.VariationStart; } }

		public List<IPGNElement> Elements { get; private set; }

		public GameVariation()
		{
			Elements = new List<IPGNElement>();
		}

		public GameVariation(List<IPGNElement> elements)
		{
			Elements = elements;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");

			IPGNElement last = null;
			for(int i = 0; i < Elements.Count; i++)
			{
				var ele = Elements[i];

				if(ele.Type == PGNTokenType.Move)
				{
					var move = ele as PGNMove;

					// move is white or first element in variation
					bool printMove = (move.Color == Color.White || last == null);

					// move is black but the last element was not also a number
					printMove |=  (move.Color == Color.Black) && (last != null && last.Type != PGNTokenType.Move);

					bool printellipsis = printMove && (move.Color == Color.Black);

					if (printMove)
					{
						sb.Append(move.MoveNumber.ToString());

						if (printellipsis)
							sb.Append("... ");
						else
							sb.Append(". ");
					}
				}

				sb.Append(ele.ToString());

				if(i != Elements.Count - 1)
					sb.Append(" ");

				last = ele;
			}

			sb.Append(")");
			return sb.ToString();
		}
	}
}

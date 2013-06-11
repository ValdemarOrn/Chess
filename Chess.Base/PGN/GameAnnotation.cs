using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class GameAnnotation : IPGNElement
	{
		public PGNTokenType Type { get { return PGNTokenType.Annotation; } }

		public string Annotation { get; private set; }

		public GameAnnotation(string annotation)
		{
			Annotation = annotation;	
		}

		public override string ToString()
		{
			return Annotation;
		}
	}
}

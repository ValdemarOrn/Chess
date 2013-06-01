using Chess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.TestPerf
{
	class BasePerftTests
	{
		public static void RunBasePerft()
		{
			Console.WriteLine("\n\n================= Chess.Base Perft Tests =================\n\n");

			var b = Notation.ReadFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
			var results = Chess.Base.Perft.RunPerft(b, 4);
			if (197281 != results.Total)
				Console.WriteLine("Chess.Base Perft Test #1: UNEXPECTED RESULT");
			else
				Console.WriteLine("Chess.Base Perft Test #1 OK");

			b = Notation.ReadFEN("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq -");
			results = Chess.Base.Perft.RunPerft(b, 3);
			if (97862 != results.Total)
				Console.WriteLine("Chess.Base Perft Test #2: UNEXPECTED RESULT");
			else
				Console.WriteLine("Chess.Base Perft Test #2 OK");

			b = Notation.ReadFEN("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - -");
			results = Chess.Base.Perft.RunPerft(b, 5);
			if (674624 != results.Total)
				Console.WriteLine("Chess.Base Perft Test #3: UNEXPECTED RESULT");
			else
				Console.WriteLine("Chess.Base Perft Test #3 OK");

			b = Notation.ReadFEN("r3k2r/Pppp1ppp/1b3nbN/nP6/BBP1P3/q4N2/Pp1P2PP/R2Q1RK1 w kq - 0 1");
			results = Chess.Base.Perft.RunPerft(b, 4);
			if (422333 != results.Total)
				Console.WriteLine("Chess.Base Perft Test #4: UNEXPECTED RESULT");
			else
				Console.WriteLine("Chess.Base Perft Test #4 OK");

			b = Notation.ReadFEN("rnbqkb1r/pp1p1ppp/2p5/4P3/2B5/8/PPP1NnPP/RNBQK2R w KQkq - 0 6");
			results = Chess.Base.Perft.RunPerft(b, 3);
			if (53392 != results.Total)
				Console.WriteLine("Chess.Base Perft Test #5: UNEXPECTED RESULT");
			else
				Console.WriteLine("Chess.Base Perft Test #5 OK");

		}
	}
}

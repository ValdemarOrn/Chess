using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.AI.PerformanceTests
{
	class Program
	{
		static void Main(string[] args)
		{
			EvaluateSpeedTest();
			EvaluateSpeedTest();
			EvaluateSpeedTest();
			Console.ReadLine();
		}

		static void EvaluateSpeedTest()
		{
			var b1 = Notation.FENtoBoard("rnbqkbnr/pp2pppp/3p4/8/3pP3/5N2/PPP2PPP/RNBQKB1R w KQkq");
			var b2 = Notation.FENtoBoard("rnbqk1nr/pppp1ppp/8/2b1p3/2B1P3/2N5/PPPP1PPP/R1BQK1NR b KQkq");
			var b3 = Notation.FENtoBoard("r1bqk2r/pppp1ppp/2n2n2/2b1p3/2B1P3/2NP4/PPP2PPP/R1BQK1NR w KQkq");
			var b4 = Notation.FENtoBoard("r1b1k2r/ppp2pp1/2np1q1p/2b1p3/2B1P3/2NP1N2/PPP2PPP/R2QK2R w KQkq");

			var boards = new Board[] { b1, b2, b3, b4 };

			DateTime start = DateTime.Now;
			int i = 0;
			while (true)
			{
				var res = Chess.AI.PositionEvaluator.EvaluatePosition(boards[i % 4]);
				i++;
				if ((DateTime.Now - start).TotalMilliseconds > 1000)
					break;
			}

			Console.WriteLine("Evaluations per second: " + i);

		}
	}
}

using System;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public unsafe class EvalTest
	{
		[Test]
		public void TestEval1()
		{
			var b = Board.Create();
			Board.Init(b, 1);

			var result = Eval.Evaluate(b);
			Assert.AreEqual(20, result);
		}

		[Test]
		public void TestEvalStats()
		{
			var b = Board.Create();
			Board.Init(b, 1);

			var result = Eval.Evaluate(b);
			var statsWhite = Eval.GetEvalStats(Board.COLOR_WHITE);
			var statsBlack = Eval.GetEvalStats(Board.COLOR_BLACK);

			Assert.AreEqual(104000, statsWhite->Material);
			Assert.AreEqual(104000, statsBlack->Material);

		}
	}
}

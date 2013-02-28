using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public unsafe class EvalTest
	{
		[TestMethod]
		public void TestEval1()
		{
			var b = Board.Create();
			Board.Init(b, 1);

			var result = Eval.Evaluate(b);
			Assert.AreEqual(20, result);

		}
	}
}

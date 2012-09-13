using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.AI.Tests
{
	[TestClass]
	public class TestPositionEvaluator
	{
		[TestMethod]
		public void TestReadTable()
		{
			int val = PositionEvaluator.ReadTable(PositionEvaluator.PawnTable, 9, Colors.White);
			Assert.AreEqual(100, val);

			val = PositionEvaluator.ReadTable(PositionEvaluator.PawnTable, 49, Colors.Black);
			Assert.AreEqual(100, val);

			val = PositionEvaluator.ReadTable(PositionEvaluator.PawnTable, 49, Colors.White);
			Assert.AreEqual(500, val);

			val = PositionEvaluator.ReadTable(PositionEvaluator.PawnTable, 9, Colors.Black);
			Assert.AreEqual(500, val);
		}
	}
}

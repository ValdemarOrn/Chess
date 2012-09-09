using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestABN
	{
		[TestMethod]
		public void TestABN1()
		{
			var board = new Board(true);
			string str = "1. e4 e5";
			var moves = ABN.ABNToMoves(board, str);
			Assert.AreEqual(1, moves[0].Turn);
			Assert.AreEqual(1, moves[1].Turn);
			Assert.AreEqual(12, moves[0].From);
			Assert.AreEqual(12+16, moves[0].To);
			Assert.AreEqual(52, moves[1].From);
			Assert.AreEqual(52-16, moves[1].To);
		}
	}
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public unsafe class SearchTest
	{
		[TestMethod]
		public void TestSearchSimple()
		{
			var bx = Chess.Notation.FENtoBoard("3k4/8/8/8/2q5/3P4/7K/7Q w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(19, bestMove.From);
			Assert.AreEqual(26, bestMove.To);
		}

		[TestMethod]
		public void TestSearchDeeper()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 4);
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);
		}

		//[TestMethod]
		public void TestSearchNodeCount()
		{
			var bx = new Chess.Board(true);
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 6);
		}
	}
}

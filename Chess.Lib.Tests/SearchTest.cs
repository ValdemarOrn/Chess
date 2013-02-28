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
			Search.SetDepth(1);
			var bx = Chess.Notation.FENtoBoard("3k4/8/8/8/2q5/3P4/7K/7Q w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b);
			Assert.AreEqual(19, bestMove.From);
			Assert.AreEqual(26, bestMove.To);
		}

		[TestMethod]
		public void TestSearchDeeper()
		{
			Search.SetDepth(4);
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b);
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);
		}

		//[TestMethod]
		public void TestSearchNodeCount()
		{
			Search.SetDepth(6);
			var bx = Chess.Notation.FENtoBoard("rnbqkbnr/pppppppp/8/8/8/2P5/PP1PPPPP/RNBQKBNR b KQkq - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b);
		}
	}
}

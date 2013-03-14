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

			var stats = Search.GetSearchStats();
		}

		[TestMethod]
		public void TestSearchCapture()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/5r2/8/8/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(21, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[TestMethod]
		public void TestSearchCapture2()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r2N2/8/8/3P1Q2/7K/8 b - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(42, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[TestMethod]
		public void TestSearchCapture3()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r2N2/8/8/3P1Q2/7K/8 b - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 2);
			Assert.AreEqual(42, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[TestMethod]
		public void TestSearchCapture4()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 3);
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[TestMethod]
		public void TestSearchCapture5()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 4); // should be the same result as TestSearchCapture4
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[TestMethod]
		public void TestSearchDeeper()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 4);
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		//[TestMethod]
		public void TestSearch6()
		{
			var bx = Chess.Notation.FENtoBoard("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");//new Chess.Board(true);
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 8);
			var stats = Search.GetSearchStats();
		}

		public void TestSearch7()
		{
			var bx = Chess.Notation.FENtoBoard("r3n1k1/p2n1pp1/1p5p/2p1P3/3p4/P4N1P/BPP2PP1/5RK1 w KQkq - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			//var b = Board.Create();
			//Board.Init(b, 1);

			DateTime start = DateTime.Now;
			var bestMove = Search.SearchPos(b, 8);
			var time = (DateTime.Now - start).TotalSeconds;
			Console.WriteLine(String.Format("Time: {0:0.00} seconds", time));
			var stats = Search.GetSearchStats();

			double first = stats->CutMoveIndex[0] / (double)stats->CutNodeCount;
			double first3 = (stats->CutMoveIndex[0] + stats->CutMoveIndex[1] + stats->CutMoveIndex[2]) / (double)stats->CutNodeCount;

			double qNodeCount = stats->QuiescentNodeCount / (double)stats->TotalNodeCount;

			Console.WriteLine("1. Move Cuts: " + first);
			Console.WriteLine("3. Move Cuts: " + first3);
			Console.WriteLine("Quiesce nodes: " + stats->QuiescentNodeCount);
			Console.WriteLine("Quiesce Ratio: " + qNodeCount);
			Console.WriteLine("Total Eval: " + stats->EvalTotal);
		}
	}
}

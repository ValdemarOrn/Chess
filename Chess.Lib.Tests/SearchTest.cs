﻿using System;
using NUnit.Framework;
using Chess.Base;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public unsafe class SearchTest
	{
		[Test]
		public void TestSearchSimple()
		{
			var bx = Notation.ReadFEN("3k4/8/8/8/2q5/3P4/7K/7Q w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(19, bestMove.From);
			Assert.AreEqual(26, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestSearchCapture()
		{
			var bx = Notation.ReadFEN("4k3/8/5r2/8/8/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(21, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestSearchCapture2()
		{
			var bx = Notation.ReadFEN("4k3/8/2r2N2/8/8/3P1Q2/7K/8 b - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(42, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestSearchCapture3()
		{
			var bx = Notation.ReadFEN("4k3/8/2r2N2/8/8/3P1Q2/7K/8 b - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 2);
			Assert.AreEqual(42, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestSearchCapture4()
		{
			var bx = Notation.ReadFEN("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 3);
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestSearchCapture5()
		{
			var bx = Notation.ReadFEN("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 4); // should be the same result as TestSearchCapture4
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestSearchDeeper()
		{
			var bx = Notation.ReadFEN("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 4);
			Assert.AreEqual(28, bestMove.From);
			Assert.AreEqual(45, bestMove.To);

			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestPromotion()
		{
			var bx = Notation.ReadFEN("k7/pp5P/8/8/8/8/8/7K w - - 0 1");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 1);
			Assert.AreEqual(55, bestMove.From);
			Assert.AreEqual(63, bestMove.To);
			Assert.AreEqual(6, bestMove.Promotion);
			var stats = Search.GetSearchStats();
		}

		[Test]
		public void TestForStalemate()
		{
			// This position caused a stalemate as the engine did not choose the mate in one
			// instead it moved the king and blundered a win
			// Edit: It was actually a bug in the UCI interface. Still, a good test

			var bx = Notation.ReadFEN("7K/8/1k5b/6n1/6P1/8/p7/8 b - - 3 61 ");
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 8);
			Board.Make(b, bestMove.From, bestMove.To);
			Board.Promote(b, bestMove.To, Board.PIECE_QUEEN);
			Board.Make(b, 63, 62);

			bestMove = Search.SearchPos(b, 8);

			// assert that we move the queen, not the king!
			Assert.AreEqual(0, bestMove.From);

			Board.Make(b, bestMove.From, bestMove.To);
			// assert check
			Assert.AreEqual(1, Board.IsChecked(b, Board.COLOR_WHITE));
		}

		//[Test]
		public void TestSearch6()
		{
			var bx = Notation.ReadFEN("4k3/8/2r5/8/4N3/3P1Q2/7K/8 w - - 0 1");//new Chess.Board(true);
			var b = Helpers.ManagedBoardToNative(bx);

			var bestMove = Search.SearchPos(b, 8);
			var stats = Search.GetSearchStats();
		}

		public void TestSearches()
		{
			GenericSearchTest("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 10);
			GenericSearchTest("r3n1k1/p2n1pp1/1p5p/2p1P3/3p4/P4N1P/BPP2PP1/5RK1 w KQkq - 0 1", 10);
			GenericSearchTest("r1b1k2r/2q1bppp/p2ppn2/1p4P1/3NPP2/2N2Q2/PPP4P/2KR1B1R b kq", 9);
		}

		public static void GenericSearchTest(string fen, int depth)
		{
			Console.WriteLine("-------------------------------------------");
			Console.WriteLine("Position: " + fen);
			Console.WriteLine("");

			var bx = Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bx);

			DateTime start = DateTime.Now;
			var bestMove = Search.SearchPos(b, depth);
			var time = (DateTime.Now - start).TotalSeconds;
			Console.WriteLine(String.Format("Time: {0:0.00} seconds", time));
			var stats = Search.GetSearchStats();

			var table = TTable.GetTable();
			var tableSize = TTable.GetTableSize();
			int used = 0;
			for(int i = 0; i < tableSize; i++)
			{
				if (table[i].Hash != 0)
					used++;
			}

			var fill = used / (double)tableSize;

			double first = stats->CutMoveIndex[0] / (double)stats->CutNodeCount * 100;
			double first3 = (stats->CutMoveIndex[0] + stats->CutMoveIndex[1] + stats->CutMoveIndex[2]) / (double)stats->CutNodeCount * 100;

			double qNodeCount = stats->QuiescentNodeCount / (double)stats->TotalNodeCount * 100;

			Console.WriteLine("1. Move Cuts: " + first);
			Console.WriteLine("3. Move Cuts: " + first3);
			Console.WriteLine("Quiesce nodes: " + stats->QuiescentNodeCount);
			Console.WriteLine("Quiesce Ratio: " + qNodeCount);
			Console.WriteLine("Total Eval: " + stats->EvalTotal);

			Board.Delete(b);
		}
	}
}

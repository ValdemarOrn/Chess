using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Chess.Uci.Tests
{
	[TestClass]
	public class UciToEngineTests
	{
		MockUciEngine engine;
		UciController ctrl;

		[TestInitialize]
		public void Init()
		{
			engine = new MockUciEngine();
			ctrl = new UciController();
			ctrl.Engine = engine;
		}

		[TestMethod]
		public void TestCallUci()
		{
			var ok = ctrl.ReadCommand("uci");
			Assert.IsTrue(ok);
			Assert.AreEqual("Uci", engine.Function);
		}

		[TestMethod]
		public void TestCallDebug()
		{
			var ok = ctrl.ReadCommand("debug on");
			Assert.IsTrue(ok);
			Assert.AreEqual("SetDebug", engine.Function);
			Assert.AreEqual(true, engine.Arguments[0]);

			ok = ctrl.ReadCommand("debug off");
			Assert.IsTrue(ok);
			Assert.AreEqual("SetDebug", engine.Function);
			Assert.AreEqual(false, engine.Arguments[0]);
		}

		[TestMethod]
		public void TestCallIsReady()
		{
			var ok = ctrl.ReadCommand("isready");
			Assert.IsTrue(ok);
			Assert.AreEqual("IsReady", engine.Function);
			Assert.AreEqual(0, engine.Arguments.Count);
		}

		[TestMethod]
		public void TestSetOption()
		{
			var ok = ctrl.ReadCommand(@"setoption name NalimovPath value c:\chess\tb\4;c:\chess\tb\5\n");
			Assert.IsTrue(ok);
			Assert.AreEqual("SetOption", engine.Function);
			Assert.AreEqual("NalimovPath", engine.Arguments[0]);
			Assert.AreEqual(@"c:\chess\tb\4;c:\chess\tb\5\n", engine.Arguments[1]);
		}

		[TestMethod]
		public void TestRegister()
		{
			var ok = ctrl.ReadCommand("register name Valdemar Erlingsson code 12345");
			Assert.IsTrue(ok);
			Assert.AreEqual("Register", engine.Function);
			Assert.AreEqual(false, engine.Arguments[0]);
			Assert.AreEqual("Valdemar Erlingsson", engine.Arguments[1]);
			Assert.AreEqual("12345", engine.Arguments[2]);

			ok = ctrl.ReadCommand("register later");
			Assert.IsTrue(ok);
			Assert.AreEqual("Register", engine.Function);
			Assert.AreEqual(true, engine.Arguments[0]);
			Assert.AreEqual(null, engine.Arguments[1]);
			Assert.AreEqual(null, engine.Arguments[2]);
		}

		[TestMethod]
		public void TestUciNewGame()
		{
			var ok = ctrl.ReadCommand("ucinewgame");
			Assert.IsTrue(ok);
			Assert.AreEqual("UciNewGame", engine.Function);
			Assert.AreEqual(0, engine.Arguments.Count);
		}

		[TestMethod]
		public void TestPosition()
		{
			var ok = ctrl.ReadCommand("position fen rnbqkb1r/ppp1pppp/5n2/1B1p4/4P3/5N2/PPPP1PPP/RNBQK2R b KQkq - 3 3 moves a2a4 c8d7");
			Assert.IsTrue(ok);
			Assert.AreEqual("Position", engine.Function);
			Assert.AreEqual("rnbqkb1r/ppp1pppp/5n2/1B1p4/4P3/5N2/PPPP1PPP/RNBQK2R b KQkq - 3 3", engine.Arguments[0]);
			var moves = (List<UciMove>)engine.Arguments[1];
			Assert.AreEqual("a2a4", moves[0].ToString());
			Assert.AreEqual("c8d7", moves[1].ToString());

			ok = ctrl.ReadCommand("position startpos moves a3a7q");
			Assert.IsTrue(ok);
			Assert.AreEqual("Position", engine.Function);
			Assert.AreEqual(null, engine.Arguments[0]);
			moves = (List<UciMove>)engine.Arguments[1];
			Assert.AreEqual("a3a7q", moves[0].ToString());
		}

		[TestMethod]
		public void TestGo1()
		{
			var ok = ctrl.ReadCommand("go infinite searchmoves e2e4 d2d4");
			Assert.IsTrue(ok);
			Assert.AreEqual("Go", engine.Function);
			var goParams = (UciGoParameters)engine.Arguments[0];
			Assert.AreEqual(true, goParams.Infinite);
			Assert.AreEqual("e2e4", goParams.SearchMoves[0].ToString());
			Assert.AreEqual("d2d4", goParams.SearchMoves[1].ToString());
			Assert.AreEqual(null, goParams.BlackInc);
			Assert.AreEqual(null, goParams.WhiteInc);
		}

		[TestMethod]
		public void TestGo2()
		{
			var ok = ctrl.ReadCommand("go wtime 300000 btime 300000 winc 0 binc 0");
			Assert.IsTrue(ok);
			Assert.AreEqual("Go", engine.Function);
			var goParams = (UciGoParameters)engine.Arguments[0];
			Assert.AreEqual(false, goParams.Infinite);
			Assert.AreEqual(300000, goParams.WhiteTime);
			Assert.AreEqual(300000, goParams.BlackTime);
			Assert.AreEqual(0, goParams.BlackInc);
			Assert.AreEqual(0, goParams.WhiteInc);
			Assert.AreEqual(null, goParams.Depth);
		}

		[TestMethod]
		public void TestGo3()
		{
			var ok = ctrl.ReadCommand("go movestogo 8 ponder");
			Assert.IsTrue(ok);
			Assert.AreEqual("Go", engine.Function);
			var goParams = (UciGoParameters)engine.Arguments[0];
			Assert.AreEqual(true, goParams.Ponder);
			Assert.AreEqual(8, goParams.MovesToGo);
		}

		[TestMethod]
		public void TestGo4()
		{
			var ok = ctrl.ReadCommand("go wtime 126000 btime 120000 winc 6000 binc 6000");
			Assert.IsTrue(ok);
			Assert.AreEqual("Go", engine.Function);
			var goParams = (UciGoParameters)engine.Arguments[0];
			Assert.AreEqual(126000, goParams.WhiteTime);
			Assert.AreEqual(120000, goParams.BlackTime);
			Assert.AreEqual(6000, goParams.BlackInc);
			Assert.AreEqual(6000, goParams.WhiteInc);
		}

		[TestMethod]
		public void TestStop()
		{
			var ok = ctrl.ReadCommand("stop");
			Assert.IsTrue(ok);
			Assert.AreEqual("Stop", engine.Function);
			Assert.AreEqual(0, engine.Arguments.Count);
		}

		[TestMethod]
		public void TestPonderHit()
		{
			var ok = ctrl.ReadCommand("ponderhit");
			Assert.IsTrue(ok);
			Assert.AreEqual("PonderHit", engine.Function);
			Assert.AreEqual(0, engine.Arguments.Count);
		}

		[TestMethod]
		public void TestQuit()
		{
			var ok = ctrl.ReadCommand("quit");
			Assert.IsTrue(ok);
			Assert.AreEqual("Quit", engine.Function);
			Assert.AreEqual(0, engine.Arguments.Count);
		}
	}
}

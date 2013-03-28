using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Chess.Uci.Tests
{
	[TestClass]
	public class UciToGuiTests
	{
		UciController ctrl;

		[TestInitialize]
		public void Init()
		{
			ctrl = new UciController();
			ctrl.UciCallback = Callback;
			Commands = new List<string>();
		}

		List<string> Commands;

		public void Callback(string command)
		{
			Commands.Add(command);
		}

		[TestMethod]
		public void TestID()
		{
			ctrl.ID("Vladivostok", "Valdemar Erlingsson");
			Assert.AreEqual(2, Commands.Count);
			Assert.AreEqual("id name Vladivostok", Commands[0]);
			Assert.AreEqual("id author Valdemar Erlingsson", Commands[1]);
		}

		[TestMethod]
		public void TestUciOk()
		{
			ctrl.UciOk();
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("uciok", Commands[0]);
		}

		[TestMethod]
		public void TestReadyOk()
		{
			ctrl.ReadyOk();
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("readyok", Commands[0]);
		}

		[TestMethod]
		public void TestBestMove1()
		{
			ctrl.BestMove(UciMove.FromString("a2a4"), null);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("bestmove a2a4", Commands[0]);
		}

		[TestMethod]
		public void TestBestMove2()
		{
			ctrl.BestMove(UciMove.FromString("b7b8q"), null);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("bestmove b7b8q", Commands[0]);
		}

		[TestMethod]
		public void TestBestMove3()
		{
			ctrl.BestMove(UciMove.FromString("b7b8q"), UciMove.FromString("a2a4"));
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("bestmove b7b8q ponder a2a4", Commands[0]);
		}

		[TestMethod]
		public void TestCopyProtection1()
		{
			ctrl.CopyProtection(true);
			Assert.AreEqual(2, Commands.Count);
			Assert.AreEqual("copyprotection checking", Commands[0]);
			Assert.AreEqual("copyprotection ok", Commands[1]);
		}

		[TestMethod]
		public void TestCopyProtection2()
		{
			ctrl.CopyProtection(false);
			Assert.AreEqual(2, Commands.Count);
			Assert.AreEqual("copyprotection checking", Commands[0]);
			Assert.AreEqual("copyprotection error", Commands[1]);
		}

		[TestMethod]
		public void TestRegistration1()
		{
			ctrl.Registration(true);
			Assert.AreEqual(2, Commands.Count);
			Assert.AreEqual("registration checking", Commands[0]);
			Assert.AreEqual("registration ok", Commands[1]);
		}

		[TestMethod]
		public void TestRegistration2()
		{
			ctrl.Registration(false);
			Assert.AreEqual(2, Commands.Count);
			Assert.AreEqual("registration checking", Commands[0]);
			Assert.AreEqual("registration error", Commands[1]);
		}

		[TestMethod]
		public void TestInfo1()
		{
			Dictionary<UciInfo, string> vals = new Dictionary<UciInfo, string>();
			vals[UciInfo.Depth] = "12";
			vals[UciInfo.Nodes] = "555";
			vals[UciInfo.NPS] = "500000";
			vals[UciInfo.PV] = "e2e4 e7e5 g1f3";

			ctrl.Info(vals);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("info depth 12 nodes 555 nps 500000 pv e2e4 e7e5 g1f3", Commands[0]);
		}

		[TestMethod]
		public void TestInfo2()
		{
			Dictionary<UciInfo, string> vals = new Dictionary<UciInfo, string>();
			vals[UciInfo.Score] = "cp 150";
			vals[UciInfo.CurrMove] = "a2a4";
			vals[UciInfo.MultiPV] = "2";

			ctrl.Info(vals);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("info score cp 150 currmove a2a4 multipv 2", Commands[0]);
		}

		[TestMethod]
		public void TestOption1()
		{
			ctrl.Option("Hash", UciOptionType.Spin, 32, 1, 1024, null);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("option name Hash type spin default 32 min 1 max 1024", Commands[0]);
		}

		[TestMethod]
		public void TestOption2()
		{
			ctrl.Option("NalimovPath", UciOptionType.String, null, null, null, null);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("option name NalimovPath type string", Commands[0]);
		}

		[TestMethod]
		public void TestOption3()
		{
			ctrl.Option("Ponder", UciOptionType.Check, true, null, null, null);
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("option name Ponder type check default true", Commands[0]);
		}

		[TestMethod]
		public void TestOption4()
		{
			ctrl.Option("Test", UciOptionType.Spin, "X", null, null, new List<object>() { "A", "B", "X", "Y" });
			Assert.AreEqual(1, Commands.Count);
			Assert.AreEqual("option name Test type spin default X var A var B var X var Y", Commands[0]);
		}
	}
}

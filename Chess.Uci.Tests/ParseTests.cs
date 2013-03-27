using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Uci.Tests
{
	[TestClass]
	public class ParseTests
	{
		[TestMethod]
		public void TestParse1()
		{
			var e = CommandParser.GetElements("name Ponder type check default true", "type", "name", "default");
			Assert.AreEqual("Ponder", e["name"]);
			Assert.AreEqual("check", e["type"]);
			Assert.AreEqual("true", e["default"]);
		}

		[TestMethod]
		public void TestParse2()
		{
			var e = CommandParser.GetElements("name NalimovPath value C:\\TB asd asd", "value", "name", "fake");
			Assert.AreEqual("C:\\TB asd asd", e["value"]);
			Assert.IsTrue(!e.ContainsKey("fake"));
		}

		[TestMethod]
		public void TestParse3()
		{
			var e = CommandParser.GetElements("name NalimovPath value C:\\TB asd asd fake true", "value", "name", "fake");
			Assert.AreEqual("true", e["fake"]);
		}

		[TestMethod]
		public void TestParse4()
		{
			var e = CommandParser.GetElements("name NalimovPath xxxy value C:\\TB asd asd fake true", "value", "name", "fake", "xxxy");
			Assert.IsTrue(e.ContainsKey("xxxy"));
		}
	}
}

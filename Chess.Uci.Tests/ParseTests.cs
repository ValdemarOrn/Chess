using System;
using NUnit.Framework;

namespace Chess.Uci.Tests
{
	[TestFixture]
	public class ParseTests
	{
		[Test]
		public void TestParse1()
		{
			var e = CommandParser.GetElements("name Ponder type check default true", "type", "name", "default");
			Assert.AreEqual("Ponder", e["name"]);
			Assert.AreEqual("check", e["type"]);
			Assert.AreEqual("true", e["default"]);
		}

		[Test]
		public void TestParse2()
		{
			var e = CommandParser.GetElements("name NalimovPath value C:\\TB asd asd", "value", "name", "fake");
			Assert.AreEqual("C:\\TB asd asd", e["value"]);
			Assert.IsTrue(!e.ContainsKey("fake"));
		}

		[Test]
		public void TestParse3()
		{
			var e = CommandParser.GetElements("name NalimovPath value C:\\TB asd asd fake true", "value", "name", "fake");
			Assert.AreEqual("true", e["fake"]);
		}

		[Test]
		public void TestParse4()
		{
			var e = CommandParser.GetElements("name NalimovPath xxxy value C:\\TB asd asd fake true", "value", "name", "fake", "xxxy");
			Assert.IsTrue(e.ContainsKey("xxxy"));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Testbed.Tests
{
	[TestClass]
	public class EngineProcessTests
	{
		[TestMethod]
		public void TestEngineStartup()
		{
			var e = new UciEngineSettings
			{
				Command = @"C:\chess\arena_3.0\Engines\Rybka\Rybka v2.2n2.mp.w32.exe"
			};
			e.LoadEngineData();
			Assert.AreEqual("Rybka 2.2n2 mp 32-bit", e.EngineId);
			Assert.AreEqual("Vasik Rajlich", e.AuthorId);
			Assert.AreEqual(27, e.Options.Length);
			Assert.AreEqual("32", e.Options.Single(x => x.Name == "Hash").DefaultValue);
		}
	}
}

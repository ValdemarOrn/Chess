using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestColors
	{
		[TestMethod]
		public void TesttoString()
		{
			Assert.AreEqual("Black", Colors.ToString(Colors.Black));
			Assert.AreEqual("White", Colors.ToString(Colors.White));

			Assert.AreEqual("", Colors.ToString(45645654));
		}

		[TestMethod]
		public void TestOpposite()
		{
			Assert.AreEqual(Colors.Black, Colors.Opposite(Colors.White));
			Assert.AreEqual(Colors.White, Colors.Opposite(Colors.Black));
			Assert.AreEqual(0, Colors.Opposite(4564));
		}
	}
}

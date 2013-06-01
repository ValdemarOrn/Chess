using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestColors
	{
		[TestMethod]
		public void TesttoString()
		{
			Assert.AreEqual("Black", Colors.ToString(Color.Black));
			Assert.AreEqual("White", Colors.ToString(Color.White));

			Assert.AreEqual("", Colors.ToString((Color)45645654));
		}

		[TestMethod]
		public void TestOpposite()
		{
			Assert.AreEqual(Color.Black, Colors.Opposite(Color.White));
			Assert.AreEqual(Color.White, Colors.Opposite(Color.Black));
			Assert.AreEqual(Color.None, Colors.Opposite((Color)4564));
		}
	}
}

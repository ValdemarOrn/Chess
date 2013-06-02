using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestColors
	{
		[Test]
		public void TesttoString()
		{
			Assert.AreEqual("Black", Colors.ToString(Color.Black));
			Assert.AreEqual("White", Colors.ToString(Color.White));

			Assert.AreEqual("", Colors.ToString((Color)45645654));
		}

		[Test]
		public void TestOpposite()
		{
			Assert.AreEqual(Color.Black, Colors.Opposite(Color.White));
			Assert.AreEqual(Color.White, Colors.Opposite(Color.Black));
			Assert.AreEqual(Color.None, Colors.Opposite((Color)4564));
		}
	}
}

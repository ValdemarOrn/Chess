using System;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public unsafe class TTableTest
	{
		[Test]
		public void TestTable1()
		{
			TTable.Delete();
			TTable.Init(256);
			Assert.AreEqual(15790320, TTable.GetTableSize());
		}
	}
}

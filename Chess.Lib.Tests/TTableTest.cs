using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public unsafe class TTableTest
	{
		[TestMethod]
		public void TesTTable1()
		{
			TTable.Init(64);
			Assert.AreEqual(3947580, TTable.GetTableSize());
		}
	}
}

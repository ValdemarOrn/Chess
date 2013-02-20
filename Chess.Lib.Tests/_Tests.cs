using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class _Tests
	{
		[AssemblyInitialize()]
		public static void Init(TestContext ctx)
		{
			Manager.InitLibrary();
		}
	}
}

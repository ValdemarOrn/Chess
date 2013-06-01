using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;

namespace Chess.Lib.Tests
{
	[TestClass]
	public unsafe class _Tests
	{
		[AssemblyInitialize()]
		public static void Init(TestContext ctx)
		{
			Manager.InitLibrary();
		}

		[AssemblyCleanup()]
		public static void Cleanup()
		{
			TTable.Delete();
		}

	}
}

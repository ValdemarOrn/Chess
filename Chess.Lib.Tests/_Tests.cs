using System;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Chess.Lib.Tests
{
	[SetUpFixture]
	public unsafe class _Tests
	{
		[SetUp]
		public static void Init()
		{
			Manager.InitLibrary();
		}

		[TearDown]
		public static void Cleanup()
		{
			TTable.Delete();
		}

	}
}

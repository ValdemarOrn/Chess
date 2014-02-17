using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chess.Testbed.Tests
{
	[SetUpFixture]
	public class _TestSetup
	{
		[SetUp]
		public void Setup()
		{
			MasterState.ExeDir = @"C:\Src\_Tree\Applications\Chess\Chess.Testbed\bin\Debug\";
			Log.InitLogging(false, true);
		}

	}
}

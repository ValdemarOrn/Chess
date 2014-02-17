using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chess.Testbed.Tests
{
	[TestFixture]
	class MatchRunnerTests
	{
		[Test]
		public void TestMatchRunner1()
		{
			var config = new UciEngineSettings
			{
				Name = "Rybka",
				Command = @"C:\chess\arena_3.0\Engines\Rybka\Rybka v2.2n2.mp.w32.exe"
			};

			var white = new UciEnginePlayer(config);
			var black = new UciEnginePlayer(config);
			var timeSettings = new TimeSettings {Name = "60 Sec Match", InitialTime = 60, TimeModeMachine = TimeMode.Blitz};
			var runner = new MatchRunner(white, black, timeSettings);
			runner.LoadAndSetup();
		}
	}
}

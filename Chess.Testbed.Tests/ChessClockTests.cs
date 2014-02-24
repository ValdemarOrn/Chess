using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chess.Testbed.Tests
{
	[TestFixture]
	public class ChessClockTests
	{
		[Test]
		public void TestClock1()
		{
			var settings = new TimeSettings
			{
				InitialTime = 60,
				MoveIncrement = 1
			};

			var clock = new ChessClock(settings);
			clock.StartClock(Base.Color.White);
			Thread.Sleep(1500);
			var elapsed = clock.Elapsed;
			clock.StopClock();
			Assert.AreEqual((double)1500, (double)elapsed, (double)10);
			Assert.AreEqual(1500, (double)clock.TimeElapsedWhite, (double)10);
			Assert.AreEqual(60000 - 1500 + 1000, (double)clock.TimeRemainingWhite, (double)10);
			Assert.AreEqual((double)0, (double)clock.Elapsed, (double)0);

			clock.StartClock(Base.Color.Black);
			Thread.Sleep(3000);
			elapsed = clock.Elapsed;
			clock.StopClock();
			Assert.AreEqual((double)3000, (double)elapsed, (double)10);
			Assert.AreEqual(3000, (double)clock.TimeElapsedBlack, (double)10);
			Assert.AreEqual(60000 - 3000 + 1000, (double)clock.TimeRemainingBlack, (double)10);
			Assert.AreEqual((double)0, (double)clock.Elapsed, (double)0);

			// assert white unaffected
			Assert.AreEqual(1500, (double)clock.TimeElapsedWhite, (double)10);
			Assert.AreEqual(60000 - 1500 + 1000, (double)clock.TimeRemainingWhite, (double)10);
		}

		[Test]
		public void TestClockSwitch()
		{
			var settings = new TimeSettings
			{
				InitialTime = 60,
				MoveIncrement = 1
			};

			var clock = new ChessClock(settings);
			clock.StartClock(Base.Color.White);
			Thread.Sleep(1500);
			clock.StartClock(Base.Color.Black);
			Thread.Sleep(3000);
			clock.StartClock(Base.Color.White);
			Thread.Sleep(2000);
			clock.StopClock();

			Assert.AreEqual((double)0, (double)clock.Elapsed, (double)0);
			Assert.AreEqual(60000 - 1500 + 1000 - 2000 + 1000, (double)clock.TimeRemainingWhite, (double)10);
			Assert.AreEqual(60000 - 3000 + 1000, (double)clock.TimeRemainingBlack, (double)10);
			
			Assert.AreEqual((double)3500, (double)clock.TimeElapsedWhite, (double)0);
			Assert.AreEqual((double)3000, (double)clock.TimeElapsedBlack, (double)0);

		}
	}
}

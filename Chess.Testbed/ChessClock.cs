using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Testbed
{
	public class ChessClock : IDisposable
	{
		private object lockObj = new object();
		private DateTime timerStarted;
		private System.Timers.Timer elapsedTimer;

		private long timeRemainingWhite;
		private long timeRemainingBlack;
		private long timeElapsedWhite;
		private long timeElapsedBlack;

		public event Action<ChessClock, Chess.Base.Color> OutOfTime;
		public event Action<ChessClock> TimeRemainingChanged;
		public event Action<ChessClock> StateChanged;

		public TimeSettings Settings { get; private set; }
		public Base.Color State { get; private set; }
		public bool InfiniteTime { get; private set; }

		public long Elapsed
		{
			get
			{
				if (State == Base.Color.None)
					return 0;
				
				return (long)(DateTime.Now - timerStarted).TotalMilliseconds;
			}
		}
		
		public long? TimeRemainingWhite
		{
			get
			{
				lock (lockObj)
				{
					if (InfiniteTime)
						return null;

					var remaining = (State == Base.Color.White) ? timeRemainingWhite - Elapsed : timeRemainingWhite;
					return remaining > 0 ? remaining : 0;
				}
			}
		}

		public long? TimeRemainingBlack
		{
			get
			{
				lock (lockObj)
				{
					if (InfiniteTime)
						return null;

					var remaining = (State == Base.Color.Black) ? timeRemainingBlack - Elapsed : timeRemainingBlack;
					return remaining > 0 ? remaining : 0;
				}
			}
		}

		public long TimeElapsedWhite
		{
			get
			{
				lock (lockObj)
				{
					return (State == Base.Color.White) ? timeElapsedWhite + Elapsed : timeElapsedWhite;
				}
			}
		}

		public long TimeElapsedBlack
		{
			get
			{
				lock (lockObj)
				{
					return (State == Base.Color.Black) ? timeElapsedBlack + Elapsed : timeElapsedBlack;
				}
			}
		}

		public ChessClock(TimeSettings settings)
		{
			State = Base.Color.None;
			Settings = settings;
			InfiniteTime = settings.InitialTime == null;
			timeRemainingWhite = (settings.InitialTime * 1000) ?? 0;
			timeRemainingBlack = (settings.InitialTime * 1000) ?? 0;
			elapsedTimer = new System.Timers.Timer(10);
			elapsedTimer.Elapsed += ElapsedTimerAction;
			elapsedTimer.Start();
		}

		private long lastElapseUpdate;
		private void ElapsedTimerAction(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock (lockObj)
			{
				if (TimeRemainingWhite == 0 && OutOfTime != null)
					OutOfTime.Invoke(this, Base.Color.White);

				if (TimeRemainingBlack == 0 && OutOfTime != null)
					OutOfTime.Invoke(this, Base.Color.Black);

				var elapsed = Elapsed;
				if (Math.Abs(lastElapseUpdate - elapsed) >= 100 || (elapsed == 0 && lastElapseUpdate != 0))
				{
					lastElapseUpdate = elapsed;
					if (TimeRemainingChanged != null)
						TimeRemainingChanged.Invoke(this);
				}
			}
		}

		public void StartClock(Base.Color player)
		{
			lock (lockObj)
			{
				if (player == State)
					return;

				StopClock(false);
				timerStarted = DateTime.Now;
				State = player;

				if (StateChanged != null)
					StateChanged.Invoke(this);
			}
		}

		public void StopClock(bool triggerStateChange = true)
		{
			lock (lockObj)
			{
				if (State == Base.Color.None)
					return;

				var elapsed = Elapsed;
				var player = State;
				State = Base.Color.None;

				if (player == Base.Color.White)
				{
					timeElapsedWhite += elapsed;
					if (!InfiniteTime)
						timeRemainingWhite = timeRemainingWhite - elapsed + (Settings.MoveIncrement * 1000 ?? 0);
				}
				else if (player == Base.Color.Black)
				{
					timeElapsedBlack += elapsed;
					if (!InfiniteTime)
						timeRemainingBlack = timeRemainingBlack - elapsed + (Settings.MoveIncrement * 1000 ?? 0);
				}
				else
				{
					return;
				}

				if (triggerStateChange && StateChanged != null)
					StateChanged.Invoke(this);
			}
		}

		private volatile bool isDisposed;
		public void Dispose()
		{
			if (!isDisposed)
			{
				elapsedTimer.Stop();
				elapsedTimer.Dispose();
				isDisposed = true;
			}
		}
	}
}

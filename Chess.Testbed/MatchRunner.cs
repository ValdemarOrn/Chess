using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Chess.Base;

namespace Chess.Testbed
{
	public class MatchRunner
	{
		public static string OutputDir = Path.Combine(MasterState.ExeDir, "Matches");

		private volatile bool isDisposed;
		private FileStream fsWhite;
		private FileStream fsBlack;
		private StreamWriter writerWhite;
		private StreamWriter writerBlack;

		public IPlayer PlayerWhite { get; private set; }
		public IPlayer PlayerBlack { get; private set; }
		public TimeSettings TimeSettings { get; private set; }
		public Base.Board Board { get; private set; }
		public ChessClock Clock { get; private set; }

		public MatchRunner(IPlayer white, IPlayer black, TimeSettings timeSettings)
		{
			TimeSettings = timeSettings;
			PlayerWhite = white;
			PlayerBlack = black;

			var matchName = DateTime.Now.ToString("yyyyMMdd-HHmmss") + "-" + PlayerWhite.Name + "-" + PlayerBlack.Name;
			var dir = Path.Combine(OutputDir, matchName);
			Directory.CreateDirectory(dir);
			var fileWhite = Path.Combine(dir, "white.log");
			var fileBlack = Path.Combine(dir, "black.log");
			fsWhite = new FileStream(fileWhite, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
			fsBlack = new FileStream(fileBlack, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
			writerWhite = new StreamWriter(fsWhite);
			writerBlack = new StreamWriter(fsBlack);
			Board = new Base.Board(true);
			Clock = new ChessClock(TimeSettings);
		}

		public void LoadAndSetup()
		{
			PlayerWhite.RegisterCommandListener((d, msg) => Log.InfoFormat("White {0}: {1}", d == UciProcess.CommandDirection.EngineInput ? "Input" : "Output", msg));
			PlayerBlack.RegisterCommandListener((d, msg) => Log.InfoFormat("Black {0}: {1}", d == UciProcess.CommandDirection.EngineInput ? "Input" : "Output", msg));
			PlayerWhite.Start();
			PlayerBlack.Start();
		}

		public void Go()
		{
			var goParams = new Uci.UciGoParameters
			{
				BlackInc = this.TimeSettings.MoveIncrement * 1000,
				BlackTime = Clock.InfiniteTime ? (long?)null : Clock.TimeRemainingBlack,
				Depth = TimeSettings.Depth,
				Infinite = TimeSettings.TimeModeMachine == TimeMode.Infinite,
				MoveTime = TimeSettings.TimeModeMachine == TimeMode.TimePerMove ? TimeSettings.TimePerMove : null,
				Nodes = TimeSettings.TimeModeMachine == TimeMode.NodeCount ? TimeSettings.NodeCount : null,
				WhiteInc = TimeSettings.MoveIncrement * 1000,
				WhiteTime = Clock.InfiniteTime ? (long?)null : Clock.TimeRemainingWhite,
			};

			if (Board.PlayerTurn == Color.White)
			{
				PlayerWhite.Play(goParams);
				Clock.StartClock(Color.White);
			}
			else
			{
				PlayerBlack.Play(goParams);
				Clock.StartClock(Color.Black);
			}
		}

		public void Stop()
		{
			if (Board.PlayerTurn == Color.White)
			{
				PlayerWhite.Stop();
				Clock.StopClock();
			}
			else
			{
				PlayerBlack.Stop();
				Clock.StopClock();
			}
		}

		public void Dispose()
		{
			if (!isDisposed)
			{
				PlayerWhite.Dispose();
				PlayerBlack.Dispose();
				Clock.Dispose();
				isDisposed = true;
			}
		}
	}
}

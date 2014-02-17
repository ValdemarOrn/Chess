using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Chess.Testbed
{
	public class MatchRunner
	{
		public static string OutputDir = Path.Combine(MasterState.ExeDir, "Matches");

		private bool isDisposed;
		private FileStream fsWhite;
		private FileStream fsBlack;
		private StreamWriter writerWhite;
		private StreamWriter writerBlack;

		/*private UciEngineSettings whiteSettings;
		private UciEngineSettings blackSettings;*/
		
		//private ScheduledMatch match;

		public IPlayer PlayerWhite { get; private set; }
		public IPlayer PlayerBlack { get; private set; }
		public TimeSettings TimeSettings { get; private set; }

		public MatchRunner(IPlayer white, IPlayer black, TimeSettings timeSettings)
		{
			/*ScheduledMatch match
			this.match = match;
			whiteSettings = MasterState.Instance.Engines.Single(x => x.Id == match.WhiteId);
			blackSettings = MasterState.Instance.Engines.Single(x => x.Id == match.BlackId);
			timeSettings = MasterState.Instance.TimeSettings.Single(x => x.Id == match.TimeControlId);*/

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
		}

		public void LoadAndSetup()
		{
			PlayerWhite.RegisterCommandListener((d, msg) => Log.InfoFormat("White {0}: {1}", d == UciProcess.CommandDirection.EngineInput ? "Input" : "Output", msg));
			PlayerBlack.RegisterCommandListener((d, msg) => Log.InfoFormat("Black {0}: {1}", d == UciProcess.CommandDirection.EngineInput ? "Input" : "Output", msg));
			PlayerWhite.Start();
			PlayerBlack.Start();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Base;
using Chess.Testbed.Control;

namespace Chess.Testbed.Views
{
	public class GameViewModel : ViewModelBase
	{
		public GameViewModel()
		{
			QueueNextGameCommand = new ModelCommand(QueueNextGame);
			whitePlayerLog = new List<string>();
			blackPlayerLog = new List<string>();
		}

		public ModelCommand QueueNextGameCommand { get; private set; }

		private MatchRunner matchRunner;
		public MatchRunner MatchRunner
		{
			get { return matchRunner; }
			set { matchRunner = value; NotifyChanged(); }
		}

		private List<string> whitePlayerLog;
		public IEnumerable<string> WhitePlayerLog
		{
			get
			{
				lock (whitePlayerLog)
				{
					var skips = whitePlayerLog.Count > 100 ? whitePlayerLog.Count - 100 : 0;
					return whitePlayerLog.Skip(skips);
				}
			}
		}

		private List<string> blackPlayerLog;
		public IEnumerable<string> BlackPlayerLog
		{
			get
			{
				lock (blackPlayerLog)
				{
					var skips = blackPlayerLog.Count > 100 ? blackPlayerLog.Count - 100 : 0;
					return blackPlayerLog.Skip(skips);
				}
			}
		}

		private void QueueNextGame()
		{
			var nextGame = MasterState.Instance.DequeueMatch();
			if (nextGame == null)
			{
				Log.InfoDialog("No games are scheduled");
				return;
			}

			whitePlayerLog = new List<string>();
			blackPlayerLog = new List<string>();

			var whiteSettings = MasterState.Instance.Engines.SingleOrDefault(x => x.Id == nextGame.WhiteId);
			var blackSettings = MasterState.Instance.Engines.SingleOrDefault(x => x.Id == nextGame.BlackId);
			var timeSettings = MasterState.Instance.TimeSettings.SingleOrDefault(x => x.Id == nextGame.TimeControlId);
			var white = new UciEnginePlayer(whiteSettings);
			var black = new UciEnginePlayer(blackSettings);

			MatchRunner = new MatchRunner(white, black, timeSettings);

			MatchRunner.PlayerWhite.RegisterCommandListener((d, msg) =>
			{
				var str = string.Format("White {0}: {1}", d == UciProcess.CommandDirection.EngineInput ? "Input" : "Output", msg);
				lock (whitePlayerLog)
					whitePlayerLog.Add(str);
				NotifyChanged(() => WhitePlayerLog);
			});

			MatchRunner.PlayerBlack.RegisterCommandListener((d, msg) =>
			{
				var str = string.Format("Black {0}: {1}", d == UciProcess.CommandDirection.EngineInput ? "Input" : "Output", msg);
				lock (blackPlayerLog)
					blackPlayerLog.Add(str);
				NotifyChanged(() => BlackPlayerLog);
			});

			NotifyChanged(() => MatchRunner);
			Task.Run(() => MatchRunner.LoadAndSetup());
		}
	}
}

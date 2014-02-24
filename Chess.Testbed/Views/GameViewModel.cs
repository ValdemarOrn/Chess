using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Base;
using Chess.Testbed.Control;
using Chess.Uci;

namespace Chess.Testbed.Views
{
	public class GameViewModel : ViewModelBase
	{
		public GameViewModel()
		{
			QueueNextGameCommand = new ModelCommand(QueueNextGame);
			EngineGoCommand = new ModelCommand(EngineGo);
			EngineStopCommand = new ModelCommand(EngineStop);
			whitePlayerLog = new List<string>();
			blackPlayerLog = new List<string>();
			whitePlayerInfo = new Dictionary<string, string>();
			blackPlayerInfo = new Dictionary<string, string>();
		}
		
		public ModelCommand QueueNextGameCommand { get; private set; }
		public ModelCommand EngineGoCommand { get; private set; }
		public ModelCommand EngineStopCommand { get; private set; }

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

		private Dictionary<string, string> whitePlayerInfo;
		public Dictionary<string, string> WhitePlayerInfo
		{
			get { return whitePlayerInfo; }
			set { whitePlayerInfo = value; NotifyChanged(); }
		}

		private Dictionary<string, string> blackPlayerInfo;
		public Dictionary<string, string> BlackPlayerInfo
		{
			get { return blackPlayerInfo; }
			set { blackPlayerInfo = value; NotifyChanged(); }
		}

		private void QueueNextGame()
		{
			if (MatchRunner != null)
			{
				MatchRunner.Dispose();
			}

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

			MatchRunner.PlayerWhite.RegisterInfoListener(dict =>
			{
				UpdatePlayerInfo(dict, whitePlayerInfo);
				NotifyChanged(() => WhitePlayerInfo);
			});

			MatchRunner.PlayerBlack.RegisterInfoListener(dict =>
			{
				UpdatePlayerInfo(dict, blackPlayerInfo);
				NotifyChanged(() => BlackPlayerInfo);
			});

			NotifyChanged(() => MatchRunner);
			Task.Run(() => MatchRunner.LoadAndSetup());
		}

		private void UpdatePlayerInfo(Dictionary<UciInfo, string> dict, Dictionary<string, string> outputDict)
		{
			foreach (var kvp in dict)
			{
				long count;
				bool ok;
				var val = kvp.Value;
				switch (kvp.Key)
				{
					case Uci.UciInfo.Depth:
						val = "Depth " + val;
						break;
					case Uci.UciInfo.Nodes:
						ok = long.TryParse(val, out count);
						val = ok ? (count / 1000).ToString() + " kNodes" : val;
						break;
					case Uci.UciInfo.NPS:
						ok = long.TryParse(val, out count);
						val = ok ? (count / 1000).ToString() + " kNodes/sec" : val;
						break;
				}

				outputDict[kvp.Key.ToString()] = val;
			}
		}

		private void EngineGo()
		{
			MatchRunner.Go();
		}

		private void EngineStop()
		{
			MatchRunner.Stop();
		}
	}
}

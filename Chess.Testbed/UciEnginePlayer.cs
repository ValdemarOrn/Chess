using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chess.Testbed
{
	public class UciEnginePlayer : IPlayer
	{
		public UciEngineSettings Settings { get; private set; }
		private UciProcess process;

		public UciEnginePlayer(UciEngineSettings settings)
		{
			Settings = settings;
			process = new UciProcess(settings);
		}

		public PlayerType Type { get { return PlayerType.UCI; } }

		public string Name { get { return Settings.Name; } }

		public void Start()
		{
			process.Start();
			process.UciNewGame();
		}

		public void SetPosition(string fenString, IEnumerable<UciMove> moves)
		{
			process.Position(fenString, moves);
		}

		public void Play(UciGoParameters parameters)
		{
			process.Go(parameters);
		}

		public void Stop()
		{
			process.Stop();
		}


		public void RegisterCommandListener(Action<UciProcess.CommandDirection, string> listener)
		{
			process.CommandSendEvent += listener;
		}

		public void RegisterInfoListener(Action<Dictionary<UciInfo, string>> listener)
		{
			process.InfoCallback += listener;
		}

		public void RegisterBestMoveListener(Action<UciMove, UciMove> listener)
		{
			process.BestMoveCallback += listener;
		}

		public void Dispose()
		{
			process.Dispose();
		}


		
	}
}

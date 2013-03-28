using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Vladivostok
{
	public class Engine : IUciEngine
	{
		// ------------------------ UCI Interface --------------------------

		public IUciGui UciGui { get; set; }
		public Action QuitCallback { get; set; }

		public void Uci()
		{
			UciGui.ID("Vladivostok", "Valdemar Erlingsson");
			UciGui.Option("Hash", UciOptionType.Spin, 32, 1, 512, null);
			UciGui.Option("UCI_Opponent", UciOptionType.String, null, null, null, null);
			UciGui.Option("My Custom Field", UciOptionType.Combo, "Great", null, null, new List<object>() { "1", "xyz", "Great" });

			bool debug = false;
			while(debug)
			{
				System.Threading.Thread.Sleep(100);
			}
			
			UciGui.UciOk();
		}

		public void SetDebug(bool debug)
		{
			// not used
		}

		public void IsReady()
		{
			UciGui.ReadyOk();
		}

		public void SetOption(string name, string value)
		{
			var str = "Setting option " + name + " - " + value;
			SendInfo(str);
		}

		public void Register(bool later, string name, string code)
		{
			// not used
		}

		public void UciNewGame()
		{
			SendInfo("Starting new game");
		}

		public void Position(string fenString, List<UciMove> moves)
		{
			SendInfo("Reading position");
		}

		public void Go(UciGoParameters parameters)
		{
			//UciGui.BestMove(UciMove.FromString("e2e4"), UciMove.FromString("a7a5"));
		}

		public void Stop()
		{
			UciGui.BestMove(UciMove.FromString("e2e4"), null);
		}

		public void PonderHit()
		{
			SendInfo("Ponder Hit!");
		}

		public void Quit()
		{
			SendInfo("Quitting");
			QuitCallback();
		}

		// ------------------------- Other --------------------------

		private void SendInfo(string data)
		{
			var vars = new Dictionary<UciInfo, string>();
			vars[UciInfo.String] = data;
			UciGui.Info(vars);
		}

	}
}

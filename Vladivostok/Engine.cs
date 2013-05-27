using Chess.Lib;
using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Vladivostok
{
	public unsafe class Engine : IUciEngine
	{
		// ------------------------ UCI Interface --------------------------

		public IUciGui UciGui { get; set; }
		public Action QuitCallback { get; set; }
		BoardStruct* BoardPtr;

		public Engine()
		{
			BoardPtr = (BoardStruct*)0;
		}

		~Engine()
		{
			Board.Delete(BoardPtr);
		}

		public void Uci()
		{
			bool debug = false;
			while (debug)
			{
				System.Threading.Thread.Sleep(100);
			}

			UciGui.ID("Vladivostok", "Valdemar Erlingsson");
			UciGui.Option("Hash", UciOptionType.Spin, 32, 1, 2048, null);
			UciGui.Option("UCI_Opponent", UciOptionType.String, null, null, null, null);
			UciGui.Option("Nullmove", UciOptionType.Check, true, null, null, null);
			//UciGui.Option("Ponder", UciOptionType.Check, false, null, null, null);
			//UciGui.Option("OwnBook", UciOptionType.Check, true, null, null, null);

			Manager.InitLibrary();
			Manager.SendMessage = EngineHandler;
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
			BoardPtr = Chess.Lib.Board.Create();
			Chess.Lib.Board.Init(BoardPtr, 1);
		}

		public void Position(string fenString, List<UciMove> moves)
		{
			SendInfo("Reading position");

			if ((long)BoardPtr != 0)
				Chess.Lib.Board.Delete(BoardPtr);

			if (fenString == null)
			{
				BoardPtr = Chess.Lib.Board.Create();
				Chess.Lib.Board.Init(BoardPtr, 1);
			}
			else
			{
				var bx = Chess.Base.Notation.FENtoBoard(fenString);
				BoardPtr = Helpers.ManagedBoardToNative(bx);
			}

			foreach(var move in moves)
				Board.Make(BoardPtr, move.From, move.To);
		}

		MoveSmall BestMove;

		public void Go(UciGoParameters parameters)
		{
			BestMove = new MoveSmall() { From = 0, Piece = 0, Score = 0, To = 0 };
			BestMove = Search.SearchPos(BoardPtr, 8);
			//UciGui.BestMove(UciMove.FromString("e2e4"), UciMove.FromString("a7a5"));
		}

		public void Stop()
		{
			var uciMove = new UciMove(BestMove.From, BestMove.To);
			UciGui.BestMove(uciMove, null);
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

		private void EngineHandler(string type, Dictionary<string, string> data)
		{
			var sb = new StringBuilder();
			foreach (var kvp in data)
				sb.Append(kvp.Key + " " + kvp.Value + " ");

			SendInfo(sb.ToString());
		}

	}
}

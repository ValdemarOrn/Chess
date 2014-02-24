using Chess.Lib;
using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
			//UciGui.Option("Nullmove", UciOptionType.Check, true, null, null, null);
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

			name = name.ToLower();

			if(name == "hash")
			{
				var sizeMB = Convert.ToInt32(value);
				TTable.Delete();
				TTable.Init(sizeMB);
			}
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
				var bx = Chess.Base.Notation.ReadFEN(fenString);
				BoardPtr = Helpers.ManagedBoardToNative(bx);
			}

			foreach (var move in moves)
			{
				var moved = Board.Make(BoardPtr, move.From, move.To);
				if (!moved)
					SendInfo("Illegal move: " + move.ToString());

				if (move.Promotion != UciPiece.None)
				{
					var promoted = Board.Promote(BoardPtr, move.To, (int)move.Promotion);
					if (!promoted)
						SendInfo("Illegal promotion: " + move.ToString());
				}
			}
		}

		MoveSmall BestMove;

		public void Go(UciGoParameters parameters)
		{
			var player = BoardPtr->PlayerTurn;
			
			int depth = parameters.Depth ?? 99;
			long time = Int32.MaxValue;
			
			if(player == Board.COLOR_WHITE && parameters.WhiteTime != null)
			{
				var toGo = parameters.MovesToGo ?? 100;
				time = (long)(parameters.WhiteTime.Value / (double)toGo + parameters.WhiteInc.GetValueOrDefault());
			}
			else if (player == Board.COLOR_BLACK && parameters.BlackTime != null)
			{
				var toGo = parameters.MovesToGo ?? 100;
				time = (long)(parameters.BlackTime.Value / (double)toGo + parameters.BlackInc.GetValueOrDefault());
			}

			time = parameters.MoveTime ?? time;

			BestMove = new MoveSmall();

			// Stop searching after time has passed
			Task.Factory.StartNew(() =>
			{
				if (parameters.Infinite)
					return;

				Thread.Sleep((int)time);
				Search.StopSearch();
			});

			// Start the search
			Task.Factory.StartNew(() =>
			{
				BestMove = Search.SearchPos(BoardPtr, depth);
				UciGui.BestMove(new UciMove(BestMove.From, BestMove.To, (UciPiece)BestMove.Promotion), null);
			});
		}

		public void Stop()
		{
			Search.StopSearch();
			// We don't need to do anything further here, the Search Task started in Go() will now stop
			// and invoke UciGui.BestMove().
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

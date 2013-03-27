using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public class UciController : IUciGui
	{
		public IUciEngine Engine { get; set; }

		/// <summary>
		/// Delegate for UCI GUI. Usually this is Console.WriteLine
		/// </summary>
		public Action<string> UciCallback { get; set; }

		/// <summary>
		/// Read command from GUI. Returns true if command was recognized and successfully parsed.
		/// Usually this comes from Console.ReadLine
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public bool ReadCommand(string commandString)
		{
			var command = GetCommand(commandString);

			if (command.Item1 == null)
				return false;

			switch (command.Item1)
			{
				case(UciToEngine.Uci):
					Engine.Uci();
					break;
				case (UciToEngine.Debug):
					Debug(command.Item2);
					break;
				case (UciToEngine.IsReady):
					Engine.IsReady();
					break;
				case (UciToEngine.SetOption):
					SetOption(command.Item2);
					break;
				case (UciToEngine.Register):
					Register(command.Item2);
					break;
				case (UciToEngine.UciNewGame):
					Engine.UciNewGame();
					break;
				case (UciToEngine.Position):
					Position(command.Item2);
					break;
				case (UciToEngine.Go):
					Go(command.Item2);
					break;
				case (UciToEngine.Stop):
					Engine.Stop();
					break;
				case (UciToEngine.PonderHit):
					Engine.PonderHit();
					break;
				case (UciToEngine.Quit):
					Engine.Quit();
					break;
				default:
					return false;
			}

			return true;
		}

		private void Go(string goString)
		{
			if (String.IsNullOrWhiteSpace(goString))
				return;

			var kvl = CommandParser.GetElements(goString, "searchmoves", "ponder", "wtime", "btime", "winc", "binc", "movestogo", "depth", "nodes", "mate", "movetime", "infinite");

			var param = new UciGoParameters()
			{
				BlackInc = kvl.ContainsKey("binc") ? Convert.ToInt64(kvl["binc"]) : new Nullable<Int64>(),
				BlackTime = kvl.ContainsKey("btime") ? Convert.ToInt64(kvl["btime"]) : new Nullable<Int64>(),
				Depth = kvl.ContainsKey("depth") ? Convert.ToInt32(kvl["depth"]) : new Nullable<Int32>(),
				Infinite = kvl.ContainsKey("infinite"),
				Mate = kvl.ContainsKey("mate") ? Convert.ToInt32(kvl["mate"]) : new Nullable<Int32>(),
				MovesToGo = kvl.ContainsKey("movestogo") ? Convert.ToInt32(kvl["movestogo"]) : new Nullable<Int32>(),
				MoveTime = kvl.ContainsKey("movetime") ? Convert.ToInt32(kvl["movetime"]) : new Nullable<Int32>(),
				Nodes = kvl.ContainsKey("nodes") ? Convert.ToInt32(kvl["nodes"]) : new Nullable<Int32>(),
				Ponder = kvl.ContainsKey("ponder"),
				SearchMoves = kvl.ContainsKey("searchmoves") ? kvl["searchmoves"].Split(' ').Select(x => UciMove.FromString(x)).ToList() : new List<UciMove>(),
				WhiteInc = kvl.ContainsKey("winc") ? Convert.ToInt64(kvl["winc"]) : new Nullable<Int64>(),
				WhiteTime = kvl.ContainsKey("wtime") ? Convert.ToInt64(kvl["wtime"]) : new Nullable<Int64>(),
			};

			Engine.Go(param);
		}

		private void Position(string positionText)
		{
			if (String.IsNullOrWhiteSpace(positionText))
				return;

			var kvl = CommandParser.GetElements(positionText, "fen", "startpos", "moves");

			string fen = kvl.ContainsKey("fen") ? kvl["fen"] : null;
			List<UciMove> moves = new List<UciMove>();

			if(kvl.ContainsKey("moves"))
			{
				string moveString = kvl["moves"];
				moves = moveString.Split(' ').Select(x => UciMove.FromString(x)).ToList();
			}

			Engine.Position(fen, moves);
		}

		private void Register(string registerText)
		{
			if (String.IsNullOrWhiteSpace(registerText))
				return;

			var kvl = CommandParser.GetElements(registerText, "later", "name", "code");
			var name = kvl.ContainsKey("name") ? kvl["name"] : null;
			var code = kvl.ContainsKey("code") ? kvl["code"] : null;
			Engine.Register(kvl.ContainsKey("later"), name, code);
		}

		private void SetOption(string optionText)
		{
			if(String.IsNullOrWhiteSpace(optionText))
				return;

			var kvl = CommandParser.GetElements(optionText, "name", "value");
			Engine.SetOption(kvl["name"], kvl["value"]);
		}

		private void Debug(string p)
		{
			if (String.IsNullOrWhiteSpace(p))
				Engine.SetDebug(false);

			p = p.ToLower();
			Engine.SetDebug(UciUtils.ParseBool(p));
		}
		
		private static Tuple<UciToEngine?, string> GetCommand(string command)
		{
			UciToEngine? engineCommand;
			string values = null;

			var parts = command.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (parts.Length == 0)
				return new Tuple<UciToEngine?, string>(null, null);

			// search through the tokens, looking for a valid UCI command string
			engineCommand = null;
			int i = 0;

			while (i < parts.Length)
			{
				engineCommand = UciUtils.GetUciCommand(parts[i]);

				if (engineCommand != null)
				{
					var indexOfCommand = command.IndexOf(parts[i]);
					values = command.Substring(indexOfCommand + parts[i].Length).Trim();
					break;
				}

				i++;
			}

			return new Tuple<UciToEngine?, string>(engineCommand, values);
		}

		// ---------------------------------Interface Methods ---------------------------------

		public void ID(string name, string author)
		{
			
		}

		public void UciOk()
		{
			
		}

		public void ReadyOk()
		{
			
		}

		public void BestMove(UciMove bestMove, UciMove ponderMove)
		{
			
		}

		public void CopyProtection(bool protectionIsOk)
		{
			
		}

		public void Registration(bool registrationIsOk)
		{
			
		}

		public void Info(Dictionary<UciInfo, string> infoValues)
		{
			
		}

		public void Option(string name, UciOptionType type, object defaultValue, object min, object max, List<object> values)
		{
			
		}
	}
}

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
			var command = GetCommand<UciToEngine>(commandString);

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

		/// <summary>
		/// Returns a struct containing the UciToEngine or UciToGui command and the argument string that follows it
		/// </summary>
		/// <typeparam name="T">Must be either UciToEngine or UciToGui</typeparam>
		/// <param name="command"></param>
		/// <returns></returns>
		private static Tuple<T?, string> GetCommand<T>(string command) where T : struct, IConvertible
		{
			Nullable<T> engineCommand;
			string values = null;

			var parts = command.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			if (parts.Length == 0)
				return new Tuple<T?, string>(null, null);

			// search through the tokens, looking for a valid UCI command string
			engineCommand = null;
			int i = 0;

			while (i < parts.Length)
			{
				if(typeof(T) == typeof(UciToEngine))
					engineCommand = (Nullable<T>)(object)UciUtils.GetEngineCommand(parts[i]);
				else if (typeof(T) == typeof(UciToGui))
					engineCommand = (Nullable<T>)(object)UciUtils.GetEngineCommand(parts[i]);
				else
					throw new Exception("Wrong argument type");

				if (engineCommand != null)
				{
					var indexOfCommand = command.IndexOf(parts[i]);
					values = command.Substring(indexOfCommand + parts[i].Length).Trim();
					break;
				}

				i++;
			}

			return new Tuple<T?, string>(engineCommand, values);
		}

		// ---------------------------------Interface Methods ---------------------------------

		public void ID(string name, string author)
		{
			name = name ?? "";
			author = author ?? "";
			UciCallback("id name " + name);
			UciCallback("id author " + author);
		}

		public void UciOk()
		{
			UciCallback("uciok");
		}

		public void ReadyOk()
		{
			UciCallback("readyok");
		}

		public void BestMove(UciMove bestMove, UciMove ponderMove)
		{
			string text = "bestmove " + bestMove.ToString();
			if (ponderMove != null)
				text += " " + ponderMove.ToString();

			UciCallback(text);
		}

		public void CopyProtection(bool protectionIsOk)
		{
			UciCallback("copyprotection checking");

			if(protectionIsOk)
				UciCallback("copyprotection ok");
			else
				UciCallback("copyprotection error");
		}

		public void Registration(bool registrationIsOk)
		{
			UciCallback("registration checking");

			if (registrationIsOk)
				UciCallback("registration ok");
			else
				UciCallback("registration error");
		}

		public void Info(Dictionary<UciInfo, string> infoValues)
		{
			var sb = new StringBuilder();

			sb.Append("info");

			foreach(var val in infoValues)
			{
				string info = val.Key.ToString().ToLower();

				sb.Append(' ');
				sb.Append(info);
				sb.Append(' ');
				sb.Append(val.Value);
			}

			var output = sb.ToString();
			UciCallback(output);
		}

		public void Option(string name, UciOptionType type, object defaultValue, object min, object max, List<object> values)
		{
			var sb = new StringBuilder();
			sb.Append("option name ");
			sb.Append(name);
			sb.Append(" type ");
			sb.Append(type.ToString().ToLower());

			if(defaultValue != null)
			{
				sb.Append(" default ");

				if(defaultValue.GetType() == typeof(bool))
					sb.Append(defaultValue.ToString().ToLower());
				else
					sb.Append(defaultValue.ToString());
			}

			if (min != null)
			{
				sb.Append(" min ");

				if (min.GetType() == typeof(bool))
					sb.Append(min.ToString().ToLower());
				else
					sb.Append(min.ToString());
			}

			if (max != null)
			{
				sb.Append(" max ");

				if (max.GetType() == typeof(bool))
					sb.Append(max.ToString().ToLower());
				else
					sb.Append(max.ToString());
			}

			if(values != null)
			{
				foreach(var v in values)
				{
					sb.Append(" var ");

					if (v.GetType() == typeof(bool))
						sb.Append(v.ToString().ToLower());
					else
						sb.Append(v.ToString());
				}
			}

			var output = sb.ToString();
			UciCallback(output);
		}
	}
}

using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Chess.Testbed
{
	public class UciProcess
	{
		public enum CommandDirection
		{
			EngineInput,
			EngineOutput
		}

		private Process engineProcess;
		private System.Threading.AutoResetEvent resetEvent;
		public volatile bool isDisposed;

		public UciEngineSettings Settings { get; private set; }
		public bool EngineStarted { get; private set; }

		public event Action<CommandDirection, string> CommandSendEvent;
		public event Action<UciMove, UciMove> BestMoveCallback;
		public event Action<Dictionary<UciInfo, string>> InfoCallback;

		// Received info from engine
		public string IdName { get; private set; }
		public string IdAuthor { get; private set; }
		public bool UciOkFlag { get; private set; }
		public bool ReadyOkFlag { get; private set; }
		public bool? CopyProtectionOkFlag { get; private set; }
		public bool? RegistrationOkFlag { get; private set; }
		public List<UciOption> Options { get; private set; }

		public UciProcess(UciEngineSettings settings)
		{
			resetEvent = new System.Threading.AutoResetEvent(false);
			Options = new List<UciOption>();
			Settings = settings;

			var startInfo = new ProcessStartInfo()
			{
				Arguments = settings.Parameters,
				CreateNoWindow = true,
				FileName = settings.Command,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				StandardErrorEncoding = Encoding.UTF8,
				StandardOutputEncoding = Encoding.UTF8,
				UseShellExecute = false,
				WindowStyle = ProcessWindowStyle.Hidden
				//WorkingDirectory = ""
			};

			engineProcess = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
			engineProcess.OutputDataReceived += (s, e) => ReadCommand(e.Data);
		}

		public void Start()
		{
			engineProcess.Start();
			EngineStarted = true;
			engineProcess.BeginOutputReadLine();
			WriteCommand("uci");
			resetEvent.WaitOne(); // set by readyok
		}

		public void WriteCommand(string data)
		{
			engineProcess.StandardInput.WriteLine(data);
			engineProcess.StandardInput.Flush();
			if (CommandSendEvent != null)
				CommandSendEvent.Invoke(CommandDirection.EngineInput, data);
		}

		private void ReadCommand(string data)
		{
			if (String.IsNullOrWhiteSpace(data))
				return;

			if (CommandSendEvent != null)
				CommandSendEvent.Invoke(CommandDirection.EngineOutput, data);

			var parts = data.Split(' ');
			if (parts.Length < 0)
				return;

			var command = UciUtils.GetEnum<UciToGui>(parts[0]);
			if (command == null)
				return;

			var cmd = command.Value;
			Dictionary<string, string> parameters;
			string name;

			switch (cmd)
			{
				case UciToGui.Id:
					parameters = CommandParser.GetElements(data, "name", "author");
					name = parameters.GetValueOrNull("name");
					var author = parameters.GetValueOrNull("author");
					ID(name, author);
					break;
				case UciToGui.UciOk:
					UciOk();
					break;
				case UciToGui.ReadyOk:
					ReadyOk();
					break;
				case UciToGui.BestMove:
					parameters = CommandParser.GetElements(data, "bestmove", "ponder");
					var bestmove = parameters["bestmove"];
					var ponder = parameters.GetValueOrNull("ponder");
					if(BestMoveCallback != null)
						BestMoveCallback(UciMove.FromString(bestmove), UciMove.FromString(ponder));
					break;
				case UciToGui.CopyProtection:
					parameters = CommandParser.GetElements(data, "copyprotection");
					var protection = parameters.GetValueOrNull("copyprotection");
					protection = protection.ToLower();
					if (protection == "error")
						CopyProtection(false);
					else if (protection == "ok")
						CopyProtection(true);
					break;
				case UciToGui.Registration:
					parameters = CommandParser.GetElements(data, "registration");
					var register = parameters.GetValueOrNull("registration");
					register = register.ToLower();
					if (register == "error")
					{
						Registration(false);
						Register(true, null, null);
					}
					else if (register == "ok")
						Registration(true);
					break;
				case UciToGui.Info:
					parameters = CommandParser.GetElements(data,
						"depth", "seldepth", "time", "nodes", "pv", "multipv", "score", "currmove", "currmovenumber",
						"hashfull", "nps", "tbhits", "sbhits", "cpuload", "string", "refutation", "currline");
					var infos = parameters
						.Where(x => UciUtils.GetEnum<UciInfo>(x.Key) != null)
						.ToDictionary(x => UciUtils.GetEnum<UciInfo>(x.Key).Value, x => x.Value);
					if (InfoCallback != null)
						InfoCallback(infos);
					break;
				case UciToGui.Option:
					parameters = CommandParser.GetElements(data, "name", "type", "default", "min", "max", "var");
					var vars = CommandParser.GetMultipleElements(data, "var").Cast<object>().ToList();
					name = parameters["name"];
					var type = UciUtils.GetEnum<UciOptionType>(parameters["type"]).Value;
					var def = parameters.GetValueOrNull("default");
					var min = parameters.ContainsKey("min") ? Convert.ToInt32(parameters["min"]) : new int?();
					var max = parameters.ContainsKey("max") ? Convert.ToInt32(parameters["max"]) : new int?();
					Option(name, type, def, min, max, vars);
					break;
			}
		}

		// ----------------- Public interface for engine process ----------------------

		public void SetOptions()
		{
			// Todo: send options

			WriteCommand("isready");
		}

		public void SetDebug(bool debug)
		{
			if (debug)
				WriteCommand("debug on");
			else
				WriteCommand("debug off");
		}

		public void SetOption(string name, string value)
		{
			WriteCommand("setoption name " + name + " value " + value);
		}

		public void Register(bool later, string name, string code)
		{
			if (later)
				WriteCommand("register later");
			else
				WriteCommand("register name " + name + " code " + code);
		}

		public void UciNewGame()
		{
			WriteCommand("ucinewgame");
		}

		public void Position(string fenString, IEnumerable<UciMove> moves)
		{
			string output = "position ";

			if (fenString == null)
				output += "startpos";
			else
				output += "fen " + fenString;

			var moveArray = moves.ToArray();
			if (moves != null && moveArray.Length > 0)
			{
				output += " moves " + moveArray.Select(x => x.ToString()).Aggregate((m, x) => m + " " + x);
			}

			WriteCommand(output);
		}

		public void Go(UciGoParameters parameters)
		{
			string output = "go";

			if (parameters.Depth != null)
				output += " depth " + parameters.Depth;
			else if (parameters.Mate != null)
				output += " mate " + parameters.Mate;
			else if (parameters.Infinite)
				output += " infinite";

			if (parameters.Nodes != null)
				output += " nodes " + parameters.Nodes;
			if (parameters.Ponder == true)
				output += " ponder";
			if (parameters.SearchMoves != null && parameters.SearchMoves.Count > 0)
				output += " searchmoves " + parameters.SearchMoves.Select(x => x.ToString()).Aggregate((m, x) => m + " " + x);

			if (parameters.BlackInc != null)
				output += " binc " + parameters.BlackInc;
			if (parameters.BlackTime != null)
				output += " btime " + parameters.BlackTime;
			if (parameters.WhiteInc != null)
				output += " winc " + parameters.WhiteInc;
			if (parameters.WhiteTime != null)
				output += " wtime " + parameters.WhiteTime;

			WriteCommand(output);
		}

		public void Stop()
		{
			WriteCommand("stop");
		}

		public void PonderHit()
		{
			WriteCommand("ponderhit");
		}

		public void Quit()
		{
			WriteCommand("quit");
		}

		// ------------------ Callbacks for engine process ------------------

		private void ID(string name, string author)
		{
			if (name != null)
				IdName = name;

			if (author != null)
				IdAuthor = author;
		}

		private void UciOk()
		{
			UciOkFlag = true;
			SetOptions();
		}

		private void ReadyOk()
		{
			ReadyOkFlag = true;
			resetEvent.Set();
		}

		private void CopyProtection(bool protectionIsOk)
		{
			CopyProtectionOkFlag = protectionIsOk;
		}

		private void Registration(bool registrationIsOk)
		{
			RegistrationOkFlag = registrationIsOk;
		}

		private void Option(string name, UciOptionType type, object defaultValue, int? min, int? max, List<object> options)
		{
			var opt = new UciOption
			{
				DefaultValue = defaultValue,
				Max = max,
				Min = min,
				Name = name,
				Type = type,
				Options = options
			};

			Options.Add(opt);
		}

		// ------------------ Dispose ------------------

		~UciProcess()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (EngineStarted && !engineProcess.HasExited && !isDisposed)
			{
				try
				{
					Quit();
					System.Threading.Thread.Sleep(200);
				}
				catch (Exception)
				{
					
				}
				finally
				{
					isDisposed = true;
					engineProcess.CancelOutputRead();
					if (!engineProcess.HasExited)
						engineProcess.Kill();
				}
			}
		}
	}
}

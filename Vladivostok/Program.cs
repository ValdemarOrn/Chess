using Chess.Uci;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chess.Vladivostok
{
	class Program
	{
		static FileStream LogStream;
		static StreamWriter LogWriter;
		static volatile bool LogWritten;

		static void Main(string[] args)
		{
			LogStream = new FileStream(string.Format("Vladivostok-{0:yyyy-MM-dd-HHmmss}.log", DateTime.Now), FileMode.Append, FileAccess.Write, FileShare.Read);
			LogWriter = new StreamWriter(LogStream);

			Task.Factory.StartNew(() => 
			{ 
				var lastUpdate = DateTime.Now;
				while(true)
				{
					if (LogWritten || (DateTime.Now - lastUpdate).TotalMilliseconds > 500)
					{
						lastUpdate = DateTime.Now;
						LogWritten = false;
						LogWriter.Flush();
					}
					Thread.Sleep(100);
				}
			});

			Log("Starting Vladivostok...");
			var program = new Program();
			program.Start();
		}

		static void Log(string message)
		{
			LogWriter.WriteLine("{0:HH:mm:ss.fff} - {1}", DateTime.Now, message);
			LogWritten = true;
		}

		ManualResetEvent QuitEvent;
		bool Running;

		UciController Controller;
		Thread ListeningThread;
		Engine Engine;

		public void Start()
		{
			QuitEvent = new ManualResetEvent(false);
			Running = true;
			
			Controller = new UciController();
			Engine = new Engine();
			Engine.UciGui = Controller;
			Engine.QuitCallback = Quit;

			Controller.UciCallback = Console.WriteLine;
			Controller.Engine = Engine;

			StartListening();

			// wait until we receive instructions to quit
			QuitEvent.WaitOne();
		}

		private void StartListening()
		{
			ListeningThread = new Thread(new ThreadStart(Listen));
			ListeningThread.Start();
		}

		private void Listen()
		{
			while(Running)
			{
				try
				{
					string input = Console.ReadLine();
					Controller.ReadCommand(input);
				}
				catch(Exception) { }

				Thread.Sleep(10);
			}
		}

		private void Quit()
		{
			Running = false;
			QuitEvent.Set();
		}
	}
}

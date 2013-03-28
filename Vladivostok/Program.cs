using Chess.Uci;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Chess.Vladivostok
{
	class Program
	{
		static void Main(string[] args)
		{
			var program = new Program();
			program.Start();
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

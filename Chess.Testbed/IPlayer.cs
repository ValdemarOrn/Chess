using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Uci;

namespace Chess.Testbed
{
	public enum PlayerType
	{
		Unknown = 0,

		Human,
		UCI,
		XBoard
	}

	public interface IPlayer
	{
		PlayerType Type { get; }
		string Name { get; }

		void Start();
		void SetPosition(string fenString, IEnumerable<UciMove> moves);
		void Play(UciGoParameters parameters);
		void Stop();
		void RegisterCommandListener(Action<UciProcess.CommandDirection, string> listener);
	}
}

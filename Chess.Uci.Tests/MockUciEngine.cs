using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci.Tests
{
	class MockUciEngine : IUciEngine
	{
		public IUciGui UciGui { get; set; }

		public string Function;
		public List<object> Arguments;

		public void Uci()
		{
			Function = "Uci";
			Arguments = new List<object>();
		}

		public void SetDebug(bool debug)
		{
			Function = "SetDebug";
			Arguments = new List<object>() { debug };
		}

		public void IsReady()
		{
			Function = "IsReady";
			Arguments = new List<object>();
		}

		public void SetOption(string name, string value)
		{
			Function = "SetOption";
			Arguments = new List<object>() { name, value };
		}

		public void Register(bool later, string name, string code)
		{
			Function = "Register";
			Arguments = new List<object>() { later, name, code };
		}

		public void UciNewGame()
		{
			Function = "UciNewGame";
			Arguments = new List<object>();
		}

		public void Position(string fenString, List<UciMove> moves)
		{
			Function = "Position";
			Arguments = new List<object>() { fenString, moves };
		}

		public void Go(UciGoParameters parameters)
		{
			Function = "Go";
			Arguments = new List<object>() { parameters };
		}

		public void Stop()
		{
			Function = "Stop";
			Arguments = new List<object>();
		}

		public void PonderHit()
		{
			Function = "PonderHit";
			Arguments = new List<object>();
		}

		public void Quit()
		{
			Function = "Quit";
			Arguments = new List<object>();
		}

	}
}

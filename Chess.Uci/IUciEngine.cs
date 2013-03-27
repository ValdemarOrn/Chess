using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Uci
{
	public interface IUciEngine
	{
		IUciGui UciGui {get; set;}

		void Uci();
		void SetDebug(bool debug);
		void IsReady();
		void SetOption(string name, string value);
		void Register(bool later, string name, string code);
		void UciNewGame();
		/// <summary>
		/// fenString is null if using startpos
		/// </summary>
		void Position(string fenString, List<UciMove> moves);
		void Go(UciGoParameters parameters);
		void Stop();
		void PonderHit();
		void Quit();
	}
}

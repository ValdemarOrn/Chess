using Chess.Base;
using Chess.Base.PGN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Chess.Lib.OpeningBookGenerator
{
	class Program
	{
		public static void Main(string[] args)
		{
			string input = @"C:\Users\Valdemar\Desktop\Chess\PGN\_CCRL-4040.[439277].pgn";
			string output = @"c:\book.txt";
			FileStream file;
			StreamWriter writer;

			try
			{
				file = File.Open(output, FileMode.CreateNew);
				writer = new StreamWriter(file);
				writer.AutoFlush = false;
			}
			catch(Exception e)
			{
				Console.WriteLine("Unable to open output file: " + e.Message);
				return;
			}

			int total = 0;

			Action<string, string> handler = (f, enc) =>
			{
				total++;

				if (total % 100 == 0)
					Console.WriteLine("Processed {0} Games", total);
			};

			Action<string, PGNParserError> error = (f, x) =>
			{
				Console.WriteLine("Error parsing game {0} in file {1}", x.GameNumber, f);
				Console.WriteLine(x.ToString());
				Console.WriteLine("");
			};

			var lines = OpeningBook.CompileBook(new List<string>() { input }, 20, 5, handler, error);

			lines.ForEach(x => writer.WriteLine(x));
			writer.Flush();
			writer.Close();
			Console.WriteLine("Process Complete");
			Console.ReadLine();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib.OpeningBookGen
{
	class Program
	{
		public static void Main(string[] args)
		{
			Manager.InitLibrary();

			if (args.Length == 3 && args[0] == "clean")
			{
				var pgnInputFile = args[1];
				var cleanfile = args[2];
				System.IO.File.WriteAllText(cleanfile, "");
				OpeningBook.StripPGNs(pgnInputFile, cleanfile);
			}
			else if (args.Length == 5 && args[0] == "make")
			{
				string cleanfile = args[1];
				int from = Convert.ToInt32(args[2]);
				int count = Convert.ToInt32(args[3]);
				string outputFile = args[4];

				System.IO.File.WriteAllText(outputFile, "");

				var inputs = System.IO.File.ReadAllLines(cleanfile);
				inputs = inputs.Skip(from).Take(count).ToArray();
				var output = OpeningBook.GenerateBook(inputs);

				System.IO.File.WriteAllText(outputFile, output);
			}
			else
			{
				Console.WriteLine("To clean and strip moves from a PGN file:");
				Console.WriteLine("Gen.exe clean pgnfile.pgn cleanfile.txt");
				Console.WriteLine("");
				Console.WriteLine("To compile the opening book from the cleaned file:");
				Console.WriteLine("Gen.exe make cleanfile.txt startPos count output.txt");
				Console.WriteLine("Example:");
				Console.WriteLine("Gen.exe make cleanfile.txt 0 1000 book.txt");
			}


		}
	}
}

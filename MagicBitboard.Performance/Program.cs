using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MagicBitboard.Performance
{
	class Program
	{
		static Dictionary<ulong, ulong> Map;
		static int idx;
		static int Bits;
		static bool Running;
		static DateTime StartTime;
        static bool Success;
        static Dictionary<int, Tuple<int, ulong>> MagicNumbers = new Dictionary<int, Tuple<int, ulong>>();

		static unsafe void Main(string[] args)
		{
            Bitboard.TryMany();
            Bitboard.FirstBit((ulong)123);

            var start = DateTime.Now;
            uint output = Bitboard.TryMany();
            var millis = (DateTime.Now - start).TotalMilliseconds;
            Console.WriteLine("Millis C++: " + millis);
            Console.WriteLine("output " + output);

            output = 0;
            start = DateTime.Now;
            for (int i = 0; i < 1000000; i++)
            {
                ulong mask = (ulong)i;
                uint index = Bitboard.FirstBit(mask);
                output += index;
            }
            millis = (DateTime.Now - start).TotalMilliseconds;
            Console.WriteLine("Millis C# call: " + millis);
            Console.WriteLine("output " + output);

            Console.ReadLine();
		}

        public static void CalculateMagic()
        {
            int timeoutSeconds = 10;

            Console.Write("Position to start at: ");
            string line = Console.ReadLine();
            idx = Convert.ToInt32(line);

            Console.Write("# of positions to search: ");
            line = Console.ReadLine();
            int idxMax = Convert.ToInt32(line) + idx;

            Console.Write("Time for each iteration (seconds): ");
            line = Console.ReadLine();
            timeoutSeconds = Convert.ToInt32(line);

            Console.Write("Number of worker threads: ");
            line = Console.ReadLine();
            int numberOfThreads = Convert.ToInt32(line);

            Bits = 14;

            while (true)
            {
                if (idx >= idxMax)
                    break;

                var perms = BitboardRook.GetRookPermutations(idx);
                Map = new Dictionary<ulong, ulong>();

                foreach (var perm in perms)
                {
                    Map[perm] = BitboardRook.GetRookMoves(perm, idx);
                }

                int cardinalCount = Map.Select(x => x.Value).Distinct().Count();

                Console.WriteLine("Position " + idx + " : " + Map.Count + "/" + cardinalCount + ". Searching for " + Bits + " bit index");

                Running = true;
                Success = false;
                StartTime = DateTime.Now;

                for (int i = 0; i < numberOfThreads; i++)
                    new Thread(new ThreadStart(Find)).Start();

                while (Running)
                {
                    if ((DateTime.Now - StartTime).TotalSeconds > timeoutSeconds)
                        Running = false;

                    Thread.Sleep(10);
                }

                Thread.Sleep(100);



                if (Success)
                {
                    Console.WriteLine("New best match for postition " + idx + ": " + MagicNumbers[idx].Item1 + " bits - " + MagicNumbers[idx].Item2);
                    Bits--;
                }
                else
                {
                    Console.WriteLine("Search aborted");
                    Bits = 14;
                    idx++;
                }
            }

            Console.WriteLine("-----------------------------");
            foreach (var kvp in MagicNumbers)
                Console.WriteLine("" + kvp.Key + ": " + kvp.Value.Item1 + " bits: " + kvp.Value.Item2);
            Console.WriteLine("-----------------------------");
            Console.WriteLine("All numbers found");
            Console.ReadLine();
        }

		public static void Find()
		{
			var start = DateTime.Now;
            ulong val = MagicBitboard.FindMagic(Map, Bits, 0, ref Running);
            if (val != 0)
            {
                Success = true;
                lock (MagicNumbers)
                {
                    MagicNumbers[idx] = new Tuple<int, ulong>(Bits, val);
                }
            }
			Running = false;
		}
	}
}

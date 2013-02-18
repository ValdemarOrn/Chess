using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib
{
    public sealed class MagicBitboard
    {
        /// <summary>
        /// Brute forces the magic constant that provides perfect hashing for all dictionary keys to values
        /// </summary>
        /// <param name="map">Dictionary containing the permutation at key and the move board as value</param>
        /// <param name="keyBits">number of bits in the lookup table key</param>
        /// <param name="seed">Seed for random number generator</param>
        /// <param name="running">reference value to indicate it calculations should continue to run</param>
        /// <returns></returns>
        public static ulong FindMagic(Dictionary<ulong, ulong> map, int keyBits, int seed, ref bool running)
        {
            // convert dictionary into array, increase speed
            ulong[] data = new ulong[map.Count * 2];
            int p = 0;
            foreach (var kvp in map)
            {
                data[p] = kvp.Key;
                data[p + 1] = kvp.Value;
                p = p+2;
            }

            ulong iterations = 0;

            ulong size = (ulong)(1 << keyBits);
            if (seed == 0)
                seed = (int)DateTime.Now.Ticks;

            var r = new Random(seed);
            byte[] buf = new byte[8];

            ulong[] table = new ulong[size];
            ulong magic = 0;

            var start = DateTime.Now;

            while (running)
            {
                // clear table
                Array.Clear(table, 0, table.Length);

                iterations++;

                r.NextBytes(buf);
                magic = BitConverter.ToUInt64(buf, 0);
                bool success = true;

                for(int i=0; i<data.Length; i +=2)
                {
                    int idx = (int)((data[i] * magic) >> (64 - keyBits));
                    if (table[idx] == 0 || table[idx] == data[i+1])
                        table[idx] = data[i+1];
                    else
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                    break;
            }

            if (running)
                return magic;
            else // stopped before we found a match
                return 0;
        }
    }
}

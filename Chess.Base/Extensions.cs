using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	static class Extensions
	{
		/// <summary>
		/// finds the first element that satisfies the comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static int BinarySearch<T>(this List<T> list, Func<T, int> comparer)
		{
			var min = 0;
			var max = list.Count;
			var i = max / 2;

			while(true)
			{
				var ev = comparer(list[i]);
				if (ev >= 0)
					max = i;
				else if (ev < 0)
					min = i;

				i = (max + min) / 2;

				if((max - min) < 4)
				{
					if (comparer(list[min]) == 0)
						return min;
					else if (comparer(list[min + 1]) == 0)
						return min + 1;
					else if (comparer(list[min + 2]) == 0)
						return min + 2;
					else if (comparer(list[min + 3]) == 0)
						return min + 3;
					else
						return -1;
				}
			}
		}

		/// <summary>
		/// finds the LAST element that satisfies the comparer
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static int BinarySearchMax<T>(this List<T> list, Func<T, int> comparer)
		{
			var min = 0;
			var max = list.Count;
			var i = max / 2;

			while (true)
			{
				var ev = comparer(list[i]);
				if (ev > 0)
					max = i;
				else if (ev <= 0)
					min = i;

				i = (max + min) / 2;

				if ((max - min) < 4)
				{
					if (comparer(list[min + 3]) == 0)
						return min + 3;
					else if (comparer(list[min + 2]) == 0)
						return min + 2;
					else if (comparer(list[min + 1]) == 0)
						return min + 1;
					else if (comparer(list[min]) == 0)
						return min;
					else
						return -1;
				}
			}
		}
	}
}

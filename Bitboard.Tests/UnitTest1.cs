using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MagicBitboard.Tests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestRookVectors()
		{
			var m = Bitboard.RookVectors;
			var str = Bitboard.ToString(m[7]);
		}

		[TestMethod]
		public void TestSetGet()
		{
			var v = Bitboard.Set(0, 8);
			Assert.AreEqual((ulong)256, v);
			Assert.IsTrue(Bitboard.Get(v, 8));

			v = Bitboard.Set(256, 7);
			Assert.AreEqual((ulong)(256+128), v);
			Assert.IsTrue(Bitboard.Get(v, 7));

			v = Bitboard.Set(563, 12);
			Assert.AreEqual((ulong)(563 + 4096), v);
			Assert.IsTrue(Bitboard.Get(v, 12));
		}

		[TestMethod]
		public void TestUnsetGet()
		{
			var v = Bitboard.Unset(256, 8);
			Assert.AreEqual((ulong)0, v);
			Assert.IsFalse(Bitboard.Get(v, 8));

			v = Bitboard.Unset(256+128, 7);
			Assert.AreEqual((ulong)256, v);
			Assert.IsFalse(Bitboard.Get(v, 7));
		}

		[TestMethod]
		public void TestRookVariations()
		{
			var perms = Bitboard.GetRookPermutations(0);
			var strs = perms.Select(x => Bitboard.ToString(x)).ToList();
		}

		[TestMethod]
		public void TestRookMoves()
		{
			int idx = 0;

			var perms = Bitboard.GetRookPermutations(idx);
			var l = new List<Tuple<string, string>>();

			foreach (var perm in perms)
			{
				var k = Bitboard.GetRookMoves(perm, idx);
				l.Add(new Tuple<string, string>(Bitboard.ToString(perm), Bitboard.ToString(k)));
			}
		}

		[TestMethod]
		public void TestRookMagic()
		{
			int idx = 0;

			var perms = Bitboard.GetRookPermutations(idx);
			var map = new Dictionary<ulong, ulong>();

			foreach (var perm in perms)
			{
				map[perm] = Bitboard.GetRookMoves(perm, idx);
			}

			int count = map.Select(x => x.Value).Distinct().Count();

			int bits = 18; // 2^18
			Bitboard.FindMagic(map, bits);
		}
	}
}

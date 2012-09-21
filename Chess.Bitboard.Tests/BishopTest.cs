using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Bitboard.Tests
{
	[TestClass]
	public class BishopTest
	{
		[TestMethod]
		public void TestBishopVectors()
		{
			var vex = Bishop.BishopVectors;
			var strs = vex.Select(x => Bitboard.ToString(x)).ToList();
			Assert.AreEqual((ulong)0x0040201008040200, vex[0]);
		}

		[TestMethod]
		public void TestBishopPermutations()
		{
			var vex = Bishop.BishopVectors;
			var perms = Bishop.GetPermutations(0);
			var strs = perms.Select(x => Bitboard.ToString(x)).ToList();
		}

		[TestMethod]
		public void TestBishopMoves1()
		{
			int idx = 27;
			var vex = Bishop.BishopVectors;
			var perms = Bishop.GetPermutations(idx);
			var moves = perms.Select(x => Bishop.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();
		}

		[TestMethod]
		public void TestBishopMoves2()
		{
			int idx = 0;
			var vex = Bishop.BishopVectors;
			var perms = Bishop.GetPermutations(idx);
			var moves = perms.Select(x => Bishop.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();

			Assert.AreEqual((ulong)0x8040201008040200, moves[0]);
		}
		
		[TestMethod]
		public void TestRookInitializeAndRead()
		{
			Bishop.Load();
			var r = Bishop.Bishop_Read(27, (ulong)0);
		}
	}
}

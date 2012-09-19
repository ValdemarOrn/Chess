using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MagicBitboard.Tests
{
	[TestClass]
	public class Rook
	{
		[TestMethod]
		public void TestRookVectors()
		{
			var vex = BitboardRook.RookVectors;
			var strs = vex.Select(x => Bitboard.ToString(x)).ToList();
			Assert.AreEqual((ulong)0x000101010101017E, vex[0]);
			Assert.AreEqual((ulong)0x0002020202027C00, vex[9]);
			Assert.AreEqual((ulong)0x007E010101010100, vex[48]);
			Assert.AreEqual((ulong)0x6E10101010101000, vex[60]);
		}

		[TestMethod]
		public void TestRookPermutations()
		{
			var vex = BitboardRook.RookVectors;
			var perms = BitboardRook.GetPermutations(0);
			var strs = perms.Select(x => Bitboard.ToString(x)).ToList();
		}

		[TestMethod]
		public void TestRookMoves1()
		{
			int idx = 27;
			var vex = BitboardRook.RookVectors;
			var perms = BitboardRook.GetPermutations(idx);
			var moves = perms.Select(x => BitboardRook.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();
		}

		[TestMethod]
		public void TestRookMoves2()
		{
			int idx = 0;
			var vex = BitboardRook.RookVectors;
			var perms = BitboardRook.GetPermutations(idx);
			var moves = perms.Select(x => BitboardRook.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();

			Assert.AreEqual((ulong)0x01010101010101FE, moves[0]);
		}

		[TestMethod]
		public void TestRookMoves3()
		{
			int idx = 8;
			var vex = BitboardRook.RookVectors;
			var perms = BitboardRook.GetPermutations(idx);
			var moves = perms.Select(x => BitboardRook.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();

			Assert.AreEqual((ulong)0x010101010101FE01, moves[0]);
		}

		[TestMethod]
		public void TestRookMoves4()
		{
			int idx = 48;
			var vex = BitboardRook.RookVectors;
			var perms = BitboardRook.GetPermutations(idx);
			var moves = perms.Select(x => BitboardRook.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();

			Assert.AreEqual((ulong)0x01FE010101010101, moves[0]);
		}

		[TestMethod]
		public void TestRookMoves5()
		{
			int idx = 59;
			var vex = BitboardRook.RookVectors;
			var perms = BitboardRook.GetPermutations(idx);
			var moves = perms.Select(x => BitboardRook.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();

			Assert.AreEqual((ulong)0xF708080808080808, moves[0]);
		}

		[TestMethod]
		public void TestRookInitializeAndRead()
		{
			BitboardRook.Initialize();
			var r = BitboardRook.Rook_Read(27, (ulong)0);
		}
	}
}

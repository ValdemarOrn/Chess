using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Chess.Lib.MoveClasses;

namespace Chess.Lib.Tests
{
	[TestFixture]
	public class BishopTest
	{
		[Test]
		public void TestBishopVectors()
		{
			var vex = Bishop.BishopVectors;
			var strs = vex.Select(x => Bitboard.ToString(x)).ToList();
			Assert.AreEqual((ulong)0x0040201008040200, vex[0]);
		}

		[Test]
		public void TestBishopPermutations()
		{
			var vex = Bishop.BishopVectors;
			var perms = Bishop.GetPermutations(0);
			var strs = perms.Select(x => Bitboard.ToString(x)).ToList();
		}

		[Test]
		public void TestBishopMoves1()
		{
			int idx = 27;
			var vex = Bishop.BishopVectors;
			var perms = Bishop.GetPermutations(idx);
			var moves = perms.Select(x => Bishop.GetMoves(x, idx)).ToList();

			var strsp = perms.Select(x => Bitboard.ToString(x)).ToList();
			var strsm = moves.Select(x => Bitboard.ToString(x)).ToList();
		}

		[Test]
		public void TestBishopMovesAll()
		{
			for (int i = 0; i < 64; i++)
			{
				var b = new Chess.Base.Board(false);
				b.State[i] = Chess.Base.Colors.Val(Chess.Base.Piece.Bishop, Chess.Base.Color.White);
				var movesBasic = Chess.Base.Moves.GetMoves(b, i);
				movesBasic = movesBasic.OrderBy(x => x).ToArray();

				var movesFast = Bishop.Read(i, 0);
				var list = Bitboard.Bitboard_BitList(movesFast);
				list = list.OrderBy(x => x).ToArray();

				Assert.AreEqual(movesBasic.Length, list.Length);
				for (int j = 0; j < movesBasic.Length; j++)
					Assert.AreEqual((int)movesBasic[j], (int)list[j]);
			}
		}

		[Test]
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
		
		[Test]
		public void TestRookInitializeAndRead()
		{
			Bishop.Load();
			var r = Bishop.Read(27, (ulong)0);
		}
	}
}

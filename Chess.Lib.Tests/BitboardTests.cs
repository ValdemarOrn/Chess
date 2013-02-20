using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class BitboardTests
	{

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
		public void TestUnsetGetTopBit()
		{
			ulong max = 0x8000000000000000;

			var bit = Bitboard.Get(max, 63);
			var bitref = Bitboard.GetRef(ref max, 63);

			Assert.IsTrue(bit);
			Assert.IsTrue(bitref);

			var unset = Bitboard.Unset(max, 63);
			Assert.AreEqual((ulong)0, unset);
			Assert.AreEqual(0x8000000000000000, max);

			Bitboard.UnsetRef(ref max, 63);
			Assert.AreEqual((ulong)0, max);

			var set = Bitboard.Set(max, 63);
			Assert.AreEqual(0x8000000000000000, set);
			Assert.AreEqual((ulong)0, max);

			Bitboard.SetRef(ref max, 63);
			Assert.AreEqual(0x8000000000000000, max);
		}

		[TestMethod]
		public void TestForwardBit()
		{
			ulong val = 0x0100100;
			var fwd = Bitboard.ForwardBit(val);
			Assert.AreEqual(8, fwd);

			val = 0x23000;
			fwd = Bitboard.ForwardBit(val);
			Assert.AreEqual(12, fwd);

			val = 0x2300000000000;
			fwd = Bitboard.ForwardBit(val);
			Assert.AreEqual(44, fwd);
		}

		[TestMethod]
		public void TestReverseBit()
		{
			ulong val = 0x0100100;
			var rwd = Bitboard.ReverseBit(val);
			Assert.AreEqual(20, rwd);

			val = 0x23000;
			rwd = Bitboard.ReverseBit(val);
			Assert.AreEqual(17, rwd);

			val = 0x2300000000000;
			rwd = Bitboard.ReverseBit(val);
			Assert.AreEqual(49, rwd);
		}

		[TestMethod]
		public void TestPopCount()
		{
			ulong val = 0xff010010010030;
			var cnt = Bitboard.PopCount(val);
			Assert.AreEqual(13, cnt);

			val = 0xffff000000000001;
			cnt = Bitboard.PopCount(val);
			Assert.AreEqual(17, cnt);
		}

		[TestMethod]
		public void TestBitList()
		{
			ulong val = (ulong)0xC004003000020003;
			var list = Bitboard.Bitboard_BitList(val);
			var list2 = "0,1,17,36,37,50,62,63".Split(',').Select(x => Convert.ToByte(x)).ToList();

			Assert.IsTrue(list2.SequenceEqual(list));	
		}

		[TestMethod]
		public void TestBitList2()
		{
			ulong val = (ulong)0xC430044A20001204;
			var list = Bitboard.Bitboard_BitList(val);
			var list2 = "2,9,12,29,33,35,38,42,52,53,58,62,63".Split(',').Select(x => Convert.ToByte(x)).ToList();

			Assert.IsTrue(list2.SequenceEqual(list));
		}

		[TestMethod]
		public void TestMake()
		{
			var board = Bitboard.Bitboard_Make(0, 10, 12, 23, 27, 34, 36, 49, 54, 63);
			Assert.AreEqual(0x8042001408801401, board);
		}
		
	}
}

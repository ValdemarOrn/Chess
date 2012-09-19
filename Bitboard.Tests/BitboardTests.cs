using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MagicBitboard.Tests
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
		public void TestForwardBit()
		{
			ulong val = 0x0100100;
			var fwd = Bitboard.Bitboard_ForwardBit(val);
			Assert.AreEqual(8, fwd);

			val = 0x23000;
			fwd = Bitboard.Bitboard_ForwardBit(val);
			Assert.AreEqual(12, fwd);

			val = 0x2300000000000;
			fwd = Bitboard.Bitboard_ForwardBit(val);
			Assert.AreEqual(44, fwd);
		}

		[TestMethod]
		public void TestReverseBit()
		{
			ulong val = 0x0100100;
			var rwd = Bitboard.Bitboard_ReverseBit(val);
			Assert.AreEqual(20, rwd);

			val = 0x23000;
			rwd = Bitboard.Bitboard_ReverseBit(val);
			Assert.AreEqual(17, rwd);

			val = 0x2300000000000;
			rwd = Bitboard.Bitboard_ReverseBit(val);
			Assert.AreEqual(49, rwd);
		}

		[TestMethod]
		public void TestPopCount()
		{
			ulong val = 0xff010010010030;
			var cnt = Bitboard.Bitboard_PopCount(val);
			Assert.AreEqual(13, cnt);

			val = 0xffff000000000001;
			cnt = Bitboard.Bitboard_PopCount(val);
			Assert.AreEqual(17, cnt);
		}
	}
}

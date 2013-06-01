using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public unsafe class SEETest
	{
		[TestMethod]
		public void TestSEESquarePinned()
		{
			string fen = @"2kq4/8/2p5/3b3R/8/4N3/8/2R3K1 w KQkq - 0 1";
			var bb = Chess.Base.Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bb);

			var result = SEE.Square(b, 35);
			Assert.AreEqual(330, result);
		}

		[TestMethod]
		public void TestSEESquareFree()
		{
			string fen = @"1k1q4/8/2p5/3b3R/8/4N3/8/2R3K1 w - - 0 1";
			var bb = Chess.Base.Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bb);

			var result = SEE.Square(b, 35);
			Assert.AreEqual(10, result);
		}

		[TestMethod]
		public void TestSEESquareBadCapture()
		{
			string fen = @"1k1q4/8/2p5/3b3R/8/5Q2/8/2R3K1 w - - 0 1";
			var bb = Chess.Base.Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bb);

			var result = SEE.Square(b, 35);
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void TestSEECaptureBad1()
		{
			string fen = @"1k1q4/8/2p5/3b3R/8/5Q2/8/2R3K1 w - - 0 1";
			var bb = Chess.Base.Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bb);

			var result = SEE.Capture(b, 39, 35);
			Assert.AreEqual(330 - 500, result);
		}

		[TestMethod]
		public void TestSEECaptureBad2()
		{
			string fen = @"1k1q4/8/2p5/3b3R/8/5Q2/8/2R3K1 w - - 0 1";
			var bb = Chess.Base.Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bb);

			var result = SEE.Capture(b, 21, 35);
			Assert.AreEqual(330 - 900, result);
		}

		[TestMethod]
		public void TestSEECapturePinned()
		{
			string fen = @"2kq4/8/2p5/3b3R/8/5Q2/8/2R3K1 w - - 0 1";
			var bb = Chess.Base.Notation.ReadFEN(fen);
			var b = Helpers.ManagedBoardToNative(bb);

			var result = SEE.Capture(b, 21, 35);
			Assert.AreEqual(330, result);
		}
	}
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Lib.Tests
{
	[TestClass]
	public class HelperTest
	{
		[TestMethod]
		public unsafe void TestManagedBoardToNativeInit()
		{
			var board = new Chess.Base.Board(true);
			var b = Helpers.ManagedBoardToNative(board);

			var b2 = Board.Create();
			Board.Init(b2, 1);

			Assert.AreEqual(b2->Hash, b->Hash);
		}

		[TestMethod]
		public unsafe void TestManagedBoardToNativeFEN()
		{
			var board = Chess.Base.Notation.FENtoBoard("rnbqkb1r/pp2pppp/3p1n2/2p5/2B1P3/5N2/PPPP1PPP/RNBQ1RK1 b kq");
			var b = Helpers.ManagedBoardToNative(board);

			Assert.AreEqual((ulong)0x1000000000000040, b->Boards[Board.BOARD_KINGS]);
			Assert.AreEqual((ulong)0x8100000000000021, b->Boards[Board.BOARD_ROOKS]);
			Assert.AreEqual((ulong)0x200200000200002, b->Boards[Board.BOARD_KNIGHTS]);
			Assert.AreEqual(Board.CASTLE_BK | Board.CASTLE_BQ, b->Castle);
			Assert.AreEqual(Board.COLOR_BLACK, b->PlayerTurn);
		}
	}
}

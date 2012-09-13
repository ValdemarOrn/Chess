using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
    [TestClass]
    public class TestMovesKnight
    {
        [TestMethod]
        public void TestKnight1()
        {
            // test free space
            var b = new Board();
            byte pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(8, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos + 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 - 1)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
        }

        [TestMethod]
        public void TestKnightCapture()
        {
            var b = new Board();
            byte pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            b.State[pos - 16 + 1] = Pieces.Pawn | Chess.Colors.Black;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(8, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos + 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 - 1)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
        }

        [TestMethod]
        public void TestKnightCaptureSameColor()
        {
            var b = new Board();
            byte pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            b.State[pos - 16 + 1] = Pieces.Pawn | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(7, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos + 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 - 1)));
            //Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
        }

        [TestMethod]
        public void TestKnightNoMoves()
        {
            var b = new Board();
            byte pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            b.State[pos + 16 - 1] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos + 16 + 1] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos + 8 - 2] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos + 8 + 2] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos - 8 - 2] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos - 8 + 2] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos - 16 - 1] = Pieces.Pawn | Chess.Colors.White;
            b.State[pos - 16 + 1] = Pieces.Pawn | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(0, moves.Length);
        }

        [TestMethod]
        public void TestKnightKillAll()
        {
            var b = new Board();
            byte pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            b.State[pos + 16 - 1] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos + 16 + 1] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos + 8 - 2] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos + 8 + 2] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos - 8 - 2] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos - 8 + 2] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos - 16 - 1] = Pieces.Pawn | Chess.Colors.Black;
            b.State[pos - 16 + 1] = Pieces.Pawn | Chess.Colors.Black;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(8, moves.Length);
        }

        [TestMethod]
        public void TestKnightCorner1()
        {
            // upper left
            var b = new Board();
            byte pos = 7 * 8;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(2, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
        }

        [TestMethod]
        public void TestKnightCorner2()
        {
            // 7 + 1
            var b = new Board();
            byte pos = 6 * 8 + 1;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(4, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos - 8 + 2)));
            Assert.IsTrue(moves.Contains((byte)(pos - 16 + 1)));
            Assert.IsTrue(moves.Contains((byte)(pos + 10)));
            Assert.IsTrue(moves.Contains((byte)(pos - 17)));
        }

        [TestMethod]
        public void TestKnightCorner3()
        {
            // lower right
            var b = new Board();
            byte pos = 7;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(2, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
        }

        [TestMethod]
        public void TestKnightCorner4()
        {
            // 1 + 6
            var b = new Board();
            byte pos = 8 + 6;
            b.State[pos] = Pieces.Knight | Chess.Colors.White;
            var moves = Moves.GetMoves(b, pos);
			Assert.AreEqual(4, moves.Length);
            Assert.IsTrue(moves.Contains((byte)(pos + 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos + 16 - 1)));
            Assert.IsTrue(moves.Contains((byte)(pos - 8 - 2)));
            Assert.IsTrue(moves.Contains((byte)(pos + 16 + 1)));
        }
    }
}

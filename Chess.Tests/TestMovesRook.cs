﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
    [TestClass]
    public class TestMovesRook
    {
        [TestMethod]
        public void TestFree()
        {
            // test free space
            var b = new Board();
            int pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Rook | Colors.White;
            var moves = Moves.GetMoves(b, pos);
            Assert.AreEqual(14, moves.Length);
            Assert.IsTrue(moves.Contains(pos + 1));
            Assert.IsTrue(moves.Contains(pos + 2));
            Assert.IsTrue(moves.Contains(pos + 3));
            Assert.IsTrue(moves.Contains(pos - 1));
            Assert.IsTrue(moves.Contains(pos - 2));
            Assert.IsTrue(moves.Contains(pos - 3));
            Assert.IsTrue(moves.Contains(pos - 4));

            Assert.IsTrue(moves.Contains(pos + 8));
            Assert.IsTrue(moves.Contains(pos + 16));
            Assert.IsTrue(moves.Contains(pos + 24));
            Assert.IsTrue(moves.Contains(pos - 8));
            Assert.IsTrue(moves.Contains(pos - 16));
            Assert.IsTrue(moves.Contains(pos - 24));
            Assert.IsTrue(moves.Contains(pos - 32));
        }

        [TestMethod]
        public void TestSameColor()
        {
            var b = new Board();
            int pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Rook | Colors.White;
            b.State[pos + 2] = Pieces.Pawn | Colors.White;
            b.State[pos - 2] = Pieces.Pawn | Colors.White;
            b.State[pos + 16] = Pieces.Pawn | Colors.White;
            b.State[pos - 16] = Pieces.Pawn | Colors.White;

            var moves = Moves.GetMoves(b, pos);
            Assert.AreEqual(4, moves.Length);
            Assert.IsTrue(moves.Contains(pos + 1));
            Assert.IsTrue(moves.Contains(pos - 1));

            Assert.IsTrue(moves.Contains(pos + 8));
            Assert.IsTrue(moves.Contains(pos - 8));
        }

        [TestMethod]
        public void TestOppositeColor()
        {
            var b = new Board();
            int pos = 4 * 8 + 4;
            b.State[pos] = Pieces.Rook | Colors.White;
            b.State[pos + 2] = Pieces.Pawn | Colors.Black;
            b.State[pos - 2] = Pieces.Pawn | Colors.Black;
            b.State[pos + 16] = Pieces.Pawn | Colors.Black;
            b.State[pos - 16] = Pieces.Pawn | Colors.Black;

            var moves = Moves.GetMoves(b, pos);
            Assert.AreEqual(8, moves.Length);
            Assert.IsTrue(moves.Contains(pos + 1));
            Assert.IsTrue(moves.Contains(pos - 1));
            Assert.IsTrue(moves.Contains(pos + 2));
            Assert.IsTrue(moves.Contains(pos - 2));

            Assert.IsTrue(moves.Contains(pos + 8));
            Assert.IsTrue(moves.Contains(pos - 8));
            Assert.IsTrue(moves.Contains(pos + 16));
            Assert.IsTrue(moves.Contains(pos - 16));
        }

    
    }
}

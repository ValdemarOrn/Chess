using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
    [TestFixture]
    public class TestMovesRook
    {
        [Test]
        public void TestFree()
        {
            // test free space
            var b = new Board();
            int pos = 4 * 8 + 4;
            b.State[pos] = Colors.Val(Piece.Rook, Color.White);
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

        [Test]
        public void TestSameColor()
        {
            var b = new Board();
            int pos = 4 * 8 + 4;
            b.State[pos] = Colors.Val(Piece.Rook, Color.White);
            b.State[pos + 2] = Colors.Val(Piece.Pawn, Color.White);
            b.State[pos - 2] = Colors.Val(Piece.Pawn, Color.White);
            b.State[pos + 16] = Colors.Val(Piece.Pawn, Color.White);
            b.State[pos - 16] = Colors.Val(Piece.Pawn, Color.White);

            var moves = Moves.GetMoves(b, pos);
            Assert.AreEqual(4, moves.Length);
            Assert.IsTrue(moves.Contains(pos + 1));
            Assert.IsTrue(moves.Contains(pos - 1));

            Assert.IsTrue(moves.Contains(pos + 8));
            Assert.IsTrue(moves.Contains(pos - 8));
        }

        [Test]
        public void TestOppositeColor()
        {
            var b = new Board();
            int pos = 4 * 8 + 4;
            b.State[pos] = Colors.Val(Piece.Rook, Color.White);
            b.State[pos + 2] = Colors.Val(Piece.Pawn, Color.Black);
            b.State[pos - 2] = Colors.Val(Piece.Pawn, Color.Black);
            b.State[pos + 16] = Colors.Val(Piece.Pawn, Color.Black);
            b.State[pos - 16] = Colors.Val(Piece.Pawn, Color.Black);

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

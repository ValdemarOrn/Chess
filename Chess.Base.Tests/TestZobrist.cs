using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestZobrist
	{
		[Test]
		public void TestZobrist1()
		{
			var b = new Board(true);
			var zob = Zobrist.Calculate(b);

			b.Move(9, 25);
			var zob2 = Zobrist.Calculate(b);

			ulong expected = zob;
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.Pawn, 9);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.Pawn, 25);
			expected = expected ^ Zobrist.Key.PlayerTurn(Color.White);
			expected = expected ^ Zobrist.Key.PlayerTurn(Color.Black);
			expected = expected ^ Zobrist.Key.EnPassant(b.EnPassantTile);

			Assert.AreEqual(expected, zob2);
		}

		[Test]
		public void TestZobristCastle()
		{
			var b = Notation.ReadFEN("rnbqk2r/pppp1ppp/5n2/2b1p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 1");
			var zob = Zobrist.Calculate(b);

			b.Move(4, 6);
			var zob2 = Zobrist.Calculate(b);

			ulong expected = zob;
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.King, 4);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.King, 6);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.Rook, 7);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.Rook, 5);

			expected = expected ^ Zobrist.Key.PlayerTurn(Color.White);
			expected = expected ^ Zobrist.Key.PlayerTurn(Color.Black);

			expected = expected ^ Zobrist.Key.Castling(new HashSet<Castling>() { Castling.KingsideWhite, Castling.QueensideWhite, Castling.KingsideBlack, Castling.QueensideBlack });
			expected = expected ^ Zobrist.Key.Castling(new HashSet<Castling>() { Castling.KingsideBlack, Castling.QueensideBlack });

			Assert.AreEqual(expected, zob2);
		}

		[Test]
		public void TestZobristCastle2()
		{
			var b = Notation.ReadFEN("rnbqk2r/pppp1ppp/5n2/2b1p3/2B1P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 1");
			var zob = Zobrist.Calculate(b);

			b.Move(4, 6);
			var zob2 = Zobrist.Calculate(b);

			ulong expected = zob;
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.King, 4);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.King, 6);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.Rook, 7);
			expected = expected ^ Zobrist.Key.Piece(Color.White, Piece.Rook, 5);

			expected = expected ^ Zobrist.Key.PlayerTurn(Color.White);
			expected = expected ^ Zobrist.Key.PlayerTurn(Color.Black);

			expected = expected ^ Zobrist.Key.Castling(Castling.KingsideWhite, Castling.QueensideWhite, Castling.KingsideBlack, Castling.QueensideBlack);
			expected = expected ^ Zobrist.Key.Castling(Castling.KingsideBlack, Castling.QueensideBlack);

			Assert.AreEqual(expected, zob2);
		}
	}
}

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Tests
{
	[TestClass]
	public class TestMoves
	{
		[TestMethod]
		public void TestIsCastlingMove()
		{
			var b = new Board();
			b.InitBoard();
			Assert.IsTrue(b.CastleKingsideWhite);

			Assert.IsTrue(Moves.IsCastlingMove(b, 4, 6));
			Assert.IsTrue(Moves.IsCastlingMove(b, 4, 2));

			Assert.IsFalse(Moves.IsCastlingMove(b, 4, 5));
			Assert.IsFalse(Moves.IsCastlingMove(b, 4, 7));
			Assert.IsFalse(Moves.IsCastlingMove(b, 4, 3));
			Assert.IsFalse(Moves.IsCastlingMove(b, 4, 1));

			Assert.IsTrue(Moves.IsCastlingMove(b, 60, 62));
			Assert.IsTrue(Moves.IsCastlingMove(b, 60, 58));

			Assert.IsFalse(Moves.IsCastlingMove(b, 60, 61));
			Assert.IsFalse(Moves.IsCastlingMove(b, 60, 63));
			Assert.IsFalse(Moves.IsCastlingMove(b, 60, 69));
			Assert.IsFalse(Moves.IsCastlingMove(b, 60, 57));
		}

		[TestMethod]
		public void TestIsEnPassantLeftWhite()
		{
			var b = new Board();
			b.Turn = Colors.Black;

			int posWhite = Notation.TextToTile("e5");
			int posBlack = Notation.TextToTile("d7");

			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;
			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;

			b.Move(posBlack, posBlack - 16);

			bool isEnPassant = Moves.IsEnPassantMove(b, posWhite, posWhite + 7);
			Assert.IsTrue(isEnPassant);

			// test the other / wrong way - not allowed!
			isEnPassant = Moves.IsEnPassantMove(b, posWhite, posWhite + 9);
			Assert.IsFalse(isEnPassant);
		}

		[TestMethod]
		public void TestIsEnPassantRightWhite()
		{
			var b = new Board();
			b.Turn = Colors.Black;

			int posWhite = Notation.TextToTile("e5");
			int posBlack = Notation.TextToTile("f7");

			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;
			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;

			b.Move(posBlack, posBlack - 16);

			bool isEnPassant = Moves.IsEnPassantMove(b, posWhite, posWhite + 9);
			Assert.IsTrue(isEnPassant);

			// test the other / wrong way - not allowed!
			isEnPassant = Moves.IsEnPassantMove(b, posWhite, posWhite + 7);
			Assert.IsFalse(isEnPassant);
		}

		[TestMethod]
		public void TestIsEnPassantLeftBlack()
		{
			var b = new Board();
			b.Turn = Colors.White;

			int posBlack = Notation.TextToTile("e4");
			int posWhite = Notation.TextToTile("d2");

			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;
			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;

			b.Move(posWhite, posWhite + 16);

			bool isEnPassant = Moves.IsEnPassantMove(b, posBlack, posBlack - 9);
			Assert.IsTrue(isEnPassant);

			// test the other / wrong way - not allowed!
			isEnPassant = Moves.IsEnPassantMove(b, posBlack, posBlack - 7);
			Assert.IsFalse(isEnPassant);
		}

		[TestMethod]
		public void TestIsEnPassantRightBlack()
		{
			var b = new Board();
			b.Turn = Colors.White;

			int posBlack = Notation.TextToTile("e4");
			int posWhite = Notation.TextToTile("f2");

			b.State[posBlack] = Pieces.Pawn | Chess.Colors.Black;
			b.State[posWhite] = Pieces.Pawn | Chess.Colors.White;

			b.Move(posWhite, posWhite + 16);

			bool isEnPassant = Moves.IsEnPassantMove(b, posBlack, posBlack - 7);
			Assert.IsTrue(isEnPassant);

			// test the other / wrong way - not allowed!
			isEnPassant = Moves.IsEnPassantMove(b, posBlack, posBlack - 9);
			Assert.IsFalse(isEnPassant);
		}
	}
}

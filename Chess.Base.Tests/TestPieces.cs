using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Chess.Base.Tests
{
	[TestFixture]
	public class TestPieces
	{
		[Test]
		public void TestToString()
		{
			Assert.AreEqual("Bishop", Pieces.ToString(Piece.Bishop));
			Assert.AreEqual("King", Pieces.ToString(Piece.King));
			Assert.AreEqual("Knight", Pieces.ToString(Piece.Knight));
			Assert.AreEqual("Pawn", Pieces.ToString(Piece.Pawn));
			Assert.AreEqual("Queen", Pieces.ToString(Piece.Queen));
			Assert.AreEqual("Rook", Pieces.ToString(Piece.Rook));

			Assert.AreEqual("", Pieces.ToString((Piece)9865));
		}
	}
}

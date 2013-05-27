using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestPieces
	{
		[TestMethod]
		public void TestToString()
		{
			Assert.AreEqual("Bishop", Pieces.ToString(Pieces.Bishop));
			Assert.AreEqual("King", Pieces.ToString(Pieces.King));
			Assert.AreEqual("Knight", Pieces.ToString(Pieces.Knight));
			Assert.AreEqual("Pawn", Pieces.ToString(Pieces.Pawn));
			Assert.AreEqual("Queen", Pieces.ToString(Pieces.Queen));
			Assert.AreEqual("Rook", Pieces.ToString(Pieces.Rook));

			Assert.AreEqual("", Pieces.ToString(9865));
		}
	}
}

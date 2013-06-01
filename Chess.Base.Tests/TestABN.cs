using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chess.Base.Tests
{
	[TestClass]
	public class TestABN
	{
		[TestMethod]
		public void TestStripPGN1()
		{
			string data = System.IO.File.ReadAllText("..\\..\\..\\TestData\\BobbyFischer.pgn");
			string[] output = ABN.StripPGN(data);
			Assert.AreEqual(1, output.Length);
		}

		[TestMethod]
		public void TestStripPGNMany()
		{
			string data = System.IO.File.ReadAllText("..\\..\\..\\TestData\\HumansVsComputers.pgn");
			string[] output = ABN.StripPGN(data);
			Assert.AreEqual(28, output.Length);

			foreach(var game in output)
			{
				var moves = ABN.ABNToMoves(new Board(true), game);
			}
		}

		[TestMethod]
		public void TestABN1()
		{
			var board = new Board(true);
			string str = "1. e4 e5";
			var moves = ABN.ABNToMoves(board, str);
			Assert.AreEqual(1, moves[0].MoveCount);
			Assert.AreEqual(1, moves[1].MoveCount);
			Assert.AreEqual(12, moves[0].From);
			Assert.AreEqual(12+16, moves[0].To);
			Assert.AreEqual(52, moves[1].From);
			Assert.AreEqual(52-16, moves[1].To);
		}

		[TestMethod]
		public void TestABN2()
		{
			// Tests knights and bishops moving
			var board = new Board(true);
			string str = "1.e4 Nf6 2.Nc3 c6 3.Bc4 d5 ";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			var board2 = Notation.ReadFEN("rnbqkb1r/pp2pppp/2p2n2/3p4/2B1P3/2N5/PPPP1PPP/R1BQK1NR w KQkq");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
			Assert.AreEqual(Color.White, board.PlayerTurn);
		}

		[TestMethod]
		public void TestABN3()
		{
			// Tests castling white kingside
			var board = new Board(true);
			string str = "1.e4 e5 2.Bd3 Bd6 3.Nf3 Nf6 4.O-O ";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			var board2 = Notation.ReadFEN("rnbqk2r/pppp1ppp/3b1n2/4p3/4P3/3B1N2/PPPP1PPP/RNBQ1RK1 b kq");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
			Assert.AreEqual(Color.Black, board.PlayerTurn);
		}

		[TestMethod]
		public void TestABN4()
		{
			// Tests castling both kingside
			var board = new Board(true);
			string str = "1.e4 e5 2.Bd3 Bd6 3.Nf3 Nf6 4.O-O O-O ";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			var board2 = Notation.ReadFEN("rnbq1rk1/pppp1ppp/3b1n2/4p3/4P3/3B1N2/PPPP1PPP/RNBQ1RK1 w -");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
			Assert.AreEqual(Color.White, board.PlayerTurn);
		}

		[TestMethod]
		public void TestABN5()
		{
			// Tests castling both queenside
			var board = new Board(true);
			string str = "1.e4 c5 2.Nf3 d6 3.d4 Nf6 4.dxc5 Qa5 5.c3 Qxc5 6.Bd3 g6 7.Be3 Qc7 8.Na3 Be6 9.Qd2 Nc6 10.O-O-O O-O-O ";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			var board2 = Notation.ReadFEN("2kr1b1r/ppq1pp1p/2npbnp1/8/4P3/N1PBBN2/PP1Q1PPP/2KR3R w -");

			Assert.IsTrue(moves[6].Capture > 0);
			Assert.IsTrue(moves[9].Capture > 0);
			Assert.IsTrue(board2.State.SequenceEqual(board.State));
			Assert.AreEqual(Color.White, board.PlayerTurn);
		}

		[TestMethod]
		public void TestABNEnPassantWhite()
		{
			// Tests castling both queenside
			var board = new Board(true);
			string str = "1.c4 g6 2.c5 d5 3.cxd6 ";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			Assert.AreEqual(Colors.Val(Piece.Pawn, Color.Black), moves[4].Capture);
			Assert.AreEqual(moves[4].CaptureTile, 35);
			Assert.AreEqual(moves[4].To, 43);
		}

		[TestMethod]
		public void TestABNEnPassantBlack()
		{
			// Tests castling both queenside
			var board = new Board(true);
			string str = "1.h3 c5 2.h4 c4 3.d4 cxd3 ";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			Assert.AreEqual(Colors.Val(Piece.Pawn, Color.White), moves[5].Capture);
			Assert.AreEqual(moves[5].CaptureTile, 27);
			Assert.AreEqual(moves[5].To, 19);
		}

		[TestMethod]
		public void TestABNFullgame1_1()
		{
			// test almost an entire game (excl. promotion)
			var board = new Board(true);
			string str = "1.e4 c5 2.Nf3 g6 3.c3 Bg7 4.d4 cxd4 5.cxd4 d5 6.exd5 Nf6 7.Bb5+ Nbd7 8.d6 exd6 9.Qe2+ Qe7 10.Bf4 Qxe2+ 11.Kxe2 d5 12.Rc1 O-O 13.Nbd2 a6 14.Bd3 Re8+ 15.Kf1 Nf8 16.Be5 Ne6 17.g3 Bd7 18.Nb3 Ne4 19.Bxg7 Kxg7 20.Nc5 N4xc5 21.dxc5 a5 22.Rc2 Rec8 23.Rac1 Rc7 24.Ne5 Be8 25.c6 b6 26.Nd7 Bxd7 27.cxd7 Rxd7 28.Bb5 Rd6 29.Rc8 Rxc8 30.Rxc8 Kf6 31.h4 h5 32.Ke1 Nc5 33.Ke2 Re6+ 34.Kf3 Ke5 35.Rc7 Rf6+ 36.Ke2 Ne4 37.f4+ Kd4 38.Kf3 Nd6 39.Bd7 Nc4 40.b3 Nd2+ 41.Ke2 Ne4 42.Kf3 Nc5 43.Be8 Kc3 44.Rxf7 Re6 45.Rf8 d4 46.f5 gxf5 47.Bxh5 Re3+ 48.Kg2 Re5 49.Bf3 d3 50.h5 Re6 51.Rxf5 d2 52.Rd5 Nd3 53.Bg4 Re4 54.h6 Rxg4";
			var moves = ABN.ABNToMoves(board, str);
			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
			}

			var board2 = Notation.ReadFEN("8/8/1p5P/p2R4/6r1/1Pkn2P1/P2p2K1/8 w - - 0 55");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
		}

		[TestMethod]
		public void TestABNFullgame1_2()
		{
			// test promotion
			var board = new Board(true);
			string str = "1.e4 c5 2.Nf3 g6 3.c3 Bg7 4.d4 cxd4 5.cxd4 d5 6.exd5 Nf6 7.Bb5+ Nbd7 8.d6 exd6 9.Qe2+ Qe7 10.Bf4 Qxe2+ 11.Kxe2 d5 12.Rc1 O-O 13.Nbd2 a6 14.Bd3 Re8+ 15.Kf1 Nf8 16.Be5 Ne6 17.g3 Bd7 18.Nb3 Ne4 19.Bxg7 Kxg7 20.Nc5 N4xc5 21.dxc5 a5 22.Rc2 Rec8 23.Rac1 Rc7 24.Ne5 Be8 25.c6 b6 26.Nd7 Bxd7 27.cxd7 Rxd7 28.Bb5 Rd6 29.Rc8 Rxc8 30.Rxc8 Kf6 31.h4 h5 32.Ke1 Nc5 33.Ke2 Re6+ 34.Kf3 Ke5 35.Rc7 Rf6+ 36.Ke2 Ne4 37.f4+ Kd4 38.Kf3 Nd6 39.Bd7 Nc4 40.b3 Nd2+ 41.Ke2 Ne4 42.Kf3 Nc5 43.Be8 Kc3 44.Rxf7 Re6 45.Rf8 d4 46.f5 gxf5 47.Bxh5 Re3+ 48.Kg2 Re5 49.Bf3 d3 50.h5 Re6 51.Rxf5 d2 52.Rd5 Nd3 53.Bg4 Re4 54.h6 Rxg4";
			str += " 55.h7 d1Q 56.h8Q+ Kc2 ";
			var moves = ABN.ABNToMoves(board, str);

			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
				if (move.Promotion != 0)
				{
					bool promoted = board.Promote(move.To, move.Promotion);
					Assert.IsTrue(promoted);
				}
			}

			var board2 = Notation.ReadFEN("7Q/8/1p6/p2R4/6r1/1P1n2P1/P1k3K1/3q4 w - - 0 57");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
		}

		[TestMethod]
		public void TestABNFullgame1_3()
		{
			// end results
			var board = new Board(true);
			string str = "1.e4 c5 2.Nf3 g6 3.c3 Bg7 4.d4 cxd4 5.cxd4 d5 6.exd5 Nf6 7.Bb5+ Nbd7 8.d6 exd6 9.Qe2+ Qe7 10.Bf4 Qxe2+ 11.Kxe2 d5 12.Rc1 O-O 13.Nbd2 a6 14.Bd3 Re8+ 15.Kf1 Nf8 16.Be5 Ne6 17.g3 Bd7 18.Nb3 Ne4 19.Bxg7 Kxg7 20.Nc5 N4xc5 21.dxc5 a5 22.Rc2 Rec8 23.Rac1 Rc7 24.Ne5 Be8 25.c6 b6 26.Nd7 Bxd7 27.cxd7 Rxd7 28.Bb5 Rd6 29.Rc8 Rxc8 30.Rxc8 Kf6 31.h4 h5 32.Ke1 Nc5 33.Ke2 Re6+ 34.Kf3 Ke5 35.Rc7 Rf6+ 36.Ke2 Ne4 37.f4+ Kd4 38.Kf3 Nd6 39.Bd7 Nc4 40.b3 Nd2+ 41.Ke2 Ne4 42.Kf3 Nc5 43.Be8 Kc3 44.Rxf7 Re6 45.Rf8 d4 46.f5 gxf5 47.Bxh5 Re3+ 48.Kg2 Re5 49.Bf3 d3 50.h5 Re6 51.Rxf5 d2 52.Rd5 Nd3 53.Bg4 Re4 54.h6 Rxg4";
			str += " 55.h7 d1Q 56.h8Q+ Kc2 57.Qc8+ Kd2 58.Qf8 Qe2+ 59.Kh3 Re4 60.Qf6 Qg4+ 0-1";
			var moves = ABN.ABNToMoves(board, str);

			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
				if (move.Promotion != 0)
				{
					bool promoted = board.Promote(move.To, move.Promotion);
					Assert.IsTrue(promoted);
				}
			}

			var board2 = Notation.ReadFEN("8/8/1p3Q2/p2R4/4r1q1/1P1n2PK/P2k4/8 w - - 0 61");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
		}

		[TestMethod]
		public void TestABNFullgame2()
		{
			// end results
			var board = new Board(true);
			string str = "1.e4 e5 2.Nf3 Nf6 3.Nxe5 d6 4.Nf3 Nxe4 5.d4 d5 6.Bd3 Nc6 7.O-O Be7 8.c4 Nb4 9.Be2 O-O 10.Nc3 Bf5 11.a3 Nxc3 12.bxc3 Nc6 13.Re1 Re8 14.Bf4 dxc4 15.Bxc4 Bd6 16.Rxe8+ Qxe8 17.Ng5 Bg6 18.Bxd6 cxd6 19.h4 Qe7 20.Qg4 h6 21.Nf3 Qe4 22.Qg3 Rd8 23.Re1 Qf5 24.Ba2 Qf6 25.Nh2 Qf5 26.Ng4 Kf8 27.Bb1 Qh5 28.Qf4 Bxb1 29.Rxb1 Rd7 30.Ne3 Ne7 31.g3 Nd5 32.Nxd5 Qxd5 33.Re1 Re7 34.Rxe7 Kxe7 35.Qe3+ Qe6 1/2";

			var moves = ABN.ABNToMoves(board, str);

			foreach (var move in moves)
			{
				board.Move(move.From, move.To, true);
				if (move.Promotion != 0)
				{
					bool promoted = board.Promote(move.To, move.Promotion);
					Assert.IsTrue(promoted);
				}
			}

			var board2 = Notation.ReadFEN("8/pp2kpp1/3pq2p/8/3P3P/P1P1Q1P1/5P2/6K1 w - - 0 36");

			Assert.IsTrue(board2.State.SequenceEqual(board.State));
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public struct Move
	{
		public int From, To;

		public Move(int from, int to)
		{
			From = from;
			To = to;
		}
	}

	public sealed class Pieces
	{
		public const int Pawn = 1;
		public const int Rook = 2;
		public const int Bishop = 3;
		public const int Knight = 4;
		public const int Queen = 5;
		public const int King = 6;

		public static int Get(int piece)
		{
			return piece & 0x0F;
		}

		public static string ToString(int piece)
		{
			switch(piece)
			{
				case Pawn:
					return "Pawn";
				case Rook:
					return "Rook";
				case Bishop:
					return "Bishop";
				case Knight:
					return "Knight";
				case Queen:
					return "Queen";
				case King:
					return "King";
				default:
					return "";
			}
		}
	}

	public sealed class Colors
	{
		public const int White = 1 << 4;
		public const int Black = 2 << 4;

		public static int Get(int piece)
		{
			return piece & 0xF0;
		}

		public static string ToString(int color)
		{
			if (color == White)
				return "White";
			if (color == Black)
				return "Black";

			return "";
		}
	}

	public sealed class Board
	{
		public int[] State;
		public int Turn;

		public Move LastMove;

		// Todo: implement 50 move rule
		public int FiftyMoveRule;

		public bool CastleQueensideWhite;
		public bool CastleKingsideWhite;
		public bool CastleQueensideBlack;
		public bool CastleKingsideBlack;

		public Board(bool init = false)
		{
			State = new int[64];
			Turn = Colors.White;

			if (init)
				InitBoard();
		}

		public static int X(int square)
		{
			// 512 = 8*8*8 constant added to prevent negative modulo results (-9 % 8 = -1)
			return (512 + square) % 8;
		}

		public static int Y(int square)
		{
			return square / 8;
		}

		public int Color(int square)
		{
			return Chess.Colors.Get(State[square]);
		}

		public int Piece(int square)
		{
			return Chess.Pieces.Get(State[square]);
		}

		public int KingLocation(int kingColor)
		{
			var val = Pieces.King | kingColor;
			for (int i = 0; i < State.Length; i++)
			{
				if (State[i] == val)
					return i;
			}

			throw new Exception("No king of this color found on the board! this should never happen");
		}

		/// <summary>
		/// Create a copy of the board
		/// </summary>
		/// <returns></returns>
		public Board Copy()
		{
			var b = new Board();
			b.CastleQueensideBlack = this.CastleQueensideBlack;
			b.CastleQueensideWhite = this.CastleQueensideWhite;
			b.CastleKingsideBlack = this.CastleKingsideBlack;
			b.CastleKingsideWhite = this.CastleKingsideWhite;
			b.LastMove = this.LastMove;
			b.Turn = this.Turn;

			this.State.CopyTo(b.State, 0);

			return b;
		}

		/// <summary>
		/// Move a piece. Returns true if move was legal and allowed, false if it was prevented
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool Move(int from, int to, bool verifyLegalMove = false)
		{
			if (Color(from) != this.Turn)
				return false;

			if (verifyLegalMove)
			{
				bool causesSelfCheck = Check.MoveSelfChecks(this, from, to);
				if (causesSelfCheck)
					return false;
			}

			State[to] = State[from];
			State[from] = 0;

			LastMove = new Move(from, to);

			Turn = (Turn == Colors.White) ? Colors.Black : Colors.White;

			// check if castling is still allowed
			CheckCastling();

			return true;
		}

		/// <summary>
		/// Checks if rooks and kings are still in their starting positions
		/// and evaluates if castling is still possible
		/// </summary>
		public void CheckCastling()
		{
			bool whiteKing = State[4] == (Pieces.King | Colors.White);
			bool blackKing = State[8 * 7 + 4] == (Pieces.King | Colors.Black);

			bool whiteRookL = State[0] == (Pieces.Rook | Colors.White);
			bool whiteRookR = State[7] == (Pieces.Rook | Colors.White);

			bool blackRookL = State[8 * 7 + 0] == (Pieces.Rook | Colors.Black);
			bool blackRookR = State[8 * 7 + 7] == (Pieces.Rook | Colors.Black);

			CastleQueensideWhite = CastleQueensideWhite && whiteKing && whiteRookL;
			CastleKingsideWhite = CastleKingsideWhite && whiteKing && whiteRookR;
			CastleQueensideBlack = CastleQueensideBlack && blackKing && blackRookL;
			CastleKingsideBlack = CastleKingsideBlack && blackKing && blackRookR;
		}

		/// <summary>
		/// Place all pieces in starting positions. Set the Turn to white
		/// </summary>
		public void InitBoard()
		{
			for (int i = 0; i < State.Length; i++)
				State[i] = 0;

			Turn = Colors.White;

			State[1 * 8 + 0] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 1] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 2] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 3] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 4] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 5] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 6] = Colors.White | Pieces.Pawn;
			State[1 * 8 + 7] = Colors.White | Pieces.Pawn;

			State[6 * 8 + 0] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 1] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 2] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 3] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 4] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 5] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 6] = Colors.Black | Pieces.Pawn;
			State[6 * 8 + 7] = Colors.Black | Pieces.Pawn;

			State[0 * 8 + 0] = Colors.White | Pieces.Rook;
			State[0 * 8 + 1] = Colors.White | Pieces.Knight;
			State[0 * 8 + 2] = Colors.White | Pieces.Bishop;
			State[0 * 8 + 3] = Colors.White | Pieces.Queen;
			State[0 * 8 + 4] = Colors.White | Pieces.King;
			State[0 * 8 + 5] = Colors.White | Pieces.Bishop;
			State[0 * 8 + 6] = Colors.White | Pieces.Knight;
			State[0 * 8 + 7] = Colors.White | Pieces.Rook;

			State[7 * 8 + 0] = Colors.Black | Pieces.Rook;
			State[7 * 8 + 1] = Colors.Black | Pieces.Knight;
			State[7 * 8 + 2] = Colors.Black | Pieces.Bishop;
			State[7 * 8 + 3] = Colors.Black | Pieces.Queen;
			State[7 * 8 + 4] = Colors.Black | Pieces.King;
			State[7 * 8 + 5] = Colors.Black | Pieces.Bishop;
			State[7 * 8 + 6] = Colors.Black | Pieces.Knight;
			State[7 * 8 + 7] = Colors.Black | Pieces.Rook;
		}
	}
}

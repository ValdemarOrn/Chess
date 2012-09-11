using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public sealed class Board
	{
		public int[] State;
		public int PlayerTurn;
		public int MoveCount;

		public int EnPassantTile;

		/// <summary>
		/// Counts halfmoves since the last pawn move or capture. NOTE: this contains half moves so the
		/// number must reach 100 before the 50 move rule has been fulfilled
		/// </summary>
		public int FiftyMoveRulePlies;

		public bool CastleQueensideWhite;
		public bool CastleKingsideWhite;
		public bool CastleQueensideBlack;
		public bool CastleKingsideBlack;

		/// <summary>
		/// Creates a new board
		/// </summary>
		/// <param name="init">If set to true the board will be initialized to the standard chess starting position</param>
		public Board(bool init = false)
		{
			MoveCount = 1;
			State = new int[64];
			PlayerTurn = Colors.White;

			if (init)
				InitBoard();
		}

		/// <summary>
		/// Get the X coordinate of the tile. 0...7
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public static int X(int tile)
		{
			// 512 = 8*8*8 constant added to prevent negative modulo results (-9 % 8 = -1)
			return (512 + tile) % 8;
		}

		/// <summary>
		/// Get the Y coordinate of the tile. 0...7
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public static int Y(int tile)
		{
			return tile / 8;
		}

		/// <summary>
		/// Get the color of the piece at the specified square
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public int Color(int square)
		{
			return Chess.Colors.Get(State[square]);
		}

		/// <summary>
		/// Get the type of piece at the specified square
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
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

		public List<int> FindByColor(int color)
		{
			var output = new List<int>();

			for (int i = 0; i < State.Length; i++)
			{
				if (Color(i) == color)
					output.Add(i);
			}

			return output;
		}

		public List<int> FindByPiece(int piece)
		{
			var output = new List<int>();

			for (int i = 0; i < State.Length; i++)
			{
				if (Piece(i) == piece)
					output.Add(i);
			}

			return output;
		}

		public List<int> FindByPieceAndColor(int pieceAndColor)
		{
			var output = new List<int>();

			for (int i = 0; i < State.Length; i++)
			{
				if(State[i] == pieceAndColor)
					output.Add(i);
			}

			return output;
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
			b.EnPassantTile = this.EnPassantTile;
			b.FiftyMoveRulePlies = this.FiftyMoveRulePlies;
			b.MoveCount = this.MoveCount;
			b.PlayerTurn = this.PlayerTurn;
			
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
			if (Color(from) != this.PlayerTurn)
				return false;

			if (verifyLegalMove)
			{
				bool causesSelfCheck = Check.MoveSelfChecks(this, from, to);
				if (causesSelfCheck)
					return false;
			}

			// Move the rook if we are castling
			int castling = Moves.IsCastlingMove(this, from, to);
			switch(castling)
			{
				case 0:
					break;
				case Moves.CastleKingsideWhite:
					State[5] = State[7];
					State[7] = 0;
					break;
				case Moves.CastleQueensideWhite:
					State[3] = State[0];
					State[0] = 0;
					break;
				case Moves.CastleKingsideBlack:
					State[61] = State[63];
					State[63] = 0;
					break;
				case Moves.CastleQueensideBlack:
					State[59] = State[56];
					State[56] = 0;
					break;
			}

			// Remove the pawn if en passant attack
			bool enpassant = Moves.IsEnPassantMove(this, from, to);
			if (enpassant)
			{
				int victim = Moves.EnPassantVictim(this, from, to);
				State[victim] = 0;
			}

			// Log the move if this enables an en passant attack
			EnPassantTile = Moves.EnPassantTile(this, from, to);

			// Check if this move resets the 50 move position
			if (Piece(from) == Pieces.Pawn || Moves.IsCaptureMove(this, from, to))
				FiftyMoveRulePlies = 0;
			else
				FiftyMoveRulePlies++;

			// Perform the move
			State[to] = State[from];
			State[from] = 0;

			// Update the round if it was black's turn to play
			if (PlayerTurn == Colors.Black)
				MoveCount++;

			PlayerTurn = (PlayerTurn == Colors.White) ? Colors.Black : Colors.White;

			// check if castling is still allowed
			CheckCastling();

			return true;
		}

		/// <summary>
		/// Promotes a pawn at the edge of the board to a new piece
		/// </summary>
		/// <param name="square"></param>
		/// <param name="pieceType"></param>
		/// <returns></returns>
		public bool Promote(int square, int pieceType)
		{
			pieceType = Pieces.Get(pieceType);

			// Can only promote pawns
			if (Piece(square) != Pieces.Pawn)
				return false;

			// Can't promote to king or pawn
			if (pieceType != Pieces.Bishop && pieceType != Pieces.Knight && pieceType != Pieces.Queen && pieceType != Pieces.Rook)
				return false;
			
			int piece = State[square];
			int color = Color(square);

			if (Board.Y(square) == 7 && color == Colors.White)
			{
				State[square] = color | pieceType;
				return true;
			}

			if (Board.Y(square) == 0 && color == Colors.Black)
			{
				State[square] = color | pieceType;
				return true;
			}

			return false;
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
			MoveCount = 1;

			for (int i = 0; i < State.Length; i++)
				State[i] = 0;

			PlayerTurn = Colors.White;

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

			AllowCastlingAll();
		}

		/// <summary>
		/// Sets all castling to true, then runs CheckCastling() to verify it is legal
		/// </summary>
		public void AllowCastlingAll()
		{
			CastleKingsideBlack = true;
			CastleKingsideWhite = true;
			CastleQueensideBlack = true;
			CastleQueensideWhite = true;
			CheckCastling();
		}
	}
}

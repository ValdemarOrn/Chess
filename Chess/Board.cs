using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public sealed class Board
	{
		public int[] State { get; set; }
		public int PlayerTurn { get; set; }
		public int MoveCount { get; set; }

		public int EnPassantTile { get; set; }

		/// <summary>
		/// Counts halfmoves since the last pawn move or capture. NOTE: this contains half moves so the
		/// number must reach 100 before the 50 move rule has been fulfilled
		/// </summary>
		public int FiftyMoveRulePlies { get; set; }

		public bool CanCastleQWhite { get { return CastleQW == Moves.CanCastle; } set { CastleQW = value ? Moves.CanCastle : Moves.CannotCastle; } }
		public bool CanCastleKWhite { get { return CastleKW == Moves.CanCastle; } set { CastleKW = value ? Moves.CanCastle : Moves.CannotCastle; } }
		public bool CanCastleQBlack { get { return CastleQB == Moves.CanCastle; } set { CastleQB = value ? Moves.CanCastle : Moves.CannotCastle; } }
		public bool CanCastleKBlack { get { return CastleKB == Moves.CanCastle; } set { CastleKB = value ? Moves.CanCastle : Moves.CannotCastle; } }

		public int CastleQW { get; set; }
		public int CastleKW { get; set; }
		public int CastleQB { get; set; }
		public int CastleKB { get; set; }

		public Board() : this(false)
		{
			
		}

		/// <summary>
		/// Creates a new board
		/// </summary>
		/// <param name="init">If set to true the board will be initialized to the standard chess starting position</param>
		public Board(bool init)
		{
			MoveCount = 1;
			State = new int[64];
			PlayerTurn = Colors.White;

			if (init)
				InitBoard();
		}

		/// <summary>
		/// Create a copy of the board
		/// </summary>
		/// <returns></returns>
		public Board Copy()
		{
			var b = new Board();

			b.CastleQW = this.CastleQW;
			b.CastleKW = this.CastleKW;
			b.CastleQB = this.CastleQB;
			b.CastleKB = this.CastleKB;

			b.EnPassantTile = this.EnPassantTile;
			b.FiftyMoveRulePlies = this.FiftyMoveRulePlies;
			b.MoveCount = this.MoveCount;
			b.PlayerTurn = this.PlayerTurn;

			this.State.CopyTo(b.State, 0);

			return b;
		}

		#region Search board state
		
		/// <summary>
		/// Get the X coordinate of the tile. 0...7
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public static int X(int tile)
		{
			// 128 constant prevent negative numbers in modulu output
			return (128 + tile) % 8;
		}

		/// <summary>
		/// Get the Y coordinate of the tile. 0...7
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public static int Y(int tile)
		{
			return tile >> 3;
		}

		/// <summary>
		/// Get the color of the piece at the specified square
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public int Color(int square)
		{
			return State[square] & 0xF0;
		}

		/// <summary>
		/// Get the type of piece at the specified square
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public int Piece(int square)
		{
			return State[square] & 0x0F;
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

		#endregion

		public bool Move(int from, int to, bool verifyLegalMove = false)
		{
			Move m = new Move(from, to);
			bool complete = Move(ref m, verifyLegalMove);
			return complete;
		}

		/// <summary>
		/// Move a piece. Returns true if move was legal and allowed, false if it was prevented.
		/// Modifies the move structure and sets properties about capture, castling, color and move count.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool Move(ref Move move, bool verifyLegalMove = false)
		{
			if (Color(move.From) != this.PlayerTurn)
				return false;

			if (verifyLegalMove)
			{
				var validMoves = Moves.GetValidMoves(this, move.From);
				if(!validMoves.Contains(move.To))
					return false;
			}

			move.Color = this.PlayerTurn;
			move.MoveCount = this.MoveCount;

			// Move the rook if we are castling
			// Note: Only moves the ROOK, we still need to move the king like any other move.
			int castling = Moves.IsCastlingMove(this, move.From, move.To);
			switch(castling)
			{
				case 0:
					break;
				case Moves.CastleKingsideWhite:
					if (!CanCastleKWhite) return false;
					State[5] = State[7];
					State[7] = 0;
					CastleKW = Moves.HasCastled;
					move.Kingside = true;
					break;
				case Moves.CastleQueensideWhite:
					if (!CanCastleQWhite) return false;
					State[3] = State[0];
					State[0] = 0;
					CastleQW = Moves.HasCastled;
					move.Queenside = true;
					break;
				case Moves.CastleKingsideBlack:
					if (!CanCastleKBlack) return false;
					State[61] = State[63];
					State[63] = 0;
					CastleKB = Moves.HasCastled;
					move.Kingside = true;
					break;
				case Moves.CastleQueensideBlack:
					if (!CanCastleQBlack) return false;
					State[59] = State[56];
					State[56] = 0;
					CastleQB = Moves.HasCastled;
					move.Queenside = true;
					break;
			}

			// Remove the pawn if en passant attack
			bool enpassant = Moves.IsEnPassantCapture(this, move.From, move.To);
			if (enpassant)
			{

				int victim = Moves.EnPassantVictim(this, move.From, move.To);
				move.CaptureTile = victim;
				move.Capture = State[victim];
				State[victim] = 0;
			}
			else
			{
				move.Capture = State[move.To];
				move.CaptureTile = move.To;
			}

			// If this is a pawn move two squares out, it can be capture with an en passant attack.
			EnPassantTile = Moves.EnPassantTile(this, move.From, move.To);

			// Check if this move resets the 50 move position
			if (Piece(move.From) == Pieces.Pawn || Moves.IsCaptureMove(this, move.From, move.To))
				FiftyMoveRulePlies = 0;
			else
				FiftyMoveRulePlies++;

			// Perform the move
			State[move.To] = State[move.From];
			State[move.From] = 0;

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

			if (CastleQW != Moves.HasCastled)
				CastleQW = (CanCastleQWhite && whiteKing && whiteRookL) ? Moves.CanCastle : Moves.CannotCastle;

			if (CastleKW != Moves.HasCastled)
				CastleKW = (CanCastleKWhite && whiteKing && whiteRookR) ? Moves.CanCastle : Moves.CannotCastle;

			if (CastleQB != Moves.HasCastled)
				CastleQB = (CanCastleQBlack && blackKing && blackRookL) ? Moves.CanCastle : Moves.CannotCastle;

			if (CastleKB != Moves.HasCastled)
				CastleKB = (CanCastleKBlack && blackKing && blackRookR) ? Moves.CanCastle : Moves.CannotCastle;
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
			CastleKB = Moves.CanCastle;
			CastleKW = Moves.CanCastle;
			CastleQB = Moves.CanCastle;
			CastleQW = Moves.CanCastle;
			CheckCastling();
		}
	}
}

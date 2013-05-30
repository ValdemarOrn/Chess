using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public sealed class Board
	{
		public int[] State;
		public Color PlayerTurn;
		public int MoveCount;
		public HashSet<Castling> CastlingRights;
		public int EnPassantTile;

		/// <summary>
		/// Counts halfmoves since the last pawn move or capture. NOTE: this contains half moves so the
		/// number must reach 100 before the 50 move rule has been fulfilled
		/// </summary>
		public int FiftyMoveRulePlies;

		public bool CanCastleQWhite 
		{
			get { return CastlingRights.Contains(Castling.QueensideWhite); }
			set 
			{ 
				if (value) 
					CastlingRights.Add(Castling.QueensideWhite); 
				else 
					CastlingRights.Remove(Castling.QueensideWhite); 
			} 
		}

		public bool CanCastleKWhite 
		{
			get { return CastlingRights.Contains(Castling.KingsideWhite); }
			set
			{
				if (value)
					CastlingRights.Add(Castling.KingsideWhite);
				else
					CastlingRights.Remove(Castling.KingsideWhite);
			} 
		}

		public bool CanCastleQBlack 
		{
			get { return CastlingRights.Contains(Castling.QueensideBlack); }
			set
			{
				if (value)
					CastlingRights.Add(Castling.QueensideBlack);
				else
					CastlingRights.Remove(Castling.QueensideBlack);
			} 
		}

		public bool CanCastleKBlack 
		{
			get { return CastlingRights.Contains(Castling.KingsideBlack); }
			set
			{
				if (value)
					CastlingRights.Add(Castling.KingsideBlack);
				else
					CastlingRights.Remove(Castling.KingsideBlack);
			} 
		}

		public Board() : this(false)
		{
			CastlingRights = new HashSet<Castling>();
		}

		/// <summary>
		/// Creates a new board
		/// </summary>
		/// <param name="init">If set to true the board will be initialized to the standard chess starting position</param>
		public Board(bool init)
		{
			MoveCount = 1;
			State = new int[64];
			PlayerTurn = Color.White;
			CastlingRights = new HashSet<Castling>();

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
			b.CastlingRights     = new HashSet<Castling>(this.CastlingRights);
			b.EnPassantTile      = this.EnPassantTile;
			b.FiftyMoveRulePlies = this.FiftyMoveRulePlies;
			b.MoveCount          = this.MoveCount;
			b.PlayerTurn         = this.PlayerTurn;

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
		public Color GetColor(int square)
		{
			return (Color)(State[square] & 0xF0);
		}

		/// <summary>
		/// Get the type of piece at the specified square
		/// </summary>
		/// <param name="square"></param>
		/// <returns></returns>
		public int GetPiece(int square)
		{
			return State[square] & 0x0F;
		}

		public int KingLocation(Color kingColor)
		{
			var val = Pieces.King | (int)kingColor;
			for (int i = 0; i < State.Length; i++)
			{
				if (State[i] == val)
					return i;
			}

			throw new Exception("Invalid position. No king of this color found on the board!");
		}

		public List<int> FindByColor(Color color)
		{
			var output = new List<int>();

			for (int i = 0; i < State.Length; i++)
			{
				if (GetColor(i) == color)
					output.Add(i);
			}

			return output;
		}

		public List<int> FindByPiece(int piece)
		{
			var output = new List<int>();

			for (int i = 0; i < State.Length; i++)
			{
				if (GetPiece(i) == piece)
					output.Add(i);
			}

			return output;
		}

		public List<int> FindByPieceAndColor(int piece, Color color)
		{
			return FindByPieceAndColor(piece | (int)color);
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

		/// <summary>
		/// Move a piece. Returns true if move was legal and allowed, false if it was prevented.
		/// Modifies the move structure and sets properties about capture, castling, color and move count.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool Move(int from, int to, bool verifyLegalMove = false)
		{
			Move m = new Move(from, to);
			bool complete = Move(ref m, verifyLegalMove);
			return complete;
		}
		
		private bool Move(ref Move move, bool verifyLegalMove = false)
		{
			if (GetColor(move.From) != this.PlayerTurn)
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
			var castling = Moves.IsCastlingMove(this, move.From, move.To);
			switch(castling)
			{
				case 0:
					break;

				case Castling.KingsideWhite:
					if (!CanCastleKWhite) 
						return false;
					State[5] = State[7];
					State[7] = 0;
					move.Kingside = true;
					break;

				case Castling.QueensideWhite:
					if (!CanCastleQWhite) 
						return false;
					State[3] = State[0];
					State[0] = 0;
					move.Queenside = true;
					break;

				case Castling.KingsideBlack:
					if (!CanCastleKBlack) 
						return false;
					State[61] = State[63];
					State[63] = 0;
					move.Kingside = true;
					break;

				case Castling.QueensideBlack:
					if (!CanCastleQBlack) 
						return false;
					State[59] = State[56];
					State[56] = 0;
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
			if (GetPiece(move.From) == Pieces.Pawn || Moves.IsCaptureMove(this, move.From, move.To))
				FiftyMoveRulePlies = 0;
			else
				FiftyMoveRulePlies++;

			// Perform the move
			State[move.To] = State[move.From];
			State[move.From] = 0;

			// Update the round if it was black's turn to play
			if (PlayerTurn == Color.Black)
				MoveCount++;

			PlayerTurn = (PlayerTurn == Color.White) ? Color.Black : Color.White;

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
			if (GetPiece(square) != Pieces.Pawn)
				return false;

			// Can't promote to king or pawn
			if (pieceType != Pieces.Bishop && pieceType != Pieces.Knight && pieceType != Pieces.Queen && pieceType != Pieces.Rook)
				return false;
			
			int piece = State[square];
			Color color = GetColor(square);

			if (Board.Y(square) == 7 && color == Color.White)
			{
				State[square] = (int)color | (int)pieceType;
				return true;
			}

			if (Board.Y(square) == 0 && color == Color.Black)
			{
				State[square] = (int)color | (int)pieceType;
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
			bool whiteKing = State[4] == ((int)Pieces.King | (int)Color.White);
			bool blackKing = State[8 * 7 + 4] == ((int)Pieces.King | (int)Color.Black);

			bool whiteRookL = State[0] == ((int)Pieces.Rook | (int)Color.White);
			bool whiteRookR = State[7] == ((int)Pieces.Rook | (int)Color.White);

			bool blackRookL = State[8 * 7 + 0] == ((int)Pieces.Rook | (int)Color.Black);
			bool blackRookR = State[8 * 7 + 7] == ((int)Pieces.Rook | (int)Color.Black);

			if (!whiteKing || !whiteRookL)
				CastlingRights.Remove(Castling.QueensideWhite);

			if (!whiteKing || !whiteRookR)
				CastlingRights.Remove(Castling.KingsideWhite);

			if (!blackKing || !blackRookL)
				CastlingRights.Remove(Castling.QueensideBlack);

			if (!blackKing || !blackRookR)
				CastlingRights.Remove(Castling.KingsideBlack);
		}

		/// <summary>
		/// Place all pieces in starting positions. Set the Turn to white
		/// </summary>
		public void InitBoard()
		{
			MoveCount = 1;

			for (int i = 0; i < State.Length; i++)
				State[i] = 0;

			PlayerTurn = Color.White;

			State[1 * 8 + 0] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 1] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 2] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 3] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 4] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 5] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 6] = (int)Color.White | (int)Pieces.Pawn;
			State[1 * 8 + 7] = (int)Color.White | (int)Pieces.Pawn;

			State[6 * 8 + 0] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 1] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 2] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 3] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 4] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 5] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 6] = (int)Color.Black | (int)Pieces.Pawn;
			State[6 * 8 + 7] = (int)Color.Black | (int)Pieces.Pawn;

			State[0 * 8 + 0] = (int)Color.White | (int)Pieces.Rook;
			State[0 * 8 + 1] = (int)Color.White | (int)Pieces.Knight;
			State[0 * 8 + 2] = (int)Color.White | (int)Pieces.Bishop;
			State[0 * 8 + 3] = (int)Color.White | (int)Pieces.Queen;
			State[0 * 8 + 4] = (int)Color.White | (int)Pieces.King;
			State[0 * 8 + 5] = (int)Color.White | (int)Pieces.Bishop;
			State[0 * 8 + 6] = (int)Color.White | (int)Pieces.Knight;
			State[0 * 8 + 7] = (int)Color.White | (int)Pieces.Rook;

			State[7 * 8 + 0] = (int)Color.Black | (int)Pieces.Rook;
			State[7 * 8 + 1] = (int)Color.Black | (int)Pieces.Knight;
			State[7 * 8 + 2] = (int)Color.Black | (int)Pieces.Bishop;
			State[7 * 8 + 3] = (int)Color.Black | (int)Pieces.Queen;
			State[7 * 8 + 4] = (int)Color.Black | (int)Pieces.King;
			State[7 * 8 + 5] = (int)Color.Black | (int)Pieces.Bishop;
			State[7 * 8 + 6] = (int)Color.Black | (int)Pieces.Knight;
			State[7 * 8 + 7] = (int)Color.Black | (int)Pieces.Rook;

			AllowCastlingAll();
		}

		/// <summary>
		/// Sets all castling to true, then runs CheckCastling() to verify it is legal
		/// </summary>
		public void AllowCastlingAll()
		{
			CastlingRights.Add(Castling.KingsideBlack);
			CastlingRights.Add(Castling.KingsideWhite);
			CastlingRights.Add(Castling.QueensideBlack);
			CastlingRights.Add(Castling.QueensideWhite);
			CheckCastling();
		}
	}
}

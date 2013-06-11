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
		public int FiftyMoveRulePlies; // counts plies, so 100 plies = 50 moves

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

		public Board() : this(false) { }

		/// <summary>
		/// Creates a new board
		/// </summary>
		/// <param name="init">
		/// If set to true the board will be initialized to the standard chess starting position
		/// </param>
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
		public Piece GetPiece(int square)
		{
			return (Piece)(State[square] & 0x0F);
		}

		public int KingLocation(Color kingColor)
		{
			var val = Colors.Val(Piece.King, kingColor);
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

		public List<int> FindByPiece(Piece piece)
		{
			var output = new List<int>();

			for (int i = 0; i < State.Length; i++)
			{
				if (GetPiece(i) == piece)
					output.Add(i);
			}

			return output;
		}

		public List<int> FindByPieceAndColor(Piece piece, Color color)
		{
			int pieceAndColor = Colors.Val(piece, color);
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
			var m = new Move(from, to);
			bool complete = Move(m, verifyLegalMove);
			return complete;
		}
		
		public bool Move(Move move, bool verifyLegalMove = false)
		{
			if (GetColor(move.From) != this.PlayerTurn)
				return false;

			if (verifyLegalMove)
			{
				var validMoves = Moves.GetValidMoves(this, move.From);
				if(!validMoves.Contains(move.To))
					return false;
			}

			// Move the rook if we are castling
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
					break;

				case Castling.QueensideWhite:
					if (!CanCastleQWhite) 
						return false;
					State[3] = State[0];
					State[0] = 0;
					break;

				case Castling.KingsideBlack:
					if (!CanCastleKBlack) 
						return false;
					State[61] = State[63];
					State[63] = 0;
					break;

				case Castling.QueensideBlack:
					if (!CanCastleQBlack) 
						return false;
					State[59] = State[56];
					State[56] = 0;
					break;
			}

			// Remove the pawn if en passant attack
			bool enpassant = Moves.IsEnPassantCapture(this, move.From, move.To);
			if (enpassant)
			{

				int victim = Moves.EnPassantVictim(this, move.From, move.To);
//				move.CaptureTile = victim;
//				move.Capture = State[victim];
				State[victim] = 0;
			}
			else
			{
//				move.Capture = State[move.To];
//				move.CaptureTile = move.To;
			}

			// If this is a pawn move two squares out, it can be capture with an en passant attack.
			EnPassantTile = Moves.EnPassantTile(this, move.From, move.To);

			// Check if this move resets the 50 move position
			if (GetPiece(move.From) == Piece.Pawn || Moves.IsCaptureMove(this, move.From, move.To))
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
		/// <param name="pieceType">New piece type to promote to</param>
		/// <returns></returns>
		public bool Promote(int square, Piece pieceType)
		{
			// Can only promote pawns
			if (GetPiece(square) != Piece.Pawn)
				return false;

			// Can't promote to king or pawn
			if (pieceType != Piece.Bishop && pieceType != Piece.Knight && pieceType != Piece.Queen && pieceType != Piece.Rook)
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
			bool whiteKing = State[4] == ((int)Piece.King | (int)Color.White);
			bool blackKing = State[8 * 7 + 4] == ((int)Piece.King | (int)Color.Black);

			bool whiteRookL = State[0] == ((int)Piece.Rook | (int)Color.White);
			bool whiteRookR = State[7] == ((int)Piece.Rook | (int)Color.White);

			bool blackRookL = State[8 * 7 + 0] == ((int)Piece.Rook | (int)Color.Black);
			bool blackRookR = State[8 * 7 + 7] == ((int)Piece.Rook | (int)Color.Black);

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

			State[1 * 8 + 0] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 1] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 2] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 3] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 4] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 5] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 6] = (int)Color.White | (int)Piece.Pawn;
			State[1 * 8 + 7] = (int)Color.White | (int)Piece.Pawn;

			State[6 * 8 + 0] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 1] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 2] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 3] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 4] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 5] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 6] = (int)Color.Black | (int)Piece.Pawn;
			State[6 * 8 + 7] = (int)Color.Black | (int)Piece.Pawn;

			State[0 * 8 + 0] = (int)Color.White | (int)Piece.Rook;
			State[0 * 8 + 1] = (int)Color.White | (int)Piece.Knight;
			State[0 * 8 + 2] = (int)Color.White | (int)Piece.Bishop;
			State[0 * 8 + 3] = (int)Color.White | (int)Piece.Queen;
			State[0 * 8 + 4] = (int)Color.White | (int)Piece.King;
			State[0 * 8 + 5] = (int)Color.White | (int)Piece.Bishop;
			State[0 * 8 + 6] = (int)Color.White | (int)Piece.Knight;
			State[0 * 8 + 7] = (int)Color.White | (int)Piece.Rook;

			State[7 * 8 + 0] = (int)Color.Black | (int)Piece.Rook;
			State[7 * 8 + 1] = (int)Color.Black | (int)Piece.Knight;
			State[7 * 8 + 2] = (int)Color.Black | (int)Piece.Bishop;
			State[7 * 8 + 3] = (int)Color.Black | (int)Piece.Queen;
			State[7 * 8 + 4] = (int)Color.Black | (int)Piece.King;
			State[7 * 8 + 5] = (int)Color.Black | (int)Piece.Bishop;
			State[7 * 8 + 6] = (int)Color.Black | (int)Piece.Knight;
			State[7 * 8 + 7] = (int)Color.Black | (int)Piece.Rook;

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

		/// <summary>
		/// Evaluates how many pieces can attack each tile.
		/// Returns an array of 64 ints, each number represents the number of attackers
		/// for that square
		/// </summary>
		/// <param name="board"></param>
		/// <param name="attackerColor"></param>
		/// <returns></returns>
		public int[] GetAttackBoard(Color attackerColor)
		{
			Board board = this;

			Color color = attackerColor;
			var attackBoard = new int[64];

			// Search for all pieces of the attacker's color
			for (int i = 0; i < 64; i++)
			{
				if (board.GetColor(i) != color)
					continue;

				// find all the moves this piece can make
				var moves = Attacks.GetAttacks(board, i);

				// mark them on the attack board
				foreach (int move in moves)
					attackBoard[move]++;
			}

			return attackBoard;
		}

		/// <summary>
		/// Checks if the king of the specified color is in check
		/// </summary>
		/// <param name="board"></param>
		/// <param name="kingColor"></param>
		/// <returns></returns>
		public bool IsChecked(Color kingColor)
		{
			Board board = this;

			int kingLocation = board.KingLocation(kingColor);
			var attackerColor = (kingColor == Color.White) ? Color.Black : Color.White;
			var attacks = board.GetAttackBoard(attackerColor);

			// king is not in check
			if (attacks[kingLocation] == 0)
				return false;
			else
				return true;
		}

		/// <summary>
		/// Check if there is a checkmate
		/// </summary>
		/// <param name="board"></param>
		/// <param name="kingColor"></param>
		/// <returns></returns>
		public bool IsCheckMate()
		{
			Color kingColor = this.PlayerTurn;

			var attackerColor = (kingColor == Color.White) ? Color.Black : Color.White;
			var attacks = this.GetAttackBoard(attackerColor);
			var kingLocation = this.KingLocation(kingColor);

			// king is not in check
			if (attacks[kingLocation] == 0)
				return false;

			// Try all possible moves for the defenders pieces
			// If there are any valid moves, then the defender can break the check.
			for (int i = 0; i < 64; i++)
			{
				if (this.GetColor(i) != kingColor)
					continue;

				var moves = Moves.GetValidMoves(this, i);
				if (moves.Length > 0)
					return false;
			}

			// King could not get out of check, he has been check mated
			return true;
		}

		/// <summary>
		/// Checks for stale mate, where there are no valid moves
		/// </summary>
		/// <param name="board"></param>
		/// <param name="kingColor"></param>
		/// <returns></returns>
		public bool IsStalemate()
		{
			Color kingColor = this.PlayerTurn;

			// It's not a stalemate if he's in check
			if (IsChecked(kingColor))
				return false;

			// Try all possible moves for the defenders pieces
			// If there are any valid moves, then the defender is not in a stalemate
			// Note: pawns are the only pieces that can be blocked from moving, aside from king
			for (int i = 0; i < 64; i++)
			{
				if (this.GetColor(i) != kingColor)
					continue;

				var moves = Moves.GetValidMoves(this, i);
				if (moves.Length > 0)
					return false;
			}

			// King could not get out of check, he has been check mated
			return true;
		}

		/// <summary>
		/// Figures out if the specified move puts or keeps your own king in check, which is illegal
		/// </summary>
		/// <param name="board"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool MoveSelfChecks(int from, int to)
		{
			Board board = this;
			Color colorToMove = board.GetColor(from);

			board = board.Copy();
			board.Move(from, to, false);
			bool ok = board.IsChecked(colorToMove);
			return ok;
		}

		/// <summary>
		/// Checks if a rook at the specified square still has the ability to castle
		/// </summary>
		/// <param name="board"></param>
		/// <param name="square"></param>
		/// <returns></returns>
		public bool CanRookCastle(int square)
		{
			Board board = this;

			switch (square)
			{
				case (0):
					return board.CanCastleQWhite;
				case (7):
					return board.CanCastleKWhite;
				case (56):
					return board.CanCastleQBlack;
				case (63):
					return board.CanCastleKBlack;
				default:
					return false;
			}
		}

		/// <summary>
		/// Checks if a piece on the board can be promoted
		/// </summary>
		/// <param name="square"></param>
		/// <param name="pieceType"></param>
		/// <returns></returns>
		public bool CanPromote(int square)
		{
			Board board = this;

			// Can only promote pawns
			if (board.GetPiece(square) != Piece.Pawn)
				return false;

			Color color = board.GetColor(square);

			if (Board.Y(square) == 7 && color == Color.White)
				return true;

			if (Board.Y(square) == 0 && color == Color.Black)
				return true;

			return false;
		}

		public void LoadFEN(string fenString)
		{
			var b = Notation.ReadFEN(fenString);
			
			this.State = b.State;
			this.PlayerTurn = b.PlayerTurn;

			this.CastlingRights = b.CastlingRights;
			this.EnPassantTile = b.EnPassantTile;
			this.FiftyMoveRulePlies = b.FiftyMoveRulePlies;
			this.MoveCount = b.MoveCount;
		}

		public string ToFEN()
		{
			return "";
		}
	}
}

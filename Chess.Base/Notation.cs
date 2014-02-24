using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public class Notation
	{
		public const string FenStartingPosition = @"rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

		public static int FileToInt(char file)
		{
			file = Char.ToLower(file);
			int x = -1;

			switch (file)
			{
				case 'a':
					x = 0; break;
				case 'b':
					x = 1; break;
				case 'c':
					x = 2; break;
				case 'd':
					x = 3; break;
				case 'e':
					x = 4; break;
				case 'f':
					x = 5; break;
				case 'g':
					x = 6; break;
				case 'h':
					x = 7; break;
				default:
					throw new ArgumentException();
			}
			return x;
		}

		public static int RankToInt(char file)
		{
			file = Char.ToLower(file);
			int x = -1;

			switch (file)
			{
				case '1':
					x = 0; break;
				case '2':
					x = 1; break;
				case '3':
					x = 2; break;
				case '4':
					x = 3; break;
				case '5':
					x = 4; break;
				case '6':
					x = 5; break;
				case '7':
					x = 6; break;
				case '8':
					x = 7; break;
				default:
					throw new ArgumentException();
			}
			return x;
		}

		public static string TileToText(int tile)
		{
			if (tile < 0 || tile >= 64)
				return "";

			int x = Board.X(tile);
			int y = Board.Y(tile);

			y++;
			char xx = (char)('a' + x);
			return xx.ToString() + y;
		}

		/// <summary>
		/// Convert algebraic notation into a tile number
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static int TextToTile(string text)
		{
			text = text.Trim().ToLower();

			if (text.Length != 2) 
				throw new Exception("Unable to parse tile number " + text);

			int x = 0;
			int y = 0;

			char file = text[0];
			switch(file)
			{
				case 'a':
					x = 0; break;
				case 'b':
					x = 1; break;
				case 'c':
					x = 2; break;
				case 'd':
					x = 3; break;
				case 'e':
					x = 4; break;
				case 'f':
					x = 5; break;
				case 'g':
					x = 6; break;
				case 'h':
					x = 7; break;
				default:
					throw new Exception("Unable to parse tile number " + text);
			}

			y = Convert.ToInt32(text[1].ToString()) - 1;
			
			if(y < 0 || y > 7)
				throw new Exception("Unable to parse tile number " + text);

			return y * 8 + x;
		}

		/// <summary>
		/// Returns the Piece type that is being promoted to, or Piece.None if there
		/// is no promotion
		/// Example: a8=Q returns Piece.Queen
		/// </summary>
		/// <param name="moveString"></param>
		/// <returns></returns>
		public static Piece GetPromotion(string moveString)
		{
			moveString = moveString.Replace("+", "").Replace("#", "");
			var lastChar = moveString[moveString.Length - 1];

			if (lastChar == 'N')
				return Piece.Knight;
			if (lastChar == 'B')
				return Piece.Bishop;
			if (lastChar == 'R')
				return Piece.Rook;
			if (lastChar == 'Q')
				return Piece.Queen;

			return Piece.None;
		}

		/// <summary>
		/// Returns the type of piece that is moving.
		/// Example: Rb2b4 returns Piece.Rook
		/// Example: b4 returns Piece.Pawn
		/// </summary>
		/// <param name="moveString"></param>
		/// <returns></returns>
		public static Piece GetPiece(string moveString)
		{
			var p = moveString[0];

			switch (p)
			{
				case 'N':
					return Piece.Knight;
				case 'B':
					return Piece.Bishop;
				case 'K':
					return Piece.King;
				case 'Q':
					return Piece.Queen;
				case 'R':
					return Piece.Rook;
				default:
					return Piece.Pawn;
			}
		}

		public static string GetPieceLetter(Piece piece)
		{
			switch (piece)
			{
				case Piece.Knight:
					return "N";
				case Piece.Bishop:
					return "B";
				case Piece.King:
					return "K";
				case Piece.Queen:
					return "Q";
				case Piece.Rook:
					return "R";
				default:
					return "";
			}
		}

		/// <summary>
		/// Convert a FEN string into a Board object
		/// </summary>
		/// <param name="fenString"></param>
		/// <returns></returns>
		public static Board ReadFEN(string fenString)
		{
			string[] parts = fenString.Split(' ');
			if (parts.Length < 3)
				throw new Exception("Malformed FEN string, missing parts detected");

			string tiles = parts[0];
			string turn = parts[1];
			string castle = parts[2];

			Board b = new Board();
			int strpos = 0;

			// process board
			int y = 7;
			int x = 0;
			foreach (char c in tiles)
			{
				int tile = y * 8 + x;

				if (c == '/') // ------------ Parse newline ------------
				{
					if (x != 8)
						throw new Exception("Malformed FEN string. Error at character " + strpos);
					y--;
					x = 0;
				}
				else if (c >= '0' && c <= '9') // ------------ Parse number ------------
				{
					int count = Convert.ToInt32(c.ToString());
					x = x + count;
					if (x > 8)
						throw new Exception("Malformed FEN string. Error at character " + strpos);
				}
				else  // ------------ Parse piece ------------
				{
					switch (c)
					{
						case 'p':
							b.State[tile] = (int)Piece.Pawn | (int)Color.Black; break;
						case 'r':
							b.State[tile] = (int)Piece.Rook | (int)Color.Black; break;
						case 'n':
							b.State[tile] = (int)Piece.Knight | (int)Color.Black; break;
						case 'b':
							b.State[tile] = (int)Piece.Bishop | (int)Color.Black; break;
						case 'q':
							b.State[tile] = (int)Piece.Queen | (int)Color.Black; break;
						case 'k':
							b.State[tile] = (int)Piece.King | (int)Color.Black; break;

						case 'P':
							b.State[tile] = (int)Piece.Pawn | (int)Color.White; break;
						case 'R':
							b.State[tile] = (int)Piece.Rook | (int)Color.White; break;
						case 'N':
							b.State[tile] = (int)Piece.Knight | (int)Color.White; break;
						case 'B':
							b.State[tile] = (int)Piece.Bishop | (int)Color.White; break;
						case 'Q':
							b.State[tile] = (int)Piece.Queen | (int)Color.White; break;
						case 'K':
							b.State[tile] = (int)Piece.King | (int)Color.White; break;

						default:
							throw new Exception("Malformed FEN string. Error at character " + strpos);
					}
					x++;
				}

				strpos++;
			}

			// Process whose turn
			if (turn.ToLower().Contains('w'))
				b.PlayerTurn = Color.White;
			else if (turn.ToLower().Contains('b'))
				b.PlayerTurn = Color.Black;
			else
				throw new Exception("Malformed FEN string. Turn color not recognized");

			// Process Castling
			if (castle.Contains('K'))
				b.CastlingRights.Add(Castling.KingsideWhite);
			if (castle.Contains('Q'))
				b.CastlingRights.Add(Castling.QueensideWhite);
			if (castle.Contains('k'))
				b.CastlingRights.Add(Castling.KingsideBlack);
			if (castle.Contains('q'))
				b.CastlingRights.Add(Castling.QueensideBlack);

			// Process en passant
			if (parts.Length >= 4)
			{
				string enp = parts[3].ToLower();
				if (enp.Contains('-'))
					b.EnPassantTile = 0;
				if (enp.Contains('3')) // white moved
				{
					int tile = Notation.TextToTile(enp);

					if(b.PlayerTurn == Color.White)
						throw new Exception("Malformed FEN string. En passant shows white moved last, but it's also his turn");

					if (b.State[tile + 8] != ((int)Piece.Pawn | (int)Color.White))
						throw new Exception("Malformed FEN string. En passant expected white pawn at tile " + Notation.TileToText(tile + 8));

					b.EnPassantTile = tile;
				}
				if (enp.Contains('6')) // black moved
				{
					int tile = Notation.TextToTile(enp);

					if (b.PlayerTurn == Color.Black)
						throw new Exception("Malformed FEN string. En passant shows black moved last, but it's also his turn");

					if (b.State[tile - 8] != ((int)Piece.Pawn | (int)Color.Black))
						throw new Exception("Malformed FEN string. En passant expected black pawn at tile " + Notation.TileToText(tile + 8));

					b.EnPassantTile = tile;
				}
			}

			if (parts.Length >= 5)
			{
				int halfmoves = -1;
				Int32.TryParse(parts[4], out halfmoves);
				if (halfmoves == -1)
					throw new Exception("Malformed FEN string. Half move count is not a valid integer");

				b.FiftyMoveRulePlies = halfmoves;
			}

			if (parts.Length >= 6)
			{
				int fullmove = -1;
				Int32.TryParse(parts[5], out fullmove);
				if (fullmove == -1)
					throw new Exception("Malformed FEN string. Fullmove count is not a valid integer");

				b.MoveCount = fullmove;
			}

			return b;
		}

		/// <summary>
		/// Create a FEN string that represents the state of the board
		/// </summary>
		/// <param name="board"></param>
		/// <returns></returns>
		public static string BoardToFEN(Board board)
		{
			Dictionary<int, char> FENPieces = new Dictionary<int,char>();
			FENPieces[Colors.Val(Color.White, Piece.Pawn)] = 'P';
			FENPieces[Colors.Val(Color.White, Piece.Knight)] = 'N';
			FENPieces[Colors.Val(Color.White, Piece.Bishop)] = 'B';
			FENPieces[Colors.Val(Color.White, Piece.Rook)] = 'R';
			FENPieces[Colors.Val(Color.White, Piece.Queen)] = 'Q';
			FENPieces[Colors.Val(Color.White, Piece.King)] = 'K';
			FENPieces[Colors.Val(Color.Black, Piece.Pawn)] = 'p';
			FENPieces[Colors.Val(Color.Black, Piece.Knight)] = 'n';
			FENPieces[Colors.Val(Color.Black, Piece.Bishop)] = 'b';
			FENPieces[Colors.Val(Color.Black, Piece.Rook)] = 'r';
			FENPieces[Colors.Val(Color.Black, Piece.Queen)] = 'q';
			FENPieces[Colors.Val(Color.Black, Piece.King)] = 'k';

			List<char> str = new List<char>();

			int empties = 0;
			for(int i = 0; i < 64; i++)
			{
				int rank = 7 - (i / 8);
				int x = i % 8;
				int idx = rank * 8 + x;

				if(i % 8 == 0 && i > 0)
				{
					if(empties > 0)
						str.Add((char)('0' + empties));

					str.Add('/');
					empties = 0;
				}

				if(board.State[idx] > 0)
				{
					if(empties > 0)
						str.Add((char)('0' + empties));

					empties = 0;
					str.Add(FENPieces[board.State[idx]]);
				}
				else
					empties++;
			}

			// add the final empties to the string
			if(empties > 0)
				str.Add((char)('0' + empties));

			str.Add(' ');

			// player turn
			str.Add((board.PlayerTurn == Color.White) ? 'w' : 'b');
			str.Add(' ');

			// castling rights
			if(board.CastlingRights.Count == 0)
			{
				str.Add('-');
			}
			else
			{
				if(board.CanCastleKWhite)
					str.Add('K');
				if(board.CanCastleQWhite)
					str.Add('Q');
				if(board.CanCastleKBlack)
					str.Add('k');
				if(board.CanCastleQBlack)
					str.Add('q');
			}
			str.Add(' ');

			// en passant
			if(board.EnPassantTile == 0)
			{
				str.Add('-');
			}
			else
			{
				string text = TileToText(board.EnPassantTile);
				str.Add(text[0]);
				str.Add(text[1]);
			}
			str.Add(' ');

			// half move count
			string moves = board.FiftyMoveRulePlies.ToString();

			str.AddRange(moves);

			str.Add(' ');

			// full move count
			moves = board.MoveCount.ToString();
			str.AddRange(moves);

			var output = new String(str.ToArray());
			return output;
		}
	}
}

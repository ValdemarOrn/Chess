﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base
{
	public class Notation
	{
		/// <summary>
		/// Get the algebraic notation for the tile, e.g. a1, e4, h3
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
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

			if(text.Length != 2)
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
		/*
		public static char File(int tile)
		{
			if (tile < 0 || tile >= 64)
				return (char)0;

			int x = Board.X(tile);
			char xx = (char)('a' + x);
			return xx;
		}

		public static int Rank(int tile)
		{
			if (tile < 0 || tile >= 64)
				return (char)0;

			int y = Board.Y(tile);

			y++;
			return y;
		}
		*/
		/// <summary>
		/// Convert a FEN string into a Board object
		/// </summary>
		/// <param name="fenString"></param>
		/// <returns></returns>
		public static Board FENtoBoard(string fenString)
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
				int halfmoves = Convert.ToInt32(parts[4]);
				b.FiftyMoveRulePlies = halfmoves;
			}

			if (parts.Length >= 6)
			{
				int round = Convert.ToInt32(parts[5]);
				b.MoveCount = round;
			}

			return b;
		}

		
	}
}

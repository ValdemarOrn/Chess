using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess
{
	public class Data
	{
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
							b.State[tile] = Pieces.Pawn | Colors.Black; break;
						case 'r':
							b.State[tile] = Pieces.Rook | Colors.Black; break;
						case 'n':
							b.State[tile] = Pieces.Knight | Colors.Black; break;
						case 'b':
							b.State[tile] = Pieces.Bishop | Colors.Black; break;
						case 'q':
							b.State[tile] = Pieces.Queen | Colors.Black; break;
						case 'k':
							b.State[tile] = Pieces.King | Colors.Black; break;

						case 'P':
							b.State[tile] = Pieces.Pawn | Colors.White; break;
						case 'R':
							b.State[tile] = Pieces.Rook | Colors.White; break;
						case 'N':
							b.State[tile] = Pieces.Knight | Colors.White; break;
						case 'B':
							b.State[tile] = Pieces.Bishop | Colors.White; break;
						case 'Q':
							b.State[tile] = Pieces.Queen | Colors.White; break;
						case 'K':
							b.State[tile] = Pieces.King | Colors.White; break;

						default:
							throw new Exception("Malformed FEN string. Error at character " + strpos);
					}
					x++;
				}

				strpos++;
			}

			// Process whose turn
			if (turn.ToLower().Contains('w'))
				b.Turn = Colors.White;
			else if (turn.ToLower().Contains('b'))
				b.Turn = Colors.Black;
			else
				throw new Exception("Malformed FEN string. Turn color not recognized");

			// Process Castling
			if (castle.Contains('K'))
				b.CastleKingsideWhite = true;
			if (castle.Contains('Q'))
				b.CastleQueensideWhite = true;
			if (castle.Contains('k'))
				b.CastleKingsideBlack = true;
			if (castle.Contains('q'))
				b.CastleQueensideBlack = true;

			// Todo: Process en passant

			// Todo: Process move counters

			return b;
		}
	}
}

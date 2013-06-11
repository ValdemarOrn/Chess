using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Base.PGN
{
	public class GameEncoder
	{
		public static char[] Encode;
		public static int[] Decode;
		public static int SpecialInstruction = 64;
		public static char SpecialInstructionChar = '*';

		static GameEncoder()
		{
			Encode = new char[65];
			Decode = new int[128];

			Encode[0] = 'A';
			Encode[1] = 'B';
			Encode[2] = 'C';
			Encode[3] = 'D';
			Encode[4] = 'E';
			Encode[5] = 'F';
			Encode[6] = 'G';
			Encode[7] = 'H';
			Encode[8] = 'I';
			Encode[9] = 'J';
			Encode[10] = 'K';
			Encode[11] = 'L';
			Encode[12] = 'M';
			Encode[13] = 'N';
			Encode[14] = 'O';
			Encode[15] = 'P';
			Encode[16] = 'Q';
			Encode[17] = 'R';
			Encode[18] = 'S';
			Encode[19] = 'T';
			Encode[20] = 'U';
			Encode[21] = 'V';
			Encode[22] = 'W';
			Encode[23] = 'X';
			Encode[24] = 'Y';
			Encode[25] = 'Z';
			Encode[26] = 'a';
			Encode[27] = 'b';
			Encode[28] = 'c';
			Encode[29] = 'd';
			Encode[30] = 'e';
			Encode[31] = 'f';
			Encode[32] = 'g';
			Encode[33] = 'h';
			Encode[34] = 'i';
			Encode[35] = 'j';
			Encode[36] = 'k';
			Encode[37] = 'l';
			Encode[38] = 'm';
			Encode[39] = 'n';
			Encode[40] = 'o';
			Encode[41] = 'p';
			Encode[42] = 'q';
			Encode[43] = 'r';
			Encode[44] = 's';
			Encode[45] = 't';
			Encode[46] = 'u';
			Encode[47] = 'v';
			Encode[48] = 'w';
			Encode[49] = 'x';
			Encode[50] = 'y';
			Encode[51] = 'z';
			Encode[52] = '0';
			Encode[53] = '1';
			Encode[54] = '2';
			Encode[55] = '3';
			Encode[56] = '4';
			Encode[57] = '5';
			Encode[58] = '6';
			Encode[59] = '7';
			Encode[60] = '8';
			Encode[61] = '9';
			Encode[62] = '+';
			Encode[63] = '/';
			Encode[64] = '*'; // denotes promotion

			Decode[(int)'A'] = 0;
			Decode[(int)'B'] = 1;
			Decode[(int)'C'] = 2;
			Decode[(int)'D'] = 3;
			Decode[(int)'E'] = 4;
			Decode[(int)'F'] = 5;
			Decode[(int)'G'] = 6;
			Decode[(int)'H'] = 7;
			Decode[(int)'I'] = 8;
			Decode[(int)'J'] = 9;
			Decode[(int)'K'] = 10;
			Decode[(int)'L'] = 11;
			Decode[(int)'M'] = 12;
			Decode[(int)'N'] = 13;
			Decode[(int)'O'] = 14;
			Decode[(int)'P'] = 15;
			Decode[(int)'Q'] = 16;
			Decode[(int)'R'] = 17;
			Decode[(int)'S'] = 18;
			Decode[(int)'T'] = 19;
			Decode[(int)'U'] = 20;
			Decode[(int)'V'] = 21;
			Decode[(int)'W'] = 22;
			Decode[(int)'X'] = 23;
			Decode[(int)'Y'] = 24;
			Decode[(int)'Z'] = 25;
			Decode[(int)'a'] = 26;
			Decode[(int)'b'] = 27;
			Decode[(int)'c'] = 28;
			Decode[(int)'d'] = 29;
			Decode[(int)'e'] = 30;
			Decode[(int)'f'] = 31;
			Decode[(int)'g'] = 32;
			Decode[(int)'h'] = 33;
			Decode[(int)'i'] = 34;
			Decode[(int)'j'] = 35;
			Decode[(int)'k'] = 36;
			Decode[(int)'l'] = 37;
			Decode[(int)'m'] = 38;
			Decode[(int)'n'] = 39;
			Decode[(int)'o'] = 40;
			Decode[(int)'p'] = 41;
			Decode[(int)'q'] = 42;
			Decode[(int)'r'] = 43;
			Decode[(int)'s'] = 44;
			Decode[(int)'t'] = 45;
			Decode[(int)'u'] = 46;
			Decode[(int)'v'] = 47;
			Decode[(int)'w'] = 48;
			Decode[(int)'x'] = 49;
			Decode[(int)'y'] = 50;
			Decode[(int)'z'] = 51;
			Decode[(int)'0'] = 52;
			Decode[(int)'1'] = 53;
			Decode[(int)'2'] = 54;
			Decode[(int)'3'] = 55;
			Decode[(int)'4'] = 56;
			Decode[(int)'5'] = 57;
			Decode[(int)'6'] = 58;
			Decode[(int)'7'] = 59;
			Decode[(int)'8'] = 60;
			Decode[(int)'9'] = 61;
			Decode[(int)'+'] = 62;
			Decode[(int)'/'] = 63;
			Decode[(int)'*'] = 64;
		}

		public static string EncodeGame(List<PGNMove> moves, GameResultsType? result = null)
		{
			var sb = new StringBuilder();
			foreach(var move in moves)
				sb.Append(EncodeMove(move.From, move.To, move.Promotion));

			if (result != null)
			{
				sb.Append('-');
				sb.Append(((int)result).ToString());
			}

			return sb.ToString();
		}

		public static string EncodeGame(List<Move> moves, GameResultsType? result = null)
		{
			var sb = new StringBuilder();
			foreach (var move in moves)
				sb.Append(EncodeMove(move.From, move.To, move.Promotion));

			if (result != null)
			{
				sb.Append('-');
				sb.Append(((int)result).ToString());
			}

			return sb.ToString();
		}

		public static string EncodeMove(PGNMove move)
		{
			return EncodeMove(move.From, move.To, move.Promotion);
		}

		public static string EncodeMove(Move move)
		{
			return EncodeMove(move.From, move.To, move.Promotion);
		}

		public static string EncodeMove(int from, int to, Piece promotion = Piece.None)
		{
			var sb = new StringBuilder();
			sb.Append(Encode[from]);
			sb.Append(Encode[to]);
			if (promotion != Piece.None)
			{
				sb.Append(Encode[SpecialInstruction]);
				sb.Append(((int)promotion).ToString());
			}
			return sb.ToString();
		}

		public static Move DecodeMove(string move)
		{
			int from = Decode[move[0]];
			int to = Decode[move[1]];
			Piece promotion = Piece.None;
			if (move.Length == 4 && move[2] == SpecialInstructionChar)
				promotion = (Piece)Convert.ToInt32(move[3].ToString());

			return new Move(from, to, promotion);
		}

		public static Move DecodeMove(string encodedGame, int moveIndex)
		{
			if (encodedGame.Length < moveIndex + 2)
				return null;

			if(encodedGame.Length >= moveIndex + 4)
				return DecodeMove(encodedGame.Substring(moveIndex, 4));
			else
				return DecodeMove(encodedGame.Substring(moveIndex, 2));
		}

		public static GameResultsType? DecodeResult(string encodedGame)
		{
			if (encodedGame.Length < 2)
				return null;

			var split = encodedGame[encodedGame.Length - 2];
			if (split != '-')
				return null;

			var res = encodedGame[encodedGame.Length - 1];
			switch(res)
			{
				case ('0'):
					return GameResultsType.Tie;
				case ('1'):
					return GameResultsType.WhiteWins;
				case ('2'):
					return GameResultsType.BlackWins;
				case ('3'):
					return GameResultsType.Unresolved;
				default:
					return null;
			}
		}

		public static string EncodeGame(PGNGame game, bool writeResults = true)
		{
			var variation = game.GetMainVariation();
			var sb = new StringBuilder();
			foreach(var move in variation)
				sb.Append(EncodeMove(move.From, move.To, move.Promotion));

			if (writeResults)
			{
				sb.Append('-');
				sb.Append(((int)game.Results.Results).ToString());
			}
			var str = sb.ToString();
			return str;
		}

		public static PGNGame DecodeGame(string encodedString)
		{
			var s = encodedString;
			int i = 0;
			Color player = Color.White;
			int move = 1;
			var moves = new List<PGNMove>();
			PGNMove lastmove = null;
			while(i < s.Length - 1)
			{
				// check if the last move wa actually a promotion
				if (Decode[s[i]] == SpecialInstruction)
				{
					lastmove.Promotion = (Piece)Decode[s[i + 1]];
					continue;
				}

				var from = Decode[s[i]];
				var to = Decode[s[i + 1]];
				lastmove = new PGNMove() { From = from, To = to, MoveNumber = move, Color = player/*, Piece = ...*/ };
				if(player == Color.Black)
					move++;
				player = (player == Color.White) ? Color.Black : Color.White;

				moves.Add(lastmove);
				i += 2;
			}

			var variation = new GameVariation(moves.Select(x => (IPGNElement)x).ToList());
			var game = new PGNGame(new Dictionary<string, string>(), variation);
			return game;
		}
	}
}

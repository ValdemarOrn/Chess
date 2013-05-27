using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib
{
	public class Helpers
	{
		public static unsafe BoardStruct* ManagedBoardToNative(Chess.Base.Board board)
		{
			var b = Board.Create();
			b->Castle = 0;

			if (board.CastleKB == Chess.Base.Moves.CanCastle)
				b->Castle |= Board.CASTLE_BK;
			if (board.CastleQB == Chess.Base.Moves.CanCastle)
				b->Castle |= Board.CASTLE_BQ;
			if (board.CastleKW == Chess.Base.Moves.CanCastle)
				b->Castle |= Board.CASTLE_WK;
			if (board.CastleQW == Chess.Base.Moves.CanCastle)
				b->Castle |= Board.CASTLE_WQ;

			b->EnPassantTile = (byte)board.EnPassantTile;
			b->FiftyMoveRulePlies = (byte)board.FiftyMoveRulePlies;
			b->PlayerTurn = (byte)board.PlayerTurn;

			b->CurrentMove = (board.MoveCount - 1) * 2;
			if (b->PlayerTurn == Board.COLOR_BLACK)
				b->CurrentMove++;
			
			for(int i = 0; i < 64; i++)
			{
				int color = board.Color(i);
				int piece = board.Piece(i);

				if(color == 0 || piece == 0)
					continue;

				Board.SetPiece(b, i, piece, color);
			}

			//b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			//b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->Hash = Zobrist.Calculate(b);
			//b->MoveHistory // N/A
			return b;
		}

		public static unsafe string GetString(byte* str)
		{
			if (str == (byte*)0)
				return null;

			byte[] bytes = new byte[1024*8];
			int i = 0;
			while(*str != 0)
			{
				bytes[i] = *str;
				str++;
				i++;
			}
			return Encoding.UTF8.GetString(bytes, 0, i);
		}
	}
}

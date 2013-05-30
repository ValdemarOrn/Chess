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

			if (board.CastlingRights.Contains(Base.Castling.KingsideBlack))
				b->Castle |= Board.CASTLE_BK;
			if (board.CastlingRights.Contains(Base.Castling.QueensideBlack))
				b->Castle |= Board.CASTLE_BQ;
			if (board.CastlingRights.Contains(Base.Castling.KingsideWhite))
				b->Castle |= Board.CASTLE_WK;
			if (board.CastlingRights.Contains(Base.Castling.QueensideWhite))
				b->Castle |= Board.CASTLE_WQ;

			b->EnPassantTile = (byte)board.EnPassantTile;
			b->FiftyMoveRulePlies = (byte)board.FiftyMoveRulePlies;
			b->PlayerTurn = (byte)board.PlayerTurn;

			b->CurrentMove = (board.MoveCount - 1) * 2;
			if (b->PlayerTurn == Board.COLOR_BLACK)
				b->CurrentMove++;
			
			for(int i = 0; i < 64; i++)
			{
				Chess.Base.Color color = board.GetColor(i);
				int piece = board.GetPiece(i);

				if(color == 0 || piece == 0)
					continue;

				Board.SetPiece(b, i, piece, (int)color);
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

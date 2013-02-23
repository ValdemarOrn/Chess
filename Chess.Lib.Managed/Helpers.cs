using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Lib
{
	public class Helpers
	{
		public static unsafe BoardStruct* ManagedBoardToNative(Chess.Board board)
		{
			var b = Board.Create();
			b->Castle = 0;

			if (board.CastleKB == Chess.Moves.CanCastle)
				b->Castle |= Board.CASTLE_BK;
			if (board.CastleQB == Chess.Moves.CanCastle)
				b->Castle |= Board.CASTLE_BQ;
			if (board.CastleKW == Chess.Moves.CanCastle)
				b->Castle |= Board.CASTLE_WK;
			if (board.CastleQW == Chess.Moves.CanCastle)
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

			b->AttacksBlack = Board.AttackMap(b, Board.COLOR_BLACK);
			b->AttacksWhite = Board.AttackMap(b, Board.COLOR_WHITE);
			b->CheckState = Board.GetCheckState(b);
			b->Hash = Zobrist.Calculate(b);
			//b->MoveHistory // N/A
			return b;
		}
	}
}

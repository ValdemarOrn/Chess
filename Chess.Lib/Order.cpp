
#include "Order.h"

int Order_PV(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		if(order->MoveList[i].From == ctx->PV[ply]->From &&
			order->MoveList[i].To == ctx->PV[ply]->To)
		{
			if (order->MoveRank[i] == 0)
			{
				order->MoveRank[i] = Order_StagePV;
				count++;
			}
		}
	}
	return count;
}

int Order_Captures(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		if(order->MoveList[i].CapturePiece > 0)
		{
			int victim = order->MoveList[i].CapturePiece;
			int attacker = order->MoveList[i].PlayerPiece;
			
			if (order->MoveRank[i] == 0)
			{
				_Bool badCapture = Board_BadCapture(ctx->Board, &order->MoveList[i]);
				order->MoveRank[i] = Order_StageCaptures + (10 * victim - attacker) - badCapture * 1000;
				count++;
			}
		}
	}
	return count;
}

int Order_Promotions(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		if(order->MoveList[i].Promotion > 0)
		{
			if (order->MoveRank[i] == 0)
			{
				order->MoveRank[i] = Order_StagePromotions + order->MoveList[i].Promotion;
				count++;
			}
		}
	}
	return count;
}

int Order_KillerMoves(SearchContext* ctx, Order* order, int ply)
{
	// don't search first and last ply
	if(ply < 2 || ply > Search_PlyMax - 2)
		return 0;

	MoveSmall* killerPly = ctx->KillerMoves[ply];
	MoveSmall* killerPrev = ctx->KillerMoves[ply - 2];
	MoveSmall* killerNext = ctx->KillerMoves[ply + 2];

	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		Move* move = &order->MoveList[i];

		if(order->MoveRank[i] != 0)
			continue;

		for(int m = 0; m < 3; m++)
		{
			if(Move_Equal(move, &killerPly[m]))
			{
				order->MoveRank[i] = Order_StageKillerMoves + killerPly[m].Score;
				count++;
			}
			else if(Move_Equal(move, &killerPrev[m]))
			{
				order->MoveRank[i] = Order_StageKillerMoves + killerPrev[m].Score - 1;
				count++;
			}
			else if(Move_Equal(move, &killerNext[m]))
			{
				order->MoveRank[i] = Order_StageKillerMoves + killerNext[m].Score - 1;
				count++;
			}
		}
	}
	return count;
}

int Order_History(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		Move* move = &order->MoveList[i];

		uint32_t historyVal = ctx->History[move->From][move->To];
		
		if (order->MoveRank[i] == 0 && historyVal > 0)
		{
			order->MoveRank[i] = Order_StageHistoryMoves + historyVal;
			count++;
		}
		
	}
	return count;
	return 0;
}

int Order_MoveBNRQ(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		int piece = order->MoveList[i].PlayerPiece;
		if(piece > PIECE_KNIGHT && piece < PIECE_KING)
		{
			if(order->MoveRank[i] == 0)
			{
				order->MoveRank[i] = Order_StageMoveBNRQ + piece;
				count++;
			}
		}
	}
	return count;
}

int Order_MoveK(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		if(order->MoveList[i].PlayerPiece == PIECE_KING)
		{
			if(order->MoveRank[i] == 0)
			{
				order->MoveRank[i] = Order_StageMoveK;
				count++;
			}
		}
	}
	return count;
}

int Order_MoveP(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;
	for(int i = 0; i < order->MoveCount; i++)
	{
		if(order->MoveList[i].PlayerPiece == PIECE_PAWN)
		{
			if(order->MoveRank[i] == 0)
			{
				order->MoveRank[i] = Order_StageMoveP;
				count++;
			}
		}
	}
	return count;
}

void Order_NextStage(SearchContext* ctx, Order* order, int ply)
{
	int count = 0;

	while(true)
	{
		switch(order->OrderStage)
		{
		case (Order_StageInit):
			count = Order_PV(ctx, order, ply);
			order->OrderStage = Order_StagePV;
			break;
		case (Order_StagePV):
			count = Order_Captures(ctx, order, ply);
			order->OrderStage = Order_StageCaptures;
			break;
		case (Order_StageCaptures):
			count = Order_Promotions(ctx, order, ply);
			order->OrderStage = Order_StagePromotions;
			break;
		case (Order_StagePromotions):
			count = Order_KillerMoves(ctx, order, ply);
			order->OrderStage = Order_StageKillerMoves;
			break;
		case (Order_StageKillerMoves):
			count = Order_History(ctx, order, ply);
			order->OrderStage = Order_StageHistoryMoves;
			break;
		case (Order_StageHistoryMoves):
			count = Order_MoveBNRQ(ctx, order, ply);
			order->OrderStage = Order_StageMoveBNRQ;
			break;
		case (Order_StageMoveBNRQ):
			count = Order_MoveK(ctx, order, ply);
			order->OrderStage = Order_StageMoveK;
			break;
		case (Order_StageMoveK):
			count = Order_MoveP(ctx, order, ply);
			order->OrderStage = Order_StageMoveP;
			break;
		case (Order_StageMoveP): // no more moves left to order
			return;
		}

		order->OrderedMoves += count;

		// return if We found some new moves
		if(order->OrderedMoves > 0)
			return;
	}
}

Move* Order_GetMove(SearchContext* ctx, Order* moves, int ply)
{
	// if there are no ordered moves ready, order the next batch of moves
	if(moves->OrderedMoves == 0 || moves->OrderStage == Order_StageInit)
		Order_NextStage(ctx, moves, ply);

	int bestScore = -1;
	int bestMove = -1;
	// find the best move
	for(int i=0; i<moves->MoveCount; i++)
	{
		if(moves->MoveRank[i] > bestScore)
		{
			bestMove = i;
			bestScore = moves->MoveRank[i];
		}
	}

	if(bestMove == -1)
		return 0;

	moves->MoveRank[bestMove] = -1; // mark the move as taken
	return &moves->MoveList[bestMove];
}

void Order_SetHashMove(SearchContext* ctx, Order* moves, int from, int to)
{
	for(int i=0; i<moves->MoveCount; i++)
	{
		if(moves->MoveList[i].From == from && moves->MoveList[i].To == to)
		{
			moves->MoveRank[i] = Order_StageHash;
			moves->OrderedMoves++;
			return;
		}
	}
}

int Order_QuiesceFilter(SearchContext* ctx, Order* moves)
{
	int quiesceMoves = 0;

	// block all moves except captures
	for(int i=0; i<moves->MoveCount; i++)
	{
		if(moves->MoveList[i].CapturePiece > 0 || moves->MoveList[i].Promotion == PIECE_QUEEN)
			quiesceMoves++;
		else
			moves->MoveRank[i] = -1;
	}

	return quiesceMoves;
}



void Order_SetKillerMove(MoveSmall* killerArray, int from, int to, int piece)
{
	MoveSmall* moves = killerArray;
	_Bool exists = false;

	int lowestScore = 10000000;
	int index = -1;

	moves[0].Score--;
	moves[1].Score--;
	moves[2].Score--;

	for(int i = 0; i < 3; i++)
	{
		// already exists
		if(moves[i].From == from && moves[i].To == to && moves[i].Piece == piece)
		{
			moves[i].Score += Order_MinKillerScore;
			if(moves[i].Score < Order_MinKillerScore)
				moves[i].Score = Order_MinKillerScore;
			return;
		}

		if(moves[i].Score <= lowestScore)
		{
			lowestScore = moves[i].Score;
			index = i;
		}
	}

	// replace move with lowest score
	moves[index].From = from;
	moves[index].To = to;
	moves[index].Piece = piece;
	moves[index].Score = Order_MinKillerScore;
}
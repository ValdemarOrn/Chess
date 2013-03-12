
#include "Search.h"
#include "Eval.h"
#include "Moves.h"
#include "Manager.h"
#include "Order.h"
#include <stdio.h>

int Search_AlphaBeta(SearchContext* ctx, int depth, int alpha, int beta);

#ifdef STATS_SEARCH
SearchStats SStats;
#endif

void ResetStats()
{
	#ifdef STATS_SEARCH
	SStats.AllNodeCount = 0;
	SStats.CutNodeCount = 0;
	SStats.EvalNodeCount = 0;
	SStats.NoneNodeCount = 0;
	SStats.QuiescentNodeCount = 0;
	SStats.PVNodeCount = 0;
	SStats.TotalNodeCount = 0;
	
	for(int i = 0; i < Search_PlyMax; i++)
	{
		SStats.MovesAtPly[i] = 0;
		SStats.NodesAtPly[i] = 0;
	}

	for(int i = 0; i < 100; i++)
	{
		SStats.CutMoveIndex[i] = 0;
		SStats.BestMoveIndex[i] = 0;
	}
	#endif
}

void Search_InitContext(SearchContext* ctx)
{
	ctx->Board = 0;

	for(int i = 0; i < 64; i++)
		for(int j = 0; j < 64; j++)
			ctx->History[i][j] = 0;

	for(int i = 0; i < Search_PlyMax; i++)
	{
		for(int j = 0; j < 3; j++)
		{
			ctx->KillerMoves[i][j].From = 0;
			ctx->KillerMoves[i][j].Piece = 0;
			ctx->KillerMoves[i][j].Score = 0;
			ctx->KillerMoves[i][j].To = 0;
		}
	}

	for(int i = 0; i < Search_PlyMax; i++)
	{
		for(int j = 0; j < Search_PlyMax; j++)
		{
			ctx->PV[i][j].From = 0;
			ctx->PV[i][j].Piece = 0;
			ctx->PV[i][j].Score = 0;
			ctx->PV[i][j].To = 0;
		}
	}
}

MoveSmall Search_SearchPos(Board* board, int searchDepth)
{
	SearchContext ctx;
	Search_InitContext(&ctx);
	ctx.Board = board;

	char text[80];
	Manager_Callback("Ply\tScore\tNodes\tPV\n");

	for(int i = 1; i <= searchDepth; i++)
	{
		// init stats
		#ifdef STATS_SEARCH
		ResetStats();
		#endif

		// Cut the history table every cycle
		for(int i = 0; i < 64; i++)
			for(int j = 0; j < 64; j++)
				ctx.History[i][j] = (int)(ctx.History[i][j] >> 1); // divide by 2

		ctx.SearchDepth = i;
		Search_AlphaBeta(&ctx, i, -99999999, 99999999);

		// print PV

		// ply
		sprintf(text, "%d\t", i);
		Manager_Callback(text);

		// score
		sprintf(text, "%d\t", ctx.PV[0][0].Score);
		Manager_Callback(text);

		// node count
		#ifdef STATS_SEARCH
		sprintf(text, "%d\t", SStats.TotalNodeCount);
		Manager_Callback(text);
		#endif

		// PV
		for(int j = 0; j < i; j++)
		{
			Move_PieceToString(ctx.PV[0][j].Piece, text);
			Manager_Callback(text);

			Move_ToString(&(ctx.PV[0][j]), text);
			Manager_Callback(text);
			Manager_Callback(" ");
		}
		Manager_Callback("\n");
	}

	return ctx.PV[0][0];
}

int Search_AlphaBeta(SearchContext* ctx, int depth, int alpha, int beta)
{
	Board* board = ctx->Board;
	int ply = ctx->SearchDepth - depth;

	int nodeType = NODE_ALL;
	int score = 0;

	int bestMoveIndex = -1;
	int cutMoveIndex = -1;
	Move* move;

	// cehcks for checkmate or stalemate
	_Bool hasValidMove = FALSE;

	#ifdef STATS_SEARCH
	SStats.TotalNodeCount++;
	SStats.NodesAtPly[ply]++;
	#endif

	if ( depth == 0 ) 
	{
		nodeType = NODE_EVAL;
		int eval = Eval_Evaluate(board);
		if(board->PlayerTurn == COLOR_WHITE)
			score = eval;
		else
			score = -eval;

		goto Finalize;
	}

	// set PV at this ply to zero
	//memset(PV[ply], 0, sizeof(MoveSmall) * Search_PlyMax);

	// Todo: if we get a hash hit, we need to clean up the PV table because it won't contain all the moves

	Order order;
	order.OrderedMoves = 0;
	order.OrderStage = Order_StageInit;
	memset(order.MoveRank, 0, 100 * sizeof(int));

	// find all the moves
	order.MoveCount = Moves_GetAllMoves(board, order.MoveList);

	#ifdef STATS_SEARCH
	SStats.MovesAtPly[ply] += order.MoveCount;
	#endif

	for (int i=0; i < order.MoveCount; i++)
	{
		// without move ordering
		//Move* move = &(order.MoveList[i]);

		// WITH move ordering
		move = Order_GetMove(ctx, &order, ply);

		assert(move != 0);

		int from = move->From;
		int to = move->To;

		#ifdef DEBUG
		int pieceMoving = Board_Piece(board, from);
		uint64_t hashBefore = board->Hash;
		#endif

		_Bool valid = Board_Make(board, from, to);
		if(valid == FALSE)
			continue;

		// Todo: Handle promotions

		hasValidMove = true;
		int val = -Search_AlphaBeta(ctx, depth - 1, -beta, -alpha);
		Board_Unmake(board);

		assert(hashBefore == board->Hash);

		if(val >= beta)
		{
			cutMoveIndex = i;
			nodeType = NODE_CUT;
			score = beta;
			goto Finalize;
		}

		if(val > alpha)
		{
			bestMoveIndex = i;
			nodeType = NODE_PV;
			ctx->PV[ply][0].From = from;
			ctx->PV[ply][0].To = to;
			ctx->PV[ply][0].Score = val;
			ctx->PV[ply][0].Piece = Board_Piece(board, from);

			memcpy(&(ctx->PV[ply][1]), &(ctx->PV[ply + 1][0]), sizeof(MoveSmall) * (Search_PlyMax - ply));
			alpha = val;
			score = val;
		}
		
	}

	// checkmate or stalemate		
	if(hasValidMove == FALSE)
	{
		nodeType = NODE_NONE;

		if(Board_IsChecked(board, board->PlayerTurn))
			score = Search_Checkmate;
		else
			score = Search_Stalemate;
	}

	if(nodeType == NODE_ALL)
		score = alpha;

Finalize:

	#ifdef STATS_SEARCH
	switch(nodeType)
	{
	case (NODE_PV):
		SStats.PVNodeCount++;
		SStats.BestMoveIndex[bestMoveIndex]++;
		break;
	case (NODE_ALL):
		SStats.AllNodeCount++;
		break;
	case (NODE_CUT):
		SStats.CutNodeCount++;
		SStats.CutMoveIndex[cutMoveIndex]++;
		break;
	case (NODE_EVAL):
		SStats.EvalNodeCount++;
		break;
	case (NODE_NONE):
		SStats.NoneNodeCount++;
		break;
	}
	#endif

	// killer moves and history
	if(nodeType == NODE_CUT)
	{
		if(move->CapturePiece == 0)
		{
			Order_SetKillerMove(ctx, ply, move->From, move->To, move->PlayerPiece);
			ctx->History[move->From][move->To] += ply * ply;
		}
	}

	// Todo: Add Hashtable entry

	return score;
}

SearchStats* Search_GetSearchStats()
{
	#ifdef STATS_SEARCH
	return &SStats;
	#endif

	#ifndef STATS_SEARCH
	return 0;
	#endif
}
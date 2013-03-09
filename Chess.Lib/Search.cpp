
#include "Search.h"
#include "Eval.h"
#include "Moves.h"
#include "Manager.h"
#include <stdio.h>

int Search_AlphaBeta(SearchContext* ctx, int depth, int alpha, int beta);

#ifdef DEBUG
SearchStats SStats;
#endif

void ResetStats()
{
	SStats.AllNodeCount = 0;
	SStats.BestMoveIndexSum = 0;
	SStats.CutMoveIndexSum = 0;
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
}

MoveSmall Search_SearchPos(Board* board, int searchDepth)
{
	SearchContext ctx;
	ctx.Board = board;
	
	// create space for the PV table
	MoveSmall PV[Search_PlyMax][Search_PlyMax];
	for(int i = 0; i < Search_PlyMax; i++)
		ctx.PV[i] = PV[i];

	char text[8];
	Manager_Callback("Ply\tScore\tNodes\tPV\n");

	for(int i = 1; i <= searchDepth; i++)
	{
		// init stats
		#ifdef DEBUG
		ResetStats();
		#endif

		ctx.SearchDepth = i;
		Search_AlphaBeta(&ctx, i, -99999999, 99999999);

		// print PV

		// ply
		sprintf(text, "%d\t", i);
		Manager_Callback(text);

		// score
		sprintf(text, "%d\t", PV[0][0].Score);
		Manager_Callback(text);

		// node count
		sprintf(text, "%d\t", SStats.TotalNodeCount);
		Manager_Callback(text);

		// PV
		for(int j = 0; j < i; j++)
		{
			Move_PieceToString(PV[0][j].Piece, text);
			Manager_Callback(text);

			Move_ToString(&PV[0][j], text);
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
	MoveSmall** PV = ctx->PV;
	int ply = ctx->SearchDepth - depth;

	int nodeType = NODE_ALL;
	int score = 0;

	int bestMoveIndex = -1;
	int cutMoveIndex = -1;

	// cehck if there's at least one valid move. Otherwise, checkmate or stalemate
	_Bool hasValidMove = FALSE;
	int moveCount = 0;

	#ifdef DEBUG
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
	memset(PV[ply], 0, sizeof(MoveSmall) * Search_PlyMax);

	// find all the moves
	Move moveList[100];
	moveCount = Moves_GetAllMoves(board, moveList);

	// Todo: order the moves / Give them a rank from best to worst

	#ifdef DEBUG
	SStats.MovesAtPly[ply] += moveCount;
	#endif

	for (int i=0; i < moveCount; i++)
	{
		Move* move = &moveList[i];
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
			PV[ply][0].From = from;
			PV[ply][0].To = to;
			PV[ply][0].Score = val;
			PV[ply][0].Piece = Board_Piece(board, from);

			memcpy(&(PV[ply][1]), &(PV[ply + 1][0]), sizeof(MoveSmall) * (Search_PlyMax - ply));
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

	#ifdef DEBUG
	switch(nodeType)
	{
	case (NODE_PV):
		SStats.PVNodeCount++;
		SStats.BestMoveIndexSum += bestMoveIndex;
		break;
	case (NODE_ALL):
		SStats.AllNodeCount++;
		break;
	case (NODE_CUT):
		SStats.CutNodeCount++;
		SStats.CutMoveIndexSum += cutMoveIndex;
		break;
	case (NODE_EVAL):
		SStats.EvalNodeCount++;
		break;
	case (NODE_NONE):
		SStats.NoneNodeCount++;
		break;

	}
	#endif

	// Todo: Add Hashtable entry

	return score;
}

SearchStats* Search_GetSearchStats()
{
	return &SStats;
}
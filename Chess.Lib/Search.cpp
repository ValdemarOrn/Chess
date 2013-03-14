
#include "Search.h"
#include "Eval.h"
#include "Moves.h"
#include "Manager.h"
#include "Order.h"
#include "TTable.h"
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

	SStats.EvalHits = 0;
	SStats.EvalTotal = 0;

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

_Bool Search_DrawByRepetition(Board* board)
{
	uint64_t currentHash = board->Hash;
	int occurences = 1;
	int i = board->CurrentMove - 1;
	while(i >= 0)
	{
		if(board->MoveHistory[i].PrevHash == currentHash)
			occurences++;

		if(occurences >= 3)
			return TRUE;

		i--;
	}

	return FALSE;
}

int Search_AlphaBeta(SearchContext* ctx, int depth, int alpha, int beta)
{
	Board* board = ctx->Board;
	int ply = ctx->SearchDepth - depth;

//	_Bool addtoHashTable = TRUE;
	int nodeType = NODE_ALL;
	int score = 0;
	int moveCount = 0;
//	TTableEntry* tableEntry = 0;

	int bestMoveIndex = -1;
	int cutMoveIndex = -1;
//	_Bool hashHitPartial = FALSE;
//	_Bool hashHitFull = FALSE;
	Move bestMove;
	bestMove.PlayerPiece = 0;
	_Bool hasValidMove = FALSE;

	#ifdef STATS_SEARCH
	SStats.TotalNodeCount++;
	SStats.NodesAtPly[ply]++;
	#endif

	// ----------------------------------------------------------------------------------
	// --------------------- Check for draw by threefold repetition ---------------------
	// ----------------------------------------------------------------------------------

	_Bool repeat = Search_DrawByRepetition(ctx->Board);
	if(repeat)
	{
//		addtoHashTable = FALSE;
		score = Search_Draw;

		if(score >= beta)
		{
			nodeType = NODE_CUT;
			score = beta;
		}

		if(score > alpha)
		{
			nodeType = NODE_PV;
			memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * Search_PlyMax);
			alpha = score;
		}

		goto Finalize;
	}

	// ----------------------------------------------------------------------------------
	// ----------------------- Enter Quiescence search at depth 0 -----------------------
	// ----------------------------------------------------------------------------------

	if(depth <= 0)
	{
		int eval = Eval_Evaluate(board);
		if(board->PlayerTurn == COLOR_WHITE)
			score = eval;
		else
			score = -eval;

		return score;
	}

	// ----------------------------------------------------------------------------------
	// ---------------------- Check for Transposition Table entry ----------------------
	// ----------------------------------------------------------------------------------

/*	tableEntry = TTable_Read(ctx->Board->Hash);
	if(tableEntry != 0)
	{
		hashHitPartial = TRUE;
		if(tableEntry->Depth >= depth && (tableEntry->NodeType == NODE_PV || tableEntry->NodeType == NODE_ALL))
		{
			hashHitFull = TRUE;
			score = tableEntry->Score;
			
			if(score >= beta)
			{
				cutMoveIndex = 0;
				bestMove.From = tableEntry->BestMoveFrom;
				bestMove.To = tableEntry->BestMoveTo;
				bestMove.PlayerPiece = 100; // just some value other than 0, so it gets added to the TT

				nodeType = NODE_CUT;
				score = beta;
				goto Finalize;
			}

			if(score > alpha)
			{
				bestMoveIndex = 0;
				//bestMove.From = tableEntry->BestMoveFrom;
				//bestMove.To = tableEntry->BestMoveTo;
				//bestMove.PlayerPiece = 100; // just some value other than 0, so it gets added to the TT

				nodeType = NODE_PV;
				ctx->PV[ply][0].From = tableEntry->BestMoveFrom;
				ctx->PV[ply][0].To = tableEntry->BestMoveTo;
				ctx->PV[ply][0].Score = score;
				ctx->PV[ply][0].Piece = Board_Piece(board, tableEntry->BestMoveFrom);

				memset(&(ctx->PV[ply][1]), 0, sizeof(MoveSmall) * (Search_PlyMax - 1));
				alpha = tableEntry->Score;
				score = tableEntry->Score;
			}
		}
	}
*/
	Order order;
	order.OrderedMoves = 0;
	order.OrderStage = Order_StageInit;
	memset(order.MoveRank, 0, 100 * sizeof(int));

	// ----------------------------------------------------------------------------------
	// ----------------------- Generate a list of all valid moves -----------------------
	// ----------------------------------------------------------------------------------

	order.MoveCount = Moves_GetAllMoves(board, order.MoveList);
	moveCount = order.MoveCount;

	// set the hash move if it is known
//	if(hashHitPartial)
//		Order_SetHashMove(ctx, &order, tableEntry->BestMoveFrom, tableEntry->BestMoveTo);

	#ifdef STATS_SEARCH
	SStats.MovesAtPly[ply] += order.MoveCount;
	#endif

	// ----------------------------------------------------------------------------------
	// ------------------------- Search through all the moves --------------------------
	// ----------------------------------------------------------------------------------

	for (int i=0; i < moveCount; i++)
	{
		Move* move = Order_GetMove(ctx, &order, ply);

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
			bestMove = *move;
			nodeType = NODE_CUT;
			score = beta;
			goto Finalize;
		}

		if(val > alpha)
		{
			bestMoveIndex = i;
			bestMove = *move;
			nodeType = NODE_PV;
			ctx->PV[ply][0].From = from;
			ctx->PV[ply][0].To = to;
			ctx->PV[ply][0].Score = val;
			ctx->PV[ply][0].Piece = Board_Piece(board, from);

			memcpy(&(ctx->PV[ply][1]), &(ctx->PV[ply + 1][0]), sizeof(MoveSmall) * (Search_PlyMax - 1));
			alpha = val;
			score = val;
		}
		
	}

	// ----------------------------------------------------------------------------------
	// ------------------------ Checkmate and stalemate handling ------------------------
	// ----------------------------------------------------------------------------------

	if(!hasValidMove)
	{
		bestMove.PlayerPiece = 0;

		if(Board_IsChecked(board, board->PlayerTurn))
			score = Search_Checkmate;
		else
			score = Search_Stalemate;

		if(score >= beta)
		{
			cutMoveIndex = order.MoveCount;
			nodeType = NODE_CUT;
			score = beta;
			goto Finalize;
		}

		if(score > alpha)
		{
			bestMoveIndex = order.MoveCount;
			nodeType = NODE_PV;
			memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * Search_PlyMax);
			alpha = score;
		}
	}

	// ----------------------------------------------------------------------------------
	// ------------------------------- All-node handling -------------------------------
	// ----------------------------------------------------------------------------------

	if(nodeType == NODE_ALL)
		score = alpha;


Finalize:

	#ifdef STATS_SEARCH

//	if(hashHitPartial)
//		SStats.HashHitsCount++;

	switch(nodeType)
	{
	case (NODE_PV):
		SStats.PVNodeCount++;
		if(bestMoveIndex >= 0)
			SStats.BestMoveIndex[bestMoveIndex]++;
		break;
	case (NODE_ALL):
		SStats.AllNodeCount++;
		break;
	case (NODE_CUT):
		SStats.CutNodeCount++;
		if(cutMoveIndex >= 0)
			SStats.CutMoveIndex[cutMoveIndex]++;
		break;
	}
	#endif

	// ----------------------------------------------------------------------------------
	// --------------------- Store killer moves and update history ---------------------
	// ----------------------------------------------------------------------------------

	if(nodeType == NODE_CUT && !(bestMove.From == 0 && bestMove.To == 0))
	{
		if(bestMove.PlayerPiece != 0 && bestMove.CapturePiece == 0)
		{
			Order_SetKillerMove(ctx, ply, bestMove.From, bestMove.To, bestMove.PlayerPiece);
			ctx->History[bestMove.From][bestMove.To] += ply * ply;
		}
	}

	// ----------------------------------------------------------------------------------
	// ------------------------ Add node to transposition table ------------------------
	// ----------------------------------------------------------------------------------

/*	if(addtoHashTable)
	{
		TTableEntry newEntry;
		if(bestMove.PlayerPiece != 0)
		{
			newEntry.BestMoveFrom = bestMove.From;
			newEntry.BestMoveTo = bestMove.To;
		}
		else
		{
			newEntry.BestMoveFrom = 0;
			newEntry.BestMoveTo = 0;
		}
		newEntry.Depth = depth;
		newEntry.Hash = ctx->Board->Hash;
		newEntry.NodeType = nodeType;
		newEntry.Score = score;
		TTable_Insert(&newEntry);
	}
*/

	return score;
}

int Search_Quiesce(SearchContext* ctx, int depth, int alpha, int beta)
{
	Board* board = ctx->Board;
	int ply = ctx->SearchDepth - depth;

	int nodeType = NODE_ALL;
	int score = 0;
	int moveCount = 0;
//	TTableEntry* tableEntry = 0;

	int bestMoveIndex = -1;
	int cutMoveIndex = -1;
//	_Bool hashHitPartial = FALSE;
//	_Bool hashHitFull = FALSE;
	Move bestMove;
	bestMove.PlayerPiece = 0;
	_Bool hasValidMove = FALSE;

	#ifdef STATS_SEARCH
	SStats.TotalNodeCount++;
	SStats.NodesAtPly[ply]++;
	#endif

	// ----------------------------------------------------------------------------------
	// ---------------------- Check for Transposition Table entry ----------------------
	// ----------------------------------------------------------------------------------

	/*tableEntry = TTable_Read(ctx->Board->Hash);
	if(tableEntry != 0)
	{
		hashHitPartial = TRUE;
		if(tableEntry->Depth >= depth && (tableEntry->NodeType == NODE_PV || tableEntry->NodeType == NODE_ALL))
		{
			hashHitFull = TRUE;
			score = tableEntry->Score;
			
			if(score >= beta)
			{
				cutMoveIndex = 0;
				bestMove.From = tableEntry->BestMoveFrom;
				bestMove.To = tableEntry->BestMoveTo;
				bestMove.PlayerPiece = 100; // just some value other than 0, so it gets added to the TT

				nodeType = NODE_CUT;
				score = beta;
				goto Finalize;
			}

			if(score > alpha)
			{
				bestMoveIndex = 0;
				//bestMove.From = tableEntry->BestMoveFrom;
				//bestMove.To = tableEntry->BestMoveTo;
				//bestMove.PlayerPiece = 100; // just some value other than 0, so it gets added to the TT

				nodeType = NODE_PV;
				ctx->PV[ply][0].From = tableEntry->BestMoveFrom;
				ctx->PV[ply][0].To = tableEntry->BestMoveTo;
				ctx->PV[ply][0].Score = score;
				ctx->PV[ply][0].Piece = Board_Piece(board, tableEntry->BestMoveFrom);

				memset(&(ctx->PV[ply][1]), 0, sizeof(MoveSmall) * (Search_PlyMax - 1));
				alpha = tableEntry->Score;
				score = tableEntry->Score;
			}
		}
	}*/

	Order order;
	order.OrderedMoves = 0;
	order.OrderStage = Order_StageInit;
	memset(order.MoveRank, 0, 100 * sizeof(int));


	// ----------------------------------------------------------------------------------
	// --------------------------------- Run Evaluate  ---------------------------------
	// ----------------------------------------------------------------------------------

	int eval = Eval_Evaluate(board);
	if(board->PlayerTurn == COLOR_WHITE)
		score = eval;
	else
		score = -eval;
		
	if(score >= beta)
	{
		nodeType = NODE_EVAL;
		score = beta;
		goto Finalize;
	}

	if(score > alpha)
	{
		nodeType = NODE_PV;
		memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * Search_PlyMax);
		alpha = score;
	}

	// check if we're too deep
	if(ply >= Search_PlyMax - 1)
	{
		nodeType = NODE_EVAL;
		goto Finalize;
	}
		

	// ----------------------------------------------------------------------------------
	// ----------------------- Generate a list of all valid moves -----------------------
	// ----------------------------------------------------------------------------------

	order.MoveCount = Moves_GetAllMoves(board, order.MoveList);
	moveCount = order.MoveCount;

	// filter out all moves except captures and promotions
	// no moves get filtered if side to move is checked
	moveCount = Order_QuiesceFilter(ctx, &order);

	// check if the move is quiet
	if(moveCount == 0)
	{
		// score already contains the eval score
		nodeType = NODE_EVAL;
		goto Finalize;
	}

	#ifdef STATS_SEARCH
	SStats.MovesAtPly[ply] += order.MoveCount;
	#endif

	// ----------------------------------------------------------------------------------
	// ------------------------- Search through all the moves --------------------------
	// ----------------------------------------------------------------------------------

	for (int i=0; i < moveCount; i++)
	{
		Move* move = Order_GetMove(ctx, &order, ply);

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
		int val = -Search_Quiesce(ctx, depth - 1, -beta, -alpha);
		Board_Unmake(board);

		assert(hashBefore == board->Hash);

		if(val >= beta)
		{
			cutMoveIndex = i;
			bestMove = *move;
			nodeType = NODE_CUT;
			score = beta;
			goto Finalize;
		}

		if(val > alpha)
		{
			bestMoveIndex = i;
			bestMove = *move;
			nodeType = NODE_PV;
			ctx->PV[ply][0].From = from;
			ctx->PV[ply][0].To = to;
			ctx->PV[ply][0].Score = val;
			ctx->PV[ply][0].Piece = Board_Piece(board, from);

			memcpy(&(ctx->PV[ply][1]), &(ctx->PV[ply + 1][0]), sizeof(MoveSmall) * (Search_PlyMax - 1));
			alpha = val;
			score = val;
		}
		
	}

	// ----------------------------------------------------------------------------------
	// ------------------------ Checkmate and stalemate handling ------------------------
	// ----------------------------------------------------------------------------------

	if(!hasValidMove)
	{
		bestMove.PlayerPiece = 0;

		if(Board_IsChecked(board, board->PlayerTurn))
			score = Search_Checkmate;
		else
			score = Search_Stalemate;

		if(score >= beta)
		{
			cutMoveIndex = order.MoveCount;
			nodeType = NODE_CUT;
			score = beta;
			goto Finalize;
		}

		if(score > alpha)
		{
			bestMoveIndex = order.MoveCount;
			nodeType = NODE_PV;
			memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * Search_PlyMax);
			alpha = score;
		}
	}

	// ----------------------------------------------------------------------------------
	// ------------------------------- All-node handling -------------------------------
	// ----------------------------------------------------------------------------------

	if(nodeType == NODE_ALL)
		score = alpha;


Finalize:

	#ifdef STATS_SEARCH

//	if(hashHitPartial)
//		SStats.HashHitsCount++;

	if(depth < 0)
		SStats.QuiescentNodeCount++;

	switch(nodeType)
	{
	case (NODE_PV):
		SStats.QPVNodeCount++;
		if(bestMoveIndex >= 0)
			SStats.QBestMoveIndex[bestMoveIndex]++;
		break;
	case (NODE_ALL):
		SStats.QAllNodeCount++;
		break;
	case (NODE_CUT):
		SStats.QCutNodeCount++;
		if(cutMoveIndex >= 0)
			SStats.QCutMoveIndex[cutMoveIndex]++;
		break;

	case (NODE_EVAL):
		SStats.EvalNodeCount++;
		break;
	}
	#endif

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
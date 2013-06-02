
#include <stdio.h>
#include "Search.h"
#include "Eval.h"
#include "Moves.h"
#include "Manager.h"
#include "Order.h"
#include "TTable.h"
#include "SEE.h"

int Search_AlphaBeta(SearchContext* ctx, int alpha, int beta, SearchParams params);
int Search_Quiesce(SearchContext* ctx, int alpha, int beta, SearchParams params);

_Bool SearchStopped;

#ifdef STATS_SEARCH
SearchStats SStats;
#endif

void Search_StopSearch()
{
	SearchStopped = TRUE;
}

void Search_ResetStats()
{
	#ifdef STATS_SEARCH
	SStats.TotalNodeCount = 0;
	SStats.QuiescentNodeCount = 0;
	SStats.EvalNodeCount = 0;

	SStats.CutNodeCount = 0;
	SStats.PVNodeCount = 0;
	SStats.AllNodeCount = 0;
	
	SStats.QCutNodeCount = 0;
	SStats.QPVNodeCount = 0;
	SStats.QAllNodeCount = 0;
	
	SStats.HashHitsCount = 0;
	SStats.HashFullHitCount = 0;
	
	SStats.EvalHits = 0;
	SStats.EvalTotal = 0;

	SStats.AttackMapCount = 0;

	SStats.PruneDelta = 0;
	SStats.PruneBadCaptures = 0;

	SStats.NullMoveReductions = 0;
	
	for(int i = 0; i < SEARCH_PLY_MAX; i++)
	{
		SStats.MovesAtPly[i] = 0;
		SStats.NodesAtPly[i] = 0;
	}

	for(int i = 0; i < 100; i++)
	{
		SStats.CutMoveIndex[i] = 0;
		SStats.BestMoveIndex[i] = 0;
		SStats.QBestMoveIndex[i] = 0;
		SStats.QCutMoveIndex[i] = 0;
	}

	#endif
}

void Search_InitContext(SearchContext* ctx)
{
	ctx->SearchBoard = 0;

	for(int i = 0; i < 64; i++)
		for(int j = 0; j < 64; j++)
			ctx->History[i][j] = 0;

	for(int i = 0; i < SEARCH_PLY_MAX; i++)
	{
		for(int j = 0; j < 3; j++)
		{
			ctx->KillerMoves[i][j].From = 0;
			ctx->KillerMoves[i][j].Piece = 0;
			ctx->KillerMoves[i][j].Score = 0;
			ctx->KillerMoves[i][j].To = 0;
		}
	}

	for(int i = 0; i < SEARCH_PLY_MAX; i++)
	{
		for(int j = 0; j < SEARCH_PLY_MAX; j++)
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
	ctx.SearchBoard = board;
	SearchParams params;
	SearchStopped = FALSE;
	MoveSmall bestMove;

	char text[80];

	// clears the Transposition Table before every search
	// Todo: Stop doing this and implement TTable entry "generation" / "age"
	TTable_ClearAll();

	// init stats
	#ifdef STATS_SEARCH
	Search_ResetStats();
	#endif

	for(int i = 1; i <= searchDepth; i++)
	{

		// Cut the history table every cycle
		for(int p = 0; p < 64; p++)
			for(int q = 0; q < 64; q++)
				ctx.History[p][q] = (int)(ctx.History[p][q] >> 1); // divide by 2

		params.Depth = i;
		params.Ply = 0;
		params.Reductions = 0;
		params.AllowReductions = TRUE;
		Search_AlphaBeta(&ctx, SEARCH_MIN_SCORE, SEARCH_MAX_SCORE, params);

		if(SearchStopped)
			return bestMove;
		else
			bestMove = ctx.PV[0][0];

		// ----------- print PV -----------

		Manager_BeginMessage(Message_SearchData);

		// ply
		sprintf(text, "%d", i);
		Manager_Write(Message_Ply, text);

		// score
		sprintf(text, "%d", ctx.PV[0][0].Score);
		Manager_Write(Message_Score, text);

		// node count
		#ifdef STATS_SEARCH
		sprintf(text, "%d", (int)SStats.TotalNodeCount);
		Manager_Write(Message_Nodes, text);
		#endif

		// PV
		for(int j = 0; j < i; j++)
		{
			Move_PieceToString(ctx.PV[0][j].Piece, text);
			Manager_Write(Message_PV, text);

			Move_ToString(&(ctx.PV[0][j]), text);
			Manager_Write(Message_PV, text);

			Manager_Write(Message_PV, " ");
		}

		Manager_EndMessage();
	}

	return bestMove;
}

_Bool Search_DrawByRepetition(Board* board)
{
	uint64_t currentHash = board->Hash;
	int occurences = 1;
	int i = board->CurrentMove - 1;
	while(i >= 0)
	{
		if(board->History[i].PrevHash == currentHash)
			occurences++;

		if(occurences >= 3)
			return TRUE;

		i--;
	}

	return FALSE;
}

int Search_AlphaBeta(SearchContext* ctx, int alpha, int beta, SearchParams params)
{
	if(SearchStopped)
		return 0;

	Board* board = ctx->SearchBoard;
	int ply = params.Ply;
	int depth = params.Depth;
	SearchParams callingParams = params;

	_Bool addtoHashTable = TRUE;
	int nodeType = NODE_ALL;
	int score = 0;
	int moveCount = 0;
	TTableEntry* tableEntry = 0;

	int bestMoveIndex = -1;
	int cutMoveIndex = -1;
	_Bool hashHitPartial = FALSE;
	_Bool hashHitFull = FALSE;
	Move bestMove;
	bestMove.From = 0;
	bestMove.To = 0;
	bestMove.Promotion = 0;
	bestMove.PlayerPiece = 0;
	bestMove.CapturePiece = 0;

	_Bool useScout = FALSE;
	int bestScore = SEARCH_MIN_SCORE; // track All-node score, needed for fail-soft
	int nullReduction = 0;
	_Bool hasValidMove = FALSE;

	_Bool doNullMoveReduction = (depth > SEARCH_NULL_REDUCE_MIN_PLY && params.AllowReductions == TRUE);

	#ifdef STATS_SEARCH
	SStats.TotalNodeCount++;
	SStats.NodesAtPly[ply]++;
	#endif

	// ----------------------------------------------------------------------------------
	// --------------------- Check for draw by threefold repetition ---------------------
	// ----------------------------------------------------------------------------------

	_Bool repeat = Search_DrawByRepetition(ctx->SearchBoard);
	if(repeat)
	{
		addtoHashTable = FALSE;
		score = Search_Draw;

		if(score >= beta)
		{
			nodeType = NODE_CUT;
		}

		if(score > alpha)
		{
			nodeType = NODE_PV;
			memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * SEARCH_PLY_MAX);
			alpha = score;
		}

		goto Finalize;
	}

	// ----------------------------------------------------------------------------------
	// ----------------------- Enter Quiescence search at depth 0 -----------------------
	// ----------------------------------------------------------------------------------

	if(depth <= 0)
	{
		// the Quiescence search takes care of all the bookkeeping, incl.
		// everything that needs to be done in Finalize!
		return Search_Quiesce(ctx, alpha, beta, callingParams);
	}

	// ----------------------------------------------------------------------------------
	// ---------------------- Check for Transposition Table entry ----------------------
	// ----------------------------------------------------------------------------------

	tableEntry = TTable_Read(ctx->SearchBoard->Hash);
	if(tableEntry != 0)
	{
		hashHitPartial = TRUE;
		if(tableEntry->Depth >= depth)
		{
			hashHitFull = TRUE;
			score = tableEntry->Score;
			
			if(tableEntry->NodeType == NODE_CUT)
			{
				if(score >= beta)
				{
					addtoHashTable = FALSE;
					cutMoveIndex = 0;
					nodeType = NODE_CUT;
					goto Finalize;
				}
			
				if(score > alpha)
				{
					bestMoveIndex = 0;
					nodeType = NODE_PV;
					hasValidMove = TRUE;
					ctx->PV[ply][0].From = tableEntry->BestMoveFrom;
					ctx->PV[ply][0].To = tableEntry->BestMoveTo;
					ctx->PV[ply][0].Promotion = tableEntry->Promotion;
					ctx->PV[ply][0].Score = score;
					ctx->PV[ply][0].Piece = Board_Piece(board, tableEntry->BestMoveFrom);

					memset(&(ctx->PV[ply][1]), 0, sizeof(MoveSmall) * (SEARCH_PLY_MAX - 1));
					alpha = tableEntry->Score;
				}
			}
			else if(tableEntry->NodeType == NODE_ALL || tableEntry->NodeType == NODE_PV)
			{
				if(score <= alpha)
				{
					addtoHashTable = FALSE;
					nodeType = NODE_ALL;
					goto Finalize;
				}
			}
		}
	}

	// ----------------------------------------------------------------------------------
	// ----------------------- Generate a list of all valid moves -----------------------
	// ----------------------------------------------------------------------------------

	Order order;
	order.OrderedMoves = 0;
	order.OrderStage = Order_StageInit;
	memset(order.MoveRank, 0, 100 * sizeof(int));

	order.MoveCount = Moves_GetAllMoves(board, order.MoveList);
	moveCount = order.MoveCount;

	// set the hash move if it is known
	if(hashHitPartial)
		Order_SetHashMove(ctx, &order, tableEntry->BestMoveFrom, tableEntry->BestMoveTo);

	#ifdef STATS_SEARCH
	SStats.MovesAtPly[ply] += order.MoveCount;
	#endif

	// ----------------------------------------------------------------------------------
	// ------------------------------ Null Move Reduction ------------------------------
	// ----------------------------------------------------------------------------------
	
	if(doNullMoveReduction)
	{
		_Bool nullValid = Board_MakeNullMove(board);
		if(nullValid)
		{
			callingParams.Depth = depth - 1 - Search_GetNullReduction(depth);
			callingParams.Ply = ply + 1;
			callingParams.AllowReductions = FALSE;
			callingParams.Reductions = params.Reductions;

			// instead of -alpha, use -beta+1
			int val = -Search_AlphaBeta(ctx, -beta, -beta+1, callingParams);
			Board_Unmake(board);

			if(SearchStopped)
				return 0;
			
			if(val >= beta) // Null move failed high. Reduce search depth
			{
				#ifdef STATS_SEARCH
				SStats.NullMoveReductions++;
				#endif

				nullReduction = Search_GetNullReduction(depth);
				callingParams.AllowReductions = FALSE;
				callingParams.Reductions = nullReduction;
			}
			else // Null move failed low. Do not readuce
			{
				nullReduction = 0;
				callingParams.AllowReductions = TRUE;
				callingParams.Reductions = params.Reductions;
			}
		}
	}
	else
	{
		nullReduction = 0;
		callingParams.AllowReductions = FALSE;
		callingParams.Reductions = 0;
	}

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

		// tries to promote the piece, if it can
		// Currenty, no under-promotions tried
		if(move->Promotion > 0)
			Board_Promote(board, to, move->Promotion);

		hasValidMove = true;

		callingParams.Depth = depth - 1 - nullReduction;
		callingParams.Ply = ply + 1;

		int val = 0;
		if(i == 0 || (depth < SEARCH_MIN_SCOUT_DEPTH) || !useScout) // normal search
		{
			val = -Search_AlphaBeta(ctx, -beta, -alpha, callingParams);
		}
		else // scout search
		{
			int b = alpha + 1;
			val = -Search_AlphaBeta(ctx, -b, -alpha, callingParams);

			// if search fails high, then do a re-search (normal search)
			// if val >= beta then there's no need to re-search, we'll just fail high
			if(val > alpha && val < beta) 
				val = -Search_AlphaBeta(ctx, -beta, -alpha, callingParams);
		}

		Board_Unmake(board);
		assert(hashBefore == board->Hash);

		if(SearchStopped)
			return 0;

		if(val > bestScore) // track all-node best score
			bestScore = val;

		if(val >= beta)
		{
			cutMoveIndex = i;
			bestMove = *move;
			nodeType = NODE_CUT;
			score = val;
			goto Finalize;
		}

		if(val > alpha)
		{
			useScout = TRUE; // we have improved alpha, turn the scout window on
			bestMoveIndex = i;
			bestMove = *move;
			nodeType = NODE_PV;
			ctx->PV[ply][0].From = from;
			ctx->PV[ply][0].To = to;
			ctx->PV[ply][0].Score = val;
			ctx->PV[ply][0].Piece = Board_Piece(board, from);
			ctx->PV[ply][0].Promotion = move->Promotion;

			memcpy(&(ctx->PV[ply][1]), &(ctx->PV[ply + 1][0]), sizeof(MoveSmall) * (SEARCH_PLY_MAX - 1));
			score = val;
			alpha = val;
		}
		
	}

	// ----------------------------------------------------------------------------------
	// ------------------------ Checkmate and stalemate handling ------------------------
	// ----------------------------------------------------------------------------------

	if(!hasValidMove)
	{
		bestMove.PlayerPiece = 0;

		if(Board_IsChecked(board, board->PlayerTurn))
			score = Search_Checkmate + ply;
		else
			score = Search_Stalemate;

		if(score > bestScore)
			bestScore = score;

		if(score >= beta)
		{
			cutMoveIndex = order.MoveCount;
			nodeType = NODE_CUT;
			goto Finalize;
		}

		if(score > alpha)
		{
			bestMoveIndex = order.MoveCount;
			nodeType = NODE_PV;
			memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * SEARCH_PLY_MAX);
			alpha = score;
		}
	}

	// ----------------------------------------------------------------------------------
	// ------------------------------- All-node handling -------------------------------
	// ----------------------------------------------------------------------------------

	if(nodeType == NODE_ALL)
	{
		if(bestScore == SEARCH_MIN_SCORE)
			score = alpha;
		else
			score = bestScore;
	}

	#ifdef DEBUG
	if(score < alpha)
		assert(nodeType == NODE_ALL);
	if(score > beta)
		assert(nodeType == NODE_CUT);

	if(nodeType == NODE_ALL)
		assert(score <= alpha);
	if(nodeType == NODE_CUT)
		assert(score >= beta);
	#endif

Finalize:

	#ifdef STATS_SEARCH

	if(hashHitPartial)
		SStats.HashHitsCount++;

	if(hashHitFull)
		SStats.HashFullHitCount++;

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
		if(bestMove.CapturePiece == 0)
		{
			Order_SetKillerMove(ctx->KillerMoves[ply], bestMove.From, bestMove.To, bestMove.PlayerPiece);
			ctx->History[bestMove.From][bestMove.To] += ply * ply;
		}
	}

	// ----------------------------------------------------------------------------------
	// ------------------------ Add node to transposition table ------------------------
	// ----------------------------------------------------------------------------------

	if(addtoHashTable)
	{
		TTableEntry newEntry;
		if(!(bestMove.From == 0 && bestMove.To == 0))
		{
			newEntry.BestMoveFrom = bestMove.From;
			newEntry.BestMoveTo = bestMove.To;
			newEntry.Promotion = bestMove.Promotion;
		}
		else
		{
			newEntry.BestMoveFrom = 0;
			newEntry.BestMoveTo = 0;
			newEntry.Promotion = 0;
		}
		newEntry.Depth = depth - nullReduction;
		newEntry.Hash = ctx->SearchBoard->Hash;
		newEntry.NodeType = nodeType;
		newEntry.Score = score; //(nodeType == NODE_CUT) ? beta : score;
		TTable_Insert(&newEntry);
	}

	return score;
}

int Search_Quiesce(SearchContext* ctx, int alpha, int beta, SearchParams params)
{
	if(SearchStopped)
		return 0;

	Board* board = ctx->SearchBoard;
	int ply = params.Ply;
	int depth = params.Depth;
	SearchParams callingParams = params;
	_Bool quiesce = (depth < 0);

	int nodeType = NODE_ALL;
	int score = 0;
	int moveCount = 0;
	int eval = 0;

	int bestMoveIndex = -1;
	int cutMoveIndex = -1;
	Move bestMove;
	bestMove.PlayerPiece = 0;
	_Bool hasValidMove = FALSE;
	_Bool isChecked = FALSE;
	_Bool generateAllMoves = FALSE;
	int bestScore = SEARCH_MIN_SCORE; // track all-node best score

	#ifdef STATS_SEARCH
	if(quiesce)
	{
		SStats.TotalNodeCount++;
		SStats.NodesAtPly[ply]++;
	}
	#endif

	// We can't stand pat if checked. Also, if checked, we try check evasion (generate all moves).
	isChecked = Board_IsChecked(ctx->SearchBoard, ctx->SearchBoard->PlayerTurn);
	
	if(isChecked)
		generateAllMoves = TRUE;

	// ----------------------------------------------------------------------------------
	// --------------------------------- Run Evaluate  ---------------------------------
	// ----------------------------------------------------------------------------------

	// check if we're too deep
	if(ply >= SEARCH_PLY_MAX - 1)
	{
		nodeType = NODE_EVAL;
		eval = Eval_Evaluate(board);
		score = eval;
		goto Finalize;
	}

	// Stand pat test
	if(!isChecked)
	{
		eval = Eval_Evaluate(board);
		score = eval;

		if(score >= beta)
		{
			nodeType = NODE_CUT;
			goto Finalize;
		}

		if(score > alpha)
		{
			nodeType = NODE_PV;
			memset(&(ctx->PV[ply][0]), 0, sizeof(MoveSmall) * SEARCH_PLY_MAX);
			alpha = score;
		}
	}
	
	// ----------------------------------------------------------------------------------
	// ----------------------- Generate a list of all valid moves -----------------------
	// ----------------------------------------------------------------------------------

	Order order;
	order.OrderedMoves = 0;
	order.OrderStage = Order_StageInit;
	memset(order.MoveRank, 0, 100 * sizeof(int));

	order.MoveCount = Moves_GetAllMoves(board, order.MoveList);
	moveCount = order.MoveCount;

	// If we're not doing check evasion, then filter away all moves except promotions and captures
	if(!generateAllMoves)
		moveCount = Order_QuiesceFilter(ctx, &order);

	// check if the move is quiet
	if(moveCount == 0 && !isChecked)
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

		// Bad capture detection
		if(move->CapturePiece > 0 && move->PlayerPiece > move->CapturePiece)
		{
			int seeExchange = SEE_Capture(board, move->From, move->To);
			if(seeExchange < 0)
			{
				#ifdef STATS_SEARCH
				SStats.PruneBadCaptures++;
				#endif

				continue;
			}
		}

		// Delta pruning
		if(!isChecked)
		{
			if(eval + Eval_PieceValues[move->CapturePiece] + Eval_PieceValues[move->Promotion] + DELTA_PRUNING_MARGIN <= alpha)
			{
				#ifdef STATS_SEARCH
				SStats.PruneDelta++;
				#endif

				continue;
			}
		}

		int from = move->From;
		int to = move->To;

		#ifdef DEBUG
		int pieceMoving = Board_Piece(board, from);
		uint64_t hashBefore = board->Hash;
		#endif

		_Bool valid = Board_Make(board, from, to);
		if(valid == FALSE)
			continue;

		// tries to promote the piece, if it can
		// Currenty, no under-promotions tried
		if(move->Promotion > 0)
			Board_Promote(board, to, move->Promotion);

		hasValidMove = true;

		callingParams.Depth = depth - 1;
		callingParams.Ply = ply + 1;

		int val = -Search_Quiesce(ctx, -beta, -alpha, callingParams);
		Board_Unmake(board);

		assert(hashBefore == board->Hash);

		if(val > bestScore)
			bestScore = val; // track all-node best move

		if(val >= beta)
		{
			cutMoveIndex = i;
			bestMove = *move;
			nodeType = NODE_CUT;
			score = val;
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
			ctx->PV[ply][0].Promotion = move->Promotion;

			memcpy(&(ctx->PV[ply][1]), &(ctx->PV[ply + 1][0]), sizeof(MoveSmall) * (SEARCH_PLY_MAX - 1));
			score = val;
			alpha = val;
		}
		
	}

	// ----------------------------------------------------------------------------------
	// ------------------------ Checkmate and stalemate handling ------------------------
	// ----------------------------------------------------------------------------------

	if(!hasValidMove && (isChecked || moveCount == 0))
	{
		if(isChecked)
			score = Search_Checkmate + ply;
		else
			score = Search_Stalemate;
		
		if(score > bestScore)
			bestScore = score;

		if(score >= beta)
		{
			nodeType = NODE_CUT;
			goto Finalize;
		}

		if(score > alpha)
		{
			nodeType = NODE_PV;
			alpha = score;
		}
	}

	// ----------------------------------------------------------------------------------
	// ------------------------------- All-node handling -------------------------------
	// ----------------------------------------------------------------------------------

	if(nodeType == NODE_ALL)
	{
		if(bestScore == SEARCH_MIN_SCORE)
			score = alpha; // returning eval as the lowest bound seemed to cause errors
		else
			score = bestScore;
	}

	#ifdef DEBUG
	if(score < alpha)
		assert(nodeType == NODE_ALL);
	if(score > beta)
		assert(nodeType == NODE_CUT);

	if(nodeType == NODE_ALL)
		assert(score <= alpha);
	if(nodeType == NODE_CUT)
		assert(score >= beta);
	#endif

Finalize:

	#ifdef STATS_SEARCH

	if(quiesce)
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
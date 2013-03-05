
#include "Search.h"
#include "Eval.h"
#include "Moves.h"
#include "Manager.h"
#include <stdio.h>

int Search_AlphaBeta(Search_Context* ctx, int depth, int alpha, int beta);

MoveSmall Search_SearchPos(Board* board, int searchDepth)
{
	Search_Context ctx;
	ctx.Board = board;

	// init stats
	ctx.Stats.AllNodeCount = 0;
	ctx.Stats.CutNodeCount = 0;
	ctx.Stats.EvalNodeCount = 0;
	ctx.Stats.PVNodeCount = 0;
	ctx.Stats.TotalNodeCount = 0;

	// create space for the PV table
	MoveSmall PV[Search_PlyMax][Search_PlyMax];
	for(int i = 0; i < Search_PlyMax; i++)
		ctx.PV[i] = PV[i];

	char text[8];
	Manager_Callback("Ply\tScore\tNodes\tPV\n");

	for(int i = 1; i <= searchDepth; i++)
	{
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
		sprintf(text, "%d\t", ctx.Stats.TotalNodeCount);
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

int Search_AlphaBeta(Search_Context* ctx, int depth, int alpha, int beta)
{
	Board* board = ctx->Board;
	MoveSmall** PV = ctx->PV;

	int ply = ctx->SearchDepth - depth;
	ctx->Stats.TotalNodeCount++;

	if ( depth == 0 ) 
	{
		ctx->Stats.EvalNodeCount++;
		int eval = Eval_Evaluate(board);
		if(board->PlayerTurn == COLOR_WHITE)
			return eval;
		else
			return -eval;
	}

	// set PV at this ply to zero
	memset(PV[ply], 0, sizeof(MoveSmall) * Search_PlyMax);

	uint64_t pieceBoard = (board->PlayerTurn == COLOR_WHITE) ? board->Boards[BOARD_WHITE] : board->Boards[BOARD_BLACK];
	uint8_t locations[64];
	int pieceCount = Bitboard_BitList(pieceBoard, locations);

	_Bool hasValidMove = FALSE;
	_Bool isPVNode = FALSE;

	for (int i=0; i < pieceCount; i++)
	{
		int from = locations[i];

		uint64_t destBoard = Moves_GetMoves(board, from);
		uint8_t destinations[64];
		int destCount = Bitboard_BitList(destBoard, destinations);

		for(int j=0; j < destCount; j++)
		{
			int to = destinations[j];

			#ifdef DEBUG
			int pieceMoving = Board_Piece(board, from);
			uint64_t hashBefore = board->Hash;
			if(16848271990604920209 == hashBefore)
				int k = 23;
			#endif

			_Bool valid = Board_Make(board, from, to);
			if(valid == FALSE)
				continue;

			// Todo: Handle promotions

			hasValidMove = true;
			int val = -Search_AlphaBeta(ctx, depth - 1, -beta, -alpha);
			Board_Unmake(board);

			#ifdef DEBUG
			if(hashBefore != board->Hash)
				int k = 23;
			#endif

			if(val >= beta)
			{
				ctx->Stats.CutNodeCount++;
				return beta;
			}

			if(val > alpha)
			{
				isPVNode = true;
				PV[ply][0].From = from;
				PV[ply][0].To = to;
				PV[ply][0].Score = val;
				PV[ply][0].Piece = Board_Piece(board, from);

				memcpy(&(PV[ply][1]), &(PV[ply + 1][0]), sizeof(MoveSmall) * (Search_PlyMax - ply));
				alpha = val;
			}
		}
	}

	// checkmate or stalemate
	if(hasValidMove == FALSE)
	{
		if(Board_IsChecked(board, board->PlayerTurn))
			return Search_Checkmate;
		else
			return Search_Stalemate;
	}

	if(isPVNode)
		ctx->Stats.PVNodeCount++;
	else
		ctx->Stats.AllNodeCount++;

	return alpha;
}
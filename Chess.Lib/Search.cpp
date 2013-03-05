
#include "Search.h"
#include "Eval.h"
#include "Moves.h"

typedef struct
{
	int TotalNodeCount;
	int EvalNodeCount;

	int CutNodeCount;
	int PVNodeCount;
	int AllNodeCount;

} StatsStruct;

MoveSmall Search_PV[Search_PlyMax][Search_PlyMax];
StatsStruct Stats;

int Search_AlphaBeta(Board* board, int depth, int alpha, int beta);

MoveSmall bestMove;
int bestScore;

void Search_SetDepth(int depth)
{
	Search_PlyCount = depth;
}

MoveSmall Search_SearchPos(Board* board)
{
	Search_AlphaBeta(board, Search_PlyCount, -999999, 999999);
	return bestMove;
}

int Search_AlphaBeta(Board* board, int depth, int alpha, int beta)
{
	int ply = Search_PlyCount - depth;
	Stats.TotalNodeCount++;

	if ( depth == 0 ) 
	{
		Stats.EvalNodeCount++;
		int eval = Eval_Evaluate(board);
		if(board->PlayerTurn == COLOR_WHITE)
			return eval;
		else
			return -eval;
	}

	// set PV at this ply to zero
	memset(Search_PV[ply], 0, sizeof(MoveSmall) * Search_PlyMax);

	uint64_t pieceBoard = (board->PlayerTurn == COLOR_WHITE) ? board->Boards[BOARD_WHITE] : board->Boards[BOARD_BLACK];
	uint8_t locations[64];
	int pieceCount = Bitboard_BitList(pieceBoard, locations);

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

			int val = -Search_AlphaBeta(board, depth - 1, -beta, -alpha);
			Board_Unmake(board);

			#ifdef DEBUG
			if(hashBefore != board->Hash)
				int k = 23;
			#endif

			if(val >= beta)
			{
				Stats.CutNodeCount++;
				return beta;
			}

			if(val > alpha)
			{
				isPVNode = true;
				Search_PV[ply][0].From = from;
				Search_PV[ply][0].To = to;

				memcpy(&(Search_PV[ply][1]), &(Search_PV[ply + 1][0]), Search_PlyMax - ply);

				alpha = val;
				if(ply == 0)
				{
					bestMove.From = from;
					bestMove.To = to;
					bestScore = val;
				}
			}
		}
	}

	if(isPVNode)
		Stats.PVNodeCount++;
	else
		Stats.AllNodeCount++;

	return alpha;
}
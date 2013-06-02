#ifndef SEARCH
#define SEARCH

#include "Default.h"
#include "Board.h"
#include "Move.h"

extern "C"
{
	// Absolute maximum depth, including quiescence search and extensions
	const int SEARCH_PLY_MAX = 40;

	// minimum depth for using scout windows. Scout doesn't provide a lot of benefit at bery shallow depth
	const int SEARCH_MIN_SCOUT_DEPTH = 3;

	// how much to reduce the search depth if null reduction fails high
	const int SEARCH_NULL_REDUCE_R = 4;

	// minimum number of plies to keep after null reduction
	const int SEARCH_NULL_REDUCE_MIN_PLY = 2;

	const int SEARCH_MIN_SCORE = -99999999;
	const int SEARCH_MAX_SCORE = 99999999;

	// node types
	const int NODE_UNKNOWN = 0;
	const int NODE_PV = 1; // exact score
	const int NODE_CUT = 2; // bound
	const int NODE_ALL = 3; // bound??
	const int NODE_EVAL = 4; // Evaluated leaf node

	// score for stalemate and checkmate, used when no valid move is found
	const int Search_Stalemate = 0;
	const int Search_Checkmate = -100000;
	const int Search_Draw = 0;

	const int DELTA_PRUNING_MARGIN = 200;

	#pragma pack(push, 1)
	typedef struct
	{
		uint64_t TotalNodeCount;
		uint64_t QuiescentNodeCount;		
		uint64_t EvalNodeCount;

		uint64_t CutNodeCount;
		uint64_t PVNodeCount;
		uint64_t AllNodeCount;

		uint64_t QCutNodeCount;
		uint64_t QPVNodeCount;
		uint64_t QAllNodeCount;

		uint64_t HashHitsCount;
		uint64_t HashFullHitCount;

		uint64_t EvalHits;
		uint64_t EvalTotal;

		uint64_t AttackMapCount;

		uint64_t PruneDelta;
		uint64_t PruneBadCaptures;

		uint64_t NullMoveReductions;

		// the index of best moves for PV nodes and cut moves for Cut nodes
		int BestMoveIndex[100];
		int CutMoveIndex[100];

		// the index of best moves in Quiescence nodes
		int QBestMoveIndex[100];
		int QCutMoveIndex[100];

		// number of nodes at each ply. Total sum should equal TotalNodeCount
		uint64_t NodesAtPly[SEARCH_PLY_MAX];

		// Number of total moves found at ply (counting all potential moves)
		// Note that most of these moves never get played, they get cut
		uint64_t MovesAtPly[SEARCH_PLY_MAX];

	} SearchStats;

	typedef struct
	{
		Board* SearchBoard;
		MoveSmall PV[SEARCH_PLY_MAX][SEARCH_PLY_MAX];
		MoveSmall KillerMoves[SEARCH_PLY_MAX][3];
		uint32_t History[64][64];

	} SearchContext;

	typedef struct
	{
		int8_t Ply;
		int8_t Depth;
		
		uint8_t AllowReductions;
		uint8_t AllowExtensions;

		uint8_t Extension;
		uint8_t Reductions;

	} SearchParams;
	#pragma pack(pop)

	#ifdef STATS_SEARCH
	extern SearchStats SStats;
	#endif

	__dllexport MoveSmall Search_SearchPos(Board* board, int searchDepth);
	__dllexport _Bool Search_DrawByRepetition(Board* board);

	// get some detailed statistics for the last search performed
	__dllexport SearchStats* Search_GetSearchStats();

	__dllexport void Search_StopSearch();

	// calculate how many plies to reduce
	__dllexport int Search_GetNullReduction(int depth);

	
	__inline_always int Search_GetNullReduction(int depth)
	{
		if(depth < SEARCH_NULL_REDUCE_MIN_PLY)
			return 0;

		if(depth >= SEARCH_NULL_REDUCE_R + SEARCH_NULL_REDUCE_MIN_PLY)
			return SEARCH_NULL_REDUCE_R;
		else
			return depth - SEARCH_NULL_REDUCE_MIN_PLY;
	}
}

#endif

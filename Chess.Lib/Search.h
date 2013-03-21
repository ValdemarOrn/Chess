#ifndef SEARCH
#define SEARCH

#include "Default.h"
#include "Board.h"
#include "Move.h"

extern "C"
{
	// Absolute maximum depth, including quiescence search and extensions
	const int SEARCH_PLY_MAX = 40;
	const int SEARCH_MIN_SCOUT_DEPTH = 3;
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
		Board* Board;
		MoveSmall PV[SEARCH_PLY_MAX][SEARCH_PLY_MAX];
		MoveSmall KillerMoves[SEARCH_PLY_MAX][3];
		uint32_t History[64][64];

	} SearchContext;

	typedef struct
	{
		int8_t Ply;
		int8_t Depth;
		uint16_t Flags;

	} SearchParams;
	#pragma pack(pop)

	// flags that indicate that side to move has been in check ever since it entered QS search
	const int SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_WHITE = 0x01;
	const int SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_BLACK = 0x02;
	// how many plies is this search extended
	const int SEARCH_FLAGS_EXTENSIONS = 0x0C;

	#ifdef STATS_SEARCH
	extern SearchStats SStats;
	#endif

	__declspec(dllexport) MoveSmall Search_SearchPos(Board* board, int searchDepth);
	__declspec(dllexport) _Bool Search_DrawByRepetition(Board* board);

	// get some detailed statistics for the last search performed
	__declspec(dllexport) SearchStats* Search_GetSearchStats();

	__declspec(dllexport) int Search_GetFlags(SearchParams* params, int flagType);
	__declspec(dllexport) void Search_SetFlags(SearchParams* params, int flagType, int value);

	// Read the search flags in the SearchParams struct
	__inline_always int Search_GetFlags(SearchParams* params, int flagType)
	{
		switch(flagType)
		{
		case SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_WHITE:
			return params->Flags & SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_WHITE;
		case SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_BLACK:
			return params->Flags & SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_BLACK >> 1;
		case SEARCH_FLAGS_EXTENSIONS:
			return (params->Flags & SEARCH_FLAGS_EXTENSIONS) >> 2;
		}

		return 0;
	}

	// Write flags in the SearchParams struct
	__inline_always void Search_SetFlags(SearchParams* params, int flagType, int value)
	{
		int newFlags = 0;

		switch(flagType)
		{
		case SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_WHITE:
			newFlags = SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_WHITE & value;
			params->Flags = (params->Flags & ~SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_WHITE) | newFlags;
			break;
		case SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_BLACK:
			newFlags = SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_BLACK & (value << 1);
			params->Flags = (params->Flags & ~SEARCH_FLAGS_QS_ALLOW_CHECK_EVADE_BLACK) | newFlags;
			break;
		case SEARCH_FLAGS_EXTENSIONS:
			newFlags = SEARCH_FLAGS_EXTENSIONS & (value << 2);
			params->Flags = (params->Flags & ~SEARCH_FLAGS_EXTENSIONS) | newFlags;
			break;
		}
	}
}

#endif

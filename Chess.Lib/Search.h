#ifndef SEARCH
#define SEARCH

#include "Default.h"
#include "Board.h"
#include "Move.h"

extern "C"
{
	// Absolute maximum depth, including quiescence search and extensions
	const int Search_PlyMax = 20;

	// node types
	const int NODE_UNKNOWN = 0;
	const int NODE_PV = 1; // exact score
	const int NODE_CUT = 2; // bound
	const int NODE_ALL = 3; // bound??
	const int NODE_EVAL = 4; // Evaluated leaf node
	const int NODE_NONE = 5; // no valid moves, mate or stalemate, exact score

	// score for stalemate and checkmate, used when no valid move is found
	const int Search_Stalemate = 0;
	const int Search_Checkmate = -100000;

	#pragma pack(push, 1)
	typedef struct
	{
		uint64_t TotalNodeCount;
		uint64_t EvalNodeCount;
		uint64_t QuiescentNodeCount;
		uint64_t NoneNodeCount;

		uint64_t CutNodeCount;
		uint64_t PVNodeCount;
		uint64_t AllNodeCount;

		// the index of best moves for PV nodes and cut moves for Cut nodes
		int BestMoveIndex[100];
		int CutMoveIndex[100];

		// number of nodes at each ply. Total sum should equal TotalNodeCount
		uint64_t NodesAtPly[Search_PlyMax];

		// Number of total moves found at ply (counting all potential moves)
		// Note that most of these moves never get played, they get cut
		uint64_t MovesAtPly[Search_PlyMax];

	} SearchStats;

	typedef struct
	{
		Board* Board;
		uint8_t SearchDepth;
		MoveSmall PV[Search_PlyMax][Search_PlyMax];
		MoveSmall KillerMoves[Search_PlyMax][3];
		uint32_t History[64][64];

	} SearchContext;

	#pragma pack(pop)

	__declspec(dllexport) MoveSmall Search_SearchPos(Board* board, int searchDepth);

	// get some detailed statistics for the last search performed
	__declspec(dllexport) SearchStats* Search_GetSearchStats();
}

#endif

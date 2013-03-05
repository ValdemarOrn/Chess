#ifndef SEARCH
#define SEARCH

#include "Default.h"
#include "Board.h"
#include "Move.h"

extern "C"
{
	// Absolute maximum depth, including quiescence search and extensions
	const int Search_PlyMax = 20;

	// score for stalemate and checkmate, used when no valid move is found
	const int Search_Stalemate = 0;
	const int Search_Checkmate = -100000;

	typedef struct
	{
		int TotalNodeCount;
		int EvalNodeCount;

		int CutNodeCount;
		int PVNodeCount;
		int AllNodeCount;

	} Search_Stats;

	typedef struct
	{
		Board* Board;
		uint8_t SearchDepth;
		Search_Stats Stats;
		MoveSmall* PV[Search_PlyMax];

	} Search_Context;

	__declspec(dllexport) MoveSmall Search_SearchPos(Board* board, int searchDepth);
}

#endif

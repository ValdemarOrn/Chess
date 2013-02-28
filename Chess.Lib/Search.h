#ifndef SEARCH
#define SEARCH

#include "Default.h"
#include "Board.h"
#include "Move.h"

extern "C"
{
	int Search_PlyCount;
	const int Search_PlyMax = 20;

	extern MoveSmall Search_PV[Search_PlyMax][Search_PlyMax];

	__declspec(dllexport) void Search_SetDepth(int depth);
	__declspec(dllexport) MoveSmall Search_SearchPos(Board* board);
}

#endif

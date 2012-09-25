#ifndef MOVES
#define MOVES

#include "inttypes.h"
#include "Board.h"
#include "Move.h"


extern "C"
{
	__declspec(dllexport) uint64_t Moves_GetMoves(Board* board, int tile);
	__declspec(dllexport) uint64_t Moves_GetAttacks(Board* board, int tile);
}

#endif
#ifndef PERFT
#define PERFT

#include "Default.h"
#include "Board.h"

extern "C"
{

	__declspec(dllexport) uint64_t Perft_Search(Board* board, int depth);
}

#endif

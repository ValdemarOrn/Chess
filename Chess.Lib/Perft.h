#ifndef PERFT
#define PERFT

#include "Default.h"
#include "Board.h"

extern "C"
{
	#pragma pack(push, 1)

	// Each entry show the first move, and the number of moves below it
	typedef struct
	{
		uint8_t From;
		uint8_t To;
		uint64_t Count;

	} PerftEntry;

	typedef struct
	{
		int StartDepth;
		PerftEntry* Entries;
		int EntryCount;
		uint64_t Total;

	} PerftResults;

	#pragma pack(pop)

	__declspec(dllexport) PerftResults* Perft_Search(Board* board, int depth);
}

#endif

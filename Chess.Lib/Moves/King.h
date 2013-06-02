#ifndef KING
#define KING

#include "../Default.h"

extern "C"
{
	__dllexport void King_Load(int pos, uint64_t moveBoard);
	__dllexport uint64_t King_Read(int pos);

	extern uint64_t King_Table[64];

	/// Loads a new move board into the KingTable array
	__inline_always void King_Load(int pos, uint64_t moveBoard)
	{
		King_Table[pos] = moveBoard;
	}

	__inline_always uint64_t King_Read(int pos)
	{
		uint64_t val = King_Table[pos];
		return val;
	}
}

#endif

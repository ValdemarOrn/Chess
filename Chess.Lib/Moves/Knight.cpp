
#include "Knight.h"

uint64_t KnightTable[64];

/// Loads a new move board into the KnightTable array
void Knight_Load(int pos, uint64_t moveBoard)
{
	KnightTable[pos] = moveBoard;
}

uint64_t Knight_Read(int pos)
{
	uint64_t val = KnightTable[pos];
	return val;
}

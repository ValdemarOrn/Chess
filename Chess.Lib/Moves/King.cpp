
#include "King.h"

uint64_t KingTable[64];

/// Loads a new move board into the KingTable array
void King_Load(int pos, uint64_t moveBoard)
{
	KingTable[pos] = moveBoard;
}

uint64_t King_Read(int pos)
{
	uint64_t val = KingTable[pos];
	return val;
}


#include "Bishop.h"
#include "BishopMagic.h"

uint64_t** BishopTables;

inline int GetIndex(int pos, uint64_t permutation)
{
	int index = (int)((permutation * BishopMagic[pos]) >> (64 - BishopBits[pos]));
	return index;
}

/// Create all lookup tables and initialize them to zeros
void Bishop_SetupTables()
{
	BishopTables = new uint64_t*[64];

	for(int i=0; i<64; i++)
	{
		int bits = BishopBits[i];
		int len = 1 << bits;
		BishopTables[i] = new uint64_t[len];
		for(int k=0; k<len; k++)
			BishopTables[i][k] = (uint64_t)0;
	}
}

/// Loads a new entry into the LUT. If if the table contains an unexpected value (collision)
/// the function returns 1. Returns 0 on success.
/// Note: There should never, ever be a collision if the magic constant is correct. 
/// This is only for testing and safety reasons.
int Bishop_Load(int pos, uint64_t permutation, uint64_t moveBoard)
{
	uint64_t* table = BishopTables[pos];
	int index = GetIndex(pos, permutation);

	if(table[index] == 0 || table[index] == moveBoard)
	{
		table[index] = moveBoard;
		return 0;
	}
	else
		return 1;

}

uint64_t Bishop_Read(int pos, uint64_t permutation)
{
	uint64_t* table = BishopTables[pos];
	int index = GetIndex(pos, permutation);

	uint64_t moveBoard = table[index];
	return moveBoard;
}

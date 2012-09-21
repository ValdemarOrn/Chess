
#include "Rook.h"
#include "RookMagic.h"

uint64_t** RookTables;

inline int RookIndex(int pos, uint64_t permutation)
{
	int index = (int)((permutation * RookMagic[pos]) >> (64 - RookBits[pos]));
	return index;
}

/// Create all lookup tables and initialize them to zeros
void Rook_SetupTables()
{
	RookTables = new uint64_t*[64];

	for(int i=0; i<64; i++)
	{
		int bits = RookBits[i];
		int len = 1 << bits;
		RookTables[i] = new uint64_t[len];
		for(int k=0; k<len; k++)
			RookTables[i][k] = (uint64_t)0;
	}
}

/// Loads a new entry into the LUT. If if the table contains an unexpected value (collision)
/// the function returns 1. Returns 0 on success.
/// Note: There should never, ever be a collision if the magic constant is correct. 
/// This is only for testing and safety reasons.
int Rook_Load(int pos, uint64_t permutation, uint64_t moveBoard)
{
	uint64_t* table = RookTables[pos];
	int index = RookIndex(pos, permutation);

	if(table[index] == 0 || table[index] == moveBoard)
	{
		table[index] = moveBoard;
		return 0;
	}
	else
		return 1;

}

uint64_t Rook_Read(int pos, uint64_t permutation)
{
	uint64_t* table = RookTables[pos];
	int index = RookIndex(pos, permutation);

	uint64_t moveBoard = table[index];
	return moveBoard;
}

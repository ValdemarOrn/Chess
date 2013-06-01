#ifndef BISHOP
#define BISHOP

#include "../Default.h"
#include "BishopMagic.h"

extern "C"
{
	__declspec(dllexport) void Bishop_SetupTables();
	__declspec(dllexport) int Bishop_Load(int pos, uint64_t permutation, uint64_t moveBoard);
	__declspec(dllexport) void Bishop_LoadVector(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t Bishop_Read(int pos, uint64_t occupancy);

	extern uint64_t** Bishop_Tables;

	// an array containing bitboard with the unmasked bishop move vectors
	extern uint64_t Bishop_Vectors[64];

	// an array containing the number of bits to shift the magic constant to get the index
	extern uint64_t Bishop_Shift[64];

	inline int Bishop_Index(int pos, uint64_t permutation)
	{
		uint64_t val = (permutation * Bishop_Magic[pos]);
		uint32_t shift = (int)Bishop_Shift[pos];
		int index = (int)(val >> shift);
		return index;
	}

	/// Create all lookup tables and initialize them to zeros
	inline void Bishop_SetupTables()
	{
		Bishop_Tables = new uint64_t*[64];

		for(int i=0; i<64; i++)
		{
			uint64_t bits = Bishop_Bits[i];
			Bishop_Shift[i] = 64 - Bishop_Bits[i];
			int len = 1 << bits;
			Bishop_Tables[i] = new uint64_t[len];
			for(int k=0; k<len; k++)
				Bishop_Tables[i][k] = (uint64_t)0;
		}
	}

	/// Loads a new entry into the LUT. If if the table contains an unexpected value (collision)
	/// the function returns 1. Returns 0 on success.
	/// Note: There should never, ever be a collision if the magic constant is correct. 
	/// This is only for testing and safety reasons.
	inline int Bishop_Load(int pos, uint64_t permutation, uint64_t moveBoard)
	{
		uint64_t* table = Bishop_Tables[pos];
		int index = Bishop_Index(pos, permutation);

		if(table[index] == 0 || table[index] == moveBoard)
		{
			table[index] = moveBoard;
			return 0;
		}
		else
			return 1;

	}

	inline void Bishop_LoadVector(int pos, uint64_t moveBoard)
	{
		Bishop_Vectors[pos] = moveBoard;
	}

	__inline_always uint64_t Bishop_Read(int pos, uint64_t occupancy)
	{
		uint64_t permutation = occupancy & Bishop_Vectors[pos];

		uint64_t* table = Bishop_Tables[pos];
		int index = Bishop_Index(pos, permutation);

		uint64_t moveBoard = table[index];
		return moveBoard;
	}

}

#endif

#ifndef ROOK
#define ROOK

#include "../Default.h"
#include "RookMagic.h"

extern "C"
{
	__declspec(dllexport) void Rook_SetupTables();
	__declspec(dllexport) int Rook_Load(int pos, uint64_t permutation, uint64_t moveBoard);
	__declspec(dllexport) void Rook_LoadVector(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t Rook_Read(int pos, uint64_t occupancy);

	uint64_t** Rook_Tables;

	// an array containing bitboard with the unmasked rook move vectors
	uint64_t Rook_Vectors[64];

	// an array containing the number of bits to shift the magic constant to get the index
	uint64_t Rook_Shift[64];

	inline int Rook_Index(int pos, uint64_t permutation)
	{
		int index = (int)((permutation * Rook_Magic[pos]) >> Rook_Shift[pos]);
		return index;
	}

	/// Create all lookup tables and initialize them to zeros
	void Rook_SetupTables()
	{
		Rook_Tables = new uint64_t*[64];

		for(int i=0; i<64; i++)
		{
			int bits = Rook_Bits[i];
			Rook_Shift[i] = 64 - Rook_Bits[i];
			int len = 1 << bits;
			Rook_Tables[i] = new uint64_t[len];
			for(int k=0; k<len; k++)
				Rook_Tables[i][k] = (uint64_t)0;
		}
	}

	/// Loads a new entry into the LUT. If if the table contains an unexpected value (collision)
	/// the function returns 1. Returns 0 on success.
	/// Note: There should never, ever be a collision if the magic constant is correct. 
	/// This is only for testing and safety reasons.
	int Rook_Load(int pos, uint64_t permutation, uint64_t moveBoard)
	{
		uint64_t* table = Rook_Tables[pos];
		int index = Rook_Index(pos, permutation);

		if(table[index] == 0 || table[index] == moveBoard)
		{
			table[index] = moveBoard;
			return 0;
		}
		else
			return 1;

	}

	void Rook_LoadVector(int pos, uint64_t moveBoard)
	{
		Rook_Vectors[pos] = moveBoard;
	}

	__inline_always uint64_t Rook_Read(int pos, uint64_t occupancy)
	{
		uint64_t permutation = occupancy & Rook_Vectors[pos];

		uint64_t* table = Rook_Tables[pos];
		int index = Rook_Index(pos, permutation);

		uint64_t moveBoard = table[index];
		return moveBoard;
	}

}

#endif
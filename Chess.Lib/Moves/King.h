#ifndef KING
#define KING

#include "../Default.h"

extern "C"
{
	__declspec(dllexport) void King_Load(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t King_Read(int pos);

	uint64_t King_Table[64];

	/// Loads a new move board into the KingTable array
	void King_Load(int pos, uint64_t moveBoard)
	{
		King_Table[pos] = moveBoard;
	}

	inline uint64_t King_Read(int pos)
	{
		uint64_t val = King_Table[pos];
		return val;
	}
}

#endif
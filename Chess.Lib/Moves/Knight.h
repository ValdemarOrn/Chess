#ifndef KNIGHT
#define KNIGHT

#include "../Default.h"

extern "C"
{
	__declspec(dllexport) void Knight_Load(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t Knight_Read(int pos);

	uint64_t Knight_Table[64];

	/// Loads a new move board into the KnightTable array
	void Knight_Load(int pos, uint64_t moveBoard)
	{
		Knight_Table[pos] = moveBoard;
	}

	inline uint64_t Knight_Read(int pos)
	{
		uint64_t val = Knight_Table[pos];
		return val;
	}

}

#endif

#ifndef QUEEN
#define QUEEN

#include "Rook.h"
#include "Bishop.h"
#include "../Default.h"

extern "C"
{
	__declspec(dllexport) uint64_t Queen_Read(int pos, uint64_t occupancy);

	__inline_always uint64_t Queen_Read(int pos, uint64_t occupancy)
	{
		uint64_t rookAttacks = Rook_Read(pos, occupancy);
		uint64_t bishopAttacks = Bishop_Read(pos, occupancy);
		return rookAttacks | bishopAttacks;
	}
}

#endif
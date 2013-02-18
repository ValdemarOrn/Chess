
#include "Queen.h"
#include "Rook.h"
#include "Bishop.h"


uint64_t Queen_Read(int pos, uint64_t occupancy)
{
	uint64_t rookAttacks = Rook_Read(pos, occupancy);
	uint64_t bishopAttacks = Bishop_Read(pos, occupancy);
	return rookAttacks | bishopAttacks;
}

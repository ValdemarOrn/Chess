
#include "Bitboard.h"
#include <intrin.h>
#pragma intrinsic(_BitScanForward)

// ------------------------ Basic Bitboard twiddling --------------------------

uint64_t Bitboard_Unset(uint64_t val, int index)
{
	uint64_t inv = ~(uint64_t)((uint64_t)1 << index);
	return val & inv;
}

uint64_t Bitboard_Set(uint64_t val, int index)
{
	uint64_t mask = (uint64_t)((uint64_t)1 << index);
	return val | mask;
}

void Bitboard_UnsetRef(uint64_t* val, int index)
{
	uint64_t inv = ~(uint64_t)((uint64_t)1 << index);
	*val = *val & inv;
}

void Bitboard_SetRef(uint64_t* val, int index)
{
	uint64_t mask = ((uint64_t)1 << index);
	*val = *val | mask;
}


// ----------------------------------------------------

int Bitboard_Get(uint64_t val, int index)
{
	uint64_t mask = ((uint64_t)1 << index);
	return ((val & mask) > 0) ? 1 : 0;
}

uint64_t Bitboard_Make(int* tiles, int count)
{
	uint64_t bitboard = 0;
	for(int i=0; i<count; i++)
		bitboard = Bitboard_Set(bitboard, tiles[i]);
	
	return bitboard;
}

// ------------------------- Bitscan functions --------------------------

//  least significant 1 bit (LSB)
int Bitboard_ForwardBit(uint64_t val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanForward64(&index, val);
	
	if(isNonzero)
		return index;
	else
		return -1;
}

// most significant 1 bit (MSB) 
int Bitboard_ReverseBit(uint64_t val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanReverse64(&index, val);
	
	if(isNonzero)
		return index;
	else
		return -1;
}

// Count number of 1 bits
int Bitboard_PopCount(uint64_t val)
{
	unsigned long count = __popcnt64(val);
	return (int)count;
}

void Bitboard_BitList(uint64_t val, uint8_t* outputList_s28, int* count)
{
	int i = 0;
	while(true)
	{
		int fwd = Bitboard_ForwardBit(val);
		if(fwd == -1)
			break;

		val = Bitboard_Unset(val, fwd);
		uint8_t byte = (uint8_t)fwd;
		outputList_s28[i] = byte;
		i++;
	}

	*count = i;
}


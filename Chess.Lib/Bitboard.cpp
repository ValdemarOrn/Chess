
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

int Bitboard_ForwardBit(uint64_t val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanForward64(&index, val);
	return index;
}

int Bitboard_ReverseBit(uint64_t val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanReverse64(&index, val);
	return (int)index;
}

int Bitboard_PopCount(uint64_t val)
{
	unsigned long count = __popcnt64(val);
	return (int)count;
}

int Bitboard_TryMany()
{
	unsigned long mask = 0x1000;
	unsigned int output = 0;

	for(int i=0; i<1000000; i++)
	{
		mask = i;
		unsigned int index = Bitboard_ForwardBit(mask);
		output += index;
	}

	return (int)output;
}



#include "Bitscan.h"
#include <intrin.h>
#pragma intrinsic(_BitScanForward)

int Bitscan_ForwardBit(uint64_t val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanForward64(&index, val);
	return index;
}

int Bitscan_ReverseBit(uint64_t val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanReverse64(&index, val);
	return (int)index;
}

int Bitscan_PopCount(uint64_t val)
{
	unsigned long count = __popcnt64(val);
	return (int)count;
}

int Bitscan_TryMany()
{
	unsigned long mask = 0x1000;
	unsigned int output = 0;

	for(int i=0; i<1000000; i++)
	{
		mask = i;
		unsigned int index = Bitscan_ForwardBit(mask);
		output += index;
	}

	return (int)output;
}


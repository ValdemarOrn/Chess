#ifndef BITSCAN
#define BITSCAN

#include "inttypes.h"

extern "C"
{
	__declspec(dllexport) int Bitscan_ForwardBit(uint64_t val);
	__declspec(dllexport) int Bitscan_ReverseBit(uint64_t val);
	__declspec(dllexport) int Bitscan_PopCount(uint64_t val);
	__declspec(dllexport) int Bitscan_TryMany();
}

#endif
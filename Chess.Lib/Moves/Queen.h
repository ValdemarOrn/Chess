#ifndef QUEEN
#define QUEEN

#include "../inttypes.h"

extern "C"
{
	__declspec(dllexport) uint64_t Queen_Read(int pos, uint64_t occupancy);
}

#endif
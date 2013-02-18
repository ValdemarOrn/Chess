#ifndef KNIGHT
#define KNIGHT

#include "../inttypes.h"

extern "C"
{
	__declspec(dllexport) void Knight_Load(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t Knight_Read(int pos);
}

#endif
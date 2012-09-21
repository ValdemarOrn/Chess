#ifndef KING
#define KING

#include "inttypes.h"

extern "C"
{
	__declspec(dllexport) void King_Load(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t King_Read(int pos);
}

#endif
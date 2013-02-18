#ifndef BISHOP
#define BISHOP

#include "../inttypes.h"

extern "C"
{
	__declspec(dllexport) void Bishop_SetupTables();
	__declspec(dllexport) int Bishop_Load(int pos, uint64_t permutation, uint64_t moveBoard);
	__declspec(dllexport) void Bishop_LoadVector(int pos, uint64_t moveBoard);
	__declspec(dllexport) uint64_t Bishop_Read(int pos, uint64_t occupancy);
}

#endif
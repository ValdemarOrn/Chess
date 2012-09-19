#ifndef ROOK
#define ROOK

#include "inttypes.h"

extern "C"
{
	__declspec(dllexport) void Rook_SetupTables();
	__declspec(dllexport) int Rook_Load(int pos, uint64_t permutation, uint64_t moveBoard);
	__declspec(dllexport) uint64_t Rook_Read(int pos, uint64_t permutation);
}

#endif
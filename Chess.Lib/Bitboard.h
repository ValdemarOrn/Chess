#ifndef BITBOARD
#define BITBOARD

#include "inttypes.h"

extern "C"
{
	// ------------- Bit Twiddling -------------

	__declspec(dllexport) uint64_t Bitboard_Unset(uint64_t val, int index);
	__declspec(dllexport) uint64_t Bitboard_Set(uint64_t val, int index);
	__declspec(dllexport) void Bitboard_UnsetRef(uint64_t* val, int index);
	__declspec(dllexport) void Bitboard_SetRef(uint64_t* val, int index);
	__declspec(dllexport) int Bitboard_Get(uint64_t val, int index);
	__declspec(dllexport) uint64_t Bitboard_Make(int* tiles, int count);

	// ------------- Bitscan functions -------------

	__declspec(dllexport) int Bitboard_ForwardBit(uint64_t val);
	__declspec(dllexport) int Bitboard_ReverseBit(uint64_t val);
	__declspec(dllexport) int Bitboard_PopCount(uint64_t val);
	__declspec(dllexport) int Bitboard_TryMany();
}

#endif
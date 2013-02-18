#ifndef PAWN
#define PAWN

#include "../inttypes.h"

extern "C"
{
	__declspec(dllexport) void Pawn_LoadWhiteMove(int pos, uint64_t moveBoard);
	__declspec(dllexport) void Pawn_LoadBlackMove(int pos, uint64_t moveBoard);
	__declspec(dllexport) void Pawn_LoadWhiteAttack(int pos, uint64_t moveBoard);
	__declspec(dllexport) void Pawn_LoadBlackAttack(int pos, uint64_t moveBoard);

	__declspec(dllexport) uint64_t Pawn_ReadWhiteMove(int pos);
	__declspec(dllexport) uint64_t Pawn_ReadBlackMove(int pos);
	__declspec(dllexport) uint64_t Pawn_ReadWhiteAttack(int pos);
	__declspec(dllexport) uint64_t Pawn_ReadBlackAttack(int pos);
}

#endif
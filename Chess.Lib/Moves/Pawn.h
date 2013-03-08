#ifndef PAWN
#define PAWN

#include "../Default.h"

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

	uint64_t Pawn_TableWhiteMove[64];
	uint64_t Pawn_TableBlackMove[64];
	uint64_t Pawn_TableWhiteAttack[64];
	uint64_t Pawn_TableBlackAttack[64];

	/// Loads a new move board into the array
	inline void Pawn_LoadWhiteMove(int pos, uint64_t moveBoard)
	{
		Pawn_TableWhiteMove[pos] = moveBoard;
	}

	/// Loads a new move board into the array
	inline void Pawn_LoadBlackMove(int pos, uint64_t moveBoard)
	{
		Pawn_TableBlackMove[pos] = moveBoard;
	}

	/// Loads a new move board into the array
	inline void Pawn_LoadWhiteAttack(int pos, uint64_t moveBoard)
	{
		Pawn_TableWhiteAttack[pos] = moveBoard;
	}

	/// Loads a new move board into the array
	inline void Pawn_LoadBlackAttack(int pos, uint64_t moveBoard)
	{
		Pawn_TableBlackAttack[pos] = moveBoard;
	}

	// -------------------------------------------------------

	__inline_always uint64_t Pawn_ReadWhiteMove(int pos)
	{
		uint64_t val = Pawn_TableWhiteMove[pos];
		return val;
	}

	__inline_always uint64_t Pawn_ReadBlackMove(int pos)
	{
		uint64_t val = Pawn_TableBlackMove[pos];
		return val;
	}

	__inline_always uint64_t Pawn_ReadWhiteAttack(int pos)
	{
		uint64_t val = Pawn_TableWhiteAttack[pos];
		return val;
	}

	__inline_always uint64_t Pawn_ReadBlackAttack(int pos)
	{
		uint64_t val = Pawn_TableBlackAttack[pos];
		return val;
	}

}

#endif

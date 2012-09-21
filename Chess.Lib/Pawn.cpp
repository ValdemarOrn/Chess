
#include "Pawn.h"

uint64_t PawnTableWhiteMove[64];
uint64_t PawnTableBlackMove[64];
uint64_t PawnTableWhiteAttack[64];
uint64_t PawnTableBlackAttack[64];

/// Loads a new move board into the array
void Pawn_LoadWhiteMove(int pos, uint64_t moveBoard)
{
	PawnTableWhiteMove[pos] = moveBoard;
}

/// Loads a new move board into the array
void Pawn_LoadBlackMove(int pos, uint64_t moveBoard)
{
	PawnTableBlackMove[pos] = moveBoard;
}

/// Loads a new move board into the array
void Pawn_LoadWhiteAttack(int pos, uint64_t moveBoard)
{
	PawnTableWhiteAttack[pos] = moveBoard;
}

/// Loads a new move board into the array
void Pawn_LoadBlackAttack(int pos, uint64_t moveBoard)
{
	PawnTableBlackAttack[pos] = moveBoard;
}

// -------------------------------------------------------

uint64_t Pawn_ReadWhiteMove(int pos)
{
	uint64_t val = PawnTableWhiteMove[pos];
	return val;
}

uint64_t Pawn_ReadBlackMove(int pos)
{
	uint64_t val = PawnTableBlackMove[pos];
	return val;
}

uint64_t Pawn_ReadWhiteAttack(int pos)
{
	uint64_t val = PawnTableWhiteAttack[pos];
	return val;
}

uint64_t Pawn_ReadBlackAttack(int pos)
{
	uint64_t val = PawnTableBlackAttack[pos];
	return val;
}

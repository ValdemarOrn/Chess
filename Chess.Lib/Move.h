#ifndef MOVE
#define MOVE

#include "Default.h"

extern "C"
{
	typedef struct
	{
		uint32_t MoveCount;

		uint8_t From;
		uint8_t To;
		
		uint8_t Color;
		uint8_t Capture;
		uint8_t CaptureTile; // only differs from "To" in en passant attacks
		uint8_t Promotion;
		uint8_t CheckmateState;
		uint8_t Castle;
		
		uint8_t PrevAttacksWhite;
		uint8_t PrevAttacksBlack;
		uint8_t PrevHash;

		uint8_t PrevEnPassantTile;
		uint8_t PrevFiftyMoveRulePlies;
		uint8_t PrevCastleState;
		uint8_t PrevCheckmateState;

		uint8_t Unused; // make it multiple of 4 bytes

	} Move;

}

#endif

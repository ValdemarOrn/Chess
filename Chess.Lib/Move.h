#ifndef MOVE
#define MOVE

#include "Default.h"

extern "C"
{
	#pragma pack(push, 1)
	typedef struct
	{
		uint8_t From;
		uint8_t To;
		
		uint8_t PlayerColor;
		uint8_t PlayerPiece;
		uint8_t CapturePiece;
		uint8_t CaptureTile; // only differs from "To" in en passant attacks
		uint8_t Promotion;
		uint8_t CheckState;
		uint8_t Castle;

	} Move;
	
	typedef struct
	{
		Move Move;
		
		uint64_t PrevAttacksWhite;
		uint64_t PrevAttacksBlack;
		uint64_t PrevHash;

		uint8_t PrevEnPassantTile;
		uint8_t PrevFiftyMoveRulePlies;
		uint8_t PrevCastleState;
		uint8_t PrevCheckState;

	} MoveHistory;
	#pragma pack(pop)
}

#endif

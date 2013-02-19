#ifndef MOVE
#define MOVE

#include "inttypes.h"

extern "C"
{
	typedef struct
	{
		uint32_t PlyCount;

		uint8_t From;
		uint8_t To;
		
		uint8_t Color;
		uint8_t Capture;
		uint8_t CaptureTile; // only differs from "To" in en passant attacks
		uint8_t Promotion;
		uint8_t CheckMateState;
		
		uint8_t PreviousChecMateState;
		uint8_t PreviousCastleState;
		uint8_t PreviousEnPassantTile;

	} Move;
	/*
	__declspec(dllexport) Move* Move_CreateSimple(uint8_t from, uint8_t to);

	__declspec(dllexport) Move* Move_Create(
		uint8_t from,
		uint8_t to,
		uint8_t moveCount,
		uint8_t color,
		uint8_t capture,
		uint8_t captureTile,
		uint8_t promotion,
		
		uint8_t check,
		uint8_t mate,
		uint8_t queenside,
		uint8_t kingside);

	__declspec(dllexport) void Move_Delete(Move* move);
	*/
}

#endif

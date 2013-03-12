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
		uint8_t Castle;

	} Move;

	typedef struct
	{
		uint8_t From;
		uint8_t To;
		uint8_t Piece;
		int Score;

	} MoveSmall;
	
	typedef struct
	{
		Move Move;
		
		uint64_t PrevAttacksWhite;
		uint64_t PrevAttacksBlack;
		uint64_t PrevHash;

		uint8_t PrevEnPassantTile;
		uint8_t PrevFiftyMoveRulePlies;
		uint8_t PrevCastleState;

	} MoveHistory;
	#pragma pack(pop)

	__declspec(dllexport) void Move_ToString(MoveSmall* move, char* dest);
	__declspec(dllexport) void Move_SquareToString(int square, char* dest);
	__declspec(dllexport) void Move_PieceToString(int piece, char* dest);

	__declspec(dllexport) _Bool Move_Equal(Move* move, MoveSmall* moveSmall);

	__inline_always _Bool Move_Equal(Move* move, MoveSmall* moveSmall)
	{
		return (move->From == moveSmall->From && move->To && moveSmall->To && move->PlayerPiece == moveSmall->Piece);
	}
}

#endif

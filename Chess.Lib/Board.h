#ifndef BOARD
#define BOARD

#include "inttypes.h"
#include "Move.h"

extern "C"
{
	#pragma pack(1)
	typedef struct
	{
		uint64_t White;
		uint64_t Black;

		uint64_t Pawns;
		uint64_t Knights;
		uint64_t Bishops;
		uint64_t Rooks;
		uint64_t Queens;
		uint64_t Kings;

		uint32_t MoveCount;

		uint8_t PlayerTurn;
		uint8_t EnPassantTile;
		uint8_t FiftyMoveRulePlies;

		uint8_t CastleKW;
		uint8_t CastleQW;
		uint8_t CastleKB;
		uint8_t CastleQB;

	} Board;

	__declspec(dllexport) Board* Board_Copy(Board* board);
	__declspec(dllexport) int Board_X(Board* board, int tile);
	__declspec(dllexport) int Board_Y(Board* board, int tile);
	__declspec(dllexport) void Board_Color(Board* board, int square);
	__declspec(dllexport) int Board_Piece(Board* board, int square);

	__declspec(dllexport) int Board_Move(Board* board, int from, int to, int verifyLegalMove);
	__declspec(dllexport) int Board_Move(Board* board, Move* move, int verifyLegalMove);
	__declspec(dllexport) int Board_Promote(Board* board, int square, int pieceType);

	__declspec(dllexport) void Board_CheckCastling(Board* board);
	__declspec(dllexport) void Board_InitBoard(Board* board);
	__declspec(dllexport) void Board_AllowCastlingAll(Board* board);
}

#endif
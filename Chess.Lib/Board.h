#ifndef BOARD
#define BOARD

#include "inttypes.h"
#include "Move.h"

extern "C"
{
	const int COLOR_WHITE = 1 << 4;
	const int COLOR_BLACK = 2 << 4;

	const int PIECE_PAWN = 1;
	const int PIECE_ROOK = 2;
	const int PIECE_BISHOP = 3;
	const int PIECE_KNIGHT = 4;
	const int PIECE_QUEEN = 5;
	const int PIECE_KING = 6;

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

	__declspec(dllexport) Board* Board_Create();
	__declspec(dllexport) void Board_Delete(Board* board);

	__declspec(dllexport) Board* Board_Copy(Board* board);
	__declspec(dllexport) int Board_X(int tile);
	__declspec(dllexport) int Board_Y(int tile);
	__declspec(dllexport) int Board_Color(Board* board, int square);
	__declspec(dllexport) int Board_Piece(Board* board, int square);

	__declspec(dllexport) int Board_Make(Board* board, int from, int to, int verifyLegalMove);
	__declspec(dllexport) int Board_MakeMove(Board* board, Move* move, int verifyLegalMove);
	__declspec(dllexport) int Board_Unmake(Board* board, Move* move, int verifyLegalMove);
	__declspec(dllexport) int Board_Promote(Board* board, int square, int pieceType);

	__declspec(dllexport) void Board_CheckCastling(Board* board);
	__declspec(dllexport) void Board_InitBoard(Board* board);
	__declspec(dllexport) void Board_AllowCastlingAll(Board* board);
}

#endif
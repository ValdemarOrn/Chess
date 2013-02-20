#ifndef BOARD
#define BOARD

#include "Default.h"
#include "Move.h"
#include "Bitboard.h"

extern "C"
{
	const int COLOR_WHITE = 1 << 4;
	const int COLOR_BLACK = 2 << 4;

	const int PIECE_PAWN = 2;
	const int PIECE_KNIGHT = 3;
	const int PIECE_BISHOP = 4;
	const int PIECE_ROOK = 5;
	const int PIECE_QUEEN = 6;
	const int PIECE_KING = 7;

	const int CASTLE_WK = 1 << 0;
	const int CASTLE_WQ = 1 << 1;
	const int CASTLE_BK = 1 << 2;
	const int CASTLE_BQ = 1 << 3;
	
	const int CHECK_NONE = 0;
	const int CHECK_CHECK = 1;
	const int CHECK_MATE = 2;
	
	const int BOARD_WHITE = 0;
	const int BOARD_BLACK = 1;
	const int BOARD_PAWNS = 2;
	const int BOARD_KNIGHTS = 3;
	const int BOARD_BISHOPS = 4;
	const int BOARD_ROOKS = 5;
	const int BOARD_QUEENS = 6;
	const int BOARD_KINGS = 7;

	#pragma pack(push, 1)
	typedef struct
	{
		uint64_t Boards[8];

		uint64_t Hash;
		uint32_t CurrentMove;

		uint8_t PlayerTurn;
		uint8_t EnPassantTile; // set to 0 if not available. tile 0 can never be an en-passant square anyway
		uint8_t FiftyMoveRulePlies;
		uint8_t Castle;
		uint8_t CheckmateState;
		
		Move* Moves;

	} Board;
	#pragma pack(pop)

	__declspec(dllexport) Board* Board_Create();
	__declspec(dllexport) void Board_Delete(Board* board);
	__declspec(dllexport) Board* Board_Copy(Board* board);
	__declspec(dllexport) void Board_Init(Board* board, int setPieces);

	__declspec(dllexport) int Board_X(int tile);
	__declspec(dllexport) int Board_Y(int tile);
	__declspec(dllexport) int Board_Color(Board* board, int square);
	__declspec(dllexport) int Board_Piece(Board* board, int square);

	__declspec(dllexport) void Board_SetPiece(Board* board, int square, int pieceType, int color);
	__declspec(dllexport) void Board_ClearPiece(Board* board, int square);

	__declspec(dllexport) int Board_Make(Board* board, int from, int to, int verifyLegalMove);
	__declspec(dllexport) int Board_Unmake(Board* board, int verifyLegalMove);
	__declspec(dllexport) int Board_Promote(Board* board, int square, int pieceType);

	__declspec(dllexport) void Board_CheckCastling(Board* board);
	__declspec(dllexport) void Board_AllowCastlingAll(Board* board);

	// ------------ Inline function definitions

	__inline_always int Board_X(int tile)
	{
		// 128 constant prevent negative numbers in modulo output
		return (128 + tile) % 8;
	}

	__inline_always int Board_Y(int tile)
	{
		return tile >> 3; // ( x >> 3 == x / 8)
	}

	__inline_always int Board_Color(Board* board, int tile)
	{
		if(Bitboard_Get(board->Boards[BOARD_WHITE], tile) == 1)
			return COLOR_WHITE;
	
		if(Bitboard_Get(board->Boards[BOARD_BLACK], tile) == 1)
			return COLOR_BLACK;
	
		return 0;
	}
}

#endif


#include "Moves.h"
#include "Bitboard.h"
#include "Board.h"
#include "Moves/Pawn.h"
#include "Moves/Knight.h"
#include "Moves/Bishop.h"
#include "Moves/Rook.h"
#include "Moves/King.h"
#include "Moves/Queen.h"

// square that can't be attacked during castling
uint64_t WK_NOATTACK = 0x70;
uint64_t WQ_NOATTACK = 0x1C;
uint64_t BK_NOATTACK = 0x7000000000000000;
uint64_t BQ_NOATTACK = 0x1C00000000000000;

// squares that can't be occupied during castling
uint64_t WK_CLEAR = 0x60;
uint64_t WQ_CLEAR = 0xE;
uint64_t BK_CLEAR = 0x6000000000000000;
uint64_t BQ_CLEAR = 0xE00000000000000;

// castling lookup table, slightly faster than if/else
// the key is (from + to) % 15
int CastlingTableMovesToTypes[15];

// converts castling types to bitboard og possible king moves
// the key is castling type
uint64_t CastlingTableTypesToBitboard[16];

void Moves_Init()
{
	CastlingTableMovesToTypes[(4 + 2) % 15] = CASTLE_WQ;
	CastlingTableMovesToTypes[(4 + 6) % 15] = CASTLE_WK;
	CastlingTableMovesToTypes[(60 + 58) % 15] = CASTLE_BQ;
	CastlingTableMovesToTypes[(60 + 62) % 15] = CASTLE_BK;


	CastlingTableTypesToBitboard[CASTLE_WK]             = 0x40;
	CastlingTableTypesToBitboard[CASTLE_WQ]             = 0x4;
	CastlingTableTypesToBitboard[CASTLE_WK | CASTLE_WQ] = 0x44;

	CastlingTableTypesToBitboard[CASTLE_BK]             = 0x4000000000000000;
	CastlingTableTypesToBitboard[CASTLE_BQ]             = 0x400000000000000;
	CastlingTableTypesToBitboard[CASTLE_BK | CASTLE_BQ] = 0x4400000000000000;
}

uint64_t Moves_GetMoves(Board* board, int tile)
{
	int piece = Board_Piece(board, tile);
	int color = Board_Color(board, tile);
	uint64_t value = 0;
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

	if(piece == 0 || color == 0)
		return 0;

	if(piece == PIECE_PAWN)
	{
		if(color == COLOR_WHITE)
		{
			uint64_t moves = Pawn_ReadWhiteMove(tile);
			uint64_t attacks = Pawn_ReadWhiteAttack(tile);
			uint64_t enPassant = Moves_GetEnPassantMove(board, tile);

			moves = moves & ~occupancy;
			attacks = attacks & board->Boards[BOARD_BLACK];
			return moves | attacks | enPassant;
		}
		else
		{
			uint64_t moves = Pawn_ReadBlackMove(tile);
			uint64_t attacks = Pawn_ReadBlackAttack(tile);
			uint64_t enPassant = Moves_GetEnPassantMove(board, tile);

			moves = moves & ~occupancy;
			attacks = attacks & board->Boards[BOARD_WHITE];
			return moves | attacks | enPassant;
		}
	}

	switch(piece)
	{
	case(PIECE_KNIGHT):
		value = Knight_Read(tile);
		break;
	case(PIECE_BISHOP):
		value = Bishop_Read(tile, occupancy);
		break;
	case(PIECE_ROOK):
		value = Rook_Read(tile, occupancy);
		break;
	case(PIECE_QUEEN):
		value = Queen_Read(tile, occupancy);
		break;
	case(PIECE_KING):
		value = King_Read(tile);
		value |= (tile == 4 || tile == 60) ? Moves_GetCastlingMoves(board, color) : 0;
		break;
	}

	if(color == COLOR_WHITE)
		value = value & ~(board->Boards[BOARD_WHITE]);
	else
		value = value & ~(board->Boards[BOARD_BLACK]);

	return value;
}

uint64_t Moves_GetAttacks(Board* board, int tile)
{
	int piece = Board_Piece(board, tile);
	int color = Board_Color(board, tile);
	uint64_t value = 0;
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

	if(piece == 0 || color == 0)
		return 0;

	if(piece == PIECE_PAWN)
	{
		uint64_t enPassant = Moves_GetEnPassantMove(board, tile);

		if(color == COLOR_WHITE)
		{
			uint64_t attacks = Pawn_ReadWhiteAttack(tile);
			return attacks | enPassant;
		}
		else
		{
			uint64_t attacks = Pawn_ReadBlackAttack(tile);
			return attacks | enPassant;
		}
	}

	switch(piece)
	{
	case(PIECE_KNIGHT):
		value = Knight_Read(tile);
		break;
	case(PIECE_BISHOP):
		value = Bishop_Read(tile, occupancy);
		break;
	case(PIECE_ROOK):
		value = Rook_Read(tile, occupancy);
		break;
	case(PIECE_QUEEN):
		value = Queen_Read(tile, occupancy);
		break;
	case(PIECE_KING):
		value = King_Read(tile);
		break;
	}

	return value;
}


__inline_always uint8_t Moves_GetCastlingType(Board* board, int from, int to)
{
	uint8_t isKing = Bitboard_GetRef(&board->Boards[BOARD_KINGS], from);
	uint8_t castleVal = CastlingTableMovesToTypes[(from + to) % 15];
	return isKing * castleVal;
}

// return castling types that can be performed at this moment
uint8_t Moves_GetAvailableCastlingTypes(Board* board, int color)
{
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

	if(color == COLOR_WHITE)
	{
		int wk = (((WK_NOATTACK & board->AttacksBlack) | (WK_CLEAR & occupancy)) == 0) ? 1 : 0;
		int wq = (((WQ_NOATTACK & board->AttacksBlack) | (WQ_CLEAR & occupancy)) == 0) ? 1 : 0;
		
		return (wk * (CASTLE_WK & board->Castle)) | (wq *  (CASTLE_WQ & board->Castle));
	}
	else
	{
		int bk = (((BK_NOATTACK & board->AttacksWhite) | (BK_CLEAR & occupancy)) == 0) ? 1 : 0;
		int bq = (((BQ_NOATTACK & board->AttacksWhite) | (BQ_CLEAR & occupancy)) == 0) ? 1 : 0;

		return (bk * (CASTLE_BK & board->Castle)) | (bq *  (CASTLE_BQ & board->Castle));
	}
}

uint64_t Moves_GetCastlingMoves(Board* board, int color)
{
	uint8_t availableTypes = Moves_GetAvailableCastlingTypes(board, color);
	return CastlingTableTypesToBitboard[availableTypes];
}


__inline_always int Moves_CanPromote(Board* board, int square)
{
	if(!Bitboard_Get(board->Boards[BOARD_PAWNS], square))
		return 0;

	int color = Board_Color(board, square);

	if (Board_Y(square) == 7 && color == COLOR_WHITE)
		return 1;

	if (Board_Y(square) == 0 && color == COLOR_BLACK)
		return 1;

	return 0;
}

__inline_always int Moves_IsCaptureMove(Board* board, int from, int to)
{
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

	if (Bitboard_Get(occupancy, to) > 0 || to == board->EnPassantTile)
		return true;

	return false;
}

__inline_always int Moves_IsEnPassantCapture(Board* board, int from, int to)
{
	return (board->EnPassantTile == to) && Bitboard_GetRef(&board->Boards[BOARD_PAWNS], from);
}

__inline_always int Moves_GetEnPassantVictimTile(Board* board, int from, int to)
{
	int color = Board_Color(board, from);

	if (color == COLOR_WHITE)
		return to - 8;

	if (color == COLOR_BLACK)
		return to + 8;

	return 0;
}

__inline_always int Moves_GetEnPassantTile(Board* board, int from, int to)
{
	int color = Board_Color(board, from);
	int y = Board_Y(from);

	if (Bitboard_Get(board->Boards[BOARD_PAWNS], from) == 0)
		return 0;

	if (color == COLOR_WHITE && y == 1 && to == (from + 16))
		return from + 8;

	if (color == COLOR_BLACK && y == 6 && to == (from - 16))
		return from - 8;

	return 0;
}

__inline_always uint64_t Moves_GetEnPassantMove(Board* board, int from)
{
	if(board->EnPassantTile < 0)
		return 0;

	int color = Board_Color(board, from);

	if(color == COLOR_WHITE)
	{
		if(from + 7 == board->EnPassantTile || from + 9 == board->EnPassantTile)
			return ((uint64_t)1) << ((uint64_t)board->EnPassantTile);
	}
	else
	{
		if(from - 7 == board->EnPassantTile || from - 9 == board->EnPassantTile)
			return ((uint64_t)1) << ((uint64_t)board->EnPassantTile);
	}

	return 0;
}
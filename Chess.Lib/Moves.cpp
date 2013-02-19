
#include "Moves.h"
#include "Bitboard.h"
#include "Board.h"
#include "Moves/Pawn.h"
#include "Moves/Knight.h"
#include "Moves/Bishop.h"
#include "Moves/Rook.h"
#include "Moves/King.h"
#include "Moves/Queen.h"

uint64_t Moves_GetMoves(Board* board, int tile)
{
	int piece = Board_Piece(board, tile);
	int color = Board_Color(board, tile);
	uint64_t value = 0;
	uint64_t occupancy = board->White | board->Black;

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
			attacks = attacks & board->Black;
			return moves | attacks | enPassant;
		}
		else
		{
			uint64_t moves = Pawn_ReadBlackMove(tile);
			uint64_t attacks = Pawn_ReadBlackAttack(tile);

			uint64_t enPassant = Moves_GetEnPassantMove(board, tile);

			moves = moves & ~occupancy;
			attacks = attacks & board->White;
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
		value = value | Moves_GetCastlingMoves(board, tile);
		break;
	}

	if(color == COLOR_WHITE)
		value = value & ~(board->White);
	else
		value = value & ~(board->Black);

	return value;
}

uint64_t Moves_GetAttacks(Board* board, int tile)
{
	int piece = Board_Piece(board, tile);
	int color = Board_Color(board, tile);
	uint64_t value = 0;
	uint64_t occupancy = board->White | board->Black;

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

uint8_t Moves_IsCastlingMove(Board* board, int from, int to)
{
	int color = Board_Color(board, from);
	int isKing = Bitboard_Get(board->Kings, from);
	
	if(!isKing)
	{
		return 0;
	}
	else if(color == COLOR_WHITE && from == 4)
	{
		// white kingside
		if (from == 4 && to == 6)
			return CASTLE_WK;
		
		if (from == 4 && to == 2)
			return CASTLE_WQ;
	}
	else if(color == COLOR_BLACK && from == 60)
	{
		// black kingside
		if (from == 60 && to == 62)
			return CASTLE_BK;

		// black queenside
		if (from == 60 && to == 58)
			return CASTLE_BQ;
	}

	return 0;
}

uint64_t Moves_GetCastlingMoves(Board* board, int from)
{
	if(from != 4 && from != 60)
		return 0;

	uint64_t output = 0;
	uint64_t occupancy = board->Black | board->White;

	int color = Board_Color(board, from);
	int isKing = Bitboard_Get(board->Kings, from);
	
	if(!isKing)
	{
		return 0;
	}

	// Todo: King cannot move over or into checked squares!

	if (color == COLOR_WHITE && from == 4)
	{
		if ((board->Castle & CASTLE_WK) > 0 && !Bitboard_Get(occupancy, 5) && !Bitboard_Get(occupancy, 6))
			Bitboard_SetRef(&output, 6);

		if ((board->Castle & CASTLE_WQ) > 0 && !Bitboard_Get(occupancy, 1) && !Bitboard_Get(occupancy, 2) && !Bitboard_Get(occupancy, 3))
			Bitboard_SetRef(&output, 2);
	}
	else if (color == COLOR_BLACK && from == 60)
	{
		if ((board->Castle & CASTLE_BK) > 0 && !Bitboard_Get(occupancy, 61) && !Bitboard_Get(occupancy, 62))
			Bitboard_SetRef(&output, 62);

		if ((board->Castle & CASTLE_BQ) > 0 && !Bitboard_Get(occupancy, 59) && !Bitboard_Get(occupancy, 58) && !Bitboard_Get(occupancy, 57))
			Bitboard_SetRef(&output, 58);
	}

	return output;
}


int Moves_CanPromote(Board* board, int square)
{
	if(!Bitboard_Get(board->Pawns, square))
		return 0;

	int color = Board_Color(board, square);

	if (Board_Y(square) == 7 && color == COLOR_WHITE)
		return 1;

	if (Board_Y(square) == 0 && color == COLOR_BLACK)
		return 1;

	return 0;
}

int Moves_IsCaptureMove(Board* board, int from, int to)
{
	uint64_t occupancy = board->White | board->Black;

	if (Bitboard_Get(occupancy, to) > 0 || to == board->EnPassantTile)
		return true;

	return false;
}

int Moves_IsEnPassantCapture(Board* board, int from, int to)
{
	if (board->EnPassantTile != to)
		return false;

	int color = Board_Color(board, from);

	if (Bitboard_Get(board->Pawns, from) == 0)
		return false;

	if (color == COLOR_WHITE)
	{
		if (to == from + 7 || to == from + 9)
			return true;
	}

	if (color == COLOR_BLACK)
	{
		if (to == from - 7 || to == from - 9)
			return true;
	}

	return false;
}

int Moves_GetEnPassantVictimTile(Board* board, int from, int to)
{
	int color = Board_Color(board, from);

	if (color == COLOR_WHITE)
		return to - 8;

	if (color == COLOR_BLACK)
		return to + 8;

	return 0;
}

int Moves_GetEnPassantTile(Board* board, int from, int to)
{
	int color = Board_Color(board, from);
	int y = Board_Y(from);

	if (Bitboard_Get(board->Pawns, from) == 0)
		return 0;

	if (color == COLOR_WHITE && y == 1 && to == (from + 16))
		return from + 8;

	if (color == COLOR_BLACK && y == 6 && to == (from - 16))
		return from - 8;

	return 0;
}

uint64_t Moves_GetEnPassantMove(Board* board, int from)
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
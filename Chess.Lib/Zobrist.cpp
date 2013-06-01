
#include <stdlib.h>
#include "Zobrist.h"
#include "Board.h"

uint8_t Zobrist_Index[256];

void Zobrist_Init()
{
	// define indexes for pieces
	Zobrist_Index[COLOR_WHITE | PIECE_PAWN]   = 0;
	Zobrist_Index[COLOR_WHITE | PIECE_KNIGHT] = 1;
	Zobrist_Index[COLOR_WHITE | PIECE_BISHOP] = 2;
	Zobrist_Index[COLOR_WHITE | PIECE_ROOK]   = 3;
	Zobrist_Index[COLOR_WHITE | PIECE_QUEEN]  = 4;
	Zobrist_Index[COLOR_WHITE | PIECE_KING]   = 5;

	Zobrist_Index[COLOR_BLACK | PIECE_PAWN]   = 6;
	Zobrist_Index[COLOR_BLACK | PIECE_KNIGHT] = 7;
	Zobrist_Index[COLOR_BLACK | PIECE_BISHOP] = 8;
	Zobrist_Index[COLOR_BLACK | PIECE_ROOK]   = 9;
	Zobrist_Index[COLOR_BLACK | PIECE_QUEEN]  = 10;
	Zobrist_Index[COLOR_BLACK | PIECE_KING]   = 11;

	// No piece or color, indexes empty space
	Zobrist_Index[0]              = 15;
	Zobrist_Index[PIECE_PAWN]     = 15;
	Zobrist_Index[PIECE_KNIGHT]   = 15;
	Zobrist_Index[PIECE_BISHOP]   = 15;
	Zobrist_Index[PIECE_ROOK]     = 15;
	Zobrist_Index[PIECE_QUEEN]    = 15;
	Zobrist_Index[PIECE_KING]     = 15;
	Zobrist_Index[COLOR_WHITE]    = 15;
	Zobrist_Index[COLOR_BLACK]    = 15;

	// zero out the key for the "no en-passant" square.
	Zobrist_Keys[ZOBRIST_ENPASSANT][0] = 0;
}

uint64_t Zobrist_Calculate(Board* board)
{
	uint64_t hash = 0;

	for(int i = 0; i < 64; i++)
	{
		int color = Board_Color(board, i);
		if(color == 0)
			continue;

		int piece = Board_Piece(board, i);

		hash = hash ^ Zobrist_Keys[Zobrist_Index[color | piece]][i];
	}

	hash = hash ^ Zobrist_Keys[ZOBRIST_SIDE][board->PlayerTurn];
	hash = hash ^ Zobrist_Keys[ZOBRIST_CASTLING][board->Castle];
	hash = hash ^ Zobrist_Keys[ZOBRIST_ENPASSANT][board->EnPassantTile];

	return hash;
}

uint8_t Zobrist_IndexRead(int pieceAndColor)
{
	return Zobrist_Index[pieceAndColor];
}

uint64_t Zobrist_Read(int index, int square)
{
	return Zobrist_Keys[index][square];
}
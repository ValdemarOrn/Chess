	
#include "Zobrist.h"
#include "Board.h"
#include <stdlib.h>

// Create a random 64 bit number
uint64_t Zobrist_GetRandom()
{
	uint64_t rand1 = rand() & 255;
	uint64_t rand2 = rand() & 255;
	uint64_t rand3 = rand() & 255;
	uint64_t rand4 = rand() & 255;
	uint64_t rand5 = rand() & 255;
	uint64_t rand6 = rand() & 255;
	uint64_t rand7 = rand() & 255;
	uint64_t rand8 = rand() & 255;

	return	(rand1 << 0)  | 
			(rand2 << 8)  |
			(rand3 << 16) |
			(rand4 << 24) |
			(rand5 << 32) |
			(rand6 << 40) |
			(rand7 << 48) |
			(rand8 << 56) ;
}


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

	srand(42);

	for(int i=0; i<15; i++)
	{
		for(int j=0; j<64; j++)
		{
			Zobrist_Keys[i][j] = Zobrist_GetRandom();
		}
	}

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

uint8_t Zobrist_Index_Read(int pieceAndColor)
{
	return Zobrist_Index[pieceAndColor];
}

uint64_t Zobrist_Read(int index, int square)
{
	return Zobrist_Keys[index][square];
}
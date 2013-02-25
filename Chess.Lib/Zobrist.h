#ifndef ZOBRIST
#define ZOBRIST

#include "Default.h"
#include "Board.h"

extern "C"
{
	// lookup table for piece indexes. Use Color | Piece as key in this lookup table
	// to get the zobrist table index
	// example: uint64_t zobSquare = Zobrist_Keys[Zobrist_Index[COLORS_WHITE | PIECES_PAWN]][square]
	extern uint8_t Zobrist_Index[256];

	const int ZOBRIST_SIDE = 12;
	const int ZOBRIST_CASTLING = 13;
	const int ZOBRIST_ENPASSANT = 14;

	// use PIECE_XXX constants to index the second dimension

	// Zobrist_Keys[0-11] = piece locations, Subindex = square
	// Zobrist_Keys[12] = Which side to play, Subindex = Color
	// Zobrist_Keys[13] = Castling rights, Subindex = castling rights OR'ed
	// Zobrist_Keys[14] = En Passant square, Subindex = square
	// Zobrist_Keys[15] = Empty area all zeros
	extern uint64_t Zobrist_Keys[16][64];

	// initialize random values and the zobrist piece index
	__declspec(dllexport) void Zobrist_Init();

	__declspec(dllexport) uint64_t Zobrist_Calculate(Board* board);

	// exposed for .NET interop
	__declspec(dllexport) uint8_t Zobrist_IndexRead(int pieceAndColor);

	// exposed for .NET interop
	__declspec(dllexport) uint64_t Zobrist_Read(int index, int square);

}

#endif

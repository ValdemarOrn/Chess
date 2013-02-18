
#include "Board.h"
#include "Bitboard.h"
#include <string.h>

Board* Board_Create()
{
	Board* board = new Board();
	board->CurrentMove = 0;
	board->Moves = new Move[512];
	return board;
}

void Board_Delete(Board* board)
{
	delete board->Moves;
	delete board;
}

Board* Board_Copy(Board* board)
{
	Board* newBoard = Board_Create();
	memcpy(newBoard->Moves, board->Moves, 512);
	return newBoard;
}

int Board_X(int tile)
{
	// 128 constant prevent negative numbers in modulo output
	return (128 + tile) % 8;
}

int Board_Y(int tile)
{
	return tile >> 3; // ( x >> 3 == x / 8)
}

int Board_Color(Board* board, int tile)
{
	if(Bitboard_Get(board->White, tile) == 1)
		return COLOR_WHITE;
	
	if(Bitboard_Get(board->Black, tile) == 1)
		return COLOR_BLACK;
	
	return 0;
}

int Board_Piece(Board* board, int tile)
{
	// check if empty tile
	if(Bitboard_Get(board->White | board->Black, tile) == 0)
		return 0;

	if(Bitboard_Get(board->Pawns, tile) == 1)
		return PIECE_PAWN;
	
	if(Bitboard_Get(board->Knights, tile) == 1)
		return PIECE_KNIGHT;

	if(Bitboard_Get(board->Bishops, tile) == 1)
		return PIECE_BISHOP;

	if(Bitboard_Get(board->Rooks, tile) == 1)
		return PIECE_ROOK;

	if(Bitboard_Get(board->Queens, tile) == 1)
		return PIECE_QUEEN;

	if(Bitboard_Get(board->Kings, tile) == 1)
		return PIECE_KING;
	
	return 0;
}


int Board_Make(Board* board, int from, int to, int verifyLegalMove)
{
	return 0;
	// todo: Implement make
}

/*int Board_MakeMove(Board* board, Move* move, int verifyLegalMove)
{
	return 0;
}*/

int Board_Unmake(Board* board, Move* move, int verifyLegalMove)
{
	return 0;
	// todo: Implement unmake
}

int Board_Promote(Board* board, int square, int pieceType)
{
	return 0;
	// todo: Implement promote
}


void Board_CheckCastling(Board* board)
{
	// todo: Implement castling check
}

void Board_InitBoard(Board* board)
{
	// todo: Implement init board
}

void Board_AllowCastlingAll(Board* board)
{
	// todo: Implement allow castling
}



#include "Board.h"
#include "Bitboard.h"

Board* Board_Create()
{
	return new Board();
}

void Board_Delete(Board* board)
{
	delete board;
}

Board* Board_Copy(Board* board)
{
	return 0;
}

int Board_X(int tile)
{
	// 128 constant prevent negative numbers in modulu output
	return (128 + tile) % 8;
}

int Board_Y(int tile)
{
	return tile >> 3;
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
}

int Board_MakeMove(Board* board, Move* move, int verifyLegalMove)
{
	return 0;
}

int Board_Unmake(Board* board, Move* move, int verifyLegalMove)
{
	return 0;
}

int Board_Promote(Board* board, int square, int pieceType)
{
	return 0;
}


void Board_CheckCastling(Board* board)
{

}

void Board_InitBoard(Board* board)
{

}

void Board_AllowCastlingAll(Board* board)
{

}


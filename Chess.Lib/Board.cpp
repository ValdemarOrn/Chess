
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

void Board_SetPiece(Board* board, int square, int pieceType, int color)
{
	switch(pieceType)
	{
	case PIECE_PAWN:
		Bitboard_SetRef(&board->Pawns, square);
		break;
	case PIECE_KNIGHT:
		Bitboard_SetRef(&board->Knights, square);
		break;
	case PIECE_BISHOP:
		Bitboard_SetRef(&board->Bishops, square);
		break;
	case PIECE_ROOK:
		Bitboard_SetRef(&board->Rooks, square);
		break;
	case PIECE_QUEEN:
		Bitboard_SetRef(&board->Queens, square);
		break;
	case PIECE_KING:
		Bitboard_SetRef(&board->Kings, square);
		break;
	}

	if(color == COLOR_WHITE)
		Bitboard_SetRef(&board->White, square);
	else
		Bitboard_SetRef(&board->Black, square);
}

void Board_ClearPiece(Board* board, int square)
{
	Bitboard_UnsetRef(&board->Pawns, square);
	Bitboard_UnsetRef(&board->Knights, square);
	Bitboard_UnsetRef(&board->Bishops, square);
	Bitboard_UnsetRef(&board->Rooks, square);
	Bitboard_UnsetRef(&board->Queens, square);
	Bitboard_UnsetRef(&board->Kings, square);

	Bitboard_UnsetRef(&board->White, square);
	Bitboard_UnsetRef(&board->Black, square);
}

int Board_Make(Board* board, int from, int to, int verifyLegalMove)
{
	return 0;
	// todo: Implement make
}

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



#include "Board.h"
#include "Bitboard.h"
#include <string.h>

Board* Board_Create()
{
	Board* board = new Board();
	board->Moves = new Move[512];

	Board_Init(board, 0);
	return board;
}

void Board_Delete(Board* board)
{
	delete board->Moves;
	delete board;
}

Board* Board_Copy(Board* board)
{
	Board* newBoard = new Board();
	newBoard->Moves = new Move[512];

	memcpy(newBoard->Boards, board->Boards, 8 * sizeof(uint64_t));
	memcpy(newBoard->Moves, board->Moves, 512);
	return newBoard;
}

void Board_Init(Board* board, int setPieces)
{
	board->Castle = CASTLE_BK | CASTLE_BQ | CASTLE_WK | CASTLE_WQ;
	board->CheckmateState = CHECK_NONE;
	board->CurrentMove = 0;
	board->EnPassantTile = 0;
	board->FiftyMoveRulePlies = 0;
	board->Hash = 0;
	board->PlayerTurn = COLOR_WHITE;

	if(!setPieces)
		return;

	board->Boards[BOARD_WHITE] = 0xFFFF;
	board->Boards[BOARD_BLACK] = 0xFFFF000000000000;
	
	board->Boards[BOARD_PAWNS] = 0xFF00000000FF00;
	board->Boards[BOARD_ROOKS] = 0x8100000000000081;
	board->Boards[BOARD_KNIGHTS] = 0x4200000000000042;
	board->Boards[BOARD_BISHOPS] = 0x2400000000000024;
	board->Boards[BOARD_QUEENS] = 0x800000000000008;
	board->Boards[BOARD_KINGS] = 0x1000000000000010;
}

int Board_Piece(Board* board, int tile)
{
	// check if empty tile
	if(Bitboard_Get(board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK], tile) == 0)
		return 0;

	if(Bitboard_Get(board->Boards[BOARD_PAWNS], tile) == 1)
		return PIECE_PAWN;
	
	if(Bitboard_Get(board->Boards[BOARD_KNIGHTS], tile) == 1)
		return PIECE_KNIGHT;

	if(Bitboard_Get(board->Boards[BOARD_BISHOPS], tile) == 1)
		return PIECE_BISHOP;

	if(Bitboard_Get(board->Boards[BOARD_ROOKS], tile) == 1)
		return PIECE_ROOK;

	if(Bitboard_Get(board->Boards[BOARD_QUEENS], tile) == 1)
		return PIECE_QUEEN;

	if(Bitboard_Get(board->Boards[BOARD_KINGS], tile) == 1)
		return PIECE_KING;
	
	return 0;
}

void Board_SetPiece(Board* board, int square, int pieceType, int color)
{
	Bitboard_SetRef(&board->Boards[pieceType], square);

	uint64_t* colorBoard = (color == COLOR_WHITE) ? &board->Boards[BOARD_WHITE] : &board->Boards[BOARD_BLACK];
	Bitboard_SetRef(colorBoard, square);
}

void Board_ClearPiece(Board* board, int square)
{
	Bitboard_UnsetRef(&board->Boards[BOARD_PAWNS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_KNIGHTS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_BISHOPS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_ROOKS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_QUEENS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_KINGS], square);

	Bitboard_UnsetRef(&board->Boards[BOARD_WHITE], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_BLACK], square);
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

void Board_AllowCastlingAll(Board* board)
{
	// todo: Implement allow castling
}


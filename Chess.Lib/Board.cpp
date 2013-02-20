
#include "Board.h"
#include "Bitboard.h"
#include "Zobrist.h"
#include "Moves.h"
#include <string.h>

Board* Board_Create()
{
	Board* board = new Board();
	board->MoveHistory = new MoveHistory[512];

	Board_Init(board, 0);
	return board;
}

void Board_Delete(Board* board)
{
	delete board->MoveHistory;
	delete board;
}

Board* Board_Copy(Board* board)
{
	Board* newBoard = new Board();
	memcpy(newBoard, board, sizeof(board));

	newBoard->MoveHistory = new MoveHistory[512];
	memcpy(newBoard->MoveHistory, board->MoveHistory, 512 * sizeof(MoveHistory));
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
	{
		board->Hash ^= Zobrist_Keys[ZOBRIST_SIDE][board->PlayerTurn];
		board->Hash ^= Zobrist_Keys[ZOBRIST_CASTLING][board->Castle];
		board->Hash ^= Zobrist_Keys[ZOBRIST_ENPASSANT][board->EnPassantTile];
		return;
	}

	board->Boards[BOARD_WHITE] = 0xFFFF;
	board->Boards[BOARD_BLACK] = 0xFFFF000000000000;
	
	board->Boards[BOARD_PAWNS] = 0xFF00000000FF00;
	board->Boards[BOARD_ROOKS] = 0x8100000000000081;
	board->Boards[BOARD_KNIGHTS] = 0x4200000000000042;
	board->Boards[BOARD_BISHOPS] = 0x2400000000000024;
	board->Boards[BOARD_QUEENS] = 0x800000000000008;
	board->Boards[BOARD_KINGS] = 0x1000000000000010;

	board->Hash = Zobrist_Calculate(board);
	board->AttacksWhite = Board_AttackMap(board, COLOR_WHITE);
	board->AttacksBlack = Board_AttackMap(board, COLOR_BLACK);
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
	board->Hash ^= Zobrist_Keys[Zobrist_Index[pieceType | color]][square];
}

void Board_ClearPiece(Board* board, int square)
{
	int color = Board_Color(board, square);

	if(color == 0)
		return;

	int piece = Board_Piece(board, square);
	board->Hash ^= Zobrist_Keys[Zobrist_Index[piece | color]][square];

	Bitboard_UnsetRef(&board->Boards[BOARD_PAWNS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_KNIGHTS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_BISHOPS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_ROOKS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_QUEENS], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_KINGS], square);

	Bitboard_UnsetRef(&board->Boards[BOARD_WHITE], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_BLACK], square);
}

uint64_t Board_AttackMap(Board* board, int color)
{
	uint64_t attackBoard = 0;

	uint8_t positions[20];
	uint64_t bitboard = (color == COLOR_WHITE) ? board->Boards[BOARD_WHITE] : board->Boards[BOARD_BLACK];
	int count = Bitboard_BitList(bitboard, positions);

	for(int i = 0; i < count; i++)
	{
		int pos = positions[i];
		uint64_t attacks = Moves_GetAttacks(board, pos);
		attackBoard |= attacks;
	}

	return attackBoard;
}

int Board_Make(Board* board, int from, int to)
{
	// 0: Check if en passant or castling
	// 1. put info in history
	// 2. Create the Move object
	// 3. Make the move
	//    3.1 if castling move, check that it's OK
	//    3.2 if castling move, move the rook
	//    3.3 If en passant, remove pawn
	// 4. update board data, enpassant square, castling, check state, 50 move rule, hash
	// 5. Verify it doesn't self check
	//    5.1 If it does, unmake
	
	// 0: Check if en passant or castling
	int isEnPassant = Moves_IsEnPassantCapture(board, from, to);
	int castlingType = Moves_GetCastlingType(board, from, to);

	// 1. put info in history
	MoveHistory* history = &board->MoveHistory[board->CurrentMove];
	history->PrevAttacksBlack = board->AttacksBlack;
	history->PrevAttacksWhite = board->AttacksWhite;
	history->PrevCastleState = board->Castle;
	history->PrevCheckmateState = board->CheckmateState;
	history->PrevEnPassantTile = board->EnPassantTile;
	history->PrevFiftyMoveRulePlies = board->FiftyMoveRulePlies;
	history->PrevHash = board->Hash;

	// 2. Create the Move object
	history->Move.CapturePiece = (isEnPassant) ? PIECE_PAWN : Board_Piece(board, to);
	history->Move.CaptureTile = (isEnPassant) ? board->EnPassantTile : to;
	history->Move.Castle = castlingType;
	// history->Move.CheckmateState = 0; // set later
	history->Move.From = from;
	history->Move.PlayerColor = board->PlayerTurn;
	// history->Move.Promotion = 0; // set outside the Make() function, during search
	history->Move.To = to;

	// 3. Make the move

	//    3.1 if castling move, check that it's OK
	if(castlingType > 0)
	{
		//uint64_t possibleMoves = Moves_GetCastlingMoves(board, from);

		//    3.2 if castling move, move the rook
	}

	
	//    3.3 If en passant, remove pawn



	return 1;
}

void Board_Unmake(Board* board, Move* move)
{
	// todo: Implement unmake
}

void Board_Promote(Board* board, int square, int pieceType)
{
	// todo: Implement promote
}


uint8_t Board_GetCastling(Board* board)
{
	return 0;
	// todo: Implement castling check
}


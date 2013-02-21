
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
	board->CheckState = CHECK_NONE;
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
	//    3.1 if castling move, move the rook
	//    3.2 If en passant, remove pawn
	// 4. update board data, enpassant square, castling, 50 move rule, hash
	// 5. Verify it doesn't self check
	//    5.1 If it does, unmake
	
	int piece = Board_Piece(board, to);
	int color = board->PlayerTurn;

	// 0: Check if en passant or castling
	int isEnPassantCapture = Moves_IsEnPassantCapture(board, from, to);
	int castlingType = Moves_GetCastlingType(board, from, to);

	// 1. put info in history
	MoveHistory* history = &board->MoveHistory[board->CurrentMove];
	history->PrevAttacksBlack = board->AttacksBlack;
	history->PrevAttacksWhite = board->AttacksWhite;
	history->PrevCastleState = board->Castle;
	history->PrevCheckState = board->CheckState;
	history->PrevEnPassantTile = board->EnPassantTile;
	history->PrevFiftyMoveRulePlies = board->FiftyMoveRulePlies;
	history->PrevHash = board->Hash;

	// 2. Create the Move object
	history->Move.CapturePiece = (isEnPassantCapture) ? PIECE_PAWN : piece;
	history->Move.CaptureTile = (isEnPassantCapture) ? board->EnPassantTile : to;
	history->Move.Castle = castlingType;
	// history->Move.CheckmateState = 0; // set later
	history->Move.From = from;
	history->Move.PlayerColor = color;
	// history->Move.Promotion = 0; // set outside the Make() function, during search
	history->Move.To = to;

	// 3. Make the move
	Board_ClearPiece(board, from);
	Board_ClearPiece(board, to);
	Board_SetPiece(board, to, piece, color);

	//    3.1 if castling move, move the rook
	if(castlingType == 0)
	{
		switch(castlingType)
		{
		case CASTLE_WK:
			Board_ClearPiece(board, 7);
			Board_SetPiece(board, 5, PIECE_ROOK, COLOR_WHITE);
			break;
		case CASTLE_WQ:
			Board_ClearPiece(board, 0);
			Board_SetPiece(board, 3, PIECE_ROOK, COLOR_WHITE);
			break;
		case CASTLE_BK:
			Board_ClearPiece(board, 63);
			Board_SetPiece(board, 61, PIECE_ROOK, COLOR_BLACK);
			break;
		case CASTLE_BQ:
			Board_ClearPiece(board, 56);
			Board_SetPiece(board, 59, PIECE_ROOK, COLOR_BLACK);
			break;
		default:
			break;
		}
	}
	
	//    3.2 If en passant, remove pawn
	if(isEnPassantCapture)
	{
		int victimTile = Moves_GetEnPassantVictimTile(board, from, to);
		Board_ClearPiece(board, victimTile);
	}

	// 4. update board data, enpassant square, castling, 50 move rule, hash

	board->Castle = Board_GetCastling(board);
	board->AttacksBlack = Board_AttackMap(board, COLOR_BLACK);
	board->AttacksWhite = Board_AttackMap(board, COLOR_WHITE);
	board->CurrentMove += 1;
	board->CheckState = Board_GetCheckState(board);

	if(piece == PIECE_PAWN)
	{
		if(to == from + 16) // white two-square advance
			board->EnPassantTile = from + 8;

		if(to == from - 16) // black two-square advance
			board->EnPassantTile = from - 8;
	}
	else
	{
		board->EnPassantTile = 0;
	}

	board->FiftyMoveRulePlies = (history->Move.CapturePiece > 0 || piece == PIECE_PAWN) ? 0 : board->FiftyMoveRulePlies + 1;
	board->PlayerTurn = (board->PlayerTurn == COLOR_WHITE) ? COLOR_BLACK : COLOR_WHITE;

	// 5. Verify it doesn't self check
	if(Board_IsChecked(board, color))
	{
		//    5.1 If it does, unmake
		Board_Unmake(board);
		return 0;
	}

	return 1;
}

void Board_Unmake(Board* board)
{
	// todo: Implement unmake
}

void Board_Promote(Board* board, int square, int pieceType)
{
	// todo: Implement promote
}


uint8_t Board_GetCastling(Board* board)
{
	int whiteKing = Bitboard_Get(board->Boards[COLOR_WHITE] & board->Boards[PIECE_KING], 4);
	int blackKing = Bitboard_Get(board->Boards[COLOR_BLACK] & board->Boards[PIECE_KING], 60);

	uint64_t whiteRooks = board->Boards[COLOR_WHITE] & board->Boards[PIECE_ROOK];
	uint64_t blackRooks = board->Boards[COLOR_BLACK] & board->Boards[PIECE_ROOK];

	uint8_t wk = (whiteKing & whiteRooks & 0x80) > 0 ? CASTLE_WK : 0;
	uint8_t wq = (whiteKing & whiteRooks & 0x1) > 0 ? CASTLE_WQ : 0;
	uint8_t bk = (blackKing & blackRooks & 0x8000000000000000) > 0 ? CASTLE_BK : 0;
	uint8_t bq = (blackKing & blackRooks & 0x100000000000000) > 0 ? CASTLE_BQ : 0;

	uint8_t output = (wk | wq | bk | bq) & board->Castle;
	return output;
}

int Board_GetCheckState(Board* board)
{
	uint64_t whiteKing = board->Boards[COLOR_WHITE] & board->Boards[PIECE_KING];
	uint64_t blackKing = board->Boards[COLOR_BLACK] & board->Boards[PIECE_KING];

	int output = ((whiteKing & board->AttacksBlack) | (blackKing & board->AttacksWhite)) > 0 ? 1 : 0;
	return output;
}

int Board_IsChecked(Board* board, int color)
{
	if(color == COLOR_WHITE)
	{
		uint64_t whiteKing = board->Boards[COLOR_WHITE] & board->Boards[PIECE_KING];
		int output = (whiteKing & board->AttacksBlack) > 0 ? 1 : 0;
		return output;
	}
	else
	{
		uint64_t blackKing = board->Boards[COLOR_BLACK] & board->Boards[PIECE_KING];
		int output = (blackKing & board->AttacksWhite) > 0 ? 1 : 0;
		return output;
	}
}
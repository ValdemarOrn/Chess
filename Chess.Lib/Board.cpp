
#include "Board.h"
#include "Bitboard.h"
#include "Zobrist.h"
#include "Moves.h"
#include "Moves\Rook.h"
#include "Moves\Bishop.h"
#include "Moves\King.h"
#include "Moves\Knight.h"
#include <string.h>

#define MAX_GAME_LENGTH 1024

Board* Board_Create()
{
	Board* board = new Board();
	board->MoveHistory = new MoveHistory[MAX_GAME_LENGTH];

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
	memcpy(newBoard, board, sizeof(Board));

	newBoard->MoveHistory = new MoveHistory[MAX_GAME_LENGTH];
	memcpy(newBoard->MoveHistory, board->MoveHistory, MAX_GAME_LENGTH * sizeof(MoveHistory));
	return newBoard;
}

void Board_Init(Board* board, int setPieces)
{
	board->Castle = CASTLE_BK | CASTLE_BQ | CASTLE_WK | CASTLE_WQ;
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

	Board_SetPiece(board, 0, PIECE_ROOK, COLOR_WHITE);
	Board_SetPiece(board, 1, PIECE_KNIGHT, COLOR_WHITE);
	Board_SetPiece(board, 2, PIECE_BISHOP, COLOR_WHITE);
	Board_SetPiece(board, 3, PIECE_QUEEN, COLOR_WHITE);
	Board_SetPiece(board, 4, PIECE_KING, COLOR_WHITE);
	Board_SetPiece(board, 5, PIECE_BISHOP, COLOR_WHITE);
	Board_SetPiece(board, 6, PIECE_KNIGHT, COLOR_WHITE);
	Board_SetPiece(board, 7, PIECE_ROOK, COLOR_WHITE);

	Board_SetPiece(board, 8, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 9, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 10, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 11, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 12, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 13, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 14, PIECE_PAWN, COLOR_WHITE);
	Board_SetPiece(board, 15, PIECE_PAWN, COLOR_WHITE);

	Board_SetPiece(board, 48, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 49, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 50, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 51, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 52, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 53, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 54, PIECE_PAWN, COLOR_BLACK);
	Board_SetPiece(board, 55, PIECE_PAWN, COLOR_BLACK);

	Board_SetPiece(board, 56, PIECE_ROOK, COLOR_BLACK);
	Board_SetPiece(board, 57, PIECE_KNIGHT, COLOR_BLACK);
	Board_SetPiece(board, 58, PIECE_BISHOP, COLOR_BLACK);
	Board_SetPiece(board, 59, PIECE_QUEEN, COLOR_BLACK);
	Board_SetPiece(board, 60, PIECE_KING, COLOR_BLACK);
	Board_SetPiece(board, 61, PIECE_BISHOP, COLOR_BLACK);
	Board_SetPiece(board, 62, PIECE_KNIGHT, COLOR_BLACK);
	Board_SetPiece(board, 63, PIECE_ROOK, COLOR_BLACK);

	board->Hash = Zobrist_Calculate(board);
	board->AttacksWhite = 0; //Board_AttackMap(board, COLOR_WHITE);
	board->AttacksBlack = 0; //Board_AttackMap(board, COLOR_BLACK);
}

void Board_SetPiece(Board* board, int square, int pieceType, int color)
{
	if(pieceType == 0 || color == 0)
		return;

	Bitboard_SetRef(&board->Boards[pieceType], square);

	uint64_t* colorBoard = (color == COLOR_WHITE) ? &board->Boards[BOARD_WHITE] : &board->Boards[BOARD_BLACK];
	Bitboard_SetRef(colorBoard, square);
	board->Hash ^= Zobrist_Keys[Zobrist_Index[pieceType | color]][square];
	board->Tiles[square] = pieceType | color;
}

void Board_ClearPiece(Board* board, int square)
{
	int color = Board_Color(board, square);

	if(color == 0)
		return;

	int piece = Board_Piece(board, square);
	board->Hash ^= Zobrist_Keys[Zobrist_Index[piece | color]][square];

	Bitboard_UnsetRef(&board->Boards[piece], square);

	Bitboard_UnsetRef(&board->Boards[BOARD_WHITE], square);
	Bitboard_UnsetRef(&board->Boards[BOARD_BLACK], square);
	board->Tiles[square] = 0;
}

void Board_GenerateTileMap(Board* board)
{
	for(int i = 0; i < 64; i++)
	{
		int color = 0;
		int piece = 0;

		if(Bitboard_GetRef(&board->Boards[BOARD_WHITE], i) > 0)
			color = COLOR_WHITE;
		else if (Bitboard_GetRef(&board->Boards[BOARD_BLACK], i) > 0)
			color = COLOR_BLACK;

		if(Bitboard_GetRef(&board->Boards[PIECE_PAWN], i) > 0)
			piece = PIECE_PAWN;
		else if(Bitboard_GetRef(&board->Boards[PIECE_KNIGHT], i) > 0)
			piece = PIECE_KNIGHT;
		else if(Bitboard_GetRef(&board->Boards[PIECE_BISHOP], i) > 0)
			piece = PIECE_BISHOP;
		else if(Bitboard_GetRef(&board->Boards[PIECE_ROOK], i) > 0)
			piece = PIECE_ROOK;
		else if(Bitboard_GetRef(&board->Boards[PIECE_QUEEN], i) > 0)
			piece = PIECE_QUEEN;
		else if(Bitboard_GetRef(&board->Boards[PIECE_KING], i) > 0)
			piece = PIECE_KING;

		board->Tiles[i] = color | piece;
	}
}

uint64_t Board_AttackMap(Board* board, int color)
{
	uint64_t attackBoard = 0;

	uint8_t positions[20];

	for(int i = 0; i < 64; i++)
	{
		if(Board_Color(board, i) == color)
		{
			uint64_t attacks = Moves_GetAttacks(board, i);
			attackBoard |= attacks;
		}
	}

	return attackBoard;
}
/*
_Bool Board_IsAttacked(Board* board, int square, int attackerColor)
{
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];
	_Bool attacked = FALSE;

	if(attackerColor == COLOR_WHITE)
	{
		uint64_t enemies = board->Boards[BOARD_WHITE];

		// check for rook & queen attacks
		uint64_t rookAttacks = Rook_Read(square, occupancy);
		uint64_t rooksAndQueens = board->Boards[PIECE_ROOK] | board->Boards[PIECE_QUEEN];
		attacked = ((rooksAndQueens & enemies) & rookAttacks) > 0;
		if(attacked)
			return TRUE;

		// check for bishop & queen attacks
		uint64_t bishopAttacks = Bishop_Read(square, occupancy);
		uint64_t bishopsAndQueens = board->Boards[PIECE_BISHOP] | board->Boards[PIECE_QUEEN];
		attacked = ((bishopsAndQueens & enemies) & bishopAttacks) > 0;
		if(attacked)
			return TRUE;

		// check for knight attacks
		uint64_t knightAttacks = Knight_Read(square, occupancy);
		uint64_t bishopsAndQueens = board->Boards[PIECE_BISHOP] | board->Boards[PIECE_QUEEN];
		attacked = ((bishopsAndQueens & enemies) & bishopAttacks) > 0;
		if(attacked)
			return TRUE;
	}
	else
	{

	}
}*/

_Bool Board_Make(Board* board, int from, int to)
{
	// 0: Check if en passant or castling
	// 1. put info in history
	// 2. Create the Move object
	// 3. Make the move, clear both squares, then set piece
	//    3.1 if castling move, move the rook
	//    3.2 If en passant, remove pawn
	// 4. update board data, enpassant square, castling, 50 move rule, hash
	// 5. Verify it doesn't self check
	//    5.1 If it does, unmake

	int playerPiece = Board_Piece(board, from);
	int capturePiece = Board_Piece(board, to);
	int color = board->PlayerTurn;
	int victimTile = to; // which square to clear. Only differs from the "to" square during en passant attacks

	// safety check, we are actually moving a piece that belongs to the current player
	if(board->PlayerTurn != Board_Color(board, from))
		return 0;

	// 0: Check if en passant or castling
	int isEnPassantCapture = Moves_IsEnPassantCapture(board, from, to);
	int castlingType = Moves_GetCastlingType(board, from, to);

	// Set the captured piece to pawn if en passant (otherwise it's zero because the "to" square is empty)
	if(isEnPassantCapture)
	{
		capturePiece = PIECE_PAWN;
		victimTile = Moves_GetEnPassantVictimTile(board, from, to);
	}

	// 1. put info in history
	MoveHistory* history = &board->MoveHistory[board->CurrentMove];
	history->PrevAttacksBlack = board->AttacksBlack;
	history->PrevAttacksWhite = board->AttacksWhite;
	history->PrevCastleState = board->Castle;
	history->PrevEnPassantTile = board->EnPassantTile;
	history->PrevFiftyMoveRulePlies = board->FiftyMoveRulePlies;
	history->PrevHash = board->Hash;

	// 2. Create the Move object
	history->Move.CapturePiece = capturePiece;
	history->Move.CaptureTile = victimTile;
	history->Move.Castle = castlingType;
	history->Move.From = from;
	history->Move.PlayerColor = color;
	history->Move.PlayerPiece = playerPiece;
	history->Move.Promotion = 0; // set outside the Make() function, during search
	history->Move.To = to;

	// 3. Make the move
	Board_ClearPiece(board, from);
	Board_ClearPiece(board, to);
	Board_SetPiece(board, to, playerPiece, color);

	//    3.1 if castling move, move the rook
	if(castlingType > 0)
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
		Board_ClearPiece(board, victimTile);
	}

	// 4. update board data, enpassant square, castling, 50 move rule, hash

	board->Hash ^= Zobrist_Keys[ZOBRIST_CASTLING][board->Castle];
	board->Castle = Board_GetCastling(board);
	board->Hash ^= Zobrist_Keys[ZOBRIST_CASTLING][board->Castle];

	board->CurrentMove += 1;
	
	board->Hash ^= Zobrist_Keys[ZOBRIST_ENPASSANT][board->EnPassantTile];
	if(playerPiece == PIECE_PAWN)
	{
		if(to == from + 16) // white two-square advance
			board->EnPassantTile = from + 8;
		else if(to == from - 16) // black two-square advance
			board->EnPassantTile = from - 8;
		else
			board->EnPassantTile = 0;
	}
	else
	{
		board->EnPassantTile = 0;
	}
	board->Hash ^= Zobrist_Keys[ZOBRIST_ENPASSANT][board->EnPassantTile];

	board->FiftyMoveRulePlies = (history->Move.CapturePiece > 0 || playerPiece == PIECE_PAWN) ? 0 : board->FiftyMoveRulePlies + 1;
	
	board->Hash ^= Zobrist_Keys[ZOBRIST_SIDE][board->PlayerTurn];
	board->PlayerTurn = (board->PlayerTurn == COLOR_WHITE) ? COLOR_BLACK : COLOR_WHITE;
	board->Hash ^= Zobrist_Keys[ZOBRIST_SIDE][board->PlayerTurn];

	// 5. Verify it doesn't self check
	//    5.1 If it does, unmake

	// I switch between these two cases because if the move is illegal we can save 50%
	// of the move generation time by starting with the right color
	if(board->PlayerTurn == COLOR_WHITE)
	{
		board->AttacksWhite = Board_AttackMap(board, COLOR_WHITE);
		uint64_t blackKing = board->Boards[BOARD_BLACK] & board->Boards[PIECE_KING];
		if((blackKing & board->AttacksWhite) > 0) // white attacks black king
		{
			Board_Unmake(board);
			return 0;
		}

		// it's legal, now calculate black's attacks
		board->AttacksBlack = Board_AttackMap(board, COLOR_BLACK);
	}
	else
	{
		board->AttacksBlack = Board_AttackMap(board, COLOR_BLACK);
		uint64_t whiteKing = board->Boards[BOARD_WHITE] & board->Boards[PIECE_KING];
		if((whiteKing & board->AttacksBlack) > 0) // black attacks white king
		{
			Board_Unmake(board);
			return 0;
		}

		// it's legal, now calculate white's attacks
		board->AttacksWhite = Board_AttackMap(board, COLOR_WHITE);
	}

	return 1;
}

void Board_Unmake(Board* board)
{
	if(board->CurrentMove == 0) // no moves to unmake
		return;

	// 1. Set board data from history, enpassant square, castling, 50 move, hash. Recalculate what's needed
	// 2. Unmake the move, clear "to", set player piece at old location, then set captured piece
	//    2.1 If castling move, move the rook back
	// 3. Pop history off the stack
	// 4. For debugging, compute new hash and compare with hash from history. must match!

	// 1. Set board data from history, enpassant square, castling, 50 move, hash. Recalculate what's needed
	MoveHistory* history = &board->MoveHistory[board->CurrentMove - 1];
	Move* move = &history->Move;

	#ifdef DEBUG
	if(Board_Piece(board, move->To) != move->PlayerPiece)
		int k = 23;
	#endif

	board->AttacksBlack = history->PrevAttacksBlack;
	board->AttacksWhite = history->PrevAttacksWhite;

	board->Hash ^= Zobrist_Keys[ZOBRIST_CASTLING][board->Castle];
	board->Castle = history->PrevCastleState;
	board->Hash ^= Zobrist_Keys[ZOBRIST_CASTLING][board->Castle];

	board->Hash ^= Zobrist_Keys[ZOBRIST_ENPASSANT][board->EnPassantTile];
	board->EnPassantTile = history->PrevEnPassantTile;
	board->Hash ^= Zobrist_Keys[ZOBRIST_ENPASSANT][board->EnPassantTile];

	board->FiftyMoveRulePlies = history->PrevFiftyMoveRulePlies;

	board->Hash ^= Zobrist_Keys[ZOBRIST_SIDE][board->PlayerTurn];
	board->PlayerTurn = move->PlayerColor;
	board->Hash ^=  Zobrist_Keys[ZOBRIST_SIDE][board->PlayerTurn];

	int capturedColor = (board->PlayerTurn == COLOR_WHITE) ? COLOR_BLACK : COLOR_WHITE;
	
	// 2. Unmake the move, clear "to", set player piece at old location...
	Board_ClearPiece(board, move->To);
	Board_SetPiece(board, move->From, move->PlayerPiece, move->PlayerColor);
	// 2. ...then set captured piece
	Board_SetPiece(board, move->CaptureTile, move->CapturePiece, capturedColor);

	//    2.1 If castling move, move the rook back
	if(move->Castle > 0)
	{
		switch(move->Castle)
		{
		case CASTLE_WK:
			Board_ClearPiece(board, 5);
			Board_SetPiece(board, 7, PIECE_ROOK, COLOR_WHITE);
			break;
		case CASTLE_WQ:
			Board_ClearPiece(board, 3);
			Board_SetPiece(board, 0, PIECE_ROOK, COLOR_WHITE);
			break;
		case CASTLE_BK:
			Board_ClearPiece(board, 61);
			Board_SetPiece(board, 63, PIECE_ROOK, COLOR_BLACK);
			break;
		case CASTLE_BQ:
			Board_ClearPiece(board, 59);
			Board_SetPiece(board, 56, PIECE_ROOK, COLOR_BLACK);
			break;
		default:
			break;
		}
	}

	// 3. Pop history off the stack
	board->CurrentMove -= 1;

	// 4. For debugging, compute new hash and compare with hash from history. must match!
	#ifdef DEBUG
	if(board->Hash != history->PrevHash)
		int k = 23;
	#endif
}

_Bool Board_CanPromote(Board* board, int square, int color, int piece)
{
	int y = Board_Y(square);

	return (piece == PIECE_PAWN) && ((y == 7 && color == COLOR_WHITE) | (y == 0 && color == COLOR_BLACK));
}

_Bool Board_Promote(Board* board, int square, int pieceType)
{
	if(!Board_CanPromote(board, square, Board_Color(board, square), Board_Piece(board, square)))
		return 0;

	int color = Board_Color(board, square);
	Board_ClearPiece(board, square);
	Board_SetPiece(board, square, pieceType, color);
	board->MoveHistory[board->CurrentMove - 1].Move.Promotion = pieceType;
	return 1;
}


uint8_t Board_GetCastling(Board* board)
{
	int whiteKing = Bitboard_Get(board->Boards[BOARD_WHITE] & board->Boards[PIECE_KING], 4);
	int blackKing = Bitboard_Get(board->Boards[BOARD_BLACK] & board->Boards[PIECE_KING], 60);

	uint64_t whiteRooks = board->Boards[BOARD_WHITE] & board->Boards[PIECE_ROOK];
	uint64_t blackRooks = board->Boards[BOARD_BLACK] & board->Boards[PIECE_ROOK];

	uint8_t wk = (whiteKing * (whiteRooks & 0x80)) > 0 ? CASTLE_WK : 0;
	uint8_t wq = (whiteKing * (whiteRooks & 0x1)) > 0 ? CASTLE_WQ : 0;
	uint8_t bk = (blackKing * (blackRooks & 0x8000000000000000)) > 0 ? CASTLE_BK : 0;
	uint8_t bq = (blackKing * (blackRooks & 0x100000000000000)) > 0 ? CASTLE_BQ : 0;

	uint8_t output = (wk | wq | bk | bq) & board->Castle;
	return output;
}

_Bool Board_IsChecked(Board* board, int color)
{
	if(color == COLOR_WHITE)
	{
		uint64_t whiteKing = board->Boards[BOARD_WHITE] & board->Boards[PIECE_KING];
		int output = (whiteKing & board->AttacksBlack) > 0 ? TRUE : FALSE;
		return output;
	}
	else
	{
		uint64_t blackKing = board->Boards[BOARD_BLACK] & board->Boards[PIECE_KING];
		int output = (blackKing & board->AttacksWhite) > 0 ? TRUE : FALSE;
		return output;
	}
}
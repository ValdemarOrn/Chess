
#include "Moves.h"
#include "Bitboard.h"
#include "Board.h"
#include "Moves/Pawn.h"
#include "Moves/Knight.h"
#include "Moves/Bishop.h"
#include "Moves/Rook.h"
#include "Moves/King.h"
#include "Moves/Queen.h"

// converts castling types to bitboard og possible king moves
// the key is castling type
uint64_t CastlingTableTypesToBitboard[16];

void Moves_Init()
{
	CastlingTableTypesToBitboard[CASTLE_WK]             = 0x40;
	CastlingTableTypesToBitboard[CASTLE_WQ]             = 0x4;
	CastlingTableTypesToBitboard[CASTLE_WK | CASTLE_WQ] = 0x44;

	CastlingTableTypesToBitboard[CASTLE_BK]             = 0x4000000000000000;
	CastlingTableTypesToBitboard[CASTLE_BQ]             = 0x400000000000000;
	CastlingTableTypesToBitboard[CASTLE_BK | CASTLE_BQ] = 0x4400000000000000;
}

uint64_t Moves_GetMoves(Board* board, int tile)
{
	int piece = Board_Piece(board, tile);
	int color = Board_Color(board, tile);
	uint64_t value = 0;
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

	if(piece == 0 || color == 0)
		return 0;

	if(piece == PIECE_PAWN)
	{
		if(color == COLOR_WHITE)
		{
			int blocked = Bitboard_GetRef(&occupancy, tile + 8);
			uint64_t moves = Pawn_ReadWhiteMove(tile);
			moves = (moves * !blocked) & ~occupancy;

			uint64_t attacks = Pawn_ReadWhiteAttack(tile);
			uint64_t enPassant = Moves_GetEnPassantMove(board, tile);

			attacks = attacks & board->Boards[BOARD_BLACK];
			return moves | attacks | enPassant;
		}
		else
		{
			int blocked = Bitboard_GetRef(&occupancy, tile - 8);
			uint64_t moves = Pawn_ReadBlackMove(tile);
			moves = (moves * !blocked) & ~occupancy;

			uint64_t attacks = Pawn_ReadBlackAttack(tile);
			uint64_t enPassant = Moves_GetEnPassantMove(board, tile);
			
			attacks = attacks & board->Boards[BOARD_WHITE];
			return moves | attacks | enPassant;
		}
	}

	switch(piece)
	{
	case(PIECE_KNIGHT):
		value = Knight_Read(tile);
		break;
	case(PIECE_BISHOP):
		value = Bishop_Read(tile, occupancy);
		break;
	case(PIECE_ROOK):
		value = Rook_Read(tile, occupancy);
		break;
	case(PIECE_QUEEN):
		value = Queen_Read(tile, occupancy);
		break;
	case(PIECE_KING):
		value = King_Read(tile);
		value |= (tile == 4 || tile == 60) ? Moves_GetCastlingMoves(board, color) : 0;
		break;
	}

	if(color == COLOR_WHITE)
		value = value & ~(board->Boards[BOARD_WHITE]);
	else
		value = value & ~(board->Boards[BOARD_BLACK]);

	return value;
}

uint64_t Moves_GetAttacks(Board* board, int tile)
{
	int piece = Board_Piece(board, tile);
	int color = Board_Color(board, tile);
	uint64_t value = 0;
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

	if(piece == PIECE_PAWN)
	{
		uint64_t enPassant = Moves_GetEnPassantMove(board, tile);

		if(color == COLOR_WHITE)
		{
			uint64_t attacks = Pawn_ReadWhiteAttack(tile);
			return attacks | enPassant;
		}
		else
		{
			uint64_t attacks = Pawn_ReadBlackAttack(tile);
			return attacks | enPassant;
		}
	}

	switch(piece)
	{
	case(PIECE_KNIGHT):
		value = Knight_Read(tile);
		break;
	case(PIECE_BISHOP):
		value = Bishop_Read(tile, occupancy);
		break;
	case(PIECE_ROOK):
		value = Rook_Read(tile, occupancy);
		break;
	case(PIECE_QUEEN):
		value = Queen_Read(tile, occupancy);
		break;
	case(PIECE_KING):
		value = King_Read(tile);
		break;
	}

	return value;
}

int Moves_GetAllMoves(Board* board, Move* moveList100)
{
	int moveCount = 0;
	uint8_t destinations[64];

	// for all pieces
	for (int i=0; i < 64; i++)
	{
		if(Board_Color(board, i) != board->PlayerTurn)
			continue;

		int from = i;
		int piece = Board_Piece(board, from);

		uint64_t destBoard = Moves_GetMoves(board, from);
		int destCount = Bitboard_BitList(destBoard, destinations);

		// for all of the piece's moves
		for(int j=0; j < destCount; j++)
		{
			int to = destinations[j];

			Move* move = &moveList100[moveCount];

			// check for en passant captures
			if(to == board->EnPassantTile && to != 0 && piece == PIECE_PAWN)
			{
				move->CapturePiece = PIECE_PAWN;
				move->CaptureTile = Moves_GetEnPassantVictimTile(board, from, to);
			}
			else
			{
				move->CapturePiece = Board_Piece(board, to);
				move->CaptureTile = to;
			}

			move->Castle = 0;
			move->From = from;
			move->PlayerColor = board->PlayerTurn;
			move->PlayerPiece = piece;
			move->To = to;

			if(Board_CanPromote(to, board->PlayerTurn, piece))
			{
				memcpy(&moveList100[moveCount + 1], move, sizeof(Move));
				memcpy(&moveList100[moveCount + 2], move, sizeof(Move));
				memcpy(&moveList100[moveCount + 3], move, sizeof(Move));

				moveList100[moveCount].Promotion = PIECE_KNIGHT;
				moveList100[moveCount + 1].Promotion = PIECE_BISHOP;
				moveList100[moveCount + 2].Promotion = PIECE_ROOK;
				moveList100[moveCount + 3].Promotion = PIECE_QUEEN;

				moveCount += 4;
			}
			else
			{
				move->Promotion = 0;
				moveCount += 1;
			}
		}
	}

	return moveCount;
}


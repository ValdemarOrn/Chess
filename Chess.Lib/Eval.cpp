
#include "Eval.h"
#include "EvalData.h"
#include "Moves.h"
#include "Moves/King.h"
#include <string.h>

// Reverse the array for the evaluation tables into the correct format
void Eval_ReverseArray(int* array64)
{
	int temp[64];
	memcpy(temp, array64, 64 * sizeof(int));

	for(int i = 0; i < 64; i++)
	{
		int line = i / 8;
		int fromLine = 7 - line;
		int col = i % 8;

		array64[i] = temp[fromLine * 8 + col];
	}
}

void Eval_Init()
{
	Eval_PieceValues[PIECE_PAWN]   = 100;
	Eval_PieceValues[PIECE_KNIGHT] = 320;
	Eval_PieceValues[PIECE_BISHOP] = 330;
	Eval_PieceValues[PIECE_ROOK]   = 500;
	Eval_PieceValues[PIECE_QUEEN]  = 900;
	Eval_PieceValues[PIECE_KING]   = 100000;

	Eval_MobilityBonus[PIECE_PAWN]   = 7;
	Eval_MobilityBonus[PIECE_KNIGHT] = 7;
	Eval_MobilityBonus[PIECE_BISHOP] = 5;
	Eval_MobilityBonus[PIECE_ROOK]   = 5;
	Eval_MobilityBonus[PIECE_QUEEN]  = 5;
	Eval_MobilityBonus[PIECE_KING]   = 10;

	Eval_UndefendedPiecePenalty[PIECE_PAWN]   = 0;
	Eval_UndefendedPiecePenalty[PIECE_KNIGHT] = 10;
	Eval_UndefendedPiecePenalty[PIECE_BISHOP] = 10;
	Eval_UndefendedPiecePenalty[PIECE_ROOK]   = 20;
	Eval_UndefendedPiecePenalty[PIECE_QUEEN]  = 20;
	Eval_UndefendedPiecePenalty[PIECE_KING]   = 20;

	Eval_ReverseArray(Eval_PositionPawn);
	Eval_ReverseArray(Eval_PositionKnight);
	Eval_ReverseArray(Eval_PositionBishop);
	Eval_ReverseArray(Eval_PositionKing);
	Eval_ReverseArray(Eval_PositionKingEndgame);

	Eval_Positions[PIECE_PAWN] = Eval_PositionPawn;
	Eval_Positions[PIECE_KNIGHT] = Eval_PositionKnight;
	Eval_Positions[PIECE_BISHOP] = Eval_PositionBishop;
	Eval_Positions[PIECE_ROOK] = Eval_PositionRook;
	Eval_Positions[PIECE_QUEEN] = Eval_PositionQueen;
	Eval_Positions[PIECE_KING] = Eval_PositionKing;
}

int Eval_Evaluate(Board* board)
{
	uint8_t locations[64];
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];
	int count = Bitboard_BitList(occupancy, locations);

	int whiteValue = 0;
	int blackValue = 0;

	for(int i = 0; i < count; i++)
	{
		int pValue = 0;
		int square = locations[i];
		int x = Board_X(square);
		int y = Board_Y(square);
		int piece = Board_Piece(board, square);
		int color = Board_Color(board, square);
		uint64_t moveMap = Moves_GetMoves(board, square);
		int moveCount = Bitboard_PopCount(moveMap);

		// index to read position tables
		int index = square;
		if(color == COLOR_BLACK)
			index = Flip[index];
		
		pValue += Eval_PieceValues[piece];
//		pValue += Eval_Positions[piece][index];
//		pValue += Eval_MobilityBonus[piece] * moveCount;

/*		if(color == COLOR_WHITE)
		{
			if(Bitboard_GetRef(&board->AttacksWhite, square) == 0)
				pValue -= Eval_UndefendedPiecePenalty[piece];

			if(piece == PIECE_PAWN)
			{
				uint64_t whitePawns = board->Boards[BOARD_WHITE] & board->Boards[BOARD_PAWNS];
				uint64_t pawnsOnFile = Bitboard_Files[x] & whitePawns;
				if(Bitboard_PopCount(pawnsOnFile) > 1)
					pValue -= Eval_Penalty_DoublePawn;

				// get all the squares around the pawn, I just use the King_Move table for that
				uint64_t pawnAreaBoard = King_Read(square);
				if(whitePawns & pawnAreaBoard == 0)
					pValue -= Eval_Penalty_IsolatedPawn;
			}
		}
		else
		{
			if(Bitboard_GetRef(&board->AttacksBlack, square) == 0)
				pValue -= Eval_UndefendedPiecePenalty[piece];

			if(piece == PIECE_PAWN)
			{
				uint64_t blackPawns = board->Boards[BOARD_BLACK] & board->Boards[BOARD_PAWNS];
				uint64_t pawnsOnFile = Bitboard_Files[x] & blackPawns;
				if(Bitboard_PopCount(pawnsOnFile) > 1)
					pValue -= Eval_Penalty_DoublePawn;

				// get all the squares around the pawn, I just use the King_Move table for that
				uint64_t pawnAreaBoard = King_Read(square);
				if(blackPawns & pawnAreaBoard == 0)
					pValue -= Eval_Penalty_IsolatedPawn;
			}
		}
*/
		if(color == COLOR_WHITE)
			whiteValue += pValue;
		else
			blackValue += pValue;
	}

	// slight bonus for current player
/*	if(board->PlayerTurn == COLOR_WHITE)
		whiteValue += Eval_Bonus_CurrentPlayer;
	else
		blackValue += Eval_Bonus_CurrentPlayer;
*/
	return whiteValue - blackValue;
}
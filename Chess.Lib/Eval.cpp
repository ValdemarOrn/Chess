
#include "Eval.h"
#include "EvalData.h"
#include "Moves.h"
#include "Moves/King.h"
#include "Search.h"
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

#ifdef DEBUG
EvalStats EStats[33];
#endif

typedef struct
{
	uint64_t Hash;
	int Score;
	
} EvalEntry;

const int Eval_EntryCount = 2000000;
const int Eval_NoEntry = 1000000000;
EvalEntry Eval_Entries[Eval_EntryCount];

int Eval_ReadEntry(uint64_t hash)
{
	int index = hash % Eval_EntryCount;
	EvalEntry* entry = &Eval_Entries[index];
	return (entry->Hash == hash) ? entry->Score : 1000000000;
}

void Eval_WriteEntry(uint64_t hash, int score)
{
	int index = hash % Eval_EntryCount;
	EvalEntry* entry = &Eval_Entries[index];
	entry->Hash = hash;
	entry->Score = score;
}

int Eval_Evaluate(Board* board)
{
	#ifdef STATS_SEARCH
	SStats.EvalTotal++;
	#endif

	int stored = Eval_ReadEntry(board->Hash);
	if(stored != Eval_NoEntry)
	{
		#ifdef STATS_SEARCH
		SStats.EvalHits++;
		#endif

		return stored;
	}

	#ifdef DEBUG
	EStats[COLOR_WHITE].DoublePawnPenalty = 0;
	EStats[COLOR_WHITE].IsolatedPawnPenalty = 0;
	EStats[COLOR_WHITE].Material = 0;
	EStats[COLOR_WHITE].MobilityBonus = 0;
	EStats[COLOR_WHITE].PositionalBonus = 0;
	EStats[COLOR_WHITE].TempoBonus = 0;
	EStats[COLOR_WHITE].UndefendedPenalty = 0;
	EStats[COLOR_BLACK].DoublePawnPenalty = 0;
	EStats[COLOR_BLACK].IsolatedPawnPenalty = 0;
	EStats[COLOR_BLACK].Material = 0;
	EStats[COLOR_BLACK].MobilityBonus = 0;
	EStats[COLOR_BLACK].PositionalBonus = 0;
	EStats[COLOR_BLACK].TempoBonus = 0;
	EStats[COLOR_BLACK].UndefendedPenalty = 0;
	#endif

	uint8_t locations[64];
	uint64_t whiteAttacks = Board_AttackMap(board, COLOR_WHITE);
	uint64_t blackAttacks = Board_AttackMap(board, COLOR_BLACK);

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
		pValue += Eval_Positions[piece][index];
		pValue += Eval_MobilityBonus[piece] * moveCount;

		#ifdef DEBUG
		EStats[color].Material += Eval_PieceValues[piece];
		EStats[color].PositionalBonus += Eval_Positions[piece][index];
		EStats[color].MobilityBonus += Eval_MobilityBonus[piece] * moveCount;
		#endif

		if(color == COLOR_WHITE)
		{
			if(Bitboard_GetRef(&whiteAttacks, square) == 0)
				pValue -= Eval_UndefendedPiecePenalty[piece];

			#ifdef DEBUG
			EStats[color].UndefendedPenalty -= Eval_UndefendedPiecePenalty[piece];
			#endif

			if(piece == PIECE_PAWN)
			{
				uint64_t whitePawns = board->Boards[BOARD_WHITE] & board->Boards[BOARD_PAWNS];
				uint64_t pawnsOnFile = Bitboard_Files[x] & whitePawns;
				if(Bitboard_PopCount(pawnsOnFile) > 1)
				{
					pValue -= Eval_Penalty_DoublePawn;
					#ifdef DEBUG
					EStats[color].DoublePawnPenalty -= Eval_Penalty_DoublePawn;
					#endif
				}

				// get all the squares around the pawn, I just use the King_Move table for that
				uint64_t pawnAreaBoard = King_Read(square);
				if(whitePawns & pawnAreaBoard == 0)
				{
					pValue -= Eval_Penalty_IsolatedPawn;
					#ifdef DEBUG
					EStats[color].IsolatedPawnPenalty -= Eval_Penalty_IsolatedPawn;
					#endif
				}
			}
		}
		else
		{
			if(Bitboard_GetRef(&blackAttacks, square) == 0)
				pValue -= Eval_UndefendedPiecePenalty[piece];

			#ifdef DEBUG
			EStats[color].UndefendedPenalty -= Eval_UndefendedPiecePenalty[piece];
			#endif

			if(piece == PIECE_PAWN)
			{
				uint64_t blackPawns = board->Boards[BOARD_BLACK] & board->Boards[BOARD_PAWNS];
				uint64_t pawnsOnFile = Bitboard_Files[x] & blackPawns;
				if(Bitboard_PopCount(pawnsOnFile) > 1)
				{
					pValue -= Eval_Penalty_DoublePawn;
					#ifdef DEBUG
					EStats[color].DoublePawnPenalty -= Eval_Penalty_DoublePawn;
					#endif
				}

				// get all the squares around the pawn, I just use the King_Move table for that
				uint64_t pawnAreaBoard = King_Read(square);
				if(blackPawns & pawnAreaBoard == 0)
				{
					pValue -= Eval_Penalty_IsolatedPawn;
					#ifdef DEBUG
					EStats[color].IsolatedPawnPenalty -= Eval_Penalty_IsolatedPawn;
					#endif
				}
			}
		}

		if(color == COLOR_WHITE)
			whiteValue += pValue;
		else
			blackValue += pValue;
	}

	// slight bonus for current player
	if(board->PlayerTurn == COLOR_WHITE)
		whiteValue += Eval_Bonus_CurrentPlayer;
	else
		blackValue += Eval_Bonus_CurrentPlayer;

	#ifdef DEBUG
	EStats[board->PlayerTurn].TempoBonus += Eval_Bonus_CurrentPlayer;
	#endif

	int output = whiteValue - blackValue;
	Eval_WriteEntry(board->Hash, output);
	return output;
}

EvalStats* Eval_GetEvalStats(int color)
{
	#ifdef DEBUG
	return &EStats[color];
	#endif
	#ifndef DEBUG
	return 0;
	#endif
}
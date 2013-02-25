
#include "Board.h"

int Eval_Values[8];

Eval_Values[PIECE_PAWN]   = 100;
Eval_Values[PIECE_KNIGHT] = 295;
Eval_Values[PIECE_BISHOP] = 300;
Eval_Values[PIECE_ROOK]   = 500;
Eval_Values[PIECE_QUEEN]  = 900;
Eval_Values[PIECE_KING]   = 1000000;

Eval_MobilityBonus[8];
Eval_Values[PIECE_PAWN]   = 7;
Eval_Values[PIECE_KNIGHT] = 7;
Eval_Values[PIECE_BISHOP] = 5;
Eval_Values[PIECE_ROOK]   = 5;
Eval_Values[PIECE_QUEEN]  = 5;
Eval_Values[PIECE_KING]   = 10;

Eval_UndefendedPiecePenalty[8];
Eval_UndefendedPiecePenalty[PIECE_PAWN]   = 0;
Eval_UndefendedPiecePenalty[PIECE_KNIGHT] = 10;
Eval_UndefendedPiecePenalty[PIECE_BISHOP] = 10;
Eval_UndefendedPiecePenalty[PIECE_ROOK]   = 20;
Eval_UndefendedPiecePenalty[PIECE_QUEEN]  = 20;
Eval_UndefendedPiecePenalty[PIECE_KING]   = 20;

Eval_Penalty_TrappedRook = 50;
Eval_Penalty_DoublePawn = 20;
Eval_Penalty_IsolatedPawn = 30;

int Eval_PositionPawn[64] = {
	  0,   0,   0,   0,   0,   0,   0,   0,
	  5,  10,  15,  20,  20,  15,  10,   5,
	  4,   8,  12,  16,  16,  12,   8,   4,
	 -5,   6,   5,  15,  15,   5,   6,  -5,
	 -5,   4,   5,  10,  10,   5,   4,  -5,
	 -5,   2,   2, -10, -10,   2,   2,  -5,
	 -5,   0,   0, -30, -30,   0,   0,  -5,
	  0,   0,   0,   0,   0,   0,   0,   0
};

int Eval_PositionKnight[64] = {
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10,   0,   0,   0,   0,   0,   0, -10,
	-10,   0,   5,   5,   5,   5,   0, -10,
	-10,   0,   5,  10,  10,   5,   0, -10,
	-10,   0,   5,  10,  10,   5,   0, -10,
	-10,   0,   5,   5,   5,   5,   0, -10,
	-10,   0,   0,   0,   0,   0,   0, -10,
	-10, -30, -10, -10, -10, -10, -30, -10
};

int Eval_PositionBishop[64] = {
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10,   0,   0,   0,   0,   0,   0, -10,
	-10,   0,   3,   5,   5,   3,   0, -10,
	-10,   0,   5,  12,  12,   5,   0, -10,
	-10,   0,   5,  12,  12,   5,   0, -10,
	-10,   0,   3,   5,   5,   3,   0, -10,
	-10,   0,   0,   0,   0,   0,   0, -10,
	-10, -10, -20, -10, -10, -20, -10, -10
};

int Eval_PositionKing[64] = {
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10, -10, -10, -10, -10, -10, -10, -10,
	-10, -10, -10, -10, -10, -10, -10, -10,
	-20, -15, -10, -10, -10, -10, -15, -20,
	-30, -15,  20, -10,   0, -10,  20, -30
};

int Eval_PositionKingEndgame[64] = {
	-50, -30,  -5,   0,   0,  -5, -30, -50,
	-30,  -5,   0,   0,   0,   0,  -5, -30,
	 -5,   0,   0,   5,   5,   0,   0,  -5,
	  0,   0,   5,  10,  10,   5,   0,   0,
	  0,   0,   5,  10,  10,   5,   0,   0,
	 -5,   0,   0,   5,   5,   0,   0,  -5,
	-30,  -5,   0,   0,   0,   0,  -5, -30,
	-50, -30,  -5,   0,   0,  -5, -30, -50,
}
#ifndef EVAL
#define EVAL

#include "Default.h"
#include "Board.h"

extern "C"
{
	extern int Eval_PieceValues[8];
	extern int Eval_MobilityBonus[8];
	extern int Eval_UndefendedPiecePenalty[8];
	extern int* Eval_Positions[8];

	const int Eval_Bonus_CurrentPlayer = 20;
	const int Eval_Penalty_TrappedRook = 50;
	const int Eval_Penalty_DoublePawn = 20;
	const int Eval_Penalty_IsolatedPawn = 30;

	extern int Eval_PositionPawn[64];
	extern int Eval_PositionKnight[64];
	extern int Eval_PositionBishop[64];
	extern int Eval_PositionRook[64];
	extern int Eval_PositionQueen[64];
	extern int Eval_PositionKing[64];
	extern int Eval_PositionKingEndgame[64];
	extern int Eval_Flip[64];

	#pragma pack(push, 1)
	typedef struct
	{
		int Material;
		int PositionalBonus;
		int MobilityBonus;
		int UndefendedPenalty;
		int DoublePawnPenalty;
		int IsolatedPawnPenalty;
		int TempoBonus;

	} EvalStats;
	#pragma pack(pop)

	__declspec(dllexport) void Eval_Init();
	__declspec(dllexport) int Eval_Evaluate(Board* board);

	// get some detailed statistics for the last evaluation performed
	__declspec(dllexport) EvalStats* Eval_GetEvalStats(int color);
}

#endif

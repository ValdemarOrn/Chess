#ifndef EVAL
#define EVAL

#include "Default.h"
#include "Board.h"

extern "C"
{
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

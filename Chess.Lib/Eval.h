#ifndef EVAL
#define EVAL

#include "Default.h"
#include "Board.h"

extern "C"
{
	__declspec(dllexport) void Eval_Init();
	__declspec(dllexport) int Eval_Evaluate(Board* board);
}

#endif

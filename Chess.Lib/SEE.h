#ifndef SEE
#define SEE

#include "Default.h"
#include "Board.h"

extern "C"
{
	// calculates Static exchange evaluation. Checks for pinned pieces
	__declspec(dllexport) int SEE_Square(Board* board, int square);

	// evaluate if a capture is good or bad. Starts by making the capture
	// and then performs standard SEE on the "to" square
	__declspec(dllexport) int SEE_Capture(Board* board, int from, int to);
}

#endif

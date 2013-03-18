#ifndef SEE
#define SEE

#include "Default.h"
#include "Board.h"

extern "C"
{
	// define a constant that gets returned when SEE is unable to resolve the value due to a pinned piece
	const int SEE_Failed = 100;

	// performs a very basic SEE. Checks for pinned pieces but does not attempt to resolve them
	__declspec(dllexport) int SEE_Basic(Board* board, int from, int to);
}

#endif

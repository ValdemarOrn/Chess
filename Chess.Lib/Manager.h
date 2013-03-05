#ifndef MANAGER
#define MANAGER

#include "Default.h"

extern "C"
{
	typedef void (*callbackFunction)(char* text);

	extern callbackFunction Manager_Callback;

	// Allocate memory for board and move array, initializes the new board.
	// Calculates Zobrist hash for empty board, but with castling rights
	__declspec(dllexport) void Manager_SetCallback(callbackFunction);

}

#endif

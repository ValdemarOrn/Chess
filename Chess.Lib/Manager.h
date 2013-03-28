#ifndef MANAGER
#define MANAGER

#include "Default.h"

extern "C"
{
	typedef void (*callbackFunction)(const char* key, const char* value);
	typedef void (*flushFunction)();

	const char* const Message_BeginMessage = "%%BEGIN%%";
	const char* const Message_EndMessage = "%%END%%";

	const char* const Message_SearchData = "SearchData";
	const char* const Message_Ply = "Ply";
	const char* const Message_Score = "Score";
	const char* const Message_Nodes = "Nodes";
	const char* const Message_PV = "PV";

	// Allocate memory for board and move array, initializes the new board.
	// Calculates Zobrist hash for empty board, but with castling rights
	__declspec(dllexport) void Manager_SetCallback(callbackFunction callback);

	__declspec(dllexport) void Manager_BeginMessage(const char* messageType);
	__declspec(dllexport) void Manager_Write(const char* key, const char* value);
	__declspec(dllexport) void Manager_EndMessage();

}

#endif

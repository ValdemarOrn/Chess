#ifndef MANAGER
#define MANAGER

#include "Default.h"

extern "C"
{
	typedef void (__stdcall *callbackFunction)(const char* key, const char* value);

	const char* const Message_BeginMessage = "%%BEGIN%%";
	const char* const Message_EndMessage = "%%END%%";

	const char* const Message_SearchData = "SearchData";
	const char* const Message_Ply = "Ply";
	const char* const Message_Score = "Score";
	const char* const Message_Nodes = "Nodes";
	const char* const Message_PV = "PV";

	__dllexport void Manager_SetCallback(callbackFunction callback);

	__dllexport void Manager_BeginMessage(const char* messageType);
	__dllexport void Manager_Write(const char* key, const char* value);
	__dllexport void Manager_EndMessage();

}

#endif

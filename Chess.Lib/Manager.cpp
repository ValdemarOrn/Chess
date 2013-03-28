
#include "Manager.h"

callbackFunction Manager_Callback;

void Manager_SetCallback(callbackFunction callback)
{
	Manager_Callback = callback;
}

void Manager_BeginMessage(const char* messageType)
{
	Manager_Callback(Message_BeginMessage, messageType);
}

void Manager_Write(const char* key, const char* value)
{
	Manager_Callback(key, value);
}

void Manager_EndMessage()
{
	Manager_Callback(Message_EndMessage, 0);
}
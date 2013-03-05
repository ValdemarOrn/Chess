
#include "Manager.h"

callbackFunction Manager_Callback;

void Manager_SetCallback(callbackFunction callback)
{
	Manager_Callback = callback;
}
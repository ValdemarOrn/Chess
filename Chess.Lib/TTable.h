#ifndef TTABLE
#define TTABLE

#include "Default.h"
#include <string.h>

extern "C"
{
	#pragma pack(push, 1)
	typedef struct
	{
		uint64_t Hash;
		
		uint8_t BestMoveFrom;
		uint8_t BestMoveTo;

		uint8_t NodeType;
		int8_t Depth;

		int Score;
		
	} TTableEntry;
	#pragma pack(pop)

	extern TTableEntry* TTable;
	extern uint32_t TTableSize;

	__declspec(dllexport) int TTable_GetTableSize();
	__declspec(dllexport) TTableEntry* TTable_GetTable();

	__declspec(dllexport) void TTable_Init(int sizeMB);
	__declspec(dllexport) void TTable_Delete();

	__declspec(dllexport) TTableEntry* TTable_Read(uint64_t hash);

	// Attempt to insert a new entry. resolves conflicts if any.
	// Returns true if entry gets added, false if not
	__declspec(dllexport) _Bool TTable_Insert(TTableEntry* entry);

	__inline_always int TTable_GetTableSize()
	{
		return TTableSize;
	}

	__inline_always TTableEntry* TTable_GetTable()
	{
		return TTable;
	}

	__inline_always TTableEntry* TTable_Read(uint64_t hash)
	{
		TTableEntry* entry = &(TTable[hash % TTableSize]);
		return (entry->Hash == hash) ? entry : 0;
	}

	__inline_always _Bool TTable_Insert(TTableEntry* entry)
	{
		uint64_t index = entry->Hash % TTableSize;
		TTableEntry* existing = &TTable[index];

		_Bool copy = FALSE;

		if(existing->Hash == 0)
			copy = TRUE;
		else if(existing->Depth < entry->Depth)
			copy = TRUE;

		if(copy == TRUE)
			memcpy(&TTable[index], entry, sizeof(TTableEntry));	

		return copy;
	}
}

#endif

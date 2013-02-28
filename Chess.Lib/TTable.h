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
		uint8_t Depth;
		uint8_t Age;

		int16_t Score;
		
	} TTableEntry;
	#pragma pack(pop)

	TTableEntry* TTable;
	uint32_t TTableSize;

	__declspec(dllexport) int TTable_GetTableSize();

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

	__inline_always TTableEntry* TTable_Read(uint64_t hash)
	{
		return &(TTable[hash % TTableSize]);
	}

	__inline_always _Bool TTable_Insert(TTableEntry* entry)
	{
		uint64_t index = entry->Hash % TTableSize;
		TTableEntry* existing = &TTable[index];

		_Bool copy = FALSE;

		if(existing->Hash == 0)
			copy = TRUE;
		else if(existing->Depth <= entry->Depth)
			copy = TRUE;

		if(copy == TRUE)
			memcpy(&TTable[index], entry, sizeof(TTableEntry));	

		return copy;
	}
}

#endif

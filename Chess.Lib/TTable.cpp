
#include "TTable.h"
#include "Bitboard.h"

TTableEntry* TTable;
uint32_t TTableSize;

void TTable_Init(int sizeMB)
{
	uint64_t count = sizeMB * 1024 * 1024;
	int entrySize = sizeof(TTableEntry);
	count = count / entrySize;
	TTable = new TTableEntry[(int)count];
	TTableSize = (int)count;

	memset(TTable, 0, sizeof(TTableEntry) * TTableSize);
}

void TTable_Delete()
{
	delete TTable;
	TTableSize = 0;
}

void TTable_ClearAll()
{
	memset(TTable, 0, sizeof(TTableEntry) * TTableSize);
}
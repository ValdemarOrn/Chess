
#include "TTable.h"
#include "Bitboard.h"

TTableEntry* TTable;
uint32_t TTableSize;

void TTable_Init(int sizeMB)
{
	uint64_t count = sizeMB * 1024 * 1024;
	count = count / sizeof(TTableEntry);
	TTable = new TTableEntry[count];
	TTableSize = count;

	memset(TTable, 0, count * sizeof(TTableEntry));
}

void TTable_Delete()
{
	delete TTable;
}
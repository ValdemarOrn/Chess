
#include "Perft.h"
#include "Moves.h"

uint64_t Perft(Board* board, int depth);
PerftResults Results;

PerftResults* Perft_Search(Board* board, int depth)
{
	delete Results.Entries;
	
	Results.Entries = new PerftEntry[200];
	Results.StartDepth = depth;
	Results.EntryCount = 0;
	Results.Total = 0;

	Results.Total = Perft(board, depth);
	return &Results;
}

uint64_t Perft(Board* board, int depth)
{
	uint64_t total = 0;

	// find all the moves
	Move moveList[100];
	int moveCount = Moves_GetAllMoves(board, moveList);
 
	for (int i = 0; i < moveCount; i++) {
		Move* move = &moveList[i];
		_Bool valid = Board_Make(board, move->From, move->To);
		
		if(!valid)
			continue;

		if(move->Promotion > 0)
			Board_Promote(board, move->To, move->Promotion);

		uint64_t cnt = (depth == 1) ? 1 : Perft(board, depth - 1);
		total += cnt;
		Board_Unmake(board);

		// log data in the top node
		if(Results.StartDepth == depth)
		{
			Results.Entries[Results.EntryCount].Count = cnt;
			Results.Entries[Results.EntryCount].From = move->From;
			Results.Entries[Results.EntryCount].To = move->To;
			Results.EntryCount++;
		}
	}

	return total;
}
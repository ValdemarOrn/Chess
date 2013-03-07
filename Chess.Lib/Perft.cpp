
#include "Perft.h"
#include "Moves.h"

uint64_t Perft_Search(Board* board, int depth)
{
	uint64_t count = 0;

	// find all the moves
	Move moveList[100];
	int moveCount = Moves_GetAllMoves(board, moveList);
 
	for (int i = 0; i < moveCount; i++) {
		Move* move = &moveList[i];
		_Bool valid = Board_Make(board, move->From, move->To);
		
		if(!valid)
			continue;

		count += (depth == 1) ? 1 : Perft_Search(board, depth - 1); 
		Board_Unmake(board);
	}

	return count;
}
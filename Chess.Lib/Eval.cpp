
#include "Eval.h"

int Eval_Evaluate(Board* board)
{
	uint8_t locations[64];
	uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];
	int count = Bitboard_BitList(occupancy, locations);

	for(int i = 0; i < count; i++)
	{
		int square = locations[i];

		int piece = Board_Piece(board, square);
		int color = Board_Color(board, square);
	}
}
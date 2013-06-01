
#include <string.h>
#include "SEE.h"
#include "Eval.h"

int SEE_Exchange(Board* board, int square);

int SEE_Square(Board* board, int square)
{
	// create a copy of the bitboard so we don't destroy the originals
	Board newBoard;
	memcpy(newBoard.Boards, board->Boards, sizeof(uint64_t) * 8);
	memcpy(newBoard.Tiles, board->Tiles, sizeof(uint8_t) * 64);
	
	return SEE_Exchange(&newBoard, square);
}

int SEE_Exchange(Board* board, int square)
{
	uint64_t pinned = 0;

	int victimColor = Board_Color(board, square);
	int victimPiece = Board_Piece(board, square);
	int attackerColor = (victimColor == COLOR_WHITE) ? COLOR_BLACK : COLOR_WHITE;

SEE_Make:
	int from = Board_GetSmallestAttacker(board, square, attackerColor, pinned);
	int attackerPiece = Board_Piece(board, from);
	if(from == -1)
		return 0;

	Board_ClearPiece(board, from);
	Board_ClearPiece(board, square);
	Board_SetPiece(board, square, attackerPiece, attackerColor);
	
	// if move has self checked, undo move, mark piece as pinned and retry
	if(Board_IsChecked(board, attackerColor))
	{
		// mark as pinned
		Bitboard_SetRef(&pinned, from);

		// undo
		Board_ClearPiece(board, square);
		Board_SetPiece(board, from, attackerPiece, attackerColor);

		goto SEE_Make;
	}

	int nextExchange = SEE_Exchange(board, square);
	if(nextExchange < 0)
		nextExchange = 0; // if the other player would stand to lose value by recapturing, he won't do it

	int result = Eval_PieceValues[victimPiece] - nextExchange;
	return (result < 0) ? 0 : result;

}

int SEE_Capture(Board* board, int from, int to)
{
	// create a copy of the bitboard so we don't destroy the originals
	Board newBoard;
	memcpy(newBoard.Boards, board->Boards, sizeof(uint64_t) * 8);
	memcpy(newBoard.Tiles, board->Tiles, sizeof(uint8_t) * 64);

	int victim = Board_Piece(&newBoard, to);
	int attacker = Board_Piece(&newBoard, from);
	int attackerColor = Board_Color(&newBoard, from);

	Board_ClearPiece(&newBoard, from);
	Board_ClearPiece(&newBoard, to);
	Board_SetPiece(&newBoard, to, attacker, attackerColor);

	int exchange = SEE_Exchange(&newBoard, to);
	return Eval_PieceValues[victim] - exchange;
}
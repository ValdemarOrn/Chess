
#include "Move.h"
#include "Board.h"

void Move_ToString(MoveSmall* move, char* dest)
{
	char from[2];
	char to[2];

	Move_SquareToString(move->From, from);
	Move_SquareToString(move->To, to);

	dest[0] = from[0];
	dest[1] = from[1];
	dest[2] = to[0];
	dest[3] = to[1];
	dest[4] = 0;
}

void Move_SquareToString(int square, char* dest)
{
	int x = Board_X(square);
	int y = Board_Y(square);

	dest[0] = 'a' + x;
	dest[1] = '1' + y;
}

void Move_PieceToString(int piece, char* dest)
{
	switch(piece)
	{
	case PIECE_PAWN:
		dest[0] = 'P';
		dest[1] = 0;
		break;
	case PIECE_KNIGHT:
		dest[0] = 'N';
		dest[1] = 0;
		break;
	case PIECE_BISHOP:
		dest[0] = 'B';
		dest[1] = 0;
		break;
	case PIECE_ROOK:
		dest[0] = 'R';
		dest[1] = 0;
		break;
	case PIECE_QUEEN:
		dest[0] = 'Q';
		dest[1] = 0;
		break;
	case PIECE_KING:
		dest[0] = 'K';
		dest[1] = 0;
		break;
	default:
		dest[0] = 'X';
		dest[1] = 0;
	}
}

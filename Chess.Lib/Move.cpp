/*
#include "Move.h"

Move* Move_CreateSimple(uint8_t from, uint8_t to)
{
	return Move_Create(from, to, 0, 0, 0, 0, 0, 0, 0, 0, 0);
}

Move* Move_Create(
	uint8_t from,
	uint8_t to,
	uint8_t moveCount,
	uint8_t color,
	uint8_t capture,
	uint8_t captureTile,
	uint8_t promotion,

	uint8_t check,
	uint8_t mate,
	uint8_t queenside,
	uint8_t kingside)
{
	Move* move = new Move();
	move->From = from;
	move->To = to;
	move->MoveCount = moveCount;
	move->Color = color;
	move->Capture = capture;
	move->CaptureTile = captureTile;
	move->Promotion = promotion;

	move->Check = check;
	move->Mate = mate;
	move->Queenside = queenside;
	move->Kingside = kingside;

	return move;
}

void Move_Delete(Move* move)
{
	delete move;
}*/
#ifndef ORDER
#define ORDER

#include "Default.h"
#include "Search.h"

extern "C"
{
	const int Order_StageInit         = 0;
	const int Order_StagePV           = 9000000;
	const int Order_StageHash         = 8000000;
	const int Order_StageCaptures     = 7000000;
	const int Order_StagePromotions   = 6000000;
	const int Order_StageKillerMoves  = 5000000;
	const int Order_StageHistoryMoves = 4000000;
	const int Order_StageMoveBNRQ     = 3000000; // Bishop, knight, rook and queen moves
	const int Order_StageMoveK        = 2000000; // king moves
	const int Order_StageMoveP        = 1000000; // pawn moves

	#pragma pack(push, 1)
	typedef struct
	{
		Move MoveList[100];
		int MoveRank[100];
		int MoveCount;

		// where in the process are we? Have we checked PV moves? Captures? History moves?
		int OrderStage;

		// how many moves have been ordered and not played
		int OrderedMoves;

	} Order;
	#pragma pack(pop)

	__declspec(dllexport) void Order_NextStage(SearchContext* ctx, Order* order, int ply);
	__declspec(dllexport) Move* Order_GetMove(SearchContext* ctx, Order* moves, int ply);

	// replacement strategy for killer moves
	void Order_SetKillerMove(SearchContext* ctx, int ply, int from, int to, int piece);
}

#endif

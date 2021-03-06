#ifndef MOVES
#define MOVES

#include "Default.h"
#include "Board.h"
#include "Move.h"

extern "C"
{
	__dllexport void Moves_Init();

	// ------------- Find moves & attacks -------------

	__dllexport uint64_t Moves_GetMoves(Board* board, int tile);
	__dllexport uint64_t Moves_GetAttacks(Board* board, int tile);

	// Fills up the moveList with all possible pseudo-legal moves for the current player
	// This function also handles promotions and creates moves for every promotion possibility
	__dllexport int Moves_GetAllMoves(Board* board, Move* moveList100);

	// ------------- Specific moves -------------

	// Checks if the move is a castling move. Returns the type of castling that is
	// executed. Returns 0 if not a castling move.
	__dllexport uint8_t Moves_GetCastlingType(Board* board, int from, int to);

	// Returns the castling types that are currently available to the king
	// Takes into account if square between rook and king are blocked or attacked
	__dllexport uint8_t Moves_GetAvailableCastlingTypes(Board* board, int color);

	// Finds available castling moves for king of specified color
	// Returns a bitboard with valid castle moves.
	__dllexport uint64_t Moves_GetCastlingMoves(Board* board, int color);

	// Checks if this move captures another piece
	__dllexport _Bool Moves_IsCaptureMove(Board* board, int from, int to);

	// Checks if the move is an en passant move
	__dllexport _Bool Moves_IsEnPassantCapture(Board* board, int from, int to);

	// Returns the tile of the pawn that is killed in an en passant capture
	__dllexport int Moves_GetEnPassantVictimTile(Board* board, int from, int to);

	// Checks if the move is pawn moving 2 tiles, and returns the tile at which it can be captured
	// (between from and to). Returns 0 if the piece can not be captured in an en passant move
	__dllexport int Moves_GetEnPassantTile(Board* board, int from, int to);

	// Returns a bitboard containing the en passant capture, if the pawn that is about to move
	// is able to attack the en-passant tile.
	__dllexport uint64_t Moves_GetEnPassantMove(Board* board, int from);



	// ------------- Inline function definitions -----------

	// square that can't be attacked during castling
	//const uint64_t WK_NOATTACK = 0x70;
	//const uint64_t WQ_NOATTACK = 0x1C;
	//const uint64_t BK_NOATTACK = 0x7000000000000000;
	//const uint64_t BQ_NOATTACK = 0x1C00000000000000;

	// squares that can't be occupied during castling
	const uint64_t WK_CLEAR = 0x60;
	const uint64_t WQ_CLEAR = 0xE;
	const uint64_t BK_CLEAR = 0x6000000000000000;
	const uint64_t BQ_CLEAR = 0xE00000000000000;

	// converts castling types to bitboard og possible king moves
	// the key is castling type
	extern uint64_t CastlingTableTypesToBitboard[16];

	__inline_always uint8_t Moves_GetCastlingType(Board* board, int from, int to)
	{
		if(from == 4)
		{
			uint64_t whiteKing = board->Boards[BOARD_WHITE] & board->Boards[BOARD_KINGS];
			int isKing = Bitboard_GetRef(&whiteKing, 4);
			return isKing * (((to == 2) * CASTLE_WQ) | ((to == 6) * CASTLE_WK));
		}
		else if(from == 60)
		{
			uint64_t blackKing = board->Boards[BOARD_BLACK] & board->Boards[BOARD_KINGS];
			int isKing = Bitboard_GetRef(&blackKing, 60);

			return isKing * (((to == 58) * CASTLE_BQ) | ((to == 62) * CASTLE_BK));
		}

		return 0;
	}

	// return castling types that can be performed at this moment
	__inline_always uint8_t Moves_GetAvailableCastlingTypes(Board* board, int color)
	{
		uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

		if(color == COLOR_WHITE)
		{
			_Bool wkAttacked = Board_IsAttacked(board, 4, COLOR_BLACK) | Board_IsAttacked(board, 5, COLOR_BLACK) | Board_IsAttacked(board, 6, COLOR_BLACK);
			_Bool wqAttacked = Board_IsAttacked(board, 2, COLOR_BLACK) | Board_IsAttacked(board, 3, COLOR_BLACK) | Board_IsAttacked(board, 4, COLOR_BLACK);

			_Bool wk = !wkAttacked && ((WK_CLEAR & occupancy) == 0);
			_Bool wq = !wqAttacked && ((WQ_CLEAR & occupancy) == 0);
		
			return (wk * (CASTLE_WK & board->Castle)) | (wq *  (CASTLE_WQ & board->Castle));
		}
		else
		{
			_Bool bkAttacked = Board_IsAttacked(board, 60, COLOR_WHITE) | Board_IsAttacked(board, 61, COLOR_WHITE) | Board_IsAttacked(board, 62, COLOR_WHITE);
			_Bool bqAttacked = Board_IsAttacked(board, 58, COLOR_WHITE) | Board_IsAttacked(board, 59, COLOR_WHITE) | Board_IsAttacked(board, 60, COLOR_WHITE);

			_Bool bk = !bkAttacked && ((BK_CLEAR & occupancy) == 0);
			_Bool bq = !bqAttacked && ((BQ_CLEAR & occupancy) == 0);

			return (bk * (CASTLE_BK & board->Castle)) | (bq *  (CASTLE_BQ & board->Castle));
		}
	}

	__inline_always uint64_t Moves_GetCastlingMoves(Board* board, int color)
	{
		uint8_t availableTypes = Moves_GetAvailableCastlingTypes(board, color);
		return CastlingTableTypesToBitboard[availableTypes];
	}

	__inline_always _Bool Moves_IsCaptureMove(Board* board, int from, int to)
	{
		uint64_t occupancy = board->Boards[BOARD_WHITE] | board->Boards[BOARD_BLACK];

		if (Bitboard_Get(occupancy, to) > 0 || to == board->EnPassantTile)
			return true;

		return false;
	}

	__inline_always _Bool Moves_IsEnPassantCapture(Board* board, int from, int to)
	{
		return (to != 0) && (board->EnPassantTile == to) && Bitboard_GetRef(&board->Boards[BOARD_PAWNS], from);
	}

	__inline_always int Moves_GetEnPassantVictimTile(Board* board, int from, int to)
	{
		int y = Board_Y(board->EnPassantTile);
		return (y == 2) ? (to + 8) : (to - 8);
	}

	__inline_always int Moves_GetEnPassantTile(Board* board, int from, int to)
	{
		int color = Board_Color(board, from);
		int y = Board_Y(from);

		if (Bitboard_Get(board->Boards[BOARD_PAWNS], from) == 0)
			return 0;

		if (color == COLOR_WHITE && y == 1 && to == (from + 16))
			return from + 8;

		if (color == COLOR_BLACK && y == 6 && to == (from - 16))
			return from - 8;

		return 0;
	}

	__inline_always uint64_t Moves_GetEnPassantMove(Board* board, int from)
	{
		if(board->EnPassantTile == 0)
			return 0;

		int color = Board_Color(board, from);

		int epX = Board_X(board->EnPassantTile);
		int epY = Board_Y(board->EnPassantTile);

		int fromX = Board_X(from);
		int fromY = Board_Y(from);

		if(color == COLOR_WHITE)
		{
			if(fromY + 1 == epY)
				if(fromX + 1 == epX || fromX - 1 == epX)
					return Bitboard_Set(0, board->EnPassantTile);
		}
		else
		{
			if(fromY - 1 == epY)
				if(fromX + 1 == epX || fromX - 1 == epX)
					return Bitboard_Set(0, board->EnPassantTile);
		}

		return 0;
	}

}

#endif

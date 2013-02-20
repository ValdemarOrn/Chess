#ifndef MOVES
#define MOVES

#include "Default.h"
#include "Board.h"
#include "Move.h"


extern "C"
{

	__declspec(dllexport) void Moves_Init();

	// ------------- Find moves & attacks -------------

	__declspec(dllexport) uint64_t Moves_GetMoves(Board* board, int tile);
	__declspec(dllexport) uint64_t Moves_GetAttacks(Board* board, int tile);

	// ------------- Specific moves -------------


	// Checks if the move is a castling move. Returns the type of castling that is
	// executed. Returns 0 if not a castling move.
	__declspec(dllexport) uint8_t Moves_GetCastlingType(Board* board, int from, int to);

	// Returns the castling types that are currently available to the king
	__declspec(dllexport) uint8_t Moves_GetAvailableCastlingTypes(Board* board, int color);

	// Finds available castling moves for king of specified color
	// Returns a bitboard with valid castle moves.
	__declspec(dllexport) uint64_t Moves_GetCastlingMoves(Board* board, int color);


	// Checks if a piece can be promoted
	__declspec(dllexport) int Moves_CanPromote(Board* board, int square);

	// Checks if this move captures another piece
	__declspec(dllexport) int Moves_IsCaptureMove(Board* board, int from, int to);

	// Checks if the move is an en passant move
	__declspec(dllexport) int Moves_IsEnPassantCapture(Board* board, int from, int to);

	// Returns the tile of the pawn that is killed in an en passant capture
	__declspec(dllexport) int Moves_GetEnPassantVictimTile(Board* board, int from, int to);

	// Checks if the move is pawn moving 2 tiles, and returns the tile at which it can be captured
	// (between from and to). Returns 0 if the piece can not be captured in an en passant move
	__declspec(dllexport) int Moves_GetEnPassantTile(Board* board, int from, int to);

	// Returns a bitboard containing the en passant capture, if the pawn that is about to move
	// is able to attack the en-passant tile.
	__declspec(dllexport) uint64_t Moves_GetEnPassantMove(Board* board, int from);
}

#endif
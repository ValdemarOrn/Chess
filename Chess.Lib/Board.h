#ifndef BOARD
#define BOARD

#include "Default.h"
#include "Move.h"
#include "Bitboard.h"

extern "C"
{
	#ifdef DEBUG
	extern uint64_t Board_TotalMakes;
	extern uint64_t Board_MakesThatCanSelfCheck;
	#endif

	const int COLOR_WHITE = 1 << 4;
	const int COLOR_BLACK = 2 << 4;

	const int PIECE_PAWN = 2;
	const int PIECE_KNIGHT = 3;
	const int PIECE_BISHOP = 4;
	const int PIECE_ROOK = 5;
	const int PIECE_QUEEN = 6;
	const int PIECE_KING = 7;

	const int CASTLE_WK = 1 << 0;
	const int CASTLE_WQ = 1 << 1;
	const int CASTLE_BK = 1 << 2;
	const int CASTLE_BQ = 1 << 3;
	
	const int BOARD_WHITE = 0;
	const int BOARD_BLACK = 1;
	const int BOARD_PAWNS = 2;
	const int BOARD_KNIGHTS = 3;
	const int BOARD_BISHOPS = 4;
	const int BOARD_ROOKS = 5;
	const int BOARD_QUEENS = 6;
	const int BOARD_KINGS = 7;

	#pragma pack(push, 1)
	typedef struct
	{
		uint64_t Boards[8];
		uint8_t Tiles[64];

		uint64_t _AttacksWhite; // Lazy Eval, can be zero
		uint64_t _AttacksBlack; // Lazy Eval, can be zero
		uint8_t _IsChecked; // Lazy Eval, can be UNKNOWN

		uint64_t Hash;

		int CurrentMove;
		uint8_t PlayerTurn; // color of player to move

		uint8_t EnPassantTile; // set to 0 if not available. tile 0 can never be an en-passant square anyway
		uint8_t FiftyMoveRulePlies;
		uint8_t Castle;
		
		MoveHistory* MoveHistory;

	} Board;
	#pragma pack(pop)

	// Allocate memory for board and move array, initializes the new board.
	// Calculates Zobrist hash for empty board, but with castling rights
	__declspec(dllexport) Board* Board_Create();

	// unallocates board struct and move array
	__declspec(dllexport) void Board_Delete(Board* board);

	// Makes a deep copy of the board
	__declspec(dllexport) Board* Board_Copy(Board* board);

	// Initializes the board structure with castling rights, turn, etc.
	// if setPieces = 1 then pieces are set to standard initial chess position
	__declspec(dllexport) void Board_Init(Board* board, int setPieces);

	// Get X coordinate (file - 1)
	__declspec(dllexport) int Board_X(int tile);

	// Get Y coordinate (rank - 1)
	__declspec(dllexport) int Board_Y(int tile);

	// Get color of desired square. returns 0 if empty
	__declspec(dllexport) int Board_Color(Board* board, int square);

	// Get piece at current square. returns 0 if empty
	__declspec(dllexport) int Board_Piece(Board* board, int square);

	// Set a piece at the desired location and update the hash.
	// square MUST be empty! Otherwise hash gets corrupted
	__declspec(dllexport) void Board_SetPiece(Board* board, int square, int pieceType, int color);

	// Clear the piece from the desired location
	__declspec(dllexport) void Board_ClearPiece(Board* board, int square);

	// only used for testing. Regenerates the Board.Tiles map from the bitboards
	__declspec(dllexport) void Board_GenerateTileMap(Board* board);

	// Returns a bitmap of all squares attacked by the specified side
	__declspec(dllexport) uint64_t Board_AttackMap(Board* board, int color);

	// tests if a move might cause a self check. Used during legality check in Board_Make.
	// If a move might cause self check then the kings safety must be evaluated after the move.
	__declspec(dllexport) _Bool Board_MoveCanSelfCheck(Board* board, int from, int to);

	// Checks if a square is under attack by a player
	__declspec(dllexport) _Bool Board_IsAttacked(Board* board, int square, int attackerColor);

	// the the least valuable piece that attacks a square, and is not pinned.
	// returns the square of the attacker. Returns -1 if no valid attacker is found
	__declspec(dllexport) int Board_GetSmallestAttacker(Board* board, int square, int attackerColor, uint64_t pinnedPieces);

	// Makes a move on the board. Updates hash, set castling, verifies move is legal
	// if illegal it is automatically taken back.
	// Return 1 if legal, 0 if illegal and no move was made
	__declspec(dllexport) _Bool Board_Make(Board* board, int from, int to);
	
	// Do a null (pass) move
	__declspec(dllexport) _Bool Board_MakeNullMove(Board* board);

	// Takes back the last move in the move array
	__declspec(dllexport) void Board_Unmake(Board* board);

	// Checks if a piece can be promoted
	__declspec(dllexport) _Bool Board_CanPromote(Board* board, int square, int color, int piece);

	// Promotes a pawn at the specified square. Returns true if pawn was promoted
	__declspec(dllexport) _Bool Board_Promote(Board* board, int square, int pieceType);

	// returns the castling rights that are allowed, given the state of the board
	// checks location of kings and rooks and current castling rights
	__declspec(dllexport) uint8_t Board_GetCastling(Board* board);

	// Checks if the king of the specified color is in check
	__declspec(dllexport) _Bool Board_IsChecked(Board* board, int color);

	// Checks if a piece is attacking a lesser piece that is defended
	__declspec(dllexport) _Bool Board_BadCapture(Board* board, Move* move);

	// Creates a FEN representation of the board
	__declspec(dllexport) void Board_ToFEN(Board* board, char* outputString100);

	// ------------ Inline function definitions

	__inline_always int Board_X(int tile)
	{
		// 128 constant prevent negative numbers in modulo output
		return tile & 0x07; // x & 0b00000111 == x % 8
	}

	__inline_always int Board_Y(int tile)
	{
		return tile >> 3; // ( x >> 3 == x / 8)
	}

	__inline_always int Board_Color(Board* board, int tile)
	{
		return board->Tiles[tile] & 0xF0;
	}

	__inline_always int Board_Piece(Board* board, int tile)
	{
		return board->Tiles[tile] & 0x0F;
	}

	__inline_always _Bool Board_CanPromote(Board* board, int square, int color, int piece)
	{
		int y = Board_Y(square);

		return (piece == PIECE_PAWN) && ((y == 7 && color == COLOR_WHITE) | (y == 0 && color == COLOR_BLACK));
	}

	__inline_always _Bool Board_BadCapture(Board* board, Move* move)
	{
		// Note: This function needs attack board to be calculated.

		int playerColor = Board_Color(board, move->From);
		int attacker = move->PlayerPiece;
		int victim = Board_Piece(board, move->To);
		
		if (playerColor == COLOR_WHITE)
		{
			uint64_t blackAttacks = (board->_AttacksBlack > 0) ? board->_AttacksBlack : Board_AttackMap(board, COLOR_BLACK);
			int isDefended = Bitboard_GetRef(&blackAttacks, move->To);
			return (isDefended && (attacker > victim));
		}
		else
		{
			uint64_t whiteAttacks = (board->_AttacksWhite > 0) ? board->_AttacksWhite : Board_AttackMap(board, COLOR_WHITE);
			int isDefended = Bitboard_GetRef(&whiteAttacks, move->To);
			return (isDefended && (attacker > victim));
		}
	}
}

#endif

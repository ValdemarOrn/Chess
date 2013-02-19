#ifndef BITBOARD
#define BITBOARD

#include "inttypes.h"
#include "Bitboard.h"
#include <intrin.h>
#pragma intrinsic(_BitScanForward)

extern "C"
{
	// ------------- Bit Twiddling -------------

	// Unset bit at position #index in the input val bitboard. Returns a new bitboard
	__declspec(dllexport) uint64_t Bitboard_Unset(uint64_t val, int index);

	// Set bit at position #index in the input val bitboard. Returns a new bitboard
	__declspec(dllexport) uint64_t Bitboard_Set(uint64_t val, int index);

	// Unset bit at position #index in the input val bitboard. Modifies the given bitboard
	__declspec(dllexport) void Bitboard_UnsetRef(uint64_t* val, int index);

	// Set bit at position #index in the input val bitboard. Modifies the given bitboard
	__declspec(dllexport) void Bitboard_SetRef(uint64_t* val, int index);

	// returns 1 if bit #index is set, 0 if unset
	__declspec(dllexport) int Bitboard_Get(uint64_t val, int index);

	// creates a bitboard where the tiles in the input array are set. count tells how many items the tiles array contains
	__declspec(dllexport) uint64_t Bitboard_Make(int* tiles, int count);

	// ------------- Bitscan functions -------------

	// Returns the least significant bit (LSB) that is set
	// Returns -1 if no bits are set
	__declspec(dllexport) int Bitboard_ForwardBit(uint64_t val);

	// Returns the most significant bit (LSB) that is set
	// Returns -1 if no bits are set
	__declspec(dllexport) int Bitboard_ReverseBit(uint64_t val);

	// Counts the number of set bits in a bitboard
	__declspec(dllexport) int Bitboard_PopCount(uint64_t val);

	// Returns a list containing the index of all set bits in a bitboard.
	// outputList_s64 must be an allocated list of 64 bytes.
	// Returns the number of items in the output list 
	__declspec(dllexport) int Bitboard_BitList(uint64_t val, uint8_t* outputList_s64);
	

	// ------------------------ Basic Bitboard twiddling --------------------------

	inline uint64_t Bitboard_Unset(uint64_t val, int index)
	{
		uint64_t inv = ~(uint64_t)((uint64_t)1 << index);
		return val & inv;
	}

	inline uint64_t Bitboard_Set(uint64_t val, int index)
	{
		uint64_t mask = (uint64_t)((uint64_t)1 << index);
		return val | mask;
	}

	inline void Bitboard_UnsetRef(uint64_t* val, int index)
	{
		uint64_t inv = ~(uint64_t)((uint64_t)1 << index);
		*val = *val & inv;
	}

	inline void Bitboard_SetRef(uint64_t* val, int index)
	{
		uint64_t mask = ((uint64_t)1 << index);
		*val = *val | mask;
	}


	// ----------------------------------------------------

	// returns 1 if bit #index is set, 0 if unset
	inline int Bitboard_Get(uint64_t val, int index)
	{
		uint64_t mask = ((uint64_t)1 << index);
		return ((val & mask) > 0) ? 1 : 0;
	}

	// creates a bitboard where the tiles in the input array are set. count tells how many items the tiles array contains
	inline uint64_t Bitboard_Make(int* tiles, int count)
	{
		uint64_t bitboard = 0;
		for(int i=0; i<count; i++)
			bitboard = Bitboard_Set(bitboard, tiles[i]);
	
		return bitboard;
	}

	// ------------------------- Bitscan functions --------------------------

	// Returns the least significant bit (LSB) that is set
	inline int Bitboard_ForwardBit(uint64_t val)
	{
		unsigned long index;
		unsigned char isNonzero = _BitScanForward64(&index, val);
	
		if(isNonzero)
			return index;
		else
			return -1;
	}

	// Returns the most significant bit (LSB) that is set
	inline int Bitboard_ReverseBit(uint64_t val)
	{
		unsigned long index;
		unsigned char isNonzero = _BitScanReverse64(&index, val);
	
		if(isNonzero)
			return index;
		else
			return -1;
	}

	// Counts the number of set bits in a bitboard
	inline int Bitboard_PopCount(uint64_t val)
	{
		unsigned long count = __popcnt64(val);
		return (int)count;
	}

	// Returns a list containing the index of all set bits in a bitboard
	inline int Bitboard_BitList(uint64_t val, uint8_t* outputList_s64)
	{
		int i = 0;
		while(true)
		{
			int fwd = Bitboard_ForwardBit(val);
			if(fwd == -1)
				break;

			val = Bitboard_Unset(val, fwd);
			uint8_t byte = (uint8_t)fwd;
			outputList_s64[i] = byte;
			i++;
		}

		return i;
	}

}

#endif
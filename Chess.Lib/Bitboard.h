#ifndef BITBOARD
#define BITBOARD

#include "Default.h"
#include "Intrinsics.h"
//#pragma intrinsic(_BitScanForward)

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

	__declspec(dllexport) int Bitboard_GetRef(uint64_t* val, int index);

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
	
	const uint64_t Bitboard_Files[8] = {
		0x101010101010101,
		0x202020202020202,
		0x404040404040404,
		0x808080808080808,
		0x1010101010101010,
		0x2020202020202020,
		0x4040404040404040,
		0x8080808080808080
	};

	const uint64_t Bitboard_Ranks[8] = {
		0xFF,
		0xFF00,
		0xFF0000,
		0xFF000000,
		0xFF00000000,
		0xFF0000000000,
		0xFF000000000000,
		0xFF00000000000000
	};

	const uint64_t Bitboard_DarkSquares = 0xAA55AA55AA55AA55;
	const uint64_t Bitboard_LightSquares = 0x55AA55AA55AA55AA;

	// ------------------------ Basic Bitboard twiddling --------------------------

	__inline_always uint64_t Bitboard_Unset(uint64_t val, int index)
	{
		uint64_t output = val;
		Intrin_BitTestAndReset64(&output, index);
		return output;
	}

	__inline_always uint64_t Bitboard_Set(uint64_t val, int index)
	{
		uint64_t output = val;
		Intrin_BitTestAndSet64(&output, index);
		return output;
	}

	__inline_always void Bitboard_UnsetRef(uint64_t* val, int index)
	{
		Intrin_BitTestAndReset64(val, index);
	}

	__inline_always void Bitboard_SetRef(uint64_t* val, int index)
	{
		Intrin_BitTestAndSet64(val, index);
	}


	// ----------------------------------------------------

	// returns 1 if bit #index is set, 0 if unset
	__inline_always int Bitboard_Get(uint64_t val, int index)
	{
		uint8_t value = Intrin_BitTest64(&val, index);
		return value;
	}

	__inline_always int Bitboard_GetRef(uint64_t* val, int index)
	{
		uint8_t value = Intrin_BitTest64(val, index);
		return value;
	}

	// creates a bitboard where the tiles in the input array are set. count tells how many items the tiles array contains
	__inline_always uint64_t Bitboard_Make(int* tiles, int count)
	{
		uint64_t bitboard = 0;
		for(int i=0; i<count; i++)
			bitboard = Bitboard_Set(bitboard, tiles[i]);
	
		return bitboard;
	}

	// ------------------------- Bitscan functions --------------------------

	// Returns the least significant bit (LSB) that is set
	__inline_always int Bitboard_ForwardBit(uint64_t val)
	{
		uint32_t index;
		unsigned char isNonzero = Intrin_BitScanForward64(&index, val);
	
		return (isNonzero) ? (int)index : -1;
	}

	// Returns the most significant bit (LSB) that is set
	__inline_always int Bitboard_ReverseBit(uint64_t val)
	{
		uint32_t index;
		unsigned char isNonzero = Intrin_BitScanReverse64(&index, val);
	
		return (isNonzero) ? (int)index : -1;
	}

	// Counts the number of set bits in a bitboard
	__inline_always int Bitboard_PopCount(uint64_t val)
	{
		unsigned long count = 0;
		count = (unsigned long)Intrin_PopCnt64(val);
		return (int)count;
	}

	// Returns a list containing the index of all set bits in a bitboard
	__inline_always int Bitboard_BitList(uint64_t val, uint8_t* outputList_s64)
	{
		int i = 0;
		while(true)
		{
			int fwd = Bitboard_ForwardBit(val);
			if(fwd == -1)
				break;

			Bitboard_UnsetRef(&val, fwd);
			uint8_t byte = (uint8_t)fwd;
			outputList_s64[i] = byte;
			i++;
		}

		return i;
	}

}

#endif

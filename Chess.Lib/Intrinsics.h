
// definitions for Bitwise operations
// Uses X64 compiler intrinsics if they are available, otherwise it mimics them in code

#include "Default.h"

#ifdef _M_X64

#include <intrin.h>
// define macros that point to the intrinsic functinos in MSVC
#define Intrin_BitTestAndReset64(a, bit) _bittestandreset64((__int64*)a, bit)
#define Intrin_BitTestAndSet64(a, bit) _bittestandset64((__int64*)a, bit)
#define Intrin_BitTest64(a, bit) _bittest64((__int64*)a, bit)
#define Intrin_BitScanForward64(index, input) _BitScanForward64(index, input)
#define Intrin_BitScanReverse64(index, input) _BitScanReverse64(index, input)
#define Intrin_PopCnt64(value) __popcnt64(value)

#else

// fall back to software emulation

__inline_always unsigned char Intrin_BitTestAndReset64(uint64_t* a, uint64_t bit)
{
	uint64_t base = 1; // must for 32 bit platforms
	uint64_t mask = (base << bit);

	uint64_t val = *a & mask;
	*a = *a & (~mask);
	return (val > 0);
}

__inline_always unsigned char Intrin_BitTestAndSet64(uint64_t* a, uint64_t bit)
{
	uint64_t base = 1; // must for 32 bit platforms
	uint64_t mask = (base << bit);

	uint64_t val = *a & mask;
	*a = *a | mask;
	return (val > 0);
}

__inline_always unsigned char Intrin_BitTest64(uint64_t* a, uint64_t bit)
{
	uint64_t base = 1; // must for 32 bit platforms
	uint64_t mask = (base << bit);

	uint64_t val = *a & mask;
	return (val > 0);
}

__inline_always unsigned char Intrin_BitScanForward64(uint32_t* index, uint64_t input)
{
	uint64_t mask = 1;
	int i = 0;
	while(i < 64)
	{
		if((mask & input) > 0)
		{
			*index = i;
			return 1;
		}

		i++;
		mask = mask << 1;
	}

	return 0;
}

__inline_always unsigned char Intrin_BitScanReverse64(uint32_t* index, uint64_t input)
{
	uint64_t mask = 9223372036854775808ULL;
	int i = 63;
	while(i >= 0)
	{
		if((mask & input) > 0)
		{
			*index = i;
			return 1;
		}

		i--;
		mask = mask >> 1;
	}

	return 0;
}

__inline_always uint64_t Intrin_PopCnt64(uint64_t value)
{
	uint64_t mask = 1;
	int cnt = 0;
	int i = 0;
	while(i < 64)
	{
		if((mask & value) > 0)
			cnt++;

		i++;
		mask = mask << 1;
	}

	return cnt;
}


#endif

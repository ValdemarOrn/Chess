
// definitions for Bitwise operations
// Uses X64 compiler intrinsics if they are available, otherwise it mimics them in code

#include "Default.h"

#ifdef _M_X64

// MSVC has compiler intrinsics for these operations. Map macros that use them
#define INTRIN_OK

#include <intrin.h>
#define Intrin_BitTestAndReset64(a, bit) _bittestandreset64((__int64*)a, bit)
#define Intrin_BitTestAndSet64(a, bit) _bittestandset64((__int64*)a, bit)
#define Intrin_BitTest64(a, bit) _bittest64((__int64*)a, bit)
#define Intrin_BitScanForward64(index, input) _BitScanForward64((unsigned long*)index, input)
#define Intrin_BitScanReverse64(index, input) _BitScanReverse64((unsigned long*)index, input)
#define Intrin_PopCnt64(value) __popcnt64(value)

#endif

#if defined(__GNUC__) && defined(__x86_64__)

// GCC has some intrinsics, but needs assembly instructions in some cases
#define INTRIN_OK

__inline_always unsigned char Intrin_BitTestAndReset64(uint64_t* a, uint64_t bit)
{
	unsigned char flag = 0; 
	asm("btr %2, %1;\n\t"
		"setb %0" 
		: "=q" (flag) 
		: "m" (*a), "r" (bit)
		);
	return flag;
}

__inline_always unsigned char Intrin_BitTestAndSet64(uint64_t* a, uint64_t bit)
{
	unsigned char flag = 0; 
	asm("bts %2, %1;\n\t"
		"setb %0" 
		: "=q" (flag) 
		: "m" (*a), "r" (bit)
		);
	return flag;
}

__inline_always unsigned char Intrin_BitTest64(uint64_t* a, uint64_t bit)
{
	unsigned char flag = 0;
	asm("bt %2, %1;\n\t"
		"setb %0" 
		: "=q" (flag) 
		: "m" (*a), "r" (bit)
		); 
	return flag; 
}

__inline_always unsigned char Intrin_BitScanForward64(uint32_t* index, uint64_t input)
{
	int pos = __builtin_ffsll(input);
	_Bool valid = (input > 0);
	*index = valid ? (pos - 1) : 0;
	return valid;
}

__inline_always unsigned char Intrin_BitScanReverse64(uint32_t* index, uint64_t input)
{
	int pos = __builtin_clzll(input);
	_Bool valid = (input > 0);
	*index = valid ? (63 - pos) : 0;
	return valid;
}

__inline_always uint64_t Intrin_PopCnt64(uint64_t value)
{
	return __builtin_popcountll(value);
}


#endif

// fall back to software emulation

#ifndef INTRIN_OK

#ifdef __GNUC__
	#warning "Compiler intrinsics not used. Falling back to SLOW software emulated bit operations."
#else
	#pragma WARNING("Compiler intrinsics not used. Falling back to SLOW software emulated bit operations.")
#endif

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

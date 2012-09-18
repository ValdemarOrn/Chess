


#include <iostream>
#include <intrin.h>
using namespace std;

extern "C"
{

#pragma intrinsic(_BitScanForward)

__declspec(dllexport) unsigned int ForwardBit(unsigned long val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanForward(&index, val);
	return index;
}

__declspec(dllexport) unsigned int ReverseBit(unsigned long val)
{
	unsigned long index;
	unsigned char isNonzero = _BitScanReverse(&index, val);
	return index;
}

__declspec(dllexport) unsigned int PopCount(unsigned long val)
{
	unsigned long count = __popcnt64(val);
	return count;
}

__declspec(dllexport) unsigned int TryMany()
{
	unsigned long mask = 0x1000;
	unsigned int output = 0;

	for(int i=0; i<1000000; i++)
	{
		mask = i;
		unsigned int index = ForwardBit(mask);
		output += index;
	}

	return output;
}

}

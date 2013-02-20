#ifndef DEFAULT

#include "inttypes.h"



#ifdef _WIN32

#define __inline_always __forceinline

#endif



#ifdef __GNUC__

#define __inline_always __attribute__((always_inline))

#endif



#endif




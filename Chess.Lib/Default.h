#ifndef DEFAULT

#include "inttypes.h"
//#define NDEBUG 

#include <assert.h>

typedef int _Bool;
#define TRUE 1
#define FALSE 0


// --------- compiler target ---------

#ifdef _WIN32

#define __inline_always __forceinline

#endif


#ifdef __GNUC__

#define __inline_always __attribute__((always_inline))

#endif



#endif




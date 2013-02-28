#ifndef DEFAULT

#include "inttypes.h"

typedef int _Bool;
#define TRUE 1
#define FALSE 0

// ---------- Compile Mode ---------

//#define DEBUG

// if NDEBUG (no-debug) is defined then assertions are disabled
//#define NDEBUG 
#include <assert.h>


// --------- compiler target ---------

#ifdef _WIN32

#define __inline_always __forceinline

#endif


#ifdef __GNUC__

#define __inline_always __attribute__((always_inline))

#endif



#endif




#ifndef DEFAULT

#include "inttypes.h"

typedef int _Bool;
#define TRUE 1
#define FALSE 0

// ---------- Compile Mode ---------

#define DEBUG
//#define _WIN32 // already defined within Visual Studio
//#define __GNUC__



#ifndef DEBUG
#define NDEBUG // if NDEBUG (no-debug) is defined then assertions are disabled
#endif

#include <assert.h>


// --------- compiler target ---------

#ifdef _WIN32

#define __inline_always __forceinline

#endif


#ifdef __GNUC__

#define __inline_always __attribute__((always_inline))

#endif



#endif




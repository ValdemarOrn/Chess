#ifndef DEFAULT

// ----------- Typedefs -----------

#include "inttypes.h"

typedef int _Bool;
#define TRUE 1
#define FALSE 0
#define UNKNOWN 2


// ---------- Compile Mode: Change these settings to affect how the engine runs ----------


// enables assertions and other debugging code
#define DEBUG 

// enables statistics in the search module (node count, cutoff metrics, etc)
#define STATS_SEARCH

// Compile for a Windows target already defined within Visual Studio
//#define _WIN32 

// Compile for a Linux target (not currently implemented, but will be soon)
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




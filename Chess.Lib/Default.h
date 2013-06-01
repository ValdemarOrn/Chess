#ifndef DEFAULT

// ----------- Typedefs -----------

#if !defined(_STDINT_H) && !defined(_GCC_STDINT_H)
	#include "inttypes.h"
#endif

typedef int _Bool;
#define TRUE 1
#define FALSE 0
#define UNKNOWN 2


// ---------- Compile Mode: Change these settings to affect how the engine runs ----------

// enables assertions and other debugging code
#define DEBUG 

// enables statistics in the search module (node count, cutoff metrics, etc)
#define STATS_SEARCH

// Compile for a Windows target, defined by Visual Studio
//#define _WIN32 

// Compile for a Linux target, defined by g++
//#define __GNUC__




#ifndef DEBUG
	#define NDEBUG // if NDEBUG (no-debug) is defined then assertions are disabled
#endif

#include <assert.h>

// --------- compiler target ---------

#ifdef _WIN32

	#ifdef DEBUG
		#define __inline_always inline
	#else
		#define __inline_always __forceinline
	#endif

#endif


#ifdef __GNUC__

	#ifdef DEBUG
		#define __inline_always inline
	#else
		#define __inline_always inline __attribute__((always_inline))
	#endif

#endif

#endif




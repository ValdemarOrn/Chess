#ifndef DEFAULT

// ----------- Typedefs -----------
typedef int _Bool;
#define TRUE 1
#define FALSE 0
#define UNKNOWN 2

// ---------- Compile Mode: Change these settings to affect how the engine runs ----------

// enables statistics in the search module (node count, cutoff metrics, etc)
#define STATS_SEARCH
// enables statistic in the eval module
#define STATS_EVAL

// enables assertions and other debugging code
//#define DEBUG 

// Compile for a Windows target, defined by Visual Studio
//#define _WIN32 

// Compile for a Linux target, defined by g++
//#define __GNUC__


#ifndef DEBUG
	#define NDEBUG // if NDEBUG (no-debug) is defined then assertions are disabled
#endif

#include <assert.h>



// --------- compiler target ---------

#ifdef _MSC_VER

	// ---------- Define #pragma WARNING -----------
	#define STRINGIZE_HELPER(x) #x
	#define STRINGIZE(x) STRINGIZE_HELPER(x)
	#define WARNING(desc) message(__FILE__ "(" STRINGIZE(__LINE__) ") : Warning: " #desc)

	#include "inttypes.h"

	#define __dllexport __declspec(dllexport)

	#ifdef DEBUG
		#define __inline_always inline
	#else
		#define __inline_always __forceinline
	#endif

#endif


#ifdef __GNUC__

	#include<stdint.h>

	#define __dllexport 
	#define __stdcall __attribute__((stdcall))

	#ifdef DEBUG
		#define __inline_always inline
	#else
		#define __inline_always inline __attribute__((always_inline))
	#endif

#endif

#endif




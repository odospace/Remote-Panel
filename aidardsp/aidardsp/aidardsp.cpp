// aidardsp.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "aidardsp.h"


// This is an example of an exported variable
AIDARDSP_API int naidardsp=0;

// This is an example of an exported function.
AIDARDSP_API int fnaidardsp(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see aidardsp.h for the class definition
Caidardsp::Caidardsp()
{
	return;
}

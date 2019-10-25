// Welch, Wright, & Morrow, 
// Real-time Digital Signal Processing, 2017

///////////////////////////////////////////////////////////////////////
// Filename: main.c
//
// Synopsis: Main program file for demonstration code
//
///////////////////////////////////////////////////////////////////////

#include "DSP_Config.h"

int main()
{
    // call StartUp for application specific code
    // defined in each application directory
    StartUp();

	// initialize DSP board
  	DSP_Init();
	
	// main stalls here, interrupts drive operation
  	while(1) {
  	    ;
  	}
}



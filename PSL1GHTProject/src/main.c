#include <stdio.h>
#include <string.h>
#include <unistd.h>

/*
	IMPORTANT NOTE
	You must add a new system environment variable (unless already done) called PSL1GHTSDKPath containing the path to the SDK
	For instance, I have the SDK located at Z:\PSDK3v2\ so I'd create a new environment variable
	named PSL1GHTSDKPath and set its value to Z:\PSDK3v2 NOT Z:\PSDK3v2
	
	How to add a system environment variable:
	https://www.java.com/en/download/help/path.xml

	Happy deving!
	~ Dnawrkshp

	P.S. IntelliSense spits out a ton of errors, so it's best to right-click the Error List and uncheck Show IntelliSense Errors
 */

/*
	Be sure to setup an appropriate APPID and title in the makefile and sfo.xml
	
	Also, When your Active Solution Configuration is Debug, building will create the appropriate .self
	However, if it is Release, building will create the appropriate .pkg
	

	Another tip actually, the MinGW make requires that there are no spaces in the path to the project so either make this project
	in a path without spaces or migrate your entire Visual Studio folder to something like VisualStudio (then edit the appropriate Tools->Options)
*/

signed int main(signed int argc, const char* argv[])
{
	printf("Hello World!\n");

	return 0;
}

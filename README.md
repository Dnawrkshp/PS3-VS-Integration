##Visual Studio PS3 Integration
A set of Visual Studio 2013 Community Edition integration tools for PS3 development.

###Consists of 
* [PSL1GHTBuilder](https://github.com/Dnawrkshp/PS3-VS-Integration/tree/master/PSL1GHTBuilder): C# Console Application that creates a msys child process to compile
* [PSL1GHTProject](https://github.com/Dnawrkshp/PS3-VS-Integration/tree/master/PSL1GHTProject): VS 2013 C++ Project Template setup with appropriate Project Settings
* [PSL1GHTProjectVSIX](https://github.com/Dnawrkshp/PS3-VS-Integration/tree/master/PSL1GHTProjectVSIX): VS 2013 Template Installer

###How to Setup
* Download latest PSL1GHT VSIX template installer from [PSL1GHTProjectVSIX.vsix](https://github.com/Dnawrkshp/PS3-VS-Integration/blob/master/PSL1GHTProjectVSIX/bin/Debug/PSL1GHTProjectVSIX.vsix)
* Run the VSIX to install template
* Create a new system environment variable called PSL1GHTSDKPath
* Set the path to your PSL1GHT root folder (for me it's Z:\PSDK3v2)
* Open Visual Studio and create a new Visual C++ Project with the newly installed template. Make sure the project path has NO spaces in it
* Edit the makefile and sfo.xml with a valid app id and title
* Turn off IntelliSense errors by opening the error list (View->Error List), right clicking the error box, and unchecking Show IntelliSense Errors.
* Have fun deving!

###To-do
- [ ] Implement support for CELL SDK selfs
- [ ] Implement support for CELL SDK prxs
- [X] Implement error and warning logs
- [X] Implement support for PSL1GHT SDK selfs
- [X] Implement IntelliSense integration


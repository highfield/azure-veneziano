/* 
	Editor: http://www.visualmicro.com
	        visual micro and the arduino ide ignore this code during compilation. this code is automatically maintained by visualmicro, manual changes to this file will be overwritten
	        the contents of the Visual Micro sketch sub folder can be deleted prior to publishing a project
	        all non-arduino files created by visual micro and all visual studio project or solution files can be freely deleted and are not required to compile a sketch (do not delete your own code!).
	        note: debugger breakpoints are stored in '.sln' or '.asln' files, knowledge of last uploaded breakpoints is stored in the upload.vmps.xml file. Both files are required to continue a previous debug session without needing to compile and upload again
	
	Hardware: LinkIt ONE, Platform=mtk, Package=arduino
*/

#ifndef _VSARDUINO_H_
#define _VSARDUINO_H_
#define __COMPILER_GCC__
#define __LINKIT_ONE__
#define __LINKIT_ONE_RELEASE__
#define USB_VID 0x0E8D
#define USB_PID 0x0023
#define USBCON
#define USB_MANUFACTURER "\"Unknown\""
#define USB_PRODUCT "\"LinkIt ONE\""
#define ARDUINO 156
#define ARDUINO_MAIN
#define printf iprintf
#define __MTK__
#define __mtk__
#define F_CPU 84000000L
#define __cplusplus


//
//

#include "C:\Portables\LinkIt-ONE-IDE-master\hardware\arduino\mtk\cores\arduino\arduino.h"
#include "C:\Portables\LinkIt-ONE-IDE-master\hardware\arduino\mtk\variants\linkit_one\pins_arduino.h" 
#include "C:\Portables\LinkIt-ONE-IDE-master\hardware\arduino\mtk\variants\linkit_one\variant.h" 
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\AzureVenezianoLinkIt.ino"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\AnalogInputWrapper.cpp"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\AnalogInputWrapper.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\IInput.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\IInputBoolean.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\IInputDouble.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\IInputInt32.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\InputPortWrapper.cpp"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\InputPortWrapper.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\MobileServiceClient.cpp"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\MobileServiceClient.h"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\RampGenerator.cpp"
#include "C:\Users\Mario\Documents\Arduino\AzureVenezianoLinkIt\RampGenerator.h"
#endif

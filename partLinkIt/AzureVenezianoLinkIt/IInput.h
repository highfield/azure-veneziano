/*
* Copyright by Mario Vernari, Cet Electronics
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

#ifndef __IINPUT_H__
#define __IINPUT_H__

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include <ArduinoJson.h>


class IInput
{
//functions
public:
    virtual ~IInput(){}
        
    /// <summary>
    /// Friendly name of the port
    /// </summary>
    virtual const char* getName() = 0;

    /// <summary>
    /// Indicate whether the port value has changed
    /// </summary>
    virtual bool getHasChanged() = 0;

    /// <summary>
    /// Perform the port sampling
    /// </summary>
    /// <returns></returns>
    virtual bool sample() = 0;

    /// <summary>
    /// Append to the container an object made up
    /// with the input port status
    /// </summary>
    /// <param name="container"></param>
    virtual void serialize(JsonArray* container) = 0;

}; //IInput

#endif //__IINPUT_H__

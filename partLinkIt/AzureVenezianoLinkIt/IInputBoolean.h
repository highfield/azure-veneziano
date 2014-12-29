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

#ifndef __IINPUTBOOLEAN_H__
#define __IINPUTBOOLEAN_H__

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include "IInput.h"


class IInputBoolean : public IInput
{
public:
    virtual ~IInputBoolean(){}
        
    /// <summary>
    /// The sampled input port value
    /// </summary>
    virtual bool getValue() = 0;
    
}; //IInputBoolean

#endif //__IINPUTBOOLEAN_H__

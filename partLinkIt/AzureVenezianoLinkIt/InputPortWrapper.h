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

#ifndef __INPUTPORTWRAPPER_H__
#define __INPUTPORTWRAPPER_H__

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include <ArduinoJson.h>
#include "IInputBoolean.h"


class InputPortWrapper : public IInputBoolean
{
public:
    InputPortWrapper(
        const char* name, 
        int portId
        );
        
    ~InputPortWrapper();
    
    const char* getName();
    bool getValue();
    bool getHasChanged();
    
    bool sample();
    void serialize(JsonArray* container);
    
protected:
private:
    InputPortWrapper( const InputPortWrapper &c );
    InputPortWrapper& operator=( const InputPortWrapper &c );

    const char* _name;
    const int _portId;
    
    bool _value;
    bool _hasChanged;
    int _oldValue;
    
}; //InputPortWrapper

#endif //__INPUTPORTWRAPPER_H__

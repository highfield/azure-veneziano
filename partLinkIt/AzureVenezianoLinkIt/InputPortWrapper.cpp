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

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include <ArduinoJson.h>
#include "InputPortWrapper.h"


// default constructor
InputPortWrapper::InputPortWrapper(
    const char* name, 
    int portId
    ) : 
    _name(name), 
    _portId(portId)
{
    pinMode(_portId, INPUT_PULLUP);
    
    _value = false;
    _hasChanged = false;
    _oldValue = -1;
} //InputPortWrapper

// default destructor
InputPortWrapper::~InputPortWrapper()
{
} //~InputPortWrapper


const char* InputPortWrapper::getName(){
    return _name;
}


bool InputPortWrapper::getValue(){
    return _value;
}


bool InputPortWrapper::getHasChanged(){
    return _hasChanged;
}


bool InputPortWrapper::sample(){
    _value = digitalRead(_portId) == HIGH;

    //detect the variation
    int rawValue = _value ? 1 : 0;
    _hasChanged = rawValue != _oldValue;
    _oldValue = rawValue;
    return _hasChanged;
}


void InputPortWrapper::serialize(JsonArray* container){
    JsonObject& jsens = container->createNestedObject();
    jsens["name"] = _name;
    jsens["value"] = _value;
}
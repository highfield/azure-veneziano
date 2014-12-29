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
#include "AnalogInputWrapper.h"


// default constructor
AnalogInputWrapper::AnalogInputWrapper(
    const char* name,
    int portId,
    double scale,
    double offset,
    double normalizedTolerance
    ) :
    _name(name), 
    _portId(portId),
    _adcScale(scale / 1024.0),
    _offset(offset),

    //precalculate the absolute variation window
    //around the reference (old) sampled value
    _absoluteToleranceDelta(scale * normalizedTolerance)
{
    _value = false;
    _hasChanged = false;
    _oldValue = -1e300; //undefined
} //AnalogInputWrapper

// default destructor
AnalogInputWrapper::~AnalogInputWrapper()
{
} //~AnalogInputWrapper


const char* AnalogInputWrapper::getName(){
    return _name;
}


double AnalogInputWrapper::getValue(){
    return _value;
}


bool AnalogInputWrapper::getHasChanged(){
    return _hasChanged;
}


bool AnalogInputWrapper::sample(){
    _value = analogRead(_portId) * _adcScale + _offset;

    //detect the variation
    _hasChanged = 
        _value < (_oldValue - _absoluteToleranceDelta) ||
        _value > (_oldValue + _absoluteToleranceDelta);

    if (_hasChanged){
        //update the reference (old) value
        _oldValue = _value;
    }
    
    return _hasChanged;
}


void AnalogInputWrapper::serialize(JsonArray* container){
    JsonObject& jsens = container->createNestedObject();
    jsens["name"] = _name;
    jsens["value"] = _value;
}
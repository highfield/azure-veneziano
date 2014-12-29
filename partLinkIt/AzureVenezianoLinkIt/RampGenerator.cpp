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
#include "RampGenerator.h"

// default constructor
RampGenerator::RampGenerator(
    const char* name,
    long period,
    long scale,
    long offset
    ) :
    _name(name),
    _period(period),
    _scale(scale),
    _offset(offset),

    //the wave being subdivided in 40 slices
    _stepPeriod(period / 40)
{
    _stepTimer = -1;    //undefined

    //vertical direction: 1=rise; -1=fall
    _rawDirection = 1;
} //RampGenerator

// default destructor
RampGenerator::~RampGenerator()
{
} //~RampGenerator


const char* RampGenerator::getName(){
    return _name;
}


long RampGenerator::getValue(){
    return _value;
}


bool RampGenerator::getHasChanged(){
    return _hasChanged;
}


bool RampGenerator::sample(){
    bool hasChanged = false;

    if (++_stepTimer <= 0)
    {
        //very first sampling
        _value = _offset;
        hasChanged = true;
    }
    else if (_stepTimer >= _stepPeriod)
    {
        if (_rawValue >= 10)
        {
            //hit the upper edge, then begin to fall
            _rawValue = 10;
            _rawDirection = -1;
        }
        else if (_rawValue <= -10)
        {
            //hit the lower edge, then begin to rise
            _rawValue = -10;
            _rawDirection = 1;
        }

        _rawValue += _rawDirection;
        _value = _offset + (long)(_scale * (_rawValue / 10.0));
        hasChanged = true;
        _stepTimer = 0;
    }
            
    return (_hasChanged = hasChanged);
}


void RampGenerator::serialize(JsonArray* container){
    JsonObject& jsens = container->createNestedObject();
    jsens["name"] = _name;
    jsens["value"] = _value;
}
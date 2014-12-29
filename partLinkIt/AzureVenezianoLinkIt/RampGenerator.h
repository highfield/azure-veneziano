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

#ifndef __RAMPGENERATOR_H__
#define __RAMPGENERATOR_H__

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include <ArduinoJson.h>
#include "IInputInt32.h"


class RampGenerator : public IInputInt32
{
public:
    RampGenerator(
        const char* name,
        long period,
        long scale,
        long offset
        );
    
    ~RampGenerator();
    
    const char* getName();
    long getValue();
    bool getHasChanged();
    
    bool sample();
    void serialize(JsonArray* container);
    
protected:
private:
    RampGenerator( const RampGenerator &c );
    RampGenerator& operator=( const RampGenerator &c );

    const char* _name;
    const long _period;
    const long _offset;
    const long _scale;

    const long _stepPeriod;
    
    long _stepTimer;
    long _rawValue;
    long _rawDirection;
    
    long _value;
    bool _hasChanged;

}; //RampGenerator

#endif //__RAMPGENERATOR_H__

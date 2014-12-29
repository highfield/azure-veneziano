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

#ifndef __ANALOGINPUTWRAPPER_H__
#define __ANALOGINPUTWRAPPER_H__

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include <ArduinoJson.h>
#include "IInputDouble.h"


class AnalogInputWrapper : public IInputDouble
{
public:
    AnalogInputWrapper(
        const char* name,
        int portId,
        double scale,
        double offset,
        double normalizedTolerance = 0.05
        );
    
    ~AnalogInputWrapper();
    
    const char* getName();
    double getValue();
    bool getHasChanged();
    
    bool sample();
    void serialize(JsonArray* container);

protected:
private:
    AnalogInputWrapper( const AnalogInputWrapper &c );
    AnalogInputWrapper& operator=( const AnalogInputWrapper &c );

    const char* _name;
    const int _portId;
    const double _offset;
    const double _adcScale;
    const double _absoluteToleranceDelta;

    double _value;
    bool _hasChanged;
    double _oldValue;

}; //AnalogInputWrapper

#endif //__ANALOGINPUTWRAPPER_H__

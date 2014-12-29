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

#define SerialUSB Serial

#include <LWiFi.h>
#include <LWiFiClient.h>
#include <LWiFiServer.h>
#include <LWiFiUdp.h>

#include <LGPS.h>

#include <b64.h>
#include <HttpClient.h>

#include <ArduinoJson.h>

#include "IInput.h"
#include "AnalogInputWrapper.h"
#include "InputPortWrapper.h"
#include "RampGenerator.h"
#include "MobileServiceClient.h"


/**
* Hardware input ports definition
**/

InputPortWrapper* _switch0;
InputPortWrapper* _switch1;

AnalogInputWrapper* _analog0;
AnalogInputWrapper* _analog1;

IInput* _inputPorts[16];
int _portCount;

#define LED 13

#define WIFINAME "(your wi-fi name)"
#define WIFIPWD "(your wi-fi password)"

int _wifiStatus;
MobileServiceClient* _ms;

gpsSentenceInfoStruct _gpsinfo;


void setup()
{
	/**
	* Hardware input ports definition
	**/
	_switch0 = new InputPortWrapper(
		"Switch0",
		0
		);

	_switch1 = new InputPortWrapper(
		"Switch1",
		1
		);


	_analog0 = new AnalogInputWrapper(
		"Analog0",
		0,
		100.0,
		0.0
		);

	_analog1 = new AnalogInputWrapper(
		"Analog1",
		1,
		100.0,
		0.0
		);

	//collect all the input ports as an array
	_inputPorts[0] = _switch0;
	_inputPorts[1] = _switch1;
	_inputPorts[2] = new RampGenerator("Ramp20min", 1200, 100, 0);
	_inputPorts[3] = new RampGenerator("Ramp30min", 1800, 150, 50);
	_inputPorts[4] = _analog0;
	_inputPorts[5] = _analog1;

	_portCount = 6;

	//just the led port used as a visual heartbeat
	pinMode(LED, OUTPUT);

	//turn-on the GPS sensor
	LGPS.powerOn();

	//setup the WI-FI connection
	LWiFi.begin();

	_wifiStatus = LWiFi.connectWPA(WIFINAME, WIFIPWD);

	//istantiate a new Azure-mobile service client
	_ms = new MobileServiceClient(
        "(your service name).azure-mobile.net",
        "(your application-id)",
        "(your master key)"
		);
}


void loop()
{
	if (_wifiStatus >= 0)
	{
		bool hasChanged = false;

		//perform the logic sampling for every port of the array
		for (int i = 0; i < _portCount; i++)
		{
			if (_inputPorts[i]->sample())
			{
				hasChanged = true;
			}
		}

		if (hasChanged)
		{
			//something has changed, so wrap up the data transaction
			StaticJsonBuffer<4096> jsonBuffer;

			//read the GPS info
			LGPS.getData(&_gpsinfo);

			JsonObject& jobj = jsonBuffer.createObject();
			jobj["devId"] = "01234567";
			jobj["ver"] = 987654321;
			jobj["pos"] = (char*)_gpsinfo.GPGGA;

			JsonArray& jdata = jobj.createNestedArray("data");

			//append only the port data which have been changed
			for (int i = 0; i < _portCount; i++)
			{
				IInput* port;
				if ((port = _inputPorts[i])->getHasChanged())
				{
					port->serialize(&jdata);
				}
			}

			//execute the query against the server
			_ms->apiOperation(
				"myapi",
				REST_Create,
				&jobj
				);
		}

		//invert the led status
		digitalWrite(
			LED,
			digitalRead(LED) == 0
			);

		//take a rest...
		delay(1000);
	}
}

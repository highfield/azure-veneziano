using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Cet.MicroJSON;

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
namespace AzureVeneziano.Netduino
{
    public class Program
    {
        /**
         * Hardware input ports definition
         **/

        private static InputPortWrapper _switch0 = new InputPortWrapper(
            "Switch0",
            Pins.GPIO_PIN_D0
            );

        private static InputPortWrapper _switch1 = new InputPortWrapper(
            "Switch1",
            Pins.GPIO_PIN_D1
            );

        private static AnalogInputWrapper _analog0 = new AnalogInputWrapper(
            "Analog0",
            AnalogChannels.ANALOG_PIN_A0,
            100.0,
            0.0
            );

        private static AnalogInputWrapper _analog1 = new AnalogInputWrapper(
            "Analog1",
            AnalogChannels.ANALOG_PIN_A1,
            100.0,
            0.0
            );


        //just the led port used as a visual heartbeat
        private static OutputPort _led = new OutputPort(
            Pins.ONBOARD_LED,
            false
            );


        public static void Main()
        {
            //istantiate a new Azure-mobile service client
            var ms = new MobileServiceClient(
                "(your service name)",
                applicationId: "(your application-id)",
                masterKey: "(your master key)"
                );

            //collect all the input ports as an array
            var inputPorts = new IInput[]
            {
                _switch0,
                _switch1,
                new RampGenerator("Ramp20min", 1200, 100, 0),
                new RampGenerator("Ramp30min", 1800, 150, 50),
                _analog0,
                _analog1,
            };

            //loops forever
            while (true)
            {
                bool hasChanged = false;

                //perform the logic sampling for every port of the array
                for (int i = 0; i < inputPorts.Length; i++)
                {
                    if (inputPorts[i].Sample())
                    {
                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    //something has changed, so wrap up the data transaction
                    var jobj = new JObject();
                    jobj["devId"] = "01234567";
                    jobj["ver"] = 987654321;

                    var jdata = new JArray();
                    jobj["data"] = jdata;

                    //append only the port data which have been changed
                    for (int i = 0; i < inputPorts.Length; i++)
                    {
                        IInput port;
                        if ((port = inputPorts[i]).HasChanged)
                        {
                            port.Serialize(jdata);
                        }
                    }

                    //execute the query against the server
                    ms.ApiOperation(
                        "myapi",
                        MobileServiceClient.Create,
                        jobj
                        );
                }

                //invert the led status
                _led.Write(
                    _led.Read() == false
                    );

                //take a rest...
                Thread.Sleep(1000);
            }
        }

    }
}

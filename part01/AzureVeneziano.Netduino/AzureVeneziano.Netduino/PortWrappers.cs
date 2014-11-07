using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
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
    /// <summary>
    /// Wrapper around the standard <see cref="Microsoft.SPOT.Hardware.InputPort"/>
    /// </summary>
    public class InputPortWrapper
        : InputPort, IInputBoolean
    {
        public InputPortWrapper(
            string name,
            Cpu.Pin portId
            )
            : base(portId, true, ResistorMode.PullUp)
        {
            this.Name = name;
        }


        //NOTE: treat the data as integer for the special "undefined" state
        private int _oldValue = -1; //undefined


        public string Name { get; private set; }
        public bool Value { get; private set; }
        public bool HasChanged { get; private set; }


        public bool Sample()
        {
            this.Value = this.Read();

            //detect the variation
            int rawValue = this.Value ? 1 : 0;
            bool hasChanged = rawValue != this._oldValue;
            this._oldValue = rawValue;
            return (this.HasChanged = hasChanged);
        }


        public void Serialize(JArray container)
        {
            var jsens = new JObject();
            jsens["name"] = this.Name;
            jsens["value"] = this.Value;
            container.Add(jsens);
        }

    }


    /// <summary>
    /// Wrapper around the standard <see cref="Microsoft.SPOT.Hardware.AnalogInput"/>
    /// </summary>
    public class AnalogInputWrapper
        : AnalogInput, IInputDouble
    {
        public AnalogInputWrapper(
            string name,
            Cpu.AnalogChannel channel,
            double scale,
            double offset,
            double normalizedTolerance = 0.05
            )
            : base(channel, scale, offset, 12)
        {
            this.Name = name;

            //precalculate the absolute variation window 
            //around the reference (old) sampled value
            this._absoluteToleranceDelta = scale * normalizedTolerance;
        }


        private double _oldValue = double.NegativeInfinity; //undefined
        private double _absoluteToleranceDelta;


        public string Name { get; private set; }
        public double Value { get; private set; }
        public bool HasChanged { get; private set; }


        public bool Sample()
        {
            this.Value = this.Read();

            //detect the variation
            bool hasChanged =
                this.Value < (this._oldValue - this._absoluteToleranceDelta) ||
                this.Value > (this._oldValue + this._absoluteToleranceDelta);

            if (hasChanged)
            {
                //update the reference (old) value
                this._oldValue = this.Value;
            }

            return (this.HasChanged = hasChanged);
        }


        public void Serialize(JArray container)
        {
            var jsens = new JObject();
            jsens["name"] = this.Name;
            jsens["value"] = this.Value;
            container.Add(jsens);
        }

    }


    /// <summary>
    /// Virtual input port simulating a triangle waveform
    /// </summary>
    public class RampGenerator
        : IInputInt32
    {
        public RampGenerator(
            string name,
            int period,
            int scale,
            int offset
            )
        {
            this.Name = name;
            this.Period = period;
            this.Scale = scale;
            this.Offset = offset;

            //the wave being subdivided in 40 slices
            this._stepPeriod = this.Period / 40;

            //vertical direction: 1=rise; -1=fall
            this._rawDirection = 1;
        }


        private int _stepTimer = -1;    //undefined
        private int _stepPeriod;
        private int _rawValue;
        private int _rawDirection;

        public int Period { get; private set; }
        public int Scale { get; private set; }
        public int Offset { get; private set; }

        public string Name { get; private set; }
        public int Value { get; private set; }
        public bool HasChanged { get; private set; }


        public bool Sample()
        {
            bool hasChanged = false;

            if (++this._stepTimer <= 0)
            {
                //very first sampling
                this.Value = this.Offset;
                hasChanged = true;
            }
            else if (this._stepTimer >= this._stepPeriod)
            {
                if (this._rawValue >= 10)
                {
                    //hit the upper edge, then begin to fall
                    this._rawValue = 10;
                    this._rawDirection = -1;
                }
                else if (this._rawValue <= -10)
                {
                    //hit the lower edge, then begin to rise
                    this._rawValue = -10;
                    this._rawDirection = 1;
                }

                this._rawValue += this._rawDirection;
                this.Value = this.Offset + (int)(this.Scale * (this._rawValue / 10.0));
                hasChanged = true;
                this._stepTimer = 0;
            }
            
            return (this.HasChanged = hasChanged);
        }


        public void Serialize(JArray container)
        {
            var jsens = new JObject();
            jsens["name"] = this.Name;
            jsens["value"] = this.Value;
            container.Add(jsens);
        }

    }
}

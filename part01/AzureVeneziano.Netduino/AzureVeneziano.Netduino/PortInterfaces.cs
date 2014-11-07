using System;
using Microsoft.SPOT;
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
    /// Generic input port abstraction
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// Friendly name of the port
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Indicate whether the port value has changed
        /// </summary>
        bool HasChanged { get; }

        /// <summary>
        /// Perform the port sampling
        /// </summary>
        /// <returns></returns>
        bool Sample();

        /// <summary>
        /// Append to the container an object made up
        /// with the input port status
        /// </summary>
        /// <param name="container"></param>
        void Serialize(JArray container);
    }


    /// <summary>
    /// Boolean-valued input port specialization
    /// </summary>
    public interface IInputBoolean
        : IInput
    {
        /// <summary>
        /// The sampled input port value
        /// </summary>
        bool Value { get; }
    }


    /// <summary>
    /// Int32-valued input port specialization
    /// </summary>
    public interface IInputInt32
        : IInput
    {
        /// <summary>
        /// The sampled input port value
        /// </summary>
        Int32 Value { get; }
    }


    /// <summary>
    /// Double-valued input port specialization
    /// </summary>
    public interface IInputDouble
        : IInput
    {
        /// <summary>
        /// The sampled input port value
        /// </summary>
        double Value { get; }
    }
}

using Cet.MicroJSON;
using Microsoft.SPOT;
using System;
using System.IO;
using System.Net;
using System.Text;

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
    /// HTTP Azure-mobile service client 
    /// </summary>
    public class MobileServiceClient
    {
        public const string Read = "GET";
        public const string Create = "POST";
        public const string Update = "PATCH";

        private const string JsonMimeType = "application/json";


        /// <summary>
        /// Create a new client for HTTP Azure-mobile servicing
        /// </summary>
        /// <param name="serviceName">The name of the target service</param>
        /// <param name="applicationId">The application ID</param>
        /// <param name="masterKey">The access secret-key</param>
        public MobileServiceClient(
            string serviceName,
            string applicationId,
            string masterKey
            )
        {
            this.ServiceName = serviceName;
            this.ApplicationId = applicationId;
            this.MasterKey = masterKey;

            this._baseUri = "http://" + this.ServiceName + ".azure-mobile.net/";
        }


        private string _baseUri;

        public string ServiceName { get; private set; }
        public string ApplicationId { get; private set; }
        public string MasterKey { get; private set; }


        /// <summary>
        /// Operate a table-oriented function
        /// </summary>
        /// <param name="tableName">The target table exposed by the service</param>
        /// <param name="method">The HTTP method required</param>
        /// <param name="data">The data to upload, whereas required</param>
        /// <returns>The resulting JSON data</returns>
        public JToken TableOperation(
            string tableName,
            string method,
            JToken data = null
            )
        {
            var address = new Uri(
                this._baseUri + "tables/" + tableName
                );

            return this.OperateCore(
                address,
                method,
                data
                );
        }


        /// <summary>
        /// Operate an API-oriented function
        /// </summary>
        /// <param name="apiName">The target API exposed by the service</param>
        /// <param name="method">The HTTP method required (typically POST)</param>
        /// <param name="data">The data to upload, whereas required</param>
        /// <returns>The resulting JSON data</returns>
        public JToken ApiOperation(
            string apiName,
            string method,
            JToken data = null
            )
        {
            var address = new Uri(
                this._baseUri + "api/" + apiName
                );

            return this.OperateCore(
                address,
                method,
                data
                );
        }


        private JToken OperateCore(
            Uri uri,
            string method,
            JToken data
            )
        {
            //create a HTTP request
            using (var request = (HttpWebRequest)WebRequest.Create(uri))
            {
                //set-up headers
                var headers = new WebHeaderCollection();
                headers.Add("X-ZUMO-APPLICATION", this.ApplicationId);
                headers.Add("X-ZUMO-MASTER", this.MasterKey);

                request.Method = method;
                request.Headers = headers;
                request.Accept = JsonMimeType;

                if (data != null)
                {
                    //serialize the data to upload
                    string serialization = JsonHelpers.Serialize(data);
                    byte[] byteData = Encoding.UTF8.GetBytes(serialization);
                    request.ContentLength = byteData.Length;
                    request.ContentType = JsonMimeType;
                    request.UserAgent = "Micro Framework";
                    //Debug.Print(serialization);

                    using (Stream postStream = request.GetRequestStream())
                    {
                        postStream.Write(
                            byteData,
                            0,
                            byteData.Length
                            );
                    }
                }

                //wait for the response
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    //deserialize the received data
                    return JsonHelpers.Parse(
                        reader.ReadToEnd()
                        );
                };
            }
        }

    }
}

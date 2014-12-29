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

#ifndef __MOBILESERVICECLIENT_H__
#define __MOBILESERVICECLIENT_H__

#if defined(ARDUINO) && ARDUINO >= 100
    #include "Arduino.h"
#else
    #include "WProgram.h"
#endif

#include <ArduinoJson.h>

#define REST_Read "GET"
#define REST_Create "POST"
#define REST_Update "PATCH"

#define JsonMimeType "application/json"
#define HttpUserAgent "Link-It"

// Number of milliseconds to wait without receiving any data before we give up
#define kNetworkTimeout 30000

// Number of milliseconds to wait if no data is available before trying again
#define kNetworkDelay 1000


class MobileServiceClient
{
public:
    MobileServiceClient(
        const char* serverName,
        const char* applicationId,
        const char* masterKey,
        const int serverPort = 80
        );
    
    ~MobileServiceClient();

/*
* Operate an API-oriented function
*
* apiName: The target table exposed by the service
* method: The HTTP method required (typically POST)
* data: The data to upload, whereas required
*
* Returns: The resulting HTTP status code
*/
    int tableOperation(
        const char* tableName,
        const char* method,
        JsonObject* data = NULL
        );

/*
* Operate an API-oriented function
*
* apiName: The target API exposed by the service
* method: The HTTP method required (typically POST)
* data: The data to upload, whereas required
*
* Returns: The resulting HTTP status code
*/
    int apiOperation(
        const char* apiName,
        const char* method,
        JsonObject* data = NULL
        );
    
protected:
private:
    MobileServiceClient( const MobileServiceClient &c );
    MobileServiceClient& operator=( const MobileServiceClient &c );

    const char* _serverName;
    const int _serverPort;
    const char* _applicationId;
    const char* _masterKey;
    
    char* _receivedBody;
        
    int operateCore(
        const char* path,
        const char* method,
        JsonObject* data
        );
        
}; //MobileServiceClient

#endif //__MOBILESERVICECLIENT_H__

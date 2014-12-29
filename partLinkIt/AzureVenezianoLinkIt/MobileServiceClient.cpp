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
#include <LWiFi.h>
#include <LWiFiClient.h>
#include <HttpClient.h>
#include "MobileServiceClient.h"


// default constructor
MobileServiceClient::MobileServiceClient(
    const char* serverName,
    const char* applicationId,
    const char* masterKey,
    const int serverPort
    ) :
    _serverName(serverName),
    _applicationId(applicationId),
    _masterKey(masterKey),
    _serverPort(serverPort),
    _receivedBody(NULL)
{
} //MobileServiceClient


// default destructor
MobileServiceClient::~MobileServiceClient()
{
    if (_receivedBody != NULL){
        delete [] _receivedBody;
        _receivedBody = NULL;
    }
} //~MobileServiceClient


int MobileServiceClient::tableOperation(
    const char* tableName,
    const char* method,
    JsonObject* data
    )
{
    char path[40];
    strcpy(path, "/tables/");
    strcat(path, tableName);

    return this->operateCore(
        &path[0],
        method,
        data
        );
}


int MobileServiceClient::apiOperation(
    const char* apiName,
    const char* method,
    JsonObject* data
    )
{
    char path[40];
    strcpy(path, "/api/");
    strcat(path, apiName);

    return this->operateCore(
        &path[0],
        method,
        data
        );
}
        
        
/*
Example of raw HTTP request:

POST /tables/TodoItem HTTP/1.1
Accept: application/json
X-ZUMO-APPLICATION: (app-id)
X-ZUMO-MASTER: (master-key)
Content-Type: application/json
User-Agent: Mozilla/5.0
Host: sharemydata.azure-mobile.net
Content-Length: 31
Connection: close

{"text":"ddd","complete":false}

*/
int MobileServiceClient::operateCore(
    const char* path,
    const char* method,
    JsonObject* data
    )
{
    if (_receivedBody != NULL){
        delete [] _receivedBody;
        _receivedBody = NULL;
    }

    //create a HTTP request
    LWiFiClient client;
    HttpClient http(client);

    //it's important to call this method, because we need to add headers and data to the request
    //without it, the request is meant finished as simply as calling "startRequest"
    http.beginRequest();

    int err = HTTP_ERROR_CONNECTION_FAILED;
    if (http.startRequest(_serverName, _serverPort, path, HTTP_METHOD_POST, HttpUserAgent) == HTTP_SUCCESS){
        //serialize the data to upload
        char buffer[1024];
        int length = data->printTo(buffer, sizeof(buffer));
        
        //set-up headers
        http.sendHeader("X-ZUMO-APPLICATION", _applicationId);
        http.sendHeader("X-ZUMO-MASTER", _masterKey);
        http.sendHeader("Accept", JsonMimeType);
        
        if (data == NULL){
            http.endRequest();
        }
        else{
            http.sendHeader("Content-Type", JsonMimeType);
            http.sendHeader("Content-Length", length);
            http.endRequest();
        
            client.write((uint8_t*)&buffer, length);
        }
        
        //wait for the response
        err = http.responseStatusCode();
        if (err >= 0)
        {
            // Usually you'd check that the response code is 200 or a
            // similar "success" code (200-299) before carrying on,
            // but we'll print out whatever response we get

            err = http.skipResponseHeaders();
            if (err >= 0)
            {
                // Now we've got to the body, so we can print it out
                unsigned long timeoutStart = millis();
                char c;
                int bodyLen = http.contentLength();
            
                if (bodyLen > 0){
                    _receivedBody = new char[bodyLen];
                    
                    // Whilst we haven't timed out & haven't reached the end of the body
                    while ( 
                        (http.connected() || http.available()) &&
                        ((millis() - timeoutStart) < kNetworkTimeout) 
                        )
                    {
                        if (http.available())
                        {
                            c = http.read();
                            // Print out this character
                            Serial.print(c);
                    
                            bodyLen--;
                            // We read something, reset the timeout counter
                            timeoutStart = millis();
                        }
                        else
                        {
                            // We haven't got any data, so let's pause to allow some to
                            // arrive
                            delay(kNetworkDelay);
                        }
                    }
                }
            }
        }
    }
    
    http.stop();
    return err;
}

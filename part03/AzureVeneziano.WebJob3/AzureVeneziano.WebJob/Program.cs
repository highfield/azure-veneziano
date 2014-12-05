using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

//see: https://github.com/Azure/azure-webjobs-sdk-samples
namespace AzureVeneziano.WebJob
{
    // To learn more about Microsoft Azure WebJobs, please see http://go.microsoft.com/fwlink/?LinkID=401557
    class Program
    {
        //The app must be marked as "STA" because the WPF rendering
        [STAThread]
        static void Main()
        {
            #region Graceful-shutdown watcher

            /**
             * Implement the code for a graceful shutdown
             * http://blog.amitapple.com/post/2014/05/webjobs-graceful-shutdown/
             **/

            //get the shutdown file path from the environment
            string shutdownFile = Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE");

            //set the flag to alert the incoming shutdown
            bool isRunning = true;

            // Setup a file system watcher on that file's directory to know when the file is created
            var fileSystemWatcher = new FileSystemWatcher(
                Path.GetDirectoryName(shutdownFile)
                );

            //define the FileSystemWatcher callback
            FileSystemEventHandler fswHandler = (_s, _e) =>
            {
                if (_e.FullPath.IndexOf(Path.GetFileName(shutdownFile), StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Found the file mark this WebJob as finished
                    isRunning = false;
                }
            };

            fileSystemWatcher.Created += fswHandler;
            fileSystemWatcher.Changed += fswHandler;
            fileSystemWatcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
            fileSystemWatcher.IncludeSubdirectories = false;
            fileSystemWatcher.EnableRaisingEvents = true;

            Console.WriteLine("Running and waiting " + DateTime.UtcNow);

            #endregion

            IMachineInternal machine = MachineStatus.Instance;

            //retrieve the persisted machine status from the storage
            machine.Load();

            //create the custom logic instance
            ICustomLogic logic = new CustomLogic();

            //create and open the connection in a using block. This 
            //ensures that all resources will be closed and disposed 
            //when the code exits. 
            using (var connection = new SqlConnection(MachineStatus.SQLConnectionString))
            {
                //create the Command object
                var command = new SqlCommand(
                    "SELECT * FROM highfieldtales.tsensors WHERE __deleted = 0",
                    connection
                    );

                //open the connection in a try/catch block.  
                //create and execute the DataReader, writing the result 
                //set to the console window. 
                try
                {
                    connection.Open();

                    //run as long as we didn't get a shutdown notification
                    int jobTimer = 0;
                    while (isRunning)
                    {
                        if (++jobTimer > 10)
                        {
                            jobTimer = 0;

                            //extract all the variables from the DB table
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var lvar = new LogicVar();

                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        switch (reader.GetName(i))
                                        {
                                            case "name":
                                                lvar.Name = reader.GetString(i);
                                                break;

                                            case "__updatedAt":
                                                lvar.LastUpdate = reader.GetDateTimeOffset(i);
                                                break;

                                            case "value":
                                                lvar.Value = reader[i];
                                                break;
                                        }
                                    }

                                    //detect value update
                                    lvar.IsChanged = lvar.LastUpdate > machine.LastUpdate;

                                    //update the variables bag
                                    MachineStatus.Instance.Variables[lvar.Name] = lvar;
                                    //Console.WriteLine(lvar);
                                }
                            }

                            //detect the most recent update timestamp as the new reference
                            foreach (LogicVar lvar in MachineStatus.Instance.Variables.Values)
                            {
                                if (lvar.LastUpdate > machine.LastUpdate)
                                {
                                    machine.LastUpdate = lvar.LastUpdate;
                                }
                            }

                            //invoke the custom logic
                            logic.Run();
                        }

                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //store the machine status
            machine.Save();

            Console.WriteLine("Stopped " + DateTime.UtcNow);
        }

    }
}

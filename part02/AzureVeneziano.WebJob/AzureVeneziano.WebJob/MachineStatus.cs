using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AzureVeneziano.WebJob
{
    internal sealed class MachineStatus
        : IMachineInternal
    {
        #region Singleton pattern

        private MachineStatus() { }


        private static MachineStatus _instance;


        public static MachineStatus Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MachineStatus();

                return _instance;
            }
        }

        #endregion


        #region PROP Variables

        private readonly Dictionary<string, LogicVar> _variables = new Dictionary<string, LogicVar>();

        public IDictionary<string, LogicVar> Variables
        {
            get { return this._variables; }
        }

        #endregion


        public void SendMail(
            MailMessage mail
            )
        {
            mail.From = new MailAddress("(sender address)", "(display name)");
            mail.Subject = "Azure Veneziano Report";

            //set-up the mail client
            var smtpClient = new SmtpClient("(smtp provider)");
            smtpClient.Port = 25;
            smtpClient.Credentials = new System.Net.NetworkCredential("(username)", "(password)");
            smtpClient.EnableSsl = false;   //SSL is unsupported

            //send the message
            smtpClient.Send(mail);
        }


        #region IMachineInternal members

        private const string connectionString =
            "DefaultEndpointsProtocol=https;" +
            "AccountName=(storage name);" +
            "AccountKey=(storage key)";


        DateTimeOffset IMachineInternal.LastUpdate { get; set; }


        //see: http://azure.microsoft.com/en-us/documentation/articles/storage-dotnet-how-to-use-blobs/
        void IMachineInternal.Load()
        {
            //retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(
                connectionString
                );

            //create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("azureveneziano");

            //create the container if it doesn't already exist.
            container.CreateIfNotExists();

            //retrieve reference to a blob named "machine-backup".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("machine-backup");

            if (blockBlob.Exists())
            {
                //download the "machine-backup" blob data...
                string text = blockBlob.DownloadText();

                //...then parse and return as a JSON object
                var data = JToken.Parse(text);

                ((IMachineInternal)this).LastUpdate = (DateTimeOffset?)data["lastUpdate"] ?? DateTimeOffset.MinValue;
            }
        }


        void IMachineInternal.Save()
        {
            //retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(
                connectionString
                );

            //create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("azureveneziano");

            //create the container if it doesn't already exist.
            container.CreateIfNotExists();

            //retrieve reference to a blob named "machine-backup".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("machine-backup");

            var data = new JObject();
            data["lastUpdate"] = ((IMachineInternal)this).LastUpdate;

            //create or overwrite the "machine-backup" blob with contents from a local file.
            blockBlob.UploadText(
                data.ToString()
                );
        }

        #endregion

    }
}

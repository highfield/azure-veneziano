//#define PDF

using AzureVeneziano.Report;
using Cet.UI.Chart;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AzureVeneziano.WebJob
{
    internal sealed class MachineStatus
        : IMachineInternal
    {
        public const string SQLConnectionString =
            "Server=tcp:(your server).database.windows.net,1433;" +
            "Database=(database name);" +
            "User ID=(username);" +
            "Password=(password);" +
            "Trusted_Connection=False;" +
            "Encrypt=True;" +
            "Connection Timeout=30;";


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
            MailMessage mail,
            ReportDataParameters rdp
            )
        {
            mail.From = new MailAddress("(sender address)", "(sender alias)");
            mail.Subject = "Azure Veneziano Report";

            this.CreateReport(
                mail,
                rdp
                );

            //set-up the mail client
            var smtpClient = new SmtpClient("(your smtp provider)");
            smtpClient.Port = 25;
            smtpClient.Credentials = new System.Net.NetworkCredential("(mail username)", "(mail address)");
            smtpClient.EnableSsl = false;   //SSL is unsupported

            //send the message
            smtpClient.Send(mail);
        }


        private FrameworkElement CreateChart(
            ReportDataParameters rdp,
            DateTime dtBeginView,
            DateTime dtEndView
            )
        {
            var uc = new MyChartControl();

            /**
             * CHART MODEL
             **/
            var cm = new ChartModelXY();
            uc.DataContext = cm;

            foreach (string id in rdp.PlotIds)
            {
                cm.Plots.Add(
                    MyPlotResources.Instance.GetPlot(id)
                    );
            }

            //adds the required axes
            foreach (var plot in cm.Plots)
            {
                var cartesian = plot as ChartPlotCartesianBase;
                if (cartesian != null)
                {
                    ChartAxisBase axis;
                    axis = MyAxisResources.Instance.AddWhenRequired(cm, cartesian.HorizontalAxisId);
                    axis = MyAxisResources.Instance.AddWhenRequired(cm, cartesian.VerticalAxisId);
                }
            }

            //update data from source
            using (var sqlConnection1 = new SqlConnection(SQLConnectionString))
            {
                sqlConnection1.Open();

                var sqlText =
                    "SELECT * FROM highfieldtales.thistory " +
                    "WHERE __createdAt >= @begin AND __createdAt <= @end " +
                    "ORDER BY __createdAt ASC";

                var cmd = new SqlCommand(
                    sqlText,
                    sqlConnection1
                    );

                cmd.Parameters.Add(
                    new SqlParameter("@begin", dtBeginView)
                    );

                cmd.Parameters.Add(
                    new SqlParameter("@end", dtEndView)
                    );

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var timestamp = (DateTimeOffset)reader["__createdAt"];

                        var name = (string)reader["name"];
                        var plot = cm.Plots
                            .FirstOrDefault(_ => _.InstanceId == name);

                        var dbl = plot as ChartPlotCartesianLinear;
                        if (dbl != null)
                        {
                            dbl.Points = dbl.Points ?? new List<Point>();

                            var pt = new Point(
                                timestamp.Ticks,
                                (double)reader["value"]
                                );

                            dbl.Points.Add(pt);
                        }
                    }
                }
            }

            var timeline = cm.Axes
                .OfType<ChartAxisTimeline>()
                .FirstOrDefault();

            if (timeline != null)
            {
                timeline.LowestBound = rdp.DateTimeBegin.Ticks;
                timeline.HightestBound = rdp.DateTimeEnd.Ticks;

                timeline.LowerBound = dtBeginView.Ticks;
                timeline.UpperBound = dtEndView.Ticks;
            }

            uc.Measure(new Size(uc.Width, uc.Height));
            uc.Arrange(new Rect(0, 0, uc.Width, uc.Height));

            cm.InvalidateRender();

            return uc;
        }


        private void CreateReport(
            MailMessage mail,
            ReportDataParameters rdp
            )
        {
            //set the basic info for the report generation
            var info = new ReportGeneratorInfo();
            info.ProjectTitle = "Azure Veneziano Project";
            info.ProjectUri = "http://highfieldtales.wordpress.com/";
            info.ProjectVersion = "2014";

            info.ReportTitle = "Alert Data Report";

            //cover logo image
            var stream = this.GetType()
                .Assembly
                .GetManifestResourceStream("AzureVeneziano.WebJob.Images.WP_000687_320x240.jpg");

            info.CoverLogoImage = new System.Drawing.Bitmap(stream);


            /**
             * Cover
             **/
            var document = ReportGeneratorHelpers.CreateDocument(info)
                .AddStandardCover(info);


            /**
             * Page 2
             **/
            
            //overview (brief description)
            document.AddHeading1("Overview")
                .AddNormal(rdp.OverviewText)
                .AddBreak(Spire.Doc.Documents.BreakType.LineBreak)
                .AddBreak(Spire.Doc.Documents.BreakType.LineBreak);

            //charts
            var chart1 = this.CreateChart(
                rdp, 
                rdp.DateTimeBegin, 
                rdp.DateTimeEnd
                );

            var chart2 = this.CreateChart(
                rdp,
                rdp.DateTimeEnd - TimeSpan.FromMinutes(15),
                rdp.DateTimeEnd
                );

            document.AddHeading1("Charts")
                .AddFrameworkElement(chart1)
                .AddBreak(Spire.Doc.Documents.BreakType.LineBreak)
                .AddFrameworkElement(chart2)
                .AddBreak(Spire.Doc.Documents.BreakType.PageBreak);


            /**
             * Page 3
             **/

            //data table
            var table = new ReportDataTable();

            {
                //add the date/time column
                var column = new ReportDataTableColumn(
                    "Timestamp",
                    "Date/time",
                    new GridLength(1, GridUnitType.Star)
                    );

                table.Columns.Add(column);
            }

            //add the remaining columns
            foreach(string id in rdp.PlotIds)
            {
                var plot = MyPlotResources.Instance.GetPlot(id);
                var column = new ReportDataTableColumn(
                    plot.InstanceId,
                    plot.Description,
                    new GridLength(0.75, GridUnitType.Pixel)    //inches
                    );

                table.Columns.Add(column);

                if (table.Columns.Count >= 7)
                    break;
            }

            //query the DB for the most recent data collected
            using (var sqlConnection1 = new SqlConnection(SQLConnectionString))
            {
                sqlConnection1.Open();

                var sqlText =
                    "SELECT * FROM highfieldtales.thistory " +
                    "WHERE __createdAt >= @begin AND __createdAt <= @end " +
                    "ORDER BY __createdAt DESC";

                var cmd = new SqlCommand(
                    sqlText,
                    sqlConnection1
                    );

                cmd.Parameters.Add(
                    new SqlParameter("@begin", rdp.DateTimeBegin)
                    );

                cmd.Parameters.Add(
                    new SqlParameter("@end", rdp.DateTimeEnd)
                    );

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new ReportDataTableRow();
                        row["Timestamp"] = reader["__createdAt"].ToString();

                        var colName = (string)reader["name"];
                        row[colName] = string.Format(
                            "{0:F1}",
                            (double)reader["value"]
                            );

                        table.Rows.Add(row);

                        if (table.Rows.Count >= 50)
                            break;
                    }
                }
            }

            document.AddHeading1("Table")
                .AddTable(table);

            {
                //save the document as Word
                var ms = new MemoryStream();
                document.SaveToStream(
                    ms,
                    Spire.Doc.FileFormat.Docx
                    );

                ms.Position = 0;
                var attachment = new Attachment(
                    ms, 
                    "Report.docx", 
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                    );

                mail.Attachments.Add(attachment);
            }
#if PDF
            {
                //save the document as PDF
                var ms = new MemoryStream();
                document.SaveToStream(
                    ms,
                    Spire.Doc.FileFormat.PDF
                    );

                ms.Position = 0;
                var attachment = new Attachment(
                    ms, 
                    "Report.pdf", 
                    "application/pdf"
                    );

                mail.Attachments.Add(attachment);
            }
#endif
        }


        #region IMachineInternal members

        private const string connectionString =
            "DefaultEndpointsProtocol=https;" +
            "AccountName=(azure storage name);" +
            "AccountKey=(azure storage key)";


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

using AzureVeneziano.Report;
using Cet.UI.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureReportDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }


        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /**
             * CHART MODEL
             **/
            var cm = new ChartModelXY();
            this.DataContext = cm;

            cm.Plots.Add(
                MyPlotResources.Instance.GetPlot("PL1")
                );

            cm.Plots.Add(
                MyPlotResources.Instance.GetPlot("PL2")
                );

            this.UpdateChart(cm);
        }


        private MyDataSource _dataSource = new MyDataSource();
        private IChartDataConverter _dataProvider = new MyDataProvider();


        private void UpdateChart(ChartModelBase cm)
        {
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
            foreach (var plot in cm.Plots)
            {
                this._dataProvider.Convert(plot, this._dataSource);
            }

            var timeline = cm.Axes
                .OfType<ChartAxisTimeline>()
                .FirstOrDefault();

            if (timeline != null)
            {
                timeline.LowerBound = new DateTime(2014, 5, 1).Ticks;
                timeline.UpperBound = new DateTime(2014, 7, 31).Ticks;
            }

            cm.InvalidateRender();
        }


        private void MenuSampleReport_Click(object sender, RoutedEventArgs e)
        {
            var info = new ReportGeneratorInfo();
            info.ProjectTitle = "Azure Veneziano Project";
            info.ProjectUri = "http://highfieldtales.wordpress.com/";
            info.ProjectVersion = "2014";

            info.ReportTitle = "Alert Data Report";

            var stream = this.GetType()
                .Assembly
                .GetManifestResourceStream("AzureReportDemo.Images.WP_000687_320x240.jpg");

            info.CoverLogoImage = new System.Drawing.Bitmap(stream);

            //front-page
            var document = ReportGeneratorHelpers.CreateDocument(info)
                .AddStandardCover(info);

            //page 2
            string overviewText =
                "Spire.Doc for .NET, a professional .NET Word component, "
                + "enables developers to perform a large range of tasks on Word document(from Version Word97-2003 to Word 2010) "
                + "for .NET in C# and VB.NET."
                + "This library is specially designed for .NET developers to help them"
                + "to create any WinForm and ASP.NET Web applications to create, open, write, edit, save and convert"
                + "Word document without Microsoft Office and any other third-party tools installed on system.";

            document.AddHeading1("Overview")
                .AddNormal(overviewText)
                .AddBreak(Spire.Doc.Documents.BreakType.LineBreak)
                .AddBreak(Spire.Doc.Documents.BreakType.LineBreak);

            document.AddHeading1("Chart")
                .AddFrameworkElement(this)
                .AddBreak(Spire.Doc.Documents.BreakType.PageBreak);

            //page 3
            var table = new ReportDataTable();

            {
                var column = new ReportDataTableColumn(
                    "Timestamp",
                    "Date/time",
                    new GridLength(2, GridUnitType.Star)
                    );

                table.Columns.Add(column);
            }
            {
                var column = new ReportDataTableColumn(
                    "Value1",
                    "Value #1",
                    new GridLength(1, GridUnitType.Pixel)
                    );

                table.Columns.Add(column);
            }
            {
                var column = new ReportDataTableColumn(
                    "Value2",
                    "Value #2",
                    new GridLength(1, GridUnitType.Pixel)
                    );

                table.Columns.Add(column);
            }
            {
                var column = new ReportDataTableColumn(
                    "Value3",
                    "Value #3",
                    new GridLength(1, GridUnitType.Pixel)
                    );

                table.Columns.Add(column);
            }
            {
                var column = new ReportDataTableColumn(
                    "Event",
                    "Event",
                    new GridLength(2, GridUnitType.Star)
                    );

                table.Columns.Add(column);
            }

            for (int n = 0; n < 50; n++)
            {
                var row = new ReportDataTableRow();
                row["Timestamp"] = (DateTime.Now + TimeSpan.FromHours(n)).ToString();
                row["Event"] = "This is the record #" + n;
                row["Value1"] = string.Format(
                    "{0:F1}",
                    (Math.PI + 0) * (n + 1)
                    );

                row["Value2"] = string.Format(
                    "{0:F1}",
                    (Math.PI + 1) * (n + 1)
                    );

                row["Value3"] = string.Format(
                    "{0:F1}",
                    (Math.PI + 2) * (n + 1)
                    );

                table.Rows.Add(row);
            }

            document.AddHeading1("Table")
                .AddTable(table);

#if true
            //save the document as Word
            document.SaveToFile(
                @"C:\Users\Mario\Documents\SampleReport.docx",
                Spire.Doc.FileFormat.Docx
                );

            //save the document as PDF
            document.SaveToFile(
                @"C:\Users\Mario\Documents\SampleReport.pdf",
                Spire.Doc.FileFormat.PDF
                );
#else
            {
                //save the document as Word
                var ms = new System.IO.MemoryStream();
                document.SaveToStream(
                    ms,
                    Spire.Doc.FileFormat.Docx
                    );

                ms.Position = 0;
                using (var reader = new System.IO.BinaryReader(ms))
                {
                    byte[] buffer = reader.ReadBytes((int)ms.Length);
                    System.IO.File.WriteAllBytes(
                        @"C:\Users\Mario\Documents\SampleReport.docx",
                        buffer
                        );
                }
            }
            {
                //save the document as PDF
                var ms = new System.IO.MemoryStream();
                document.SaveToStream(
                    ms,
                    Spire.Doc.FileFormat.PDF
                    );

                ms.Position = 0;
                using (var reader = new System.IO.BinaryReader(ms))
                {
                    byte[] buffer = reader.ReadBytes((int)ms.Length);
                    System.IO.File.WriteAllBytes(
                        @"C:\Users\Mario\Documents\SampleReport.pdf",
                        buffer
                        );
                }
            }
#endif

            Console.WriteLine("Complete.");
        }


        private void MenuBuiltInStyles_Click(object sender, RoutedEventArgs e)
        {
            const int size = 51;

            string[] names = Enum.GetNames(typeof(Spire.Doc.Documents.BuiltinStyle));
            int count = 0;

            while (count < names.Length)
            {
                //Create New Word
                using (var document = new Spire.Doc.Document())
                {
                    //Add Section
                    Spire.Doc.Section section = document.AddSection();

                    //enumerate built-in styles
                    for (int n = 0; n < size; n++)
                    {
                        if (count + n >= names.Length) break;

                        var bs = (Spire.Doc.Documents.BuiltinStyle)Enum.Parse(
                            typeof(Spire.Doc.Documents.BuiltinStyle),
                            names[count + n]
                            );

                        Spire.Doc.Documents.Paragraph p = section.AddParagraph();
                        p.ApplyStyle(bs);
                        p.AppendText(names[count + n]);
                    }

                    //Save Word
                    document.SaveToFile(
                        string.Format(@"C:\Users\Mario\Documents\BuiltInStyles_{0}.docx", count),
                        Spire.Doc.FileFormat.Docx
                        );

                    count += size;
                }
            }

            Console.WriteLine("Complete.");
        }

    }
}

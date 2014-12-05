using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AzureVeneziano.Report
{
    public class ReportGeneratorInfo
    {
        public ReportGeneratorInfo()
        {
            this.PageSize = Spire.Doc.Documents.PageSize.A4;
            this.PageOrientation = Spire.Doc.Documents.PageOrientation.Portrait;
            this.PageMargin = new Thickness(
                left: 0.5, 
                top: 1.0, 
                right: 0.5, 
                bottom: 1.0
                );

            var stream = typeof(ReportGeneratorInfo)
                .Assembly
                .GetManifestResourceStream("AzureVeneziano.Report.Images.Azure-Line-Curves-portrait-small.jpg");

            this.CoverBackgroundImage = new System.Drawing.Bitmap(stream);
        }


        public System.Drawing.SizeF PageSize;
        public Spire.Doc.Documents.PageOrientation PageOrientation;
        public Thickness PageMargin;

        public string ProjectTitle;
        public string ProjectUri;
        public string ProjectVersion;

        public string ReportTitle;

        public System.Drawing.Image CoverBackgroundImage;
        public System.Drawing.Image CoverLogoImage;

    }
}

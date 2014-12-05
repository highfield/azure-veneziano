using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AzureVeneziano.Report
{
    public static class ReportGeneratorHelpers
    {
        /// <summary>
        /// Standard dots density for a document
        /// </summary>
        public const double DPI = 72.0;


        /// <summary>
        /// Create a new Word document based on the given parameters
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Spire.Doc.Document CreateDocument(
            ReportGeneratorInfo info
            )
        {
            //create new document
            var document = new Spire.Doc.Document();

            /**
             * Define styles
             **/
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Title";
                style.CharacterFormat.FontName = "Calibri Light";
                style.CharacterFormat.FontSize = 28;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(91, 155, 213);
                style.ParagraphFormat.BeforeSpacing = InchesToDots(0.2);
                style.ParagraphFormat.AfterSpacing = InchesToDots(0.2);
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Heading 1";
                style.CharacterFormat.FontName = "Calibri Light";
                style.CharacterFormat.FontSize = 16;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(91, 155, 213);
                style.ParagraphFormat.BeforeSpacing = InchesToDots(0.1);
                style.ParagraphFormat.AfterSpacing = InchesToDots(0.1);
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Heading 2";
                style.CharacterFormat.FontName = "Calibri Light";
                style.CharacterFormat.FontSize = 13;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(91, 155, 213);
                style.ParagraphFormat.BeforeSpacing = InchesToDots(0.1 * DPI);
                style.ParagraphFormat.AfterSpacing = InchesToDots(0.1 * DPI);
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Subtitle";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(68, 84, 106);
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Normal";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Emphasis";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.Italic = true;
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Intense Emphasis";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(68, 84, 106);
                style.CharacterFormat.Italic = true;
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Strong";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.Bold = true;
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Quote";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.Italic = true;
                style.ParagraphFormat.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                style.ParagraphFormat.BeforeSpacing = InchesToDots(0.2);
                style.ParagraphFormat.AfterSpacing = InchesToDots(0.2);
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Intense quote";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(68, 84, 106);
                style.CharacterFormat.Italic = true;
                style.ParagraphFormat.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                style.ParagraphFormat.BeforeSpacing = InchesToDots(0.2);
                style.ParagraphFormat.AfterSpacing = InchesToDots(0.2);
                style.ParagraphFormat.Borders.BorderType = Spire.Doc.Documents.BorderStyle.Single;
                style.ParagraphFormat.Borders.Color = System.Drawing.Color.FromArgb(68, 84, 106);
                style.ParagraphFormat.Borders.Left.BorderType = Spire.Doc.Documents.BorderStyle.None;
                style.ParagraphFormat.Borders.Right.BorderType = Spire.Doc.Documents.BorderStyle.None;
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Intense Reference";
                style.CharacterFormat.FontName = "Calibri";
                style.CharacterFormat.FontSize = 11;
                style.CharacterFormat.TextColor = System.Drawing.Color.FromArgb(68, 84, 106);
                style.CharacterFormat.AllCaps = true;
                document.Styles.Add(style);
            }
            {
                var style = new Spire.Doc.Documents.ParagraphStyle(document);
                style.Name = "Table";
                style.CharacterFormat.FontName = "Consolas";
                style.CharacterFormat.FontSize = 10;
                document.Styles.Add(style);
            }

            //create section...
            Spire.Doc.Section section = document.AddSection();
            section.PageSetup.DifferentFirstPageHeaderFooter = true;
            section.PageSetup.DifferentOddAndEvenPagesHeaderFooter = true;

            //...define page size and orientation
            section.PageSetup.PageSize = info.PageSize;
            section.PageSetup.Orientation = info.PageOrientation;

            //...then margins...
            section.PageSetup.Margins.Top = InchesToDots(info.PageMargin.Top);
            section.PageSetup.Margins.Bottom = InchesToDots(info.PageMargin.Bottom);
            section.PageSetup.Margins.Left = InchesToDots(info.PageMargin.Left);
            section.PageSetup.Margins.Right = InchesToDots(info.PageMargin.Right);

            /**
             * Page header
             **/
            {
                //odd pages' header
                Spire.Doc.HeaderFooter header = section.HeadersFooters.OddHeader;

                Spire.Doc.Documents.Paragraph para = header.AddParagraph();
                para.ApplyStyle("Subtitle");

                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                para.Format.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.ThinThinSmallGap;
                para.Format.Borders.Bottom.Space = 0.15f;
                para.Format.Borders.Color = System.Drawing.Color.DarkGray;

                para.AppendText(info.ProjectTitle);
                para.AppendText(" - ");
                para.AppendText(info.ProjectVersion);
                para.AppendText(" - ");
                para.AppendText(info.ProjectUri);
            }
            {
                //even pages' header
                Spire.Doc.HeaderFooter header = section.HeadersFooters.EvenHeader;

                Spire.Doc.Documents.Paragraph para = header.AddParagraph();
                para.ApplyStyle("Subtitle");

                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                para.Format.Borders.Bottom.BorderType = Spire.Doc.Documents.BorderStyle.ThinThinSmallGap;
                para.Format.Borders.Bottom.Space = 0.15f;
                para.Format.Borders.Color = System.Drawing.Color.DarkGray;

                para.AppendText(info.ProjectTitle);
                para.AppendText(" - ");
                para.AppendText(info.ProjectVersion);
                para.AppendText(" - ");
                para.AppendText(info.ProjectUri);
            }

            /**
             * Page footer
             **/
            {
                //odd pages' footer
                Spire.Doc.HeaderFooter footer = section.HeadersFooters.OddFooter;
                Spire.Doc.Documents.Paragraph para = footer.AddParagraph();
                para.ApplyStyle("Emphasis");

                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Right;
                para.Format.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.ThinThinSmallGap;
                para.Format.Borders.Top.Space = 0.15f;
                para.Format.Borders.Color = System.Drawing.Color.DarkGray;

                para.AppendText("Page ");
                para.AppendField("page number", Spire.Doc.FieldType.FieldPage);
                para.AppendText(" of ");
                para.AppendField("number of pages", Spire.Doc.FieldType.FieldNumPages);
            }
            {
                //even pages' footer
                Spire.Doc.HeaderFooter footer = section.HeadersFooters.EvenFooter;
                Spire.Doc.Documents.Paragraph para = footer.AddParagraph();
                para.ApplyStyle("Emphasis");

                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                para.Format.Borders.Top.BorderType = Spire.Doc.Documents.BorderStyle.ThinThinSmallGap;
                para.Format.Borders.Top.Space = 0.15f;
                para.Format.Borders.Color = System.Drawing.Color.DarkGray;

                para.AppendText("Page ");
                para.AppendField("page number", Spire.Doc.FieldType.FieldPage);
                para.AppendText(" of ");
                para.AppendField("number of pages", Spire.Doc.FieldType.FieldNumPages);
            }

            return document;
        }


        /// <summary>
        /// Add a standard cover to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddStandardCover(
            this Spire.Doc.Document document,
            ReportGeneratorInfo info
            )
        {
            Spire.Doc.Section section = document.Sections[0];

            /**
             * Cover page
             **/
            if (info.CoverBackgroundImage != null)
            {
                //add the front-page background picture
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();

                Spire.Doc.Fields.DocPicture image = para.AppendPicture(
                    info.CoverBackgroundImage
                    );

                image.HorizontalPosition = -section.PageSetup.Margins.Left;
                image.VerticalPosition = -section.PageSetup.Margins.Top;

                image.Width = section.PageSetup.PageSize.Width / 2;
                image.Height = section.PageSetup.PageSize.Height;

                image.TextWrappingStyle = Spire.Doc.Documents.TextWrappingStyle.Behind;
            }

            if (info.CoverLogoImage != null)
            {
                //add the front-page logo picture
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();
                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                para.Format.BeforeSpacing = InchesToDots(1.5);

                Spire.Doc.Fields.DocPicture image = para.AppendPicture(
                    info.CoverLogoImage
                    );

                image.Width = InchesToDots(4.0);
                image.Height = InchesToDots(3.0);
            }

            if (string.IsNullOrEmpty(info.ReportTitle) == false)
            {
                //add the title
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();
                para.ApplyStyle("Title");
                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                para.AppendText(info.ReportTitle);
            }

            if (string.IsNullOrEmpty(info.ProjectTitle) == false)
            {
                //add the subtitle
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();
                para.ApplyStyle("Subtitle");
                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                para.AppendText(info.ProjectTitle);

                para.AppendBreak(Spire.Doc.Documents.BreakType.LineBreak);
            }

            if (string.IsNullOrEmpty(info.ProjectVersion) == false ||
                string.IsNullOrEmpty(info.ProjectUri) == false
                )
            {
                //add the subtitle
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();
                para.ApplyStyle("Subtitle");
                para.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                para.AppendText(info.ProjectVersion ?? string.Empty);

                if (string.IsNullOrEmpty(info.ProjectVersion) == false &&
                    string.IsNullOrEmpty(info.ProjectUri) == false
                    )
                {
                    para.AppendText(" - ");
                }

                para.AppendText(info.ProjectUri ?? string.Empty);

                para.AppendBreak(Spire.Doc.Documents.BreakType.LineBreak);
            }

            {
                //add some report info
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();
                para.ApplyStyle("Intense quote");
                para.AppendText("Document created on: " + DateTime.Now);

                para.AppendBreak(Spire.Doc.Documents.BreakType.PageBreak);
            }

            return document;
        }


        /// <summary>
        /// Add a "Heading1"-styled block of text to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddHeading1(
            this Spire.Doc.Document document,
            string text
            )
        {
            Spire.Doc.Section section = document.Sections[0];

            //create a new paragraph
            Spire.Doc.Documents.Paragraph para = section.AddParagraph();
            para.ApplyStyle("Heading 1");
            para.AppendText(text);

            return document;
        }


        /// <summary>
        /// Add a "Normal"-styled block of text to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddNormal(
            this Spire.Doc.Document document,
            string text
            )
        {
            Spire.Doc.Section section = document.Sections[0];

            //create a new paragraph
            Spire.Doc.Documents.Paragraph para = section.AddParagraph();
            para.ApplyStyle("Normal");
            para.AppendText(text);

            return document;
        }


        /// <summary>
        /// Add a collection of "Normal"-styled block of text to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddNormal(
            this Spire.Doc.Document document,
            IEnumerable<string> collection
            )
        {
            foreach (string text in collection)
            {
                AddNormal(document, text);
            }

            return document;
        }


        /// <summary>
        /// Add a break of any sort to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="break"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddBreak(
            this Spire.Doc.Document document,
            Spire.Doc.Documents.BreakType @break
            )
        {
            Spire.Doc.Section section = document.Sections[0];
            Spire.Doc.Documents.Paragraph para = section.Paragraphs[section.Paragraphs.Count - 1];
            para.AppendBreak(
                @break
                );

            return document;
        }


        /// <summary>
        /// Add a simple table to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddTable(
            this Spire.Doc.Document document,
            ReportDataTable table
            )
        {
            //detect the number of columns
            int columnsCount = table.Columns.Count;
            if (columnsCount == 0)
            {
                return document;
            }

            Spire.Doc.Section section = document.Sections[0];

            //calculate the fixed-width columns total size
            float totalFixedWidth = table
                .Columns
                .Select(_ => _.Width)
                .Where(_ => _.GridUnitType == GridUnitType.Pixel)
                .Sum(_ => InchesToDots(_.Value));

            //calculate the starred-size columns total relative size
            double totalStarredWidth = table
                .Columns
                .Select(_ => _.Width)
                .Where(_ => _.GridUnitType == GridUnitType.Star)
                .Sum(_ => _.Value);

            //calculate the residual space for the starred-size columns
            float residualWidth = section.PageSetup.ClientWidth - totalFixedWidth;
            if (residualWidth <= 0)
            {
                throw new InvalidOperationException("Not enough space to fit all the columns.");
            }

            //calculate the tab positions
            float[] tabPositions = new float[columnsCount];
            float currentPosition = 0;
            for (int k = 0; k < columnsCount; k++)
            {
                tabPositions[k] = currentPosition;

                GridLength gl = table.Columns[k].Width;
                switch (gl.GridUnitType)
                {
                    case GridUnitType.Pixel:
                        currentPosition += InchesToDots(gl.Value);
                        break;

                    case GridUnitType.Star:
                        currentPosition += (float)(gl.Value / totalStarredWidth * residualWidth);
                        break;

                    case GridUnitType.Auto:
                        throw new NotSupportedException();
                }
            }

            //define the handler for creating every single table row
            Action<string[]> renderRow = cells =>
            {
                //create a new paragraph
                Spire.Doc.Documents.Paragraph para = section.AddParagraph();
                para.ApplyStyle("Table");

                //redefine the tabulation positions
                para.Format.Tabs.Clear();
                for (int k = 1; k < columnsCount; k++)
                {
                    para.Format.Tabs.AddTab(
                        tabPositions[k]
                        );
                }

                //append the cells interleaving a "tab" character
                var sb = new StringBuilder();

                for (int k = 0; k < columnsCount; k++)
                {
                    if (k > 0) sb.Append("\t");
                    sb.Append(
                        cells[k]
                        );
                }

                para.AppendText(
                    sb.ToString()
                    );
            };

            //render the table header
            renderRow(
                table.Columns.Select(_ => _.Title).ToArray()
                );

            //fill the table matrix
            foreach (var row in table.Rows)
            {
                renderRow(
                    row.AsArray(table)
                    );
            }

            return document;
        }


        /// <summary>
        /// Render a FrameworkElement face as an image to the specified document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="fe"></param>
        /// <returns></returns>
        public static Spire.Doc.Document AddFrameworkElement(
            this Spire.Doc.Document document,
            FrameworkElement fe
            )
        {
            Spire.Doc.Section section = document.Sections[0];

            //create a new paragraph
            Spire.Doc.Documents.Paragraph para = section.AddParagraph();

            var w = fe.Width;
            var h = fe.Height;

            Spire.Doc.Fields.DocPicture image = para.AppendPicture(
                SaveImage(fe, w, h)
                );

            image.Width = section.PageSetup.ClientWidth;
            image.Height = (float)(section.PageSetup.ClientWidth * h / w);

            return document;
        }


        /// <summary>
        /// Render a visual to a bitmap image
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static System.Drawing.Image SaveImage(
            Visual visual,
            double width,
            double height
            )
        {
            var bitmap = new RenderTargetBitmap(
                (int)width,
                (int)height,
                96,
                96,
                PixelFormats.Pbgra32
                );

            bitmap.Render(visual);

            var image = new PngBitmapEncoder();
            image.Frames.Add(BitmapFrame.Create(bitmap));
            var ms = new System.IO.MemoryStream();
            image.Save(ms);

            return System.Drawing.Image.FromStream(ms);
        }


        /// <summary>
        /// Convert inches to dots
        /// </summary>
        /// <param name="inches"></param>
        /// <returns></returns>
        public static float InchesToDots(
            double inches
            )
        {
            return (float)(inches * DPI);
        }

    }
}

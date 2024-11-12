using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Font;
using iText.Layout.Properties;
using iText.Layout.Element;
using iText.Html2pdf.Resolver.Font;

namespace ReportGeneratorApp.Services
{
    public class PDFService
    {
        private readonly TemplateService _templateService;

        public PDFService(TemplateService templateService)
        {
            _templateService = templateService;
        }

        public void GenerateReportPdf(Dictionary<string, Dictionary<string, string>> subjectData, List<string> selectedSubjects)
        {
            string outputPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Multi_Subject_Report_{DateTime.Now:yyyyMMddHHmmss}.pdf";

            try
            {
                using (PdfWriter writer = new PdfWriter(outputPath))
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                using (Document document = new Document(pdfDoc))
                {
                    // Set up Arabic font
                    string fontPath = Path.Combine(Application.StartupPath, "Fonts", "ScheherazadeNew-Regular.ttf");
                    PdfFont arabicFont = PdfFontFactory.CreateFont(fontPath, "Identity-H");
                    document.SetFont(arabicFont).SetTextAlignment(TextAlignment.RIGHT).SetBaseDirection(BaseDirection.RIGHT_TO_LEFT);

                    foreach (var subject in selectedSubjects)
                    {
                        string templatePath = _templateService.GetTemplatePath(subject);
                        if (templatePath == null || !File.Exists(templatePath)) continue;

                        string htmlContent = File.ReadAllText(templatePath);
                        if (subjectData.TryGetValue(subject, out var data))
                        {
                            foreach (var placeholder in data)
                                htmlContent = htmlContent.Replace($"{{{placeholder.Key}}}", placeholder.Value);
                        }

                        // Add RTL and font styling
                        string rtlCss = "<style> body { direction: rtl; font-family: 'ScheherazadeNew'; margin: 0; padding: 0; } </style>";
                        htmlContent = rtlCss + htmlContent;

                        // Convert HTML to PDF page
                        using (var htmlStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlContent)))
                        {
                            PdfDocument tempPdf = new PdfDocument(new PdfWriter(new MemoryStream()));
                            HtmlConverter.ConvertToPdf(htmlStream, tempPdf, new ConverterProperties().SetFontProvider(new DefaultFontProvider(true, true, true)));

                            tempPdf.CopyPagesTo(1, tempPdf.GetNumberOfPages(), pdfDoc);
                        }
                    }

                    MessageBox.Show("تم إنشاء ملف PDF بنجاح!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"خطأ في إنشاء ملف PDF: {ex.Message}");
            }
        }
    }
}



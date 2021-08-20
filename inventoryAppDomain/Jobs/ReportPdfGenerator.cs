using System;
using inventoryAppDomain.Entities.Enums;
using inventoryAppDomain.Services;
using IronPdf;

namespace inventoryAppDomain.Jobs
{
    public abstract class ReportPdfGenerator
    {
        private IReportService ReportService { get; }

        protected ReportPdfGenerator(IReportService reportService)
        {
            ReportService = reportService;
        }
        
        
        public PdfDocument GenerateReportPdf(TimeFrame timeFrame)
        {
            var report = ReportService.CreateReport(timeFrame);
            var renderer = new HtmlToPdf
            {
                PrintOptions =
                {
                    CustomCssUrl = new Uri("https://stackpath.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.css")
                        .ToString()
                }
            };
            var pdf = renderer.RenderHtmlAsPdf(report.ProductSales).SaveAs("C:\\Users\\tochu\\Documents\\C# Projects\\inventoryapp\\InventoryApp\\report.pdf");
            pdf.AddHTMLHeaders(new HtmlHeaderFooter
            {
                CenterText = "PDF REPORT",
                FontSize = 24
            });
            return pdf;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using R1RiceMill.Core;
using R1RiceMill.Data;

namespace R1RiceMill.Services
{
    public static class ReportService
    {
        private static readonly string REPORTS_DATAFOLDER;

        static ReportService()
        {
            REPORTS_DATAFOLDER = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sales and Inventory System RI Rice Mill", "Reports");
            Directory.CreateDirectory(REPORTS_DATAFOLDER);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public static async Task SalesReport(DateTime? start, DateTime? end)
        {
            var doc = new Document();
            doc.DefaultPageSetup.TopMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.LeftMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.RightMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.BottomMargin = Unit.FromInch(1);
            doc.AddSection();

            var caption = doc.LastSection.AddParagraph();
            caption.Format.Alignment = ParagraphAlignment.Center;
            caption.AddFormattedText("SALES and INVENTORY SYSTEM RI RICE MILL", TextFormat.Bold);
            caption.AddLineBreak();
            caption.AddFormattedText("SALES REPORT", TextFormat.Bold);
            caption.Format.SpaceAfter = Unit.FromPoint(20);

            IList<Order> orders = null;
            using (var db = new DatabaseContext())
            {
                orders = await db.Orders
                    .Include(o => o.Transaction)
                    .ThenInclude(t => t.Customer)
                    .Include(o => o.Transaction)
                    .ThenInclude(t => t.User)
                    .Include(o => o.Batch)
                    .ThenInclude(b => b.Product)
                    .OrderBy(o => o.Transaction.Date)
                    .ToListAsync();
            }

            if (start.HasValue)
            {
                orders = orders.Where(o => o.Transaction.Date >= start.Value.Date).ToList();
                var startText = doc.LastSection.AddParagraph();
                startText.AddText($"Start Date: {start.Value:g}");
            }

            if (end.HasValue)
            {
                orders = orders.Where(o => o.Transaction.Date < end.Value.Date.AddDays(1)).ToList();
                var startText = doc.LastSection.AddParagraph();
                startText.AddText($"End Date: {end.Value:g}");
            }

            foreach (var item in orders)
            {
                var table = doc.LastSection.AddTable();
                table.Borders.Width = 0;
                table.Format.SpaceBefore = Unit.FromPoint(10);
                table.AddColumn(Unit.FromInch(2));
                table.AddColumn(Unit.FromInch(4.5));

                var row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                var paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Transaction #:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Transaction.TransactionNumber.ToString(), TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph  = row.Cells[0].AddParagraph();
                paragraph.AddText("Invoice:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Transaction.InvoiceCode, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Product:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Batch.Product.Variety, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Batch Code:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Batch.Code, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Quantity:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Quantity.ToString(), TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Price:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Price.ToString("C2", new CultureInfo("en-PH")), TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Customer:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Transaction.Customer.FullName, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("User:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Transaction.User.FullName, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Date:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Transaction.Date.ToString("g"), TextFormat.Bold);

                table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Bottom, BorderStyle.Single, Unit.FromPoint(1), Colors.Black);
            }

            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            var fileName = Path.Combine(REPORTS_DATAFOLDER, $"Sales Report {DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.pdf");
            renderer.PdfDocument.Save(fileName);
            Process.Start(new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            });
        }

        public static async Task ReturnsReport(DateTime? start, DateTime? end)
        {
            var doc = new Document();
            doc.DefaultPageSetup.TopMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.LeftMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.RightMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.BottomMargin = Unit.FromInch(1);
            doc.AddSection();

            var caption = doc.LastSection.AddParagraph();
            caption.Format.Alignment = ParagraphAlignment.Center;
            caption.AddFormattedText("SALES and INVENTORY SYSTEM RI RICE MILL", TextFormat.Bold);
            caption.AddLineBreak();
            caption.AddFormattedText("RETURNS REPORT", TextFormat.Bold);
            caption.Format.SpaceAfter = Unit.FromPoint(20);

            IList<Return> returns = null;
            using (var db = new DatabaseContext())
            {
                returns = await db.Returns
                    .Include(r => r.Order)
                    .ThenInclude(o => o.Transaction)
                    .ThenInclude(t => t.Customer)
                    .Include(r => r.Order)
                    .ThenInclude(o => o.Batch)
                    .ThenInclude(b => b.Product)
                    .OrderBy(r => r.Date)
                    .ToListAsync();
            }

            if (start.HasValue)
            {
                returns = returns.Where(o => o.Date >= start.Value.Date).ToList();
                var startText = doc.LastSection.AddParagraph();
                startText.AddText($"Start Date: {start.Value:g}");
            }

            if (end.HasValue)
            {
                returns = returns.Where(o => o.Date < end.Value.Date.AddDays(1)).ToList();
                var startText = doc.LastSection.AddParagraph();
                startText.AddText($"End Date: {end.Value:g}");
            }

            foreach (var item in returns)
            {
                var table = doc.LastSection.AddTable();
                table.Borders.Width = 0;
                table.Format.SpaceBefore = Unit.FromPoint(10);
                table.AddColumn(Unit.FromInch(2));
                table.AddColumn(Unit.FromInch(4.5));

                var row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                var paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Transaction #:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Order.Transaction.TransactionNumber.ToString(), TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Invoice:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Order.Transaction.InvoiceCode, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Product:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Order.Batch.Product.Variety, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Batch Code:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Order.Batch.Code, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Quantity:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Quantity.ToString(), TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Reason:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Reason, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Customer:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Order.Transaction.Customer.FullName, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Date:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Date.ToString("g"), TextFormat.Bold);

                table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Bottom, BorderStyle.Single, Unit.FromPoint(1), Colors.Black);
            }

            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            var fileName = Path.Combine(REPORTS_DATAFOLDER, $"Returns Report {DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.pdf");
            renderer.PdfDocument.Save(fileName);
            Process.Start(new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            });
        }

        public static async Task InventoryReport(DateTime? start, DateTime? end)
        {
            var doc = new Document();
            doc.DefaultPageSetup.TopMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.LeftMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.RightMargin = Unit.FromInch(1);
            doc.DefaultPageSetup.BottomMargin = Unit.FromInch(1);
            doc.AddSection();

            var caption = doc.LastSection.AddParagraph();
            caption.Format.Alignment = ParagraphAlignment.Center;
            caption.AddFormattedText("SALES and INVENTORY SYSTEM RI RICE MILL", TextFormat.Bold);
            caption.AddLineBreak();
            caption.AddFormattedText("INVENTORY REPORT", TextFormat.Bold);
            caption.Format.SpaceAfter = Unit.FromPoint(20);

            IList<Batch> batches = null;
            using (var db = new DatabaseContext())
            {
                batches = await db.Batches
                    .Include(b => b.Product)
                    .OrderBy(b => b.Date)
                    .ToListAsync();
            }

            if (start.HasValue)
            {
                batches = batches.Where(o => o.Date >= start.Value.Date).ToList();
                var startText = doc.LastSection.AddParagraph();
                startText.AddText($"Start Date: {start.Value:g}");
            }

            if (end.HasValue)
            {
                batches = batches.Where(o => o.Date < end.Value.Date.AddDays(1)).ToList();
                var startText = doc.LastSection.AddParagraph();
                startText.AddText($"End Date: {end.Value:g}");
            }

            foreach (var item in batches)
            {
                var table = doc.LastSection.AddTable();
                table.Borders.Width = 0;
                table.Format.SpaceBefore = Unit.FromPoint(10);
                table.AddColumn(Unit.FromInch(2));
                table.AddColumn(Unit.FromInch(4.5));

                var row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                var paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Product:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Product.Variety, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Batch Code:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Code, TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Available Stock:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.AvailableStock.ToString(), TextFormat.Bold);

                row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText("Date:");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText(item.Date.ToString("g"), TextFormat.Bold);

                table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Bottom, BorderStyle.Single, Unit.FromPoint(1), Colors.Black);
            }

            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            var fileName = Path.Combine(REPORTS_DATAFOLDER, $"Returns Report {DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.pdf");
            renderer.PdfDocument.Save(fileName);
            Process.Start(new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            });
        }

        public static void Receipt(Transaction transaction, decimal subtotal, Discount discountType, decimal discount, decimal tax, decimal total)
        {
            var doc = new Document();
            doc.DefaultPageSetup.TopMargin = Unit.FromInch(0.1);
            doc.DefaultPageSetup.LeftMargin = Unit.FromInch(0.1);
            doc.DefaultPageSetup.RightMargin = Unit.FromInch(0.1);
            doc.DefaultPageSetup.BottomMargin = Unit.FromInch(0.1);
            doc.DefaultPageSetup.PageHeight = Unit.FromInch(11);
            doc.DefaultPageSetup.PageWidth = Unit.FromInch(3);
            doc.AddSection();

            var caption = doc.LastSection.AddParagraph();
            caption.Format.Alignment = ParagraphAlignment.Center;
            caption.AddFormattedText("SALES and INVENTORY SYSTEM RI RICE MILL", TextFormat.Bold);
            caption.AddLineBreak();
            caption.AddFormattedText("RECEIPT", TextFormat.Bold);
            caption.Format.SpaceAfter = Unit.FromPoint(20);

            var paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("TX #:");
            paragraph.AddFormattedText(transaction.TransactionNumber.ToString(), TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Invoice:");
            paragraph.AddFormattedText(transaction.InvoiceCode, TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Customer:");
            paragraph.AddFormattedText(transaction.Customer.FullName, TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Cashier:");
            paragraph.AddFormattedText(transaction.User.FullName, TextFormat.Bold);

            var table = doc.LastSection.AddTable();
            table.Borders.Width = 0;
            table.Format.SpaceBefore = Unit.FromPoint(10);
            table.AddColumn(Unit.FromInch(2));
            table.AddColumn(Unit.FromInch(1));

            foreach (var item in transaction.Orders)
            {
                var row = table.AddRow();
                row.TopPadding = Unit.FromPoint(1);
                row.BottomPadding = Unit.FromPoint(1);
                paragraph = row.Cells[0].AddParagraph();
                paragraph.AddText(item.Batch.Product.Variety);
                paragraph.AddLineBreak();
                paragraph.AddText($"{item.Quantity} kg * {item.Price.ToString("C2", new CultureInfo("en-PH"))}");
                paragraph = row.Cells[1].AddParagraph();
                paragraph.AddFormattedText($"{((decimal)item.Quantity * item.Price).ToString("C2", new CultureInfo("en-PH"))}", TextFormat.Bold);
            }

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Sub-Total:");
            paragraph.AddFormattedText(subtotal.ToString("C2", new CultureInfo("en-PH")), TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Discount Type:");
            paragraph.AddFormattedText(discountType.Humanize(), TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Discount:");
            paragraph.AddFormattedText(discount.ToString("C2", new CultureInfo("en-PH")), TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Tax (12%):");
            paragraph.AddFormattedText(tax.ToString("C2", new CultureInfo("en-PH")), TextFormat.Bold);

            paragraph = doc.LastSection.AddParagraph();
            paragraph.AddText("Grand Total:");
            paragraph.AddFormattedText(total.ToString("C2", new CultureInfo("en-PH")), TextFormat.Bold);

            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            var fileName = Path.Combine(REPORTS_DATAFOLDER, $"Receipt {DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.pdf");
            renderer.PdfDocument.Save(fileName);
            Process.Start(new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            });
        }

    }
}

using SkyeMinder.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

namespace SkyeMinder.Services
{
    public static class Exporter
    {
        public static string GenerateCSV(List<BloodSugarEntry> entries, bool includeSummary)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Timestamp,Value (mg/dL),Notes");

            foreach (var entry in entries)
            {
                sb.AppendLine($"{entry.Timestamp:yyyy-MM-dd HH:mm},{entry.Value},{entry.Notes?.Replace(",", " ")}");
            }

            if (includeSummary && entries.Count != 0)
            {
                var avg = entries.Average(e => e.Value);
                var min = entries.Min(e => e.Value);
                var max = entries.Max(e => e.Value);
                sb.AppendLine();
                sb.AppendLine($"Average,{avg:F1}");
                sb.AppendLine($"Min,{min}");
                sb.AppendLine($"Max,{max}");
            }

            return sb.ToString();
        }
        public static byte[] GeneratePDF(List<BloodSugarEntry> entries, bool includeSummary)
        {
            using var document = new PdfDocument();
            var page = document.Pages.Add();
            var graphics = page.Graphics;
            var font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

            float y = 0;

            graphics.DrawString("Blood Sugar Log", new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y));
            y += 30;

            //graphics.DrawString("Date\t\tValue\t\tNotes", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y));
            graphics.DrawString("Date", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y));
            graphics.DrawString("Value", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(75, y)); // adjust X for spacing
            graphics.DrawString("Notes", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(125, y));
            y += 20;

            int lowThreshold = UserSettings.LowThreshold;
            int highThreshold = UserSettings.HighThreshold;

            foreach (var entry in entries)
            {
                PdfBrush brush;
                if (entry.Value < lowThreshold)
                    brush = PdfBrushes.Orange;
                else if (entry.Value > highThreshold)
                    brush = PdfBrushes.Red;
                else
                    brush = PdfBrushes.Black;

                graphics.DrawString(entry.Timestamp.ToShortDateString(), font, brush, new Syncfusion.Drawing.PointF(0, y));
                graphics.DrawString(entry.Value.ToString(), font, brush, new Syncfusion.Drawing.PointF(75, y));
                graphics.DrawString(entry.Notes ?? "", font, brush, new Syncfusion.Drawing.PointF(125, y));

                y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
            }

            if (includeSummary)
            {
                var total = entries.Count;
                var average = entries.Average(e => e.Value);
                var min = entries.Min(e => e.Value);
                var max = entries.Max(e => e.Value);
                var firstDate = entries.Min(e => e.Timestamp).ToShortDateString();
                var lastDate = entries.Max(e => e.Timestamp).ToShortDateString();
                y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
                graphics.DrawString("Summary", new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y));
                y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
                graphics.DrawString($"Date Range: {firstDate} to {lastDate}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y)); y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
                graphics.DrawString($"Total Entries: {total}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y)); y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
                graphics.DrawString($"Average Value: {average:F1}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y)); y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
                graphics.DrawString($"Min Value: {min}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y)); y += 20;
                (page, graphics, y) = EnsureSpace(document, page, y);
                graphics.DrawString($"Max Value: {max}", font, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, y));
            }

            using var stream = new MemoryStream();
            document.Save(stream);
            return stream.ToArray();
        }
        private static (PdfPage page, PdfGraphics graphics, float y) EnsureSpace(PdfDocument doc, PdfPage page, float y)
        {
            float limit = page.GetClientSize().Height - 40;

            if (y > limit)
            {
                page = doc.Pages.Add();
                var graphics = page.Graphics;
                y = 0;
                return (page, graphics, y);
            }

            return (page, page.Graphics, y);
        }
    }
}
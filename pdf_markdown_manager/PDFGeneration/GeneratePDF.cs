using System.IO;
using System.Reflection.Metadata;
using System.Web;
using Microsoft.CodeAnalysis;
using pdf_markdown_manager.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;


namespace pdf_markdown_manager.PDFGeneration
{
    public class GeneratePDF
    {
        public void generateFromDocument(Documents doc)
        {
            PdfDocument pdf = new PdfDocument();

            pdf.Info.Title = doc.title;

            PdfPage page = pdf.AddPage();


            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", double.Parse(doc.font_size), XFontStyleEx.Bold);

            gfx.DrawString(doc.content, font, XBrushes.Black, 0, 0);

            pdf.Save(doc.title + ".pdf");
            return;
        }
    }
}

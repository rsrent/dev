using Foundation;
using System;
using UIKit;
using PdfKit;

namespace RentApp
{
    public partial class PDFVC : UIViewController
    {
        public PDFVC (IntPtr handle) : base (handle)
        {
        }

        byte[] PdfBytes;
        public void ParseInfo(byte[] pdfBytes) {
            PdfBytes = pdfBytes;
        }

        string PdfPath;
        public void ParseInfo(string pdfPath)
        {
            PdfPath = pdfPath;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var pdfView = new PdfView(View.Bounds);

            View.Add(pdfView);

            PdfDocument document = null;
            if (PdfBytes != null)
                document = new PdfDocument(NSData.FromArray(PdfBytes));
            else if (PdfPath != null)
                document = new PdfDocument(new NSUrl(PdfPath));

            pdfView.AutoScales = true;

            pdfView.Document = document;
        }
    }
}
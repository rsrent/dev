using Foundation;
using System;
using UIKit;
using RentApp.Shared.Models.Document;
using ModuleLibraryiOS.Image;

namespace RentApp
{
    public partial class DocumentCell : UITableViewCell
    {
        public DocumentCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(Document document) {
            NameLabel.Text = document.Title;

            DocumentImage.SetColor(UIColor.FromName("ThemeColor")); 

            if (document.GetType() == typeof(DocumentFolder))
            {
                DocumentImage.Image = UIImage.FromBundle("folder");
            }
            else
            {
                DocumentImage.Image = UIImage.FromBundle("file");
            }
        }
    }
}
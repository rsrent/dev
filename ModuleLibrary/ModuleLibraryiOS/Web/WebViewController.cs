using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace ModuleLibraryiOS.Web
{
    public partial class WebViewController : UIViewController
    {
        public WebViewController (IntPtr handle) : base (handle)
        {
        }

        NSUrl Url;
        string WebTitle;

        public static void Start(UIViewController viewController, UIView container, string url, string title) {
			var chatStoryboard = UIStoryboard.FromName("Web", null);
			var newView = chatStoryboard.InstantiateViewController("WebViewController") as WebViewController;
            //TODO
            Instanciate.Start(container, viewController, newView);
            newView.ParseInfo(url, title);
        }

		public  void ParseInfo(string url, string title)
		{
            Url = new NSUrl(url);
            WebTitle = title;
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var urlRequest = new NSUrlRequest(Url);
            Title = WebTitle;
            WebView.LoadRequest(urlRequest);
        }

    }
}
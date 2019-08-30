using UIKit;
using System.Globalization;
using System.Threading;

namespace RentApp
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.

            CultureInfo newCulture = CultureInfo.CreateSpecificCulture("da-DK");
            Thread.CurrentThread.CurrentUICulture = newCulture;
            // Make current UI culture consistent with current culture.
            Thread.CurrentThread.CurrentCulture = newCulture;

            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
using System;
using ModuleLibraryiOS.Alert;
using UIKit;

namespace RentApp
{
    public class ErrorMessageHandler : IErrorMessageHandler
    {
        UINavigationController _navigationController;

        public void SetNavigation(UINavigationController navigationController) {
            _navigationController = navigationController;
        }

        Action IErrorMessageHandler.DisplayLoadErrorMessage()
        {
            return () => Alert.DisplayToast("Indlæsningsfejl", _navigationController.TopViewController);
        }
    }
}

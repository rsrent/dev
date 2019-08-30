using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Navigation;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Date;
using ModuleLibraryiOS.Alert;
using System.Collections.Generic;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class QualityReportDateTimeVC : UIViewController
    {
        QualityReportRepository qualityReportRepository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();
        QualityReportVM _qualityReportVM = AppDelegate.ServiceProvider.GetService<QualityReportVM>();
        LocationVM _locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();

        public QualityReportDateTimeVC (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.RightNavigationButton("Færdiggør", () =>
            {
                var options = new List<(string, Action)>();
                options.Add(("Godt", () => CompleteReport(1)));
                options.Add(("Okay", () => CompleteReport(2)));
                options.Add(("Skidt", () => CompleteReport(3)));
                this.DisplayMenu("Hvor godt synes du samarbejdet med kunden går?", options);
            });

            Picker.MinimumDate = DateTime.Now.AddDays(1).StartOfDay().DateTimeToNSDate();

            if(_locationVM.Location.IntervalOfServiceLeaderMeeting > 0) {
                Picker.MaximumDate = DateTime.Now.AddDays(_locationVM.Location.IntervalOfServiceLeaderMeeting).EndOfDay().DateTimeToNSDate();
            }
        }

        void CompleteReport(int rating) {
            qualityReportRepository.CompleteQualityRapport(_qualityReportVM.QualityReport.ID, Picker.Date.NSDateToDateTime(), rating, () =>
            {
                this.NavigationController.PopViewController(true);
                _qualityReportVM.QualityReport.CompletedTime = DateTime.Now;
            }).LoadingOverlay(this);
        }
    }
}
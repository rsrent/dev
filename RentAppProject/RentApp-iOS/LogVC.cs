using Foundation;
using System;
using UIKit;
using Rent.Shared.Models;
using Rent.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class LogVC : UIViewController
    {
        LogRepository _logRepository = AppDelegate.ServiceProvider.GetService<LogRepository>();

        public LogVC (IntPtr handle) : base (handle)
        {
        }

        LocationLog _log;

        public override void ViewDidLoad()
        {
            _logRepository.Get(_log.ID, (log) =>
            {
                _log = log;
                TextView.Text = _log.Log;
                TitleLabel.Text = _log.Title;
            }).LoadingOverlay(this);

            this.RightNavigationButton("Gem", () =>
            {
                Update().LoadingOverlay(this);
            });
        }

        async Task Update()
        {
            _log.Log = TextView.Text;
            _log.Title = TitleLabel.Text;
            await _logRepository.Update(_log.ID, _log, () => { });
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            Update();
        }

        public void Setup(LocationLog log)
        {
            _log = log;
        }
    }
}
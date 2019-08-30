using Foundation;
using System;
using UIKit;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Image;
using RentApp.ViewModels;
using System.Collections.Generic;
using ModuleLibraryiOS.Camera;
using RentApp.Repository;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class QualityReportTasksCell : UITableViewCell
    {
        QualityReportRepository repository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();
        QualityReportVM _qualityReportVM = AppDelegate.ServiceProvider.GetService<QualityReportVM>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();

        UserVM userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        public QualityReportTasksCell (IntPtr handle) : base (handle)
        {
            
        }

		QualityReportItem Item;
		UIViewController VC;

		public void UpdateCell(QualityReportItem item, UIViewController controller)
		{
			Item = item;
			VC = controller;

			DescriptionLabel.Text = item.CleaningTask.Comment + ", " + item.CleaningTask.SquareMeters + "m²";
            DescriptionLabel.TextColor = UIColor.Black;
            if (string.IsNullOrEmpty(item.CleaningTask.Comment))
            {
                DescriptionLabel.Text = item.CleaningTask.Area.Description + ", " + item.CleaningTask.SquareMeters + "m²";
                DescriptionLabel.TextColor = UIColor.FromName("FadeTextColor");
            }


            BadImage.SetColor(UIColor.LightGray);
            NeutralImage.SetColor(UIColor.LightGray);
            GoodImage.SetColor(UIColor.LightGray);

			if (item.Rating > 0 && item.Rating < 4)
			{
				setColor(item.Rating);
			}

            if (userVM.HasPermission(Permission.QualityReport, Permission.CRUDD.Create) && _qualityReportVM.QualityReport.CompletedTime == null)
			{
				GoodButton.TouchUpInside -= Good;
				GoodButton.TouchUpInside += Good;

				OkayButton.TouchUpInside -= Okay;
				OkayButton.TouchUpInside += Okay;

				BadButton.TouchUpInside -= Bad;
				BadButton.TouchUpInside += Bad;
			}

            updateImage(item, controller);
		}

        void updateImage(QualityReportItem item, UIViewController controller) {
            if (!string.IsNullOrEmpty(item.ImageLocation))
            {
                ImageButton.Hidden = false;
                ImageButton.UserInteractionEnabled = true;
                ImageButton.AddGestureRecognizer(new UITapGestureRecognizer((obj) => {

                    Alert.DisplayLoadingWhile(controller, async () => {
                        var array = await _storage.Download(item.ImageLocation, "image");
                        controller.Start<ImageVC>().ParseInfo(array);
                    });

                }));
                ImageButton.SetColor(UIColor.FromName("ThemeColor"));
            }
            else
            {
                ImageButton.Hidden = true;
            }
        }

		void setColor(int raiting)
		{
            BadImage.SetColor(UIColor.LightGray);
            NeutralImage.SetColor(UIColor.LightGray);
            GoodImage.SetColor(UIColor.LightGray);

			if (raiting == 1) GoodImage.SetColor(UIColor.FromName("GoodColor"));
            if (raiting == 2) NeutralImage.SetColor(UIColor.FromName("OkayColor"));
            if (raiting == 3) BadImage.SetColor(UIColor.FromName("BadColor"));
		}

		void Good(object sender, EventArgs e)
		{
			SetRating(1, Item, VC);
		}
		void Okay(object sender, EventArgs e)
		{
			SetRating(2, Item, VC);
		}
		void Bad(object sender, EventArgs e)
		{
			SetRating(3, Item, VC);
		}

		void SetRating(int rating, QualityReportItem item, UIViewController controller)
		{
			item.Rating = rating;
			if (rating > 1)
			{
				Alert.DisplayTextField("Beskriv problemet", "Beskrivelse...", (obj) => {
					item.Comment = obj;
                    ;

                    Alert.DisplayAlert(controller, "Tag et billede", "Dokumenter problemet med et billede", new List<(string, Action)> {
                        ("Tag billede", () => {
                            takeImage(item, controller);
                        }),
                        ("Nej", () => Update(item, controller))
                    });
				}, controller);
			}
			else
			{
                Update(item, controller);
			}
		}

        void takeImage(QualityReportItem item, UIViewController controller) {
            CameraContainerViewController.Start(null, controller, (array) =>
            {
                var imageMessage = new RentMessage.Image();
                var imageStream = array.AsStream();
                Alert.DisplayLoadingWhile(controller, async () => {
                    item.ImageLocation = await _storage.Upload(imageStream, "image");
                    Update(item, controller);
                    updateImage(item, controller);
                });

                //Post(imageMessage);
            }, null);
        }

		async Task Update(QualityReportItem item, UIViewController controller)
		{
            //item.CleaningTaskID = item.CleaningTask.ID;
            await repository.AddItem(item, () =>
            {
                setColor(item.Rating);
            }, () =>
            {
				item.Rating = 0;
				item.Comment = "";
            });
		}
    }
}
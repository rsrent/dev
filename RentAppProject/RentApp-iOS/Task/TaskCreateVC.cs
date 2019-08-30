using Foundation;
using System;
using UIKit;
using RentApp.Shared.Repositories;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using RentAppProject;
using System.Collections.Generic;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Image;

namespace RentApp
{
    public partial class TaskCreateVC : UITableViewController
    {
        private readonly FloorAreaRepository _floorAreaRepository = AppDelegate.ServiceProvider.GetService<FloorAreaRepository>();

        private readonly CleaningTasksRepository _cleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();
        private readonly QualityReportRepository _qualityReportRepository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();

        public TaskCreateVC (IntPtr handle) : base (handle) { }

        CleaningPlan plan;
        CleaningTask newTask;

        int? qualityReportID;
        int rating = 0;


        public void NewTask(int locationID, int? qualityReportID = null)
        {
            newTask = new CleaningTask { LocationID = locationID };
            this.qualityReportID = qualityReportID;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.RightNavigationButton("Tilføj", () =>
            {
                if (newTask.Area == null)
                {
                    this.DisplayToast("Et rengøringsområde skal vælges");
                    return;
                }

                if(string.IsNullOrEmpty(TimesYearly.Text) && string.IsNullOrEmpty(Frequency.Text))
                {
                    this.DisplayToast("Et rengøringsinterval skal vælges");
                    return;
                }

                if (!plan.HasFloors && !string.IsNullOrEmpty(Frequency.Text))
                {
                    System.Diagnostics.Debug.WriteLine(plan.Description + " kan ikke have en frekvenskode");
                }

                if (!string.IsNullOrEmpty(TimesYearly.Text))
                {
                    if(byte.TryParse(TimesYearly.Text, out var times))
                        newTask.TimesOfYear = times;
                    else
                    {
                        this.DisplayToast("Gange årligt er indtastet forkert");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(Frequency.Text))
                {
                    if(int.TryParse(Frequency.Text, out var fre))
                        newTask.Frequency = fre + "";
                    else 
                    {
                        this.DisplayToast("Frekvens er indtastet forkert");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(Size.Text))
                {
                    if(int.TryParse(Size.Text, out var size))
                        newTask.SquareMeters = size;
                    else 
                    {
                        this.DisplayToast("Størrelse er indtastet forkert");
                        return;
                    }
                }

				newTask.Comment = Comment.Text;

                if(qualityReportID != null && rating == 0)
                {
                    this.DisplayToast("Området er ikke bedømt endnu");
                    return;
                }

                _cleaningTasksRepository.Add(newTask.LocationID, newTask, async (createdTask) => 
                {
                    if(qualityReportID != null)
                    {
                        var qualityItem = new QualityReportItem
                        {
                            CleaningTask = createdTask,
                            Rating = rating,
                            QualityReportID = (int)qualityReportID,
                            CleaningTaskID = createdTask.ID
                        };

                        await _qualityReportRepository.AddItem(qualityItem,() => {
                            this.DisplayToast("Opgave tilføjet");
                        },() => {
                            this.DisplayToast("Opgave tilføjet, men blev ikke rated");
                        });
                    }
                    else {
                        this.DisplayToast("Opgave tilføjet");
                    }
                }).LoadingOverlay(this);

                System.Diagnostics.Debug.WriteLine("ALL OKAY");
            });

            Frequency.EditingChanged += (sender, e) => 
                TimesYearly.Text = "";

            TimesYearly.EditingChanged += (sender, e) => 
                Frequency.Text = "";

            RatingCell.Hidden = qualityReportID == null;

			Good.SetColor(UIColor.LightGray);
			Okay.SetColor(UIColor.LightGray);
            Bad.SetColor(UIColor.LightGray);

			Good.AddGestureRecognizer(new UITapGestureRecognizer(() => SetRating(1)));
			Okay.AddGestureRecognizer(new UITapGestureRecognizer(() => SetRating(2)));
            Bad.AddGestureRecognizer(new UITapGestureRecognizer(() => SetRating(3)));
        }

        void SetRating(int raiting)
        {
            this.rating = raiting;
			Good.SetColor(UIColor.LightGray);
			Okay.SetColor(UIColor.LightGray);
            Bad.SetColor(UIColor.LightGray);

            if (raiting == 1) Good.SetColor(UIColor.FromName("GoodColor"));
            if (raiting == 2) Okay.SetColor(UIColor.FromName("OkayColor"));
            if (raiting == 3) Bad.SetColor(UIColor.FromName("BadColor"));
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            TableView.DeselectRow(indexPath, true);
            var index = indexPath.Row;

            if(indexPath.Section == 0)
            {
                if (index == 0)
                {
                    _floorAreaRepository.Plans((plans) => {
                        this.Start<SimplePickerVC>().Setup("Vælg plantype", plans.Select(p => p.Description).ToArray(), (i) => {
                            plan = plans[i];
                            Plan.Text = plan.Description;
                            if (!plan.HasFloors)
                            {
                                newTask.Floor = null;
                                Floor.Text = "  ";
                            } 
                            if (newTask.Area != null && newTask.Area.CleaningPlanID != plan.ID)
                            {
                                newTask.Area = null;
                                Area.Text = "  ";
                            }

                            if(plan.ID == 1)
                            {
                                IntervalCell.Hidden = true;
                                FrequencyCell.Hidden = false;
                                TimesYearly.Text = "";
                            } else 
                            {
                                IntervalCell.Hidden = false;
                                FrequencyCell.Hidden = true;
                                Frequency.Text = "";
                            }

                        });
                    }).LoadingOverlay(this);
                }
                if (index == 1 && plan != null && plan.HasFloors)
                {
                    /*
                    _floorAreaRepository.Floors((floors) => {
                        
                    }).LoadingOverlay(this);
*/

                    var table = this.Start<SimpleTableVC>();
                    var floors = new List<Floor>();
                    table.Setup("Vælg en etage", async (update) => {
                        await _floorAreaRepository.Floors((fs) =>
                        {
                            floors = fs;
                            update(floors.Select(f => (f.Description, "", "")).ToList());
                        });
                        }, (i) =>
                        {
                            this.NavigationController.PopViewController(true);
                            newTask.Floor = floors[i];
                            Floor.Text = newTask.Floor.Description;
                        }
                    );
                    table.SetupViewPrep((SimpleTableVC obj) => {
                        obj.RightNavigationButton("Tilføj", () =>
                        {
                            this.DisplayTextField("Navngiv ny etage", "etage...", (custom) =>
                            {
                                _floorAreaRepository.AddFloor(new Floor { Description = custom }, (floor) =>
                                {
                                    table.TableController.ReloadTable();
                                }).LoadingOverlay(this, "Tilføjer etage");
                            });

                        });
                    });
                }

                if (index == 2 && plan != null)
                {
                    /*
                    _floorAreaRepository.Areas(plan.ID, (areas) => {
                        var table = this.Start<SimpleTableVC>();
                        table.Setup("Vælg en etage", areas.Select(f => (f.Description, "", "")).ToList(), (i) =>
                        {
                            this.NavigationController.PopViewController(true);
                            newTask.Area = areas[i];
                            Area.Text = newTask.Area.Description;
                        });

                        table.SetupViewPrep((SimpleTableVC obj) => {
                            obj.RightNavigationButton("Tilføj", () =>
                            {
                                this.DisplayTextField("Navngiv nyt område", "e.g. toilet", (custom) =>
                                {
                                    _floorAreaRepository.AddArea(plan.ID, new Area { Description = custom }, (area) =>
                                    {
                                        table.TableController.ReloadTable();
                                    }).LoadingOverlay(this, "Tilføjer område");
                                });
                            });
                        });
                    }).LoadingOverlay(this);
                    */

                    var table = this.Start<SimpleTableVC>();
                    var areas = new List<Area>();
                    table.Setup("Vælg et område", async (update) => {
                        await _floorAreaRepository.Areas(plan.ID, (ass) =>
                        {
                            areas = ass;
                            update(areas.Select(f => (f.Description, "", "")).ToList());
                        });
                    }, (i) =>
                    {
                        this.NavigationController.PopViewController(true);
                        newTask.Area = areas[i];
                        Area.Text = newTask.Area.Description;
                    });

                    table.SetupViewPrep((SimpleTableVC obj) => {
                        obj.RightNavigationButton("Tilføj", () =>
                        {
                            this.DisplayTextField("Navngiv nyt område", "e.g. toilet", (custom) =>
                            {
                                _floorAreaRepository.AddArea(plan.ID, new Area { Description = custom }, (area) =>
                                {
                                    table.TableController.ReloadTable();
                                }).LoadingOverlay(this, "Tilføjer område");
                            });
                        });
                    });
                }
            }
        }
    }
}
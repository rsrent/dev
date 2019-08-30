using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Chat;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using System.Collections.Generic;
using System.Linq;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Image;
using RentApp.Repository;
using Rent.Shared.ViewModels;
using Rent.Shared.Models;
using Rent.Shared.Repositories;

namespace RentApp
{
    public partial class LocationMenuVC : UIViewController
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        LogRepository _logRepository = AppDelegate.ServiceProvider.GetService<LogRepository>();
        MoreWorkRepository _moreworkRepository = AppDelegate.ServiceProvider.GetService<MoreWorkRepository>();

        CustomerVM _customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        LocationVM _locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();
        PermissionRepository _userRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();
        LocationRepository _locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();
        ChatRepository _chatRepository = AppDelegate.ServiceProvider.GetService<ChatRepository>();

        public LocationMenuVC (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();



            ImageView.Round();
            SetDetails();

            HourButton.TouchUpInside += (sender, e) => {
                this.Start<LocationHourVC>().Setup(_locationVM.Location.ID);
            };

            EconomyButton.TouchUpInside += (sender, e) => {
                this.Start<LocationEconomyVC>().Setup(_locationVM.Location.ID, null);
            };

            ChatButton.TouchUpInside += (sender, e) => {
                this.Start<ConversationsTableVC>().SetLocation(_locationVM.Location.ID);
            };
            ChatButton.Hidden = true;

            TeamButton.TouchUpInside += (sender, e) => {
                this.Start<EmployeeTableVC>().SetLoadSource_LocationUsers(_locationVM.Location, _customerVM.Customer);
            };

            CleaningPlanButton.TouchUpInside += (sender, e) => {
                AppDelegate.ServiceProvider.GetService<CleaningPlanVM>().Location = _locationVM.Location;
                this.Start<CleaningPlansTableVC>();
            };

            DocumentsButton.TouchUpInside += (sender, e) => {
                this.Start<DocumentsTableVC>().ParseInfo(null, null, _locationVM.Location.ID);
            };

            MoreWorkButton.TouchUpInside += (sender, e) =>
            {
                var vc = this.Start<SimpleTableVC>();
                List<MoreWork> loadedMorework = null;
                vc.Setup("Ekstraarbejde", (Action<ICollection<(string,string,string)>> update) =>
                {
                    _moreworkRepository.Get(_locationVM.Location.ID, (morework) =>
                    {
                        loadedMorework = morework.ToList();
                        update(morework.Select(l => 
                                               (l.Description + (l.Hours != null ? " - Færdig" : ""), 
                                                l.ExpectedCompletedTime.ToString("dd-MM-yy hh:mm") + (l.ActualCompletedTime != null ? ", udført " + l.ActualCompletedTime?.ToString("dd-MM-yy hh:mm") : ""),
                                                "")).ToList());
                    }).LoadingOverlay(vc);
                }, (i) =>
                {
                    if(loadedMorework[i].Hours == null)
                    {
                        if (_userVM.HasPermission(Permission.CompletedTask, Permission.CRUDD.Create))
                        {


                            this.DisplayMenu("Ekstraarbejde muligheder",
                                             new List<(string, Action)>() {
                            ("Udfør opgave", () => completedWork(loadedMorework[i], vc))
                            }, null);
                        }
                    }
                });
                vc.SetupViewPrep((obj) => {
                    obj.RightNavigationButton("Ny", () => {
                        this.Start<MoreWorkVC>().Setup(_locationVM.Location);
                    });
                });
            };

            LogButton.TouchUpInside += (sender, e) => {

                var vc = this.Start<SimpleTableVC>();
                List<LocationLog> loadedLogs = null; 
                vc.Setup("Logs", (Action<ICollection<(string,string,string)>> update) =>
                {
                    _logRepository.GetMany(_locationVM.Location.ID, (logs) =>
                    {
                        loadedLogs = logs.ToList();
                        update(logs.Select(l => (!String.IsNullOrEmpty(l.Title) ? l.Title : "Title", l.DateCreated.ToString("d"), "")).ToList());
                    }).LoadingOverlay(vc);
                }, (logIndex) =>
                {
                    this.Start<LogVC>().Setup(loadedLogs[logIndex]);
                });
                vc.SetupViewPrep((obj) => 
                {
                    obj.RightNavigationButton("Ny", () => {
                        _logRepository.Add(_locationVM.Location.ID, () => {
                            vc.TableController.ReloadTable();
                        }).LoadingOverlay(vc);
                    });
                });
            };

            OfficeMessageButton.TouchUpInside += (sender, e) => {
                this.DisplayTextField(
                    "Besked til kontor",
                    "Besked...",
                    (message) =>
                    {
                        _locationRepository.MailToOffice(_locationVM.Location, message, () =>
                        {
                            this.DisplayToast("Besked sendt");
                        }, () => {
                            this.DisplayToast("Fejl, prøv igen");
                        }).LoadingOverlay(this);
                    }
                );
            };

            if (_userVM.HasPermission(Permission.LOCATION, Permission.CRUDD.Update))
            {
                var options = new List<(string, Action)>();

                if (_userVM.HasPermission("Location", Permission.CRUDD.Update))
                    options.Add(("Rediger lokation", () => this.Start<AddLocationVC>().Set_EditLocation(_customerVM.Customer, _locationVM.Location)));

                if (_userVM.HasPermission("Location", Permission.CRUDD.Delete))
                    DeleteLocationOption(options);

                if (options.Count > 0)
                    this.RightNavigationButton("Rediger", (button) => this.DisplayMenu(_locationVM.Location.Name, options, button));
            }
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            SetDetails();
        }

        async void SetDetails()
        {
            await _locationRepository.Get(_locationVM.Location.ID, (l) => { _locationVM.Location = l; });

            var location = _locationVM.Location;

            TitleLabel.Text = "";
			if (_customerVM.Customer != null) TitleLabel.Text = _customerVM.Customer.Name + ", ";
            TitleLabel.Text += location.Name;
            if(location.ProjectNumber != null)
                TitleLabel.Text += "\n#" + location.ProjectNumber;

            Title = location.Name;
            this.AddNavigationStack();

            AddressLabel.Text = location.Address;

            if (_userVM.LoggedInUser().CustomerID == null && location.CustomerContact != null)
            {
                ContactLabel.Text = location.CustomerContact.FirstName + " " + location.CustomerContact.LastName + "\n" + location.CustomerContact.Email;
            } 
            else if (_userVM.LoggedInUser().CustomerID != null && location.ServiceLeader != null)
            {
                ContactLabel.Text = location.ServiceLeader.FirstName + " " + location.ServiceLeader.LastName + "\n" + location.ServiceLeader.Email;
            }

            if (_userVM.LoggedInUser().CustomerID != null)
            {
                CommentStack.Hidden = true;
                PhoneStack.Hidden = true;
                OfficeMessageButton.Hidden = true;
            }
            else
            {
                CommentLabel.Text = location.Comment;

                if(!string.IsNullOrWhiteSpace(location.Phone)) {
                    PhoneLabel.Text = location.Phone;
                    PhoneStack.Hidden = false;
                } else {
                    PhoneStack.Hidden = true;
                }
            }

            if (!_userVM.HasPermission(Permission.HOUR, Permission.CRUDD.Read)) HourButton.Hidden = true;
            else HourButton.Hidden = false;

            if (!_userVM.HasPermission(Permission.ECONOMY, Permission.CRUDD.Read)) EconomyButton.Hidden = true;
            else EconomyButton.Hidden = false;

            if (!_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Read)) TeamButton.Hidden = true;
            else TeamButton.Hidden = false;

            if (!_userVM.HasPermission(Permission.QualityReport, Permission.CRUDD.Read)) QualityReportButton.Hidden = true;
            else QualityReportButton.Hidden = false;

            if (!_userVM.HasPermission(Permission.DOCUMENT, Permission.CRUDD.Read))
                DocumentsButton.Hidden = true;
            else
                DocumentsButton.Hidden = false;

            LogButton.Hidden = _userVM.LoggedInUser().CustomerID != null;

        }

        async void TryLoadConversations() 
        {
            await _chatRepository.GetLocationConversations(_locationVM.Location.ID,(obj) => {
                if(obj.Count > 0)
                {
                    ChatButton.Hidden = false;
                } else {
                    ChatButton.Hidden = true;
                }
            });
        }


        void DeleteLocationOption(List<(string, Action)> options)
        {
            if (_locationVM.Location == null)
                return;

            if (_locationVM.Location.Disabled)
            {
                options.Add(("Aktiver lokalitet", () => {
                    this.DisplayAlert("Vil du genaktiverer denne lokalitet?", "Du er ved og aktiverer denne slettede lokalitet: " + _locationVM.Location.Name + ". Er du sikker på du vil fortsætte?", new List<(string, Action)> {
                        ("Ja", () => EnableLocation()),
                        ("Nej", () => {}),
                    });
                }
                ));
            }
            else
            {
                options.Add(("Slet lokalitet", () => {
                    this.DisplayAlert("Vil du slette denne lokalitet?", "Du er ved og slette lokaliteten: " + _locationVM.Location.Name + ". Er du sikker på du vil fortsætte?", new List<(string, Action)> {
                        ("Ja", () => DeleteLocation()),
                        ("Nej", () => {}),
                });
                }
                ));
            }
        }

        void DeleteLocation()
        {
            _locationRepository.Disable(_locationVM.Location.ID, () =>
            {
                _locationVM.Location.Disabled = true;
                this.DisplayToast("Lokalitet slettet");
                SetDetails();
            }).LoadingOverlay(this);
        }

        void EnableLocation()
        {
            _locationRepository.Enable(_locationVM.Location.ID, () =>
            {
                _locationVM.Location.Disabled = false;
                this.DisplayToast("Lokalitet gendannet");
                SetDetails();
            }).LoadingOverlay(this);
        }

        void completedWork(MoreWork moreWork, SimpleTableVC vc)
        {
            bool success = false;
            this.DisplayTextField("Antal brugte timer", "Tast ind hvor mange timer der blev brugt til at udfører opgaven", (text) =>
            {
                success = float.TryParse(text, out float hours);

                if (!success)
                {
                    this.DisplayAlert(
                    "Timer indtastet forker",
                    "Prøv igen",
                    new System.Collections.Generic.List<(string, Action)> {
                        ("Ok", () => {completedWork(moreWork, vc);}),
                        ("Nej", null),
                    }
                );
                }
                else
                {
                    _moreworkRepository.Complete(moreWork.ID, hours, () => {
                        vc.Reload();
                    }).LoadingOverlay(this);
                }
            });
        }

    }
}
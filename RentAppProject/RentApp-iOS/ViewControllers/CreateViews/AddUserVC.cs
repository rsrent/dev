using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Input;
using RentAppProject;
using ModuleLibraryiOS.Alert;
using RentApp.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using RentApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ModuleLibraryiOS.Camera;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Image;
using RentApp.Repository;

namespace RentApp
{
    /*
    public partial class AddUserVC : UIViewController
    {
        UserRepository _userRepository = AppDelegate.ServiceProvider.GetService<UserRepository>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();

        public AddUserVC (IntPtr handle) : base (handle) { }

        User User;
        NSData imageData;
        //string[] PickerOptions;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            ImageView.Round();
            if(User != null) {
                FirstNameTF.Text = User.FirstName;
                LastNameTF.Text = User.LastName;
                EmailTF.Text = User.Email;
                CommentTV.Text = User.Comment;
                PhoneTF.Text = User.Phone;
                LoadImage();
            }

            ImageButton.TouchUpInside += (sender, e) => {
                this.Start<CameraContainerViewController>("Camera").EnableImage((data) =>
                {
                    imageData = data;
                    ImageView.Image = new UIImage(imageData);
                });
            };
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TabBarController.TabBar.Hidden = false;
        }

        async void LoadImage() {
            if (!string.IsNullOrEmpty(User.ImageLocation))
            {
                ImageView.Image = await _storage.DownloadImage(User.ImageLocation, 200);
            }
        }



        public void Set_NewUser(string title, Action<User> preCreationPrep, Func<User, Task> successAction, Role role = null)
        {
            Title = title;

            this.RightNavigationButton("Tilføj", async () => {
                var newUser = await GetUser();
                if (newUser == null)
                    return;
                preCreationPrep(newUser);

                var userLoginInfo = new UserLoginInfo();
                userLoginInfo.User = newUser;

                if (role != null)
                {
                    //newUser.Role = role;
                    //newUser.RoleID = role.ID;
                    this.Start<AddLoginInformationVC>().SetUser(userLoginInfo, successAction);
                }
                else
                {
                    this.Start<SelectRoleVC>().ParseUser(userLoginInfo, successAction);
                }
            });
        }



        public void Set_EditUser(User user) {
            User = user;
            Title = user.FirstName;
            
			this.RightNavigationButton("Update", () => {
                this.DisplayLoadingWhile(UpdateUser);
			});
        }

        async Task UpdateUser() {
            var userToUpdate = await GetUser(User);
            if (userToUpdate == null)
                return;
            await _userRepository.Update(userToUpdate, async () => { 
                Alert.DisplayToast("User updated", this);
                await Task.Delay(500);
                this.NavigationController.PopViewController(true);
            });
        }

        async Task<User> GetUser(User user = null) {
            
            if(user == null)
                user = new User();

            user.FirstName = FirstNameTF.Text;
            user.LastName = LastNameTF.Text;
            user.Email = EmailTF.Text;
            user.Comment = CommentTV.Text;
            user.Phone = PhoneTF.Text;

            if (string.IsNullOrEmpty(user.FirstName)) {
                this.DisplayToast("Fornavn skal udfyldes");
                return null;
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                this.DisplayToast("Efternavn skal udfyldes");
                return null;
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                this.DisplayToast("Email skal udfyldes");
                return null;
            }
            if(!IsValidEmail(user.Email)) {
                this.DisplayToast("Indtast venligst gyldig email");
                return null;
            }

            if (imageData != null)
            {
                var imageStream = imageData.AsStream();
                user.ImageLocation = await _storage.Upload(imageStream, "image", user.ImageLocation);
            }

            return user;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
    */
}
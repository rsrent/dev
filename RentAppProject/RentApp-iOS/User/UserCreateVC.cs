using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.Camera;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ModuleLibraryiOS.Alert;
using RentApp.Repository;
using ModuleLibraryiOS.Navigation;
using System.Threading.Tasks;

namespace RentApp
{
    public partial class UserCreateVC : UITableViewController
    {
        public UserCreateVC(IntPtr handle) : base(handle) { }

        RoleRepository _roleRepository = AppDelegate.ServiceProvider.GetService<RoleRepository>();
        UserRepository _userRepository = AppDelegate.ServiceProvider.GetService<UserRepository>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        CustomerRepository _customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
        User _user;

        //int? customerID;
        int? roleID;
        Action<User> completed;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            FirstName.Text = _user.FirstName;
            LastName.Text = _user.LastName;
            Email.Text = _user.Email;
            Phone.Text = _user.Phone;
            Description.Text = _user.Comment;
            EmployeeNumber.Text = _user.EmployeeNumber + "";
            _storage.DownloadImage((image) => Image.Image = image, _user.ImageLocation, 300);

            if (roleID != null)
            {
                _roleRepository.GetRoles((roles) =>
                {
                    Role.Text = roles.FirstOrDefault(r => r.ID == roleID).Name;
                    _user.RoleID = roleID;
                }).LoadingOverlay(this);
            }

            if (_user.CustomerID != null)
            {
                UserCellUserNumber.Hidden = true;
                _customerRepository.GetName((int)_user.CustomerID, (name) =>
                {
                    Customer.Text = name;
                });
            }

            //

            if(_user.RoleID == 8 || _user.RoleID == 9)
            {
                CustomerCell.Hidden = false;
            } else {
                CustomerCell.Hidden = true;
                _user.CustomerID = null;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            TableView.DeselectRow(indexPath, true);
            System.Diagnostics.Debug.WriteLine(indexPath.Row);
            if (indexPath.Row == 0 && indexPath.Section == 0)
            {
                this.Start<CameraContainerViewController>("Camera").EnableImage((data) =>
                {
                    Image.Image = new UIImage(data);
                    _user.ImageArray = data.ToArray();
                });
            }

            if (indexPath.Row == 1 && indexPath.Section == 2 && roleID == null)
            {
                _roleRepository.GetRoles((roles) =>
                {
                    roles = roles.Where(r => r.ID != 1).ToList();
                    this.Start<SimplePickerVC>().Setup("Vælg brugerrolle", roles.Select(r => r.Name).ToArray(), (index) =>
                    {
                        Role.Text = roles.ToList()[index].Name;
                        _user.RoleID = roles.ToList()[index].ID;

                        if (_user.RoleID == 8 || _user.RoleID == 9)
                        {
                            CustomerCell.Hidden = false;
                        }
                        else
                        {
                            CustomerCell.Hidden = true;
                            _user.CustomerID = null;
                        }
                    });
                }).LoadingOverlay(this);
            }

            if(indexPath.Row == 2 && indexPath.Section == 2)
            {
                _customerRepository.GetAll((customers) =>
                {
                    this.Start<SimpleTableVC>().Setup("Vælg kunde som brugeren hører til", customers.Select(c => (c.Name, "", "")).ToList(), (index) =>
                    {
                        _user.CustomerID = customers[index].ID;
                        Customer.Text = customers[index].Name;
                        this.NavigationController.PopViewController(true);
                    });
                }).LoadingOverlay(this);
            }
        }

        public void SetEditUser(User user)
        {
            _user = user;
            roleID = user.RoleID;
            this.RightNavigationButton("Update", async () =>
            {
                if(Verified())
                {
                    await _userRepository.Update(await GetUser(), async () =>
                    {
                        Alert.DisplayToast("User updated", this);
                        await Task.Delay(500);
                        this.NavigationController.PopViewController(true);
                    });
                }
            });
        }

        public void NewUser(int? customerID = null, int? roleID = null, Action<User> completed = null)
        {
            //this.customerID = customerID;
            this.roleID = roleID;
            this.completed = completed;

            _user = new User { CustomerID = customerID };

            this.RightNavigationButton("Opret", async () =>
            {
                if(Verified())
                {
                    var usr = await GetUser();
                    usr.CustomerID = customerID;

                    this.Start<AddLoginInformationVC>().SetUser(new UserLoginInfo { User = usr }, async (newUser) =>
                    {
                        completed?.Invoke(newUser);

                        if (_user.ImageArray != null)
                        {
                            newUser.ImageLocation = await _storage.Upload(NSData.FromArray(_user.ImageArray).AsStream(), "image", "user_image_" + newUser.ID);
                            await _userRepository.Update(newUser, () => { });
                        }

                        Alert.DisplayToast("User updated", this);
                        await Task.Delay(500);
                        try {
                            this.NavigationController.PopViewController(true);
                            this.NavigationController.PopViewController(true);
                        } catch { }
                    });
                }
            });
        }

        bool Verified()
        {
            if (string.IsNullOrEmpty(FirstName.Text))
            {
                this.DisplayToast("Fornavn skal udfyldes");
                return false;
            }
            if (string.IsNullOrEmpty(LastName.Text))
            {
                this.DisplayToast("Efternavn skal udfyldes");
                return false;
            }
            if (string.IsNullOrEmpty(Email.Text))
            {
                this.DisplayToast("Email skal udfyldes");
                return false;
            }
            if (!IsValidEmail(Email.Text))
            {
                this.DisplayToast("Indtast venligst gyldig email");
                return false;
            }

            if (_user.RoleID == null)
            {
                this.DisplayToast("Ingen rolle valgt");
                return false;
            }

            return true;
        }

        async Task<User> GetUser() {
            _user.FirstName = FirstName.Text;
            _user.LastName = LastName.Text;
            _user.Email = Email.Text;
            _user.Phone = Phone.Text;
            _user.Comment = Description.Text;
            if (int.TryParse(EmployeeNumber.Text, out var num))
                _user.EmployeeNumber = num;

            if(_user.ImageArray != null && _user.ID > 0)
            {
                _user.ImageLocation = await _storage.Upload(NSData.FromArray(_user.ImageArray).AsStream(), "image", "user_image_" + _user.ID);
            }
            return _user;
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
}
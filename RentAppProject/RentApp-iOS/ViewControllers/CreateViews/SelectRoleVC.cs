using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Input;
using RentAppProject;
using System.Threading.Tasks;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Generic;

namespace RentApp
{
    /*
    public partial class SelectRoleVC : UIViewController
    {
        RoleRepository roleRepository = AppDelegate.ServiceProvider.GetService<RoleRepository>();

        public SelectRoleVC (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            GetRoles();
        }

        List<Role> Roles;
        async void GetRoles() {
            await roleRepository.GetRoles((roles) => {
                Roles = roles.ToList();
                Picker.Model = new PickerModel(new[] { Roles.Select(r => r.Name).ToArray() });
            });
        }

        public void ParseUser(UserLoginInfo userInfo, Func<User, Task> successAction) {

            this.RightNavigationButton("Add", () => {
                //userInfo.TemplateRole = Role.EmployeeRoles[Picker.SelectedRowInComponent(0)];
                //userInfo.User.Role = Roles[(int) Picker.SelectedRowInComponent(0)];



                this.Start<AddLoginInformationVC>().SetUser(userInfo, successAction);
            });
        }
    }
    */
}
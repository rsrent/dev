using Foundation;
using System;
using UIKit;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using static RentAppProject.Permission;

namespace RentApp
{
    public partial class UserPermissionCell : UITableViewCell
    {
        PermissionRepository _permissionRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();
        bool actualValue;
        public UserPermissionCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell((UserPermission, CRUDD) userPermission) {
            CRUDDLabel.Text = userPermission.Item2.ToString();
            var up = userPermission.Item1;
            UP = up;
            switch(userPermission.Item2) {
                case CRUDD.Create:
                    ActiveSwitch.On = up.Create;
                    Crudd = CRUDD.Create;
                    //Update(up, CRUDD.Create);
                    break;
                case CRUDD.Read:
                    ActiveSwitch.On = up.Read;
                    Crudd = CRUDD.Read;
                    //Update(up, CRUDD.Read);
                    break;
                case CRUDD.Update:
                    ActiveSwitch.On = up.Update;
                    Crudd = CRUDD.Update;
                    //Update(up, CRUDD.Update);
                    break;
                case CRUDD.Delete:
                    ActiveSwitch.On = up.Delete;
                    Crudd = CRUDD.Delete;
                    //Update(up, CRUDD.Delete);
                    break;
            }
            Update();
            actualValue = ActiveSwitch.On;
        }


        UserPermission UP;
        CRUDD Crudd;
        void Update() {
            /*
            ActiveSwitch.TouchUpInside +=  async (sender, e) => {
                await _userRepository.UpdateUserPermission(up, crudd, ActiveSwitch.On,
                () => { 
                    actualValue = !actualValue;
                    ActiveSwitch.On = actualValue;
                }, () => {
                    ActiveSwitch.On = actualValue;
                });
            };
*/
            try {
                ActiveSwitch.TouchUpInside -= touchMethod;
            } catch {
                
            }
            ActiveSwitch.TouchUpInside += touchMethod;


        }

        async void touchMethod(object obj, EventArgs e) {
            await _permissionRepository.UpdateUserPermission(UP, Crudd, ActiveSwitch.On,
            () => {
                actualValue = !actualValue;
                ActiveSwitch.On = actualValue;

                switch (Crudd)
                {
                    case CRUDD.Create:
                        UP.Create = actualValue;
                        break;
                    case CRUDD.Read:
                        UP.Read = actualValue;
                        break;
                    case CRUDD.Update:
                        UP.Update = actualValue;
                        break;
                    case CRUDD.Delete:
                        UP.Delete = actualValue;
                        break;
                }
            }, () => {
                ActiveSwitch.On = actualValue;
            });
        }
    }
}
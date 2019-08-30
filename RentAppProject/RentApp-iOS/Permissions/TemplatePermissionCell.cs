using Foundation;
using System;
using UIKit;
using Microsoft.Extensions.DependencyInjection;
using static RentAppProject.Permission;
using RentAppProject;

namespace RentApp
{
    public partial class TemplatePermissionCell : UITableViewCell
    {
        PermissionRepository _permissionRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();
        bool actualValue;

        public TemplatePermissionCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell((PermissionTemplate, CRUDD) userPermission)
        {
            CRUDDLabel.Text = userPermission.Item2.ToString();
            var up = userPermission.Item1;
            PT = up;
            switch (userPermission.Item2)
            {
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


        PermissionTemplate PT;
        CRUDD Crudd;
        void Update()
        {
            try
            {
                ActiveSwitch.TouchUpInside -= touchMethod;
            }
            catch
            {

            }
            ActiveSwitch.TouchUpInside += touchMethod;


        }

        async void touchMethod(object obj, EventArgs e)
        {
            await _permissionRepository.UpdatePermissionTemplate(PT, Crudd, ActiveSwitch.On,
            () => {
                actualValue = !actualValue;

                switch(Crudd)
                {
                    case CRUDD.Create :
                        PT.Create = actualValue;
                        break;
                    case CRUDD.Read:
                        PT.Read = actualValue;
                        break;
                    case CRUDD.Update:
                        PT.Update = actualValue;
                        break;
                    case CRUDD.Delete:
                        PT.Delete = actualValue;
                        break;
                }

                ActiveSwitch.On = actualValue;
            }, () => {
                ActiveSwitch.On = actualValue;
            });
        }
    }
}
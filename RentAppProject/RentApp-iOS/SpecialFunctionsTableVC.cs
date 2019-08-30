using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.ViewControllerInstanciater;
using static RentAppProject.Permission;
using RentAppProject;
using ModuleLibraryiOS.Alert;
using RentApp.ViewModels;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class SpecialFunctionsTableVC : ITableAndSourceViewController<(string, Action)>
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();

        PermissionRepository _permissionRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();
        RoleRepository _roleRepository = AppDelegate.ServiceProvider.GetService<RoleRepository>();


        public SpecialFunctionsTableVC (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableAndSourceController<SpecialFunctionsTableVC, (string, Action)>.Start(this);
        }

        public override UITableViewCell GetCell(NSIndexPath path, (string, Action) val)
        {
            return Table.StartCell<SpecialFunctionsCell>((obj) => obj.TextLabel.Text = val.Item1);
        }

        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<ICollection<(string, Action)>> updateAction)
        {
            var specialFunctions = new List<(string, Action)>();

            if(_userVM.HasPermission(Permission.LOGIN, CRUDD.Update)) 
            {
                specialFunctions.Add(("Opdater brugers kodeord", () => this.Start<UpdatePasswordVC>().ParseInfo(true)));
            }

            if (_userVM.HasPermission(Permission.PERMISSION, CRUDD.Update))
            {
                specialFunctions.Add(("Roller", () => this.Start<RolesTableVC>()));
            }





            if (_userVM.HasPermission(Permission.PERMISSION, CRUDD.Create))
            {
                specialFunctions.Add(("Rettigheder", () => this.Start<PermissionTableVC>()));
            }
            /*
            if (_userVM.HasPermission(Permission.PERMISSION, CRUDD.Create))
            {
                specialFunctions.Add(("Lav ny rolle", () => {
                    this.DisplayTextField("Navngiv rolle", "Navngiv den nye role", (obj) => {
                        _roleRepository.AddRole(new Role { Name = obj}, () =>
                        {

                        }).LoadingOverlay(this);
                    });
                }
                ));
            }  */


            updateAction.Invoke(specialFunctions);
        }

        public override void RowSelected(NSIndexPath path, (string, Action) val)
        {
            val.Item2.Invoke();
        }
    }
}
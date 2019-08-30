using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static RentAppProject.Permission;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;

namespace RentApp
{
    public partial class UserPermissionsTableVC : ITableAndSourceGroupedViewController<(UserPermission, CRUDD)>
    {

        PermissionRepository _permissionRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();
        public UserPermissionsTableVC (IntPtr handle) : base (handle) { }

        User User;

        public void ParseInfo(User user) {
            User = user;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableAndSourceGroupedController<UserPermissionsTableVC, (UserPermission, CRUDD)>.Start(this);

            Title = User.FirstName + " rettigheder";

            this.RightNavigationButton("Reset", () =>
            {
                _permissionRepository.ResetUserPermissions(User.ID, () => {
                    this.DisplayToast("Rettigheder nulstillet");
                }).LoadingOverlay(this, "Nulstil rettigheder");
            });
        }

        public override UITableViewCell GetCell(NSIndexPath path, (UserPermission, CRUDD) val)
        {
            return Table.StartCell<UserPermissionCell>((obj) => { obj.UpdateCell(val); });
        }

        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<Dictionary<string, List<(UserPermission, CRUDD)>>> updateAction)
        {
            await _permissionRepository.GetUserPermissions(User.ID, (list) =>
            {
                var dic = new Dictionary<string, List<(UserPermission, CRUDD)>>();

                foreach (var up in list)
                {

                    if (!dic.ContainsKey(up.Name))
                    {
                        dic.Add(up.Name, new List<(UserPermission, CRUDD)>());
                    }

                    dic[up.Name].Add((up, CRUDD.Create));
                    dic[up.Name].Add((up, CRUDD.Read));
                    dic[up.Name].Add((up, CRUDD.Update));
                    dic[up.Name].Add((up, CRUDD.Delete));
                }
                updateAction.Invoke(dic);
            });
        }
    }
}
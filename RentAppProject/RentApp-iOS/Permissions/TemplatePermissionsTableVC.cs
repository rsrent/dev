using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using static RentAppProject.Permission;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModuleLibraryiOS.Alert;

namespace RentApp
{
    public partial class TemplatePermissionsTableVC : ITableAndSourceGroupedViewController<(PermissionTemplate, CRUDD)>
    {
        PermissionRepository _permissionRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();

        Role Role;

        public TemplatePermissionsTableVC(IntPtr handle) : base(handle)
        {
        }

        public void ParseInfo(Role role)
        {
            Role = role;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableAndSourceGroupedController<TemplatePermissionsTableVC, (PermissionTemplate, CRUDD)>.Start(this);
        }

        public override UITableViewCell GetCell(NSIndexPath path, (PermissionTemplate, CRUDD) val)
        {
            return Table.StartCell<TemplatePermissionCell>((obj) => { obj.UpdateCell(val); });
        }

        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<Dictionary<string, List<(PermissionTemplate, CRUDD)>>> updateAction)
        {
            await _permissionRepository.GetPermissionTemplates(Role.ID, (list) =>
            {
                var dic = new Dictionary<string, List<(PermissionTemplate, CRUDD)>>();

                foreach (var up in list)
                {

                    if (!dic.ContainsKey(up.Name))
                    {
                        dic.Add(up.Name, new List<(PermissionTemplate, CRUDD)>());
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
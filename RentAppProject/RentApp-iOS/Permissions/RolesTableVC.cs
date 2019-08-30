using Foundation;
using System;
using UIKit;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Table;
using RentAppProject;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace RentApp
{
    public partial class RolesTableVC : ITableAndSourceViewController<Role>
    {
        RoleRepository roleRepository = AppDelegate.ServiceProvider.GetService<RoleRepository>();

        public RolesTableVC (IntPtr handle) : base (handle) { }

        TableAndSourceController<RolesTableVC, Role> TableController;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableController = TableAndSourceController<RolesTableVC, Role>.Start(this);
        }

        public override UITableView GetTable()
        {
            return TableView;
        }

        public override UITableViewCell GetCell(NSIndexPath path, Role val)
        {
            return TableFunctions.InstanciateCell<RoleCell>(TableView, "RoleCell", (cell) => { cell.UpdateCell(val); });
        }

        public override async Task RequestTableData(Action<ICollection<Role>> updateAction)
        {
            await roleRepository.GetRoles((list) => { updateAction.Invoke(list); });
        }

        public override void RowSelected(NSIndexPath path, Role val)
        {
            this.Start<TemplatePermissionsTableVC>().ParseInfo(val);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TableController.ReloadTable();
        }
    }
}
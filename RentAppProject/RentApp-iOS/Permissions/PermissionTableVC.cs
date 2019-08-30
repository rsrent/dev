using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using RentAppProject;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class PermissionTableVC : ITableAndSourceViewController<Permission>
    {
        PermissionRepository _permissionRepository = AppDelegate.ServiceProvider.GetService<PermissionRepository>();

        public PermissionTableVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableAndSourceController<PermissionTableVC, Permission>.Start(this);

            Title = "Rettigheder";

            this.RightNavigationButton("Tilføj ny", () =>
            {
                this.DisplayTextField("Navngiv rettighed", "Navngiv den nye rettighed", (obj) =>
                {
                    _permissionRepository.CreatePermission(new Permission { Name = obj }, () =>
                    {

                    }).LoadingOverlay(this);
                });
            });
        }

        public override UITableViewCell GetCell(NSIndexPath path, Permission val)
        {
            return TableView.StartCell<PermissionCell>((cell) => cell.TextLabel.Text = val.Name);
        }

        public override UITableView GetTable() => TableView;

        public override async Task RequestTableData(Action<ICollection<Permission>> updateAction)
        {
            await _permissionRepository.GetPermissions((permissions) => {
                updateAction.Invoke(permissions);
            });
        }

        public override void RowSelected(NSIndexPath path, Permission val)
        {
            this.DisplayAlert("Gendand rettighed", "Ved at trykke ja, gendanner du alle brugeres rettigheder for \"" + val.Name + "\". Det vil sige at de, for denne rettighed, vil få tilladelser beskrevet til deres rolle."
                              , new List<(string, Action)>() {
                ("Ja", () => { ResetPermissions(val); }),
                ("Nej", () => { })
            });
        }

        void ResetPermissions(Permission val) {
            _permissionRepository.ResetUsersSpecificPermissions(val.ID, () => {
                this.DisplayToast("Rettighed opdateret for alle brugere");
            }).LoadingOverlay(this);
        }
    }
}
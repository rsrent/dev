using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentApp.Shared.Models.Document;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using RentApp.Shared.Repositories;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;
using RentApp.Other;
using RentAppProject;
using RentApp.Repository;
using ModuleLibraryiOS.Camera;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class DocumentsTableVC : ITableAndSourceViewController<Document>
    {
        public DocumentsTableVC (IntPtr handle) : base (handle) { }

        LocationVM _locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();
        DocumentRepository _documentRepository = AppDelegate.ServiceProvider.GetService<DocumentRepository>();
        StorageRepository _storageRepository = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();

        int? _customerID;
        int? _locationID;
        Document Document;
        int DocumentFolderID = 0;

        public void ParseInfo(Document document, int? customerID, int? locationID) {
            Document = document;
            _customerID = customerID;
            _locationID = locationID;
        }

        TableAndSourceController<DocumentsTableVC, Document> Controller;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Controller = TableAndSourceController<DocumentsTableVC, Document>.Start(this);

            var options = new List<(string, Action)>();

            Title = "Dokumenter";
            if (Document != null)
                Title = Document.Title;
            this.AddNavigationStack();

            if(_userVM.HasPermission(Permission.DOCUMENT, Permission.CRUDD.Update)) 
            {
                options.Add(("Tilføj mappe", () => {
                    this.DisplayTextField("Tilføj mappe", "Mappens navn...", (obj) =>
                    {
                        _documentRepository.AddFolder(DocumentFolderID, obj, () => {
                            Controller.ReloadTable();
                        }).LoadingOverlay(this);
                    });
                }));

                options.Add(("Tilføj billede", () => {
                    CameraContainerViewController.Start(null, this, async (array) =>
                    {
                        await _storageRepository.Upload(array.AsStream(), DocumentFolderID + "-folder", DateTime.Now.ToString("s") + ".png");
                        Controller.ReloadTable();
                    }, null);
                }
                ));


            } 

            if(options.Count > 0) {
                this.RightNavigationButton("Rediger", (btn) =>
                {
                    this.DisplayMenu("Rediger folder", options, btn);
                });
            }

            /*
            this.RightNavigationButton("Rediger", () =>
            {
                this.DisplayTextField("Tilføj mappe", "Mappens navn...", (obj) =>
                {
                    _documentRepository.AddFolder(Document.ID, obj, () => {

                    }).LoadingOverlay(this);
                });
            });
            /*
            AddImageButton.TouchUpInside += (sender, e) => 
            {
                CameraContainerViewController.Start(null, this, async (array) =>
                {
                    await _storageRepository.Upload(array.AsStream(), DocumentFolderID + "-folder", DateTime.Now.ToString("s")+".png");
                    Controller.ReloadTable();
                }, null);
            }; */
        }

        public override UITableViewCell GetCell(NSIndexPath path, Document val) => 
            Table.StartCell<DocumentCell>(c => c.UpdateCell(val));
        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<ICollection<Document>> updateAction)
        {
            
            if(Document == null) {

                if(_customerID != null)
                {
                    await _documentRepository.GetForCustomer((int) _customerID, (obj) =>
                    {
                        //Title = obj.Title;
                        /*
                        var items = await _storageRepository.GetFiles(DocumentFolderID + "-folder");
                        foreach (var item in items)
                            obj.Documents.Add(item); */
                        updateAction.Invoke(obj.Documents);
                    }, new DocumentConverter());
                } else if(_locationID != null)
                {
                    await _documentRepository.GetForLocation((int)_locationID, (obj) =>
                    {
                        //Title = obj.Title;
                        /*
                        var items = await _storageRepository.GetFiles(DocumentFolderID + "-folder");
                        foreach (var item in items)
                            obj.Documents.Add(item); */
                        updateAction.Invoke(obj.Documents);
                    }, new DocumentConverter());
                }



            } else {
                DocumentFolderID = Document.ID;

                await _documentRepository.GetForFolder(DocumentFolderID, async (obj) =>
                {
                    //Title = obj.Title;
                    //Document = obj;

                    var items = await _storageRepository.GetFiles(DocumentFolderID + "-folder");
                    foreach (var item in items)
                        obj.Documents.Add(item);
                    updateAction.Invoke(obj.Documents);
                }, new DocumentConverter());
            }
        }

        public override async void RowSelected(NSIndexPath path, Document val)
        {
            if(val.GetType() == typeof(DocumentFolder)) {
                var folder = val as DocumentFolder;
                this.Start<DocumentsTableVC>().ParseInfo(folder, _customerID, _locationID);
            } else if (val.GetType() == typeof(DocumentItem)){
                var item = val as DocumentItem;
                LoadDocument(item).LoadingOverlay(this);
            }
        }

        async Task LoadDocument(DocumentItem item) {
            var array = await _storageRepository.DownloadWithTemporaryStorage(item.Title, DocumentFolderID + "-folder");

            if(item.Title.Contains(".png")) {
                this.Start<ImageVC>().ParseInfo(array);
            } else if(item.Title.Contains(".pdf")) {
				this.Start<PDFVC>().ParseInfo(array);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TabBarController.TabBar.Hidden = false;
        }
    }
}
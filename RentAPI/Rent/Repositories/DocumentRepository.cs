using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rent.Data;
using Rent.DTOs;
using Rent.Helpers;
using Rent.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Rent.Repositories
{
    public class DocumentRepository
    {
        RentContext _context;
        IStorage _storage;

        public DocumentRepository(RentContext context, IStorage storage)
        {
            _context = context;
            _storage = storage;
        }

        public async Task<DocumentDTO> Get(int userID, int folderID)
        {
            return await GetPermissioned(folderID, userID);
        }

        public async Task<DocumentDTO> GetForLocation(int requesterID, int locationID) {

            var location = await _context.Location.FindAsync(locationID);
            if (location == null)
                return null;

            var customer = await _context.Customer.FindAsync(location.CustomerID);
            if (customer == null)
                return null;

            var potentialFolderIDs = new List<int>();

            potentialFolderIDs.AddRange(_context.DocumentFolder
                                        .Where(d => (d.Standard && customer.HasStandardFolders))
                                        .Select(d => d.ID)
                                        .ToList());

            potentialFolderIDs.AddRange(new int[] {
                customer.GeneralFolderID,
                customer.PrivateFolderID
            });

            var requester = _context.User.Find(requesterID);

            if(requester.RoleID != 8 && requester.RoleID != 9)
            {
                potentialFolderIDs.AddRange(new int[] {
                location.GeneralFolderID,
                location.CleaningplanFolderID,
                (int)location.CleaningFolderID,
                (int)location.WindowFolderID,
                (int)location.FanCoilFolderID,
                (int)location.PeriodicFolderID
            });
            }

            return 
                new DocumentDTO.Folder { 
                Documents = await GetPermissioned(potentialFolderIDs, requesterID),
                Title = "Lokationsmapper", Editable = false
            };
        }

        public async Task<DocumentDTO> GetForCustomer(int userID, int customerID)
        {
            var customer = await _context.Customer.FindAsync(customerID);
            if (customer == null)
                return null;
            
            var potentialFolderIDs = new List<int>();

            potentialFolderIDs.AddRange(new int[] {
                customer.GeneralFolderID,
                customer.PrivateFolderID,
                customer.ManagementFolderID,
            });

            return
                new DocumentDTO.Folder
                {
                    Documents = await GetPermissioned(potentialFolderIDs, userID),
                    Title = "Kundemapper",
                    Editable = false
                };
        }

        public async Task<bool> RemoveFolder(int userID, DocumentFolder folder)
        {
            if (await HasEditPermission(folder.ID, userID))
            {
                var children = _context.DocumentFolder.Where(d => d.ParentDocumentFolderID == folder.ID);

                foreach (var c in children)
                    await RemoveFolder(userID, c);

                await _storage.DeleteContainerAsync(folder.ID + "-folder");

                _context.DocumentFolder.Remove(folder);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private async Task<List<DocumentDTO>> GetPermissioned(ICollection<int> potentialFolderIDs, int userID)
        {
            var permissionedCollection = new List<DocumentDTO>();
            foreach (var folderID in potentialFolderIDs)
                permissionedCollection.Add(await GetPermissioned(folderID, userID));
            return permissionedCollection.Where(d => d != null).ToList();
        }

        private async Task<DocumentDTO> GetPermissioned(int folderID, int userID)
        {
            if (await HasReadPermission(folderID, userID))
                return await GetDocument(folderID, userID);
            return null;
        }

        public async Task<int> AddStandardFolder(int userID, string title)
        {
            var folderID = await AddFolder(new DocumentFolder { Title = title, Standard = true, HasParentPermissions = false }, userID);
            var success = await AddFolderPermissions(folderID, null, new[] { 1, 2 });
            return folderID;
        }

        public async Task<int> AddFolder(int userID, int? rootFolderID, string title, bool removable, int[] ReadRoles = null, int[] EditRoles = null)
        {
            var folderID = await AddFolder(new DocumentFolder { ParentDocumentFolderID = rootFolderID, Title = title, Standard = false, Removable = removable, HasParentPermissions = rootFolderID != null }, userID);
            var success = await AddFolderPermissions(folderID, ReadRoles, EditRoles);
            return folderID;
        }

        private async Task<int> AddFolder(DocumentFolder folder, int userID)
        {
            if (folder.ParentDocumentFolderID == null || folder.ParentDocumentFolderID != null && await HasEditPermission((int)folder.ParentDocumentFolderID, userID))
            {
                await _context.DocumentFolder.AddAsync(folder);
                await _context.SaveChangesAsync();
                var containerName = folder.ID + "-folder";
                await _storage.CreateContainer(containerName);
                return folder.ID;
            }
            return 0;
        }

        private async Task<bool> AddFolderPermissions(int folderID, int[] ReadRoles = null, int[] EditRoles = null)
        {
            var folder = await _context.DocumentFolder.FindAsync(folderID);
            if (folder == null)
                return false;
            if (folder.HasParentPermissions)
                return true;
            var documentPermissions = _context.FolderPermission.Where(fp => fp.FolderID == folder.ID).ToList();
            if (documentPermissions.Count() > 0)
                return false;

            documentPermissions = new List<FolderPermission>();
            foreach (var role in _context.Role)
            {
                documentPermissions.Add(
                    new FolderPermission
                    {
                        FolderID = folder.ID,
                        RoleID = role.ID,
                        Read = ReadRoles == null || ReadRoles.Contains(role.ID),
                        Edit = EditRoles == null || EditRoles.Contains(role.ID)
                    });
            }
            await _context.FolderPermission.AddRangeAsync(documentPermissions);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasReadPermission(int folderID, int userID)
        {
            var user = await _context.User.FindAsync(userID);
            return (await GetPermissions(folderID)).Any(fp => fp.RoleID == user.RoleID && fp.Read);
        }

        public async Task<bool> HasEditPermission(int folderID, int userID)
        {
            var user = await _context.User.FindAsync(userID);
            return (await GetPermissions(folderID)).Any(fp => fp.RoleID == user.RoleID && fp.Edit);
        }

        private async Task<ICollection<FolderPermission>> GetPermissions(int folderID) {
            DocumentFolder folder = _context.DocumentFolder.Find(folderID);
            if (folder == null)
                return new List<FolderPermission>();
            if (folder.HasParentPermissions)
                return await GetPermissions((int) folder.ParentDocumentFolderID);
            return  _context.FolderPermission.Where(fp => fp.FolderID == folderID).ToList();
        }

        private async Task<DocumentDTO> GetDocument(int folderID, int userID) {
            return await GetDocument(_context.DocumentFolder.Find(folderID), userID);
        }

        private async Task<DocumentDTO> GetDocument(DocumentFolder folder, int userID)
        {
            if(folder == null) {
                return null;
            }
            
            var documents = new List<DocumentDTO>();

            var editable = await HasEditPermission(folder.ID, userID);

            var subFolders =
                _context.DocumentFolder
                .Where(subFolder => subFolder.ParentDocumentFolderID != null && subFolder.ParentDocumentFolderID == folder.ID)
                .Select(d =>
                new DocumentDTO.Folder
            { ID = d.ID, Title = d.Title, ParentFolderID = d.ParentDocumentFolderID, Removable = d.Removable, Editable = editable });

            if (subFolders != null && subFolders.Count() > 0)
                documents.AddRange(subFolders);
                
            
            DocumentDTO.Folder returnFolder = new DocumentDTO.Folder
            {
                ID = folder.ID,
                Title = folder.Title,
                Documents = documents,
                ParentFolderID = folder.ParentDocumentFolderID,
                Removable = folder.Removable,
                Editable = editable
            };
            
            return returnFolder;
        }

        /*
        public async Task<byte[]> GetContent(string containerName, int documentItemID)
        {
            var doc = _context.DocumentItem.Find(documentItemID);
            return await _storage.Get(containerName, doc.DocumentLocation);
        }*/

        public async Task<int> GetCustomerManagementFolder(int userID, int? customerID = null)
        {
			int[] readRoles = { 1, 2, 9 };
			int[] editRoles = { 1, 2, 9 };
            string title = "Managementmappe";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }

        public async Task<int> GetCustomerPrivateFolder(int userID, int? customerID = null)
        {
            int[] readRoles = { 8, 9 };
            int[] editRoles = { 9 };
            string title = "Privat kundemappe";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }

        public async Task<int> GetCustomerGeneralFolder(int userID, int? customerID = null)
        {
            int[] readRoles = null;
            int[] editRoles = { 1, 2 };
            string title = "Fælles lokationsmappe";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }

        public async Task<int> GetLocationGeneralFolder(int userID, int? locationID = null)
        {
            int[] readRoles = null;
            int[] editRoles = { 1, 2, 3 };
            string title = "Lokationsmappe";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }

        public async Task<int> GetLocationCleaningPlanFolder(int userID, int? locationID = null)
        {
			int[] readRoles = null;
			int[] editRoles = { 1, 2, 3 };
            string title = "Opgavemappe";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }


        public async Task<int> GetStandardCleaningFolder(int userID, string title)
        {
            int[] readRoles = null;
            int[] editRoles = { 1, 2, 3 };
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }
        /*
        public async Task<int> GetWindowFolder(int userID, int? locationID = null)
        {
            int[] readRoles = null;
            int[] editRoles = { 1, 2, 3 };
            string title = "Vinduespudsning";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }
        public async Task<int> GetFanCoilFolder(int userID, int? locationID = null)
        {
            int[] readRoles = null;
            int[] editRoles = { 1, 2, 3 };
            string title = "Fan coil";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }
        public async Task<int> GetPeriodicFolder(int userID, int? locationID = null)
        {
            int[] readRoles = null;
            int[] editRoles = { 1, 2, 3 };
            string title = "Periodisk rengøring";
            return await AddFolder(userID, null, title, false, ReadRoles: readRoles, EditRoles: editRoles);
        }*/
    }
}

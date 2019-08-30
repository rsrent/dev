using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Data;
using API.DTOs;
using API.Exceptions;
using System.Linq;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrganizationRepository
    {
        private readonly BMSContext _context;
        private readonly SignedInUser _signedInUser;

        public OrganizationRepository(BMSContext context, SignedInUser signedInUser)
        {
            this._context = context;
            this._signedInUser = signedInUser;
        }

        public Organization GetFromId(long Id)
        {
            return _context.Organizations.FirstOrDefault(u => u.Id == Id);
        }

        public ICollection<Organization> GetAll()
        {
            return _context.Organizations.ToList();
        }

        public async Task UpdateAsMaster(long id, Organization val)
        {
            var toUpdate = await _context.Organizations.FindAsync(id);
            if (toUpdate == null) throw new NotFoundException();
            // update values
            toUpdate.Name = val.Name;
            // commit changes
            _context.Organizations.Update(toUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Create(Organization val)
        {
            val.FirebaseOwnerId = _signedInUser.FirebaseId;
            _context.Organizations.Add(val);
            await _context.SaveChangesAsync();
            return val.Id;
        }

        public async Task Delete(long id)
        {
            var toDelete = _context.Organizations.FirstOrDefault(c => c.Id == id);
            _context.Organizations.Remove(toDelete);
            await _context.SaveChangesAsync();
        }

        public async Task Test(long folderId, long requestingID)
        {
            var folder = _context.Folders.Where(f => f.Id == folderId).Include(f => f.Parent).Include(f => f.FolderUsers.Where(fu => fu.UserId == requestingID)).FirstOrDefault();
            var folderUser = _context.Folders.Where(f => f.Id == folderId).Include(f => f.Parent).Include(f => f.FolderUsers.Where(fu => fu.UserId == requestingID)).Select(f => f.FolderUsers.Union(f.Parent.FolderUsers));

            var hasAccessToFolder = ContainsRequester(folder, requestingID);
            var hasAccessToParent = ContainsRequester(folder.Parent, requestingID);
        }

        bool ContainsRequester(Folder folder, long requesterId)
        {
            return (folder != null ? folder.FolderUsers.Any(fu => fu.UserId == requesterId) : false) ||
            ContainsRequester(folder.Parent, requesterId);
        }


        public async Task Test2(long requesterId)
        {
            var t = _context.FolderUsers
            .Where(fu => fu.UserId == requesterId)
            .Include(fu => fu.Folder).ThenInclude(f => f.Parent).Include(fu => fu.Folder).ThenInclude(f => f.FolderUsers.Where(fu => fu.User.Role == "Manager"));

            var folder = _context.Folders.Where(f => f.FolderUsers.Any(fu => fu.UserId == requesterId)).Include(f => f.Parent).Include(f => f.FolderUsers.Where(fu => fu.User.Role == "Manager"));
        }
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Models.Projects;
using Rent.Models.TimePlanning;
using Rent.Models.General;

namespace Rent.Repositories
{
    public class ProjectRoleRepository
    {
        private readonly RentContext _context;
        public ProjectRoleRepository(RentContext context)
        {
            this._context = context;
        }

        public void UpdateProjectItemUsersFromUserID(int userId)
        {
            var res = _context.ProjectItemUser.FromSql($"UpdateProjectItemUsersFromUserID {userId}").ToList();
            //FromSql("UpdateProjectItemUsersFromUserID " + userId);
        }

        public void UpdateProjectItemUsersFromProjectItemID(int projectItemID)
        {
            var res = _context.ProjectItemUser.FromSql($"UpdateProjectItemUsersFromProjectItemID {projectItemID}").ToList();
        }

        public void UpdateProjectItemUsersFromProjectID(int projectID)
        {
            var res = _context.ProjectItemUser.FromSql($"UpdateProjectItemUsersFromProjectID {projectID}").ToList();
        }

        public IQueryable<Project> GetProjectsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            if (_hasAllPermissions)
                return _context.Project;
            else
                return _context.Project.FromSql("SELECT * FROM AllProjectsForUserID(" + userId + ")");

            //return _context.ProjectItem.Where(pi => _hasAllPermissions || pi.ProjectItemType == projectItemType && pi.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }

        public bool HasAllPermissions(int requester)
        {
            if (requester == 0) return true;
            var user = _context.User.Include(u => u.ProjectRole).FirstOrDefault(u => u.ID == requester);
            if (user == null || user.ProjectRole == null) throw new UnauthorizedAccessException();
            return user.ProjectRole.HasAllPermissions;
        }

        public bool CanUserReadProjectItem(int requester, int projectItemID)
        {
            if (requester == 0) return true;
            if (HasAllPermissions(requester)) return true;
            return _context.ProjectItem.Where(pi => pi.ID == projectItemID)
                .Select(pi => pi.ProjectItemUsers.Any(piu => piu.UserID == requester && piu.Read))
                .FirstOrDefault();
        }

        public bool CanUserWriteProjectItem(int requester, int projectItemID)
        {
            if (requester == 0) return true;
            if (HasAllPermissions(requester)) return true;
            return _context.ProjectItem.Where(pi => pi.ID == projectItemID)
                .Select(pi => pi.ProjectItemUsers.Any(piu => piu.UserID == requester && piu.Write))
                .FirstOrDefault();
        }

        public IQueryable<User> UsersWithReadAccessToProjectItem(int requester, int projectItemID)
        {
            var _hasAllPermissions = HasAllPermissions(requester);
            return _context.User.Where(u => _hasAllPermissions || u.ProjectItemUsers.Any(piu => piu.ProjectItemID == projectItemID && piu.Read));
        }

        public IQueryable<User> UsersWithWriteAccessToProjectItem(int requester, int projectItemID)
        {
            var _hasAllPermissions = HasAllPermissions(requester);
            return _context.User.Where(u => _hasAllPermissions || u.ProjectItemUsers.Any(piu => piu.ProjectItemID == projectItemID && piu.Write));
        }




        public IQueryable<ProjectItem> GetReadableProjectItemsOfUser(int userId, ProjectItemType projectItemType)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.ProjectItem.Where(pi => _hasAllPermissions || pi.ProjectItemType == projectItemType && pi.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }

        public IQueryable<ProjectItem> GetWritableProjectItemsOfUser(int userId, ProjectItemType projectItemType)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.ProjectItem.Where(pi => _hasAllPermissions || pi.ProjectItemType == projectItemType && pi.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<LocationLog> GetReadableLogsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.LocationLog.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<LocationLog> GetWritableLogsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.LocationLog.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<Work> GetReadableWorkOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Work.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<Work> GetWritableWorkOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Work.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<WorkContract> GetReadableWorkContractsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.WorkContract.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<WorkContract> GetWritableWorkContractsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.WorkContract.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<CleaningTask> GetReadableCleaningTasksOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.CleaningTask.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<CleaningTask> GetWritableCleaningTasksOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.CleaningTask.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<QualityReport> GetReadableQualityReportsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.QualityReport.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<QualityReport> GetWritableQualityReportsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.QualityReport.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<QualityReportItem> GetReadableQualityReportItemsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.QualityReportItem.Where(w => _hasAllPermissions || w.QualityReport.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<QualityReportItem> GetWritableQualityReportItemsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.QualityReportItem.Where(w => _hasAllPermissions || w.QualityReport.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<Comment> GetReadableCommentsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Comment.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<Comment> GetWritableCommentsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Comment.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<Address> GetReadableAddresssOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Address.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read));
        }
        public IQueryable<Address> GetWritableAddresssOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Address.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write));
        }

        public IQueryable<DocumentFolder> GetReadableFoldersOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.DocumentFolder.Where(df => _hasAllPermissions
            || (df.ProjectItem != null && df.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read))
            || (df.RootDocumentFolder != null && df.RootDocumentFolder.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read)));
        }
        public IQueryable<DocumentFolder> GetWritableFoldersOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.DocumentFolder.Where(df => _hasAllPermissions
            || (df.ProjectItem != null && df.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write))
            || (df.RootDocumentFolder != null && df.RootDocumentFolder.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write)));
        }

        public IQueryable<Document> GetReadableDocumentsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Document.Where(d => _hasAllPermissions
            || (d.DocumentFolder.ProjectItem != null && d.DocumentFolder.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read))
            || (d.DocumentFolder.RootDocumentFolder != null && d.DocumentFolder.RootDocumentFolder.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Read)));
        }
        public IQueryable<Document> GetWritableDocumentsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.Document.Where(d => _hasAllPermissions
            || (d.DocumentFolder.ProjectItem != null && d.DocumentFolder.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write))
            || (d.DocumentFolder.RootDocumentFolder != null && d.DocumentFolder.RootDocumentFolder.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && piu.Write)));
        }

        public IQueryable<FirestoreConversation> GetConversationsOfUser(int userId)
        {
            var _hasAllPermissions = HasAllPermissions(userId);
            return _context.FirestoreConversation.Where(w => _hasAllPermissions || w.ProjectItem.ProjectItemUsers.Any(piu => piu.UserID == userId && (piu.Read || piu.Write)));
        }

    }
}

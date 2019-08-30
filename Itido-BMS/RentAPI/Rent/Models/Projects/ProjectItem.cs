using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rent.Helpers;
using Rent.Models.TimePlanning;

namespace Rent.Models.Projects
{
    public enum ProjectItemType
    {
        ProfileImage,
        Comment,
        Client,
        Location,
        Logs,
        Tasks,
        QualityReports,
        Work,
        WorkContracts,
        DocumentFolders,
        FirestoreConversations,
        Address,
        Post,
    }

    public class ProjectItem
    {
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public ProjectItemType ProjectItemType { get; set; }
        //public string Access { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<ProjectItemUser> ProjectItemUsers { get; set; }
        public virtual ICollection<CleaningTask> Tasks { get; set; }
        public virtual ICollection<QualityReport> QualityReports { get; set; }
        public virtual ICollection<LocationLog> Logs { get; set; }
        public virtual ICollection<Work> Work { get; set; }
        public virtual ICollection<WorkContract> WorkContracts { get; set; }
        public virtual FirestoreConversation FirestoreConversations { get; set; }
        public virtual DocumentFolder DocumentFolder { get; set; }
        public virtual Post Post { get; set; }
        public virtual Comment Comment { get; set; }

        public virtual Location Location { get; set; }
        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Client Client { get; set; }

        public virtual ICollection<ProjectItemAccess> ProjectItemAccesses { get; set; }

        static public Expression<Func<ProjectItem, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.ProjectItemType,
                ItemType = v.ProjectItemType.ToString(),
            } : null;
        }

        static public Expression<Func<ProjectItem, dynamic>> StandardDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                //v.Access,
                v.ProjectItemType,
                ItemType = v.ProjectItemType.ToString(),
                Name =
                        v.ProjectItemType == ProjectItemType.Comment ? v.Comment.Title != null ? v.Comment.Title : "Comment" :
                        v.ProjectItemType == ProjectItemType.DocumentFolders ? v.DocumentFolder.Title :
                        v.ProjectItemType == ProjectItemType.Address ? v.Address.AddressName : null,

                Comment = v.ProjectItemType == ProjectItemType.Comment ?
                    Comment.BasicDTO().Compile()(v.Comment) :
                    null,
                Address = v.ProjectItemType == ProjectItemType.Address ?
                    Address.BasicDTO().Compile()(v.Address) :
                    null,
                DocumentFolder = v.ProjectItemType == ProjectItemType.DocumentFolders ?
                    DocumentFolder.BasicDTO().Compile()(v.DocumentFolder)
                     : null,
                Post = v.ProjectItemType == ProjectItemType.Post ? v.Post : null,
            } : null;
        }

        static public Expression<Func<ProjectItem, dynamic>> DetailedDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                //v.Access,
                v.ProjectItemType,
                ItemType = v.ProjectItemType.ToString(),
                Name =
                        v.ProjectItemType == ProjectItemType.Comment ? v.Comment.Title != null ? v.Comment.Title : "Comment" :
                        v.ProjectItemType == ProjectItemType.DocumentFolders ? v.DocumentFolder.Title :
                        v.ProjectItemType == ProjectItemType.Address ? v.Address.AddressName : null,

                Comment = v.ProjectItemType == ProjectItemType.Comment ?
                    Comment.BasicDTO().Compile()(v.Comment) :
                    null,
                Address = v.ProjectItemType == ProjectItemType.Address ?
                    Address.BasicDTO().Compile()(v.Address) :
                    null,
                DocumentFolder = v.ProjectItemType == ProjectItemType.DocumentFolders ?
                    DocumentFolder.BasicDTO().Compile()(v.DocumentFolder)
                     : null,
                Post = v.ProjectItemType == ProjectItemType.Post ? v.Post : null,
                v.ProjectItemAccesses,
            } : null;
        }
    }
}

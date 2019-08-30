using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Rent.DTOs;
using Rent.Repositories;

namespace Rent.Models
{
    public class Customer : IDto
    {
        public int ID { get; set; }
        public bool Disabled { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string ImageLocation { get; set; }
        public DateTime Created { get; set; }
        public CustomerStatus Status { get; set; }

        public int? UpdatePasswordsAtSecondsInterval { get; set; }
        public DateTime PasswordsLastUpdated { get; set; }
        public bool HasStandardFolders { get; set; }
        public bool CanReadLogs { get; set; }
        public int GeneralFolderID { get; set; }
        public int PrivateFolderID { get; set; }
        public int ManagementFolderID { get; set; }
        public int? ConversationID { get; set; }
        public int? SalesRepID { get; set; }
        public int? MainUserID { get; set; }
        public int? KeyAccountManagerID { get; set; }

        public virtual User MainUser { get; set; }
        public virtual User KeyAccountManager { get; set; }
        public virtual User SalesRep { get; set; }
        public virtual Conversation Conversation { get; set; }
        public virtual DocumentFolder GeneralFolder { get; set; }
        public virtual DocumentFolder PrivateFolder { get; set; }
        public virtual DocumentFolder ManagementFolder { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Location> Locations { get; set; }

        public override dynamic Detailed()
        {
            return Merger.Merge(new
            {
                //UpdatePasswordsAtSecondsInterval,
                HasStandardFolders,
                //MainUserID,
                //KeyAccountManagerID,
                MainUser = MainUser?.Detailed(),
                KeyAccountManager = KeyAccountManager?.Detailed(),
                //SalesRep = SalesRep?.Basic(condition),
            }, Basic());
        }

        public override dynamic Basic()
        {
            return new
            {
                ID,
                Disabled,
                Name,
                Comment,
                ImageLocation,
                //Created,
                //Status,
            };
        }
    }
    
    public enum CustomerStatus 
    {
        Lead,
        Customer,
        DeadLead,
        Terminated
    }
}

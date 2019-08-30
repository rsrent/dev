using Rent.Data;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Rent.Models.Customer;

namespace Rent.DTOs
{
    /*
    public class CustomerDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public CustomerStatus Status { get; set; }
        public string Comment { get; set; }
        public string ImageLocation { get; set; }
        public int DocumentFolderID { get; set; }
        public bool Disabled { get; set; }

        public UserDTO MainUser { get; set; }
        public UserDTO KeyAccountManager { get; set; }
        public UserDTO SalesRep { get; set; }

        //public virtual ICollection<User> Users { get; set; }

        void SetupBasics(Customer c)
        {
            ID = c.ID;
            Name = c.Name;
            Created = c.Created;
            Status = c.Status;
            Comment = c.Comment;
            ImageLocation = c.ImageLocation;
            DocumentFolderID = c.GeneralFolderID;
            Disabled = c.Disabled;
        }

        public CustomerDTO() { }

        public CustomerDTO(Customer c)
        {
            SetupBasics(c);
            MainUser = User.Basic(c.MainUser);
            SalesRep = User.Basic(c.SalesRep);
            KeyAccountManager = User.Basic(c.KeyAccountManager);
        }

        public static CustomerDTO Basic(Customer c)
        {
            if (c == null)
                return null;
            var dto = new CustomerDTO();
            dto.SetupBasics(c);
            return dto;
        }
    }
    */
}

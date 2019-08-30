using System;
using System.Threading.Tasks;
using RentAppProject;

namespace RentApp.ViewModels
{
    public class CreateUserVM
    {
        public Func<User, Task> DoneAction { get; set; }
        public User NewUser { get; set; }
        public int CustomerID { get; set; }
    }
}

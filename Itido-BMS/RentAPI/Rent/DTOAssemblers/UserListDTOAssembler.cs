using Rent.DTOs.TimePlanningDTO;
using Rent.Models;
using System;

namespace Rent.DTOAssemblers
{

    public class UserListDTOAssembler
    {


        public UserListDTO CreateUserListDTO(User user)
        {
            Console.WriteLine("Create user list dto");
            return new UserListDTO
            {


            };

        }
    }
}
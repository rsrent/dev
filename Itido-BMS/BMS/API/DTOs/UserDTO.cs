using System;
using API.Models;
namespace API.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }


        public static UserDTO fromUser(User user){
            return new UserDTO {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };
        }

        public static User toUserModel(UserDTO user){
             return new User {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };

        
        }
    }
}

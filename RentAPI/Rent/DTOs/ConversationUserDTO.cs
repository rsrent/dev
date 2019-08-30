using System;
namespace Rent.DTOs
{
    public class ConversationUserDTO
    {
        public int ID { get; set; }
        public string ImageLocation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool NotificationsOn { get; set; }
    }
}

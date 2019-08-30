using Rent.Data;
using System;
namespace Rent.Helpers
{
    public class MyEmailNotificationChannelSettings : IEmailNotificationChannelSettings
    {
        private readonly RentContext _context;
        public MyEmailNotificationChannelSettings(RentContext context)
        {
            _context = context;
        }

        public int Port() => 587;
        public string Protocol() => "smtp-mail.outlook.com";

        public string[] ReceiverDetails(int receiverID)
        {
            var user = _context.User.Find(receiverID);
            if (user == null)
                return null;
            return new[] { user.Email, user.FirstName + " " + user.LastName };
        }

        public string SenderEmail() => "rent-app@outlook.com";
        public string SenderName() => "Rent rengøring";
        public string SenderPassword() => "lrR!EH61l@$@XD09EjV";

    }
}

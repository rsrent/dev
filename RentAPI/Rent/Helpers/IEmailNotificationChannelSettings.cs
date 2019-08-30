using System;
namespace Rent.Helpers
{
    public interface IEmailNotificationChannelSettings
    {
        string SenderEmail();
        string SenderName();
        string SenderPassword();
        string Protocol();
        int Port();
        string[] ReceiverDetails(int receiverID);
    }
}

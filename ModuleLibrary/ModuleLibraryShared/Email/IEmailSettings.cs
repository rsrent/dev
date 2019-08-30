using System;
namespace ModuleLibraryShared.Email
{
    public interface IEmailSettings
    {
        string SenderEmail();
        string SenderName();
        string SenderPassword();
        string Protocol();
        int Port();
        string[] ReceiverDetails(int receiverID);
    }
}

using System;
using System.Threading.Tasks;

namespace Rent.Helpers
{
    public interface INotificationChannel
    {
        Task<(bool, string)> Send(int receiverID, string title, string body);
    }
}

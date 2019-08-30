using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Helpers
{
    public interface IPhoneNotificationSettings
    {
        string GetConnectionString();
        string GetNotificationHubPath();
    }
}

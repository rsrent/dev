using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Helpers
{
    public class DevPhoneNotificationSettings : IPhoneNotificationSettings
    {
        public string GetConnectionString() => 
        "Endpoint=sb://rentnotifications.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fqxxVl0lEdfPBPd1qgcxfslehOa0ookaTph4a7CM3Ds=";

        public string GetNotificationHubPath() => 
        "RentNotifications";
    }
}

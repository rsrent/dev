using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Helpers
{
    public class LivePhoneNotificationSettings : IPhoneNotificationSettings
    {
        public string GetConnectionString() => 
        "Endpoint=sb://rentnotificationsproductionnamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+Ox5f6dsHx3hhIodJA1/U51HKfIEytml/HxeNKEpGsc=";

        public string GetNotificationHubPath() => 
        "RentNotificationsProductionNamespace";
    }
}

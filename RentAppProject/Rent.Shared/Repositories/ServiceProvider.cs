using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RentApp
{
    /*
    public static class ServiceProvider
    {
        public static Settings Settings { get; set; }

        public static CleaningTasksRepository CleaningTasksRepository { get; set; }
        public static CompletedCleaningTasksRepository CompletedCleaningTasksRepository { get; set; }
        public static CustomerRepository CustomerRepository { get; set; }
        public static LocationRepository LocationRepository { get; set; }
        public static QualityReportRepository QualityReportRepository { get; set; }
        public static UserRepository UserRepository { get; set; }

        public static HttpClient Client { get; set; }

        public static void Start()
        {
            Settings = new Settings();

			Client = new HttpClient()
			{
                BaseAddress = Settings.ApiBaseAddress
			};
			Client.DefaultRequestHeaders.Clear();
			Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            CleaningTasksRepository = new CleaningTasksRepository(Client);
            CompletedCleaningTasksRepository = new CompletedCleaningTasksRepository(Client);
            CustomerRepository = new CustomerRepository(Client);
            LocationRepository = new LocationRepository(Client);
            QualityReportRepository = new QualityReportRepository(Client);
            UserRepository = new UserRepository(Client);
        }
    }*/
}

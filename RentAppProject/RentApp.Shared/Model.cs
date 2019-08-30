using ModuleLibraryShared.Observer;
using RentAppProject;

namespace RentApp
{
    public class Model : ModuleObservable<Model>
	{
		//public string HttpUri = "http://localhost:5000/api/";
		//public string SocketUri = "ws://localhost:5000/chat";
		//public string HttpUri = "http://rent20170925043224.azurewebsites.net/api/";
		//public string SocketUri = "ws://rent20170925043224.azurewebsites.net/chat";
		//public string AzureStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=rentappteststorage;AccountKey=vdKiDLv4xqGWVFRQEhkCSIYWeb1kJIdvlTj5fNifYt5DUiT2BGYX7lI2cV0gwcaNReDCDClFbOoZxgUBaj+h6A==;EndpointSuffix=core.windows.net";

		static Model model;
		public static Model Instance()
		{
			if (model == null)
				model = new Model();
			return model;
		}

        public NotificationToken StoredNotificationToken = new NotificationToken();
        public User LoggedInUser;
        //public Company Company;
		public Location Location;

        //public CustomerUser Customer;
        private CleaningSchedule CleaningPlan;

        public void UpdateCleaningPlanDictionary(CleaningSchedule cleaningPlan) {
            CleaningPlan = cleaningPlan;
            Update();
        }

        public CleaningSchedule GetCleaningPlan() {
            return CleaningPlan;
        }
	}
}

using System;
namespace RentAppProject
{
    public class Role
    {
        public int ID { get; set; }
        public int Rank { get; set; }
        public string Name { get; set; }

        public static Role Master => new Role { ID = 1, Name = "Master" };
        public static Role HumanResource => new Role { ID = 2, Name = "Key account manager" };
        public static Role ServiceLeader => new Role { ID = 3, Name = "Service leader" };
        public static Role Sales => new Role { ID = 4, Name = "Sales" };
        public static Role RegularCleaningAssistant => new Role { ID = 5, Name = "Regular cleaning assistant" };
        public static Role WindowCleaningAssistant => new Role { ID = 6, Name = "Window cleaning assistant" };
        public static Role FanCoilCleaningAssistant => new Role { ID = 7, Name = "Fan coil cleaning assistant" };
		public static Role LocationManager => new Role { ID = 8, Name = "Location manager" };
        public static Role CustomerManager => new Role { ID = 9, Name = "Customer manager" };
    }
}

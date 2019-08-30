using System;
namespace RentAppProject
{
    public class UserLoginInfo
    {
        public User User { get; set; }
        public Login Login { get; set; }
        //public int CustomerID { get; set; }
		//public string TemplateRole { get; set; }
		public string Token { get; set; }
    }
}

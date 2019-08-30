using System;
namespace RentAppProject
{
    public class OutlookEventDTO
    {
		public string Token { get; set; }
		public string Email { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public string TimeZone { get; set; }
		public string Subject { get; set; }
    }
}

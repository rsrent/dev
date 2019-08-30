using System;
using System.Collections.Generic;


namespace Rent.Helpers
{
    public class SendGridEmailReport
    {
        public List<string> InvalidEmails = new List<string>();
        public List<string> ValidEmails = new List<string>();
        public System.Net.HttpStatusCode StatusCode;
        public string body = "";
    }
}

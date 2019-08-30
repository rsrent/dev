using Microsoft.AspNetCore.Http;
using System.Linq;

namespace API.Services
{
    public class SignedInUser
    {
        public string FirebaseId { get; set; }

        public SignedInUser(IHttpContextAccessor httpContext)
        {
            FirebaseId = httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "user_id").Value;
        }
    }
}

using System;

namespace Censo.API.Model
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

       public class UserToken
       {
           public string Token { get; set; }
           public DateTime Expiration { get; set; }
       }
}
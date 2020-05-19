using System;
using System.Collections.Generic;

namespace Censo.API.Model
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
         public IList<string> Roles {get; set;}
    }

       public class UserToken
       {
           public string Token { get; set; }
           public DateTime Expiration { get; set; }
       }
}
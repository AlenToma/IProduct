using Microsoft.Owin.Security.Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace IProduct.Models.OAuthProviders
{
    public class FacebookProvider : FacebookAuthenticationProvider
    {
        public override Task Authenticated(FacebookAuthenticatedContext context)
        {
            using(var m = new UserManager())
                m.Create(context);
            return base.Authenticated(context);
        }
    }
}
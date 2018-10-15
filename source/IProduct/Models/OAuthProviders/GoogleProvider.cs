using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace IProduct.Models.OAuthProviders
{
    public class GoogleProvider : GoogleOAuth2AuthenticationProvider
    {
        public override Task Authenticated(GoogleOAuth2AuthenticatedContext context)
        {
            using(var m = new UserManager())
                m.Create(context);
            return base.Authenticated(context);
        }

    }
}
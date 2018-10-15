using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IProduct.Models.OAuthProviders
{
    public class CookieProvider : CookieAuthenticationProvider
    {
        public override void Exception(CookieExceptionContext context)
        {
            base.Exception(context);
        }

        public override Task ValidateIdentity(CookieValidateIdentityContext context)
        {
            return base.ValidateIdentity(context);
        }

        public override void ApplyRedirect(CookieApplyRedirectContext context)
        {
            base.ApplyRedirect(context);
        }
    }
}
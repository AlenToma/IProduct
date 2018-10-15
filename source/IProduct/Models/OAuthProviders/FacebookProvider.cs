using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;

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
using Microsoft.Owin.Security.Google;
using System.Threading.Tasks;

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
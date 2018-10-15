using IProduct.Models.OAuthProviders;
using IProduct.Modules;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using Microsoft.AspNet.Identity;


namespace IProduct
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var googleCredentials = Actions.LoadCredentials(SignInApplication.Google);

            if(googleCredentials == null)
                throw new Exception("GoogleCredentials could not be found(GoogleOAuth2Authentication)");

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            var cookieOptions = new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Index"),
                SlidingExpiration = true,
                Provider = new CookieProvider(),
                ExpireTimeSpan = TimeSpan.FromDays(7)
            };

            app.UseCookieAuthentication(cookieOptions);

            var googleOption = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = googleCredentials.Client_Id,
                ClientSecret = googleCredentials.Client_Secret,
                CallbackPath = new PathString("/Google"),
                Provider = new GoogleProvider(),
                
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie
            };

            app.UseGoogleAuthentication(googleOption);
        }
    }
}
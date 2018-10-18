using EntityWorker.Core.Helper;
using IProduct.Modules;
using IProduct.Modules.Data;
using IProduct.Modules.Library;
using IProduct.Modules.Library.Custom;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using System;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace IProduct.Models
{
    public class UserManager : IDisposable
    {
        private DbContext _dbContext = new DbContext();

        public void SignIn(SignInApplication type, GenericView<User> user = null)
        {
            switch (type)
            {
                case SignInApplication.Cookie:
                    Create(user);
                    break;

                case SignInApplication.Facebook:
                    HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { IsPersistent = true, RedirectUri = "Account/Facebook" }, SignInApplication.Facebook.ToString());
                    break;

                case SignInApplication.Google:
                    HttpContext.Current.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { IsPersistent = true, RedirectUri = "Account/Google" }, SignInApplication.Google.ToString());
                    break;
            }
        }

        public void Create(GenericView<User> model)
        {

            var user = _dbContext.Get<User>().Where(x => x.Email.Contains(model.View.Email) && x.Password == model.View.Password).LoadChildren().ExecuteFirstOrDefault();
            if (user == null)
                return;
            Authorize(user, model.View.RememberMe);
        }

        // For facebook
        public void Create(FacebookAuthenticatedContext context)
        {

            if (string.IsNullOrEmpty(context.Email))
                return;
            var email = context.Email;
            var user = _dbContext.Get<User>().Where(x => x.Email == email).LoadChildren().ExecuteFirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Password = "xxxxxxx", // User have to change it later
                    Person = new Person()
                    {
                        FirstName = context.Name,
                        LastName = "",
                        Address = new Address()
                        {
                            AddressLine = string.Empty,
                            Country_Id = _dbContext.Get<Country>().Where(x => x.CountryCode.Contains("sv-se")).ExecuteFirstOrDefault().Id.Value
                        }
                    },
                    Role = _dbContext.Get<Role>().Where(x => x.RoleType == Roles.Customers).ExecuteFirstOrDefault()

                };
                _dbContext.Save(user).SaveChanges();
              
            }
            Authorize(user);
        }

        // For Google
        public void Create(GoogleOAuth2AuthenticatedContext context)
        {

            if (string.IsNullOrEmpty(context.Email))
                return;
            var email = context.Email;
            var user = _dbContext.Get<User>().Where(x => x.Email == email).LoadChildren().ExecuteFirstOrDefault();
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    Password = "xxxxxxx", // User have to change it later
                    Person = new Person()
                    {
                        FirstName = context.Name,
                        LastName = context.FamilyName,
                        Address = new Address()
                        {
                            AddressLine = string.Empty,
                            Country_Id = _dbContext.Get<Country>().Where(x => x.CountryCode.Contains("sv-se")).ExecuteFirstOrDefault().Id.Value
                        }
                    },
                    Role = _dbContext.Get<Role>().Where(x => x.RoleType == Roles.Customers).ExecuteFirstOrDefault()
                };
                _dbContext.Save(user).SaveChanges();
               
            }
            Authorize(user);
        }


        public void Authorize(User user, bool isPersistent = true)
        {
            var ident = new ClaimsIdentity(new[] {
              new Claim(ClaimTypes.NameIdentifier, user.Email),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider","ASP.NET Identity","http://www.w3.org/2001/XMLSchema#string"),
              new Claim(ClaimTypes.Name, user.Person.FullName),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, user.Role.Name)
            }, CookieAuthenticationDefaults.AuthenticationType);
            /// write to Cookie
            HttpContext.Current.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, ident);
        }

        public void SignOut()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        }

        public User GetCurrentUser()
        {

            var email = HttpContext.Current.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type == "email" || x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return null;

            var user = _dbContext.Get<User>().Where(x => x.Email == email).LoadChildren().IgnoreChildren(x => x.Invoices).ExecuteFirstOrDefault();
            return user;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
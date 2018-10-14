using Microsoft.AspNetCore.Http;
using OAuth.Security.Interface;
using OAuth.Security.Library;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using OAuth.Security.Modules.Library;

namespace OAuth.Security
{
    public class UserManager : IUserManager
    {
        public SignUpResult SignUp(string name, string credentialTypeCode, string identifier)
        {
            return this.SignUp(name, credentialTypeCode, identifier, null);
        }

        public SignUpResult SignUp(string name, string credentialTypeCode, string identifier, string secret)
        {
            User user = new User
            {
                Name = name,
                Created = DateTime.Now
            };
            SecurityConfigrationManager.SecuritySettings.Save(user);

            CredentialType credentialType = SecurityConfigrationManager.SecuritySettings.Get<CredentialType>(ct => ct.Code.Contains(credentialTypeCode)).FirstOrDefault();

            if (credentialType == null)
                return new SignUpResult(success: false, error: SignUpResultError.CredentialTypeNotFound);

            Credential credential = new Credential();

            credential.User_Id = user.Id.Value;
            credential.CredentialType_Id = credentialType.Id.Value;
            credential.Identifier = identifier;

            if (!string.IsNullOrEmpty(secret))
            {
                byte[] salt = SecurityConfigrationManager.GenerateRandomSalt();
                string hash = SecurityConfigrationManager.ComputeHash(secret, salt);

                credential.Secret = hash;
                credential.Extra = Convert.ToBase64String(salt);
            }

            SecurityConfigrationManager.SecuritySettings.Save(credential);
            return new SignUpResult(user: user, success: true);
        }

        public ChangeSecretResult ChangeSecret(string credentialTypeCode, string identifier, string secret)
        {
            CredentialType credentialType = SecurityConfigrationManager.SecuritySettings.Get<CredentialType>(ct => ct.Code.Contains(credentialTypeCode)).FirstOrDefault();

            if (credentialType == null)
                return new ChangeSecretResult(success: false, error: ChangeSecretResultError.CredentialTypeNotFound);

            Credential credential = SecurityConfigrationManager.SecuritySettings.Get<Credential>(c => c.CredentialType_Id == credentialType.Id && c.Identifier == identifier).FirstOrDefault();

            if (credential == null)
                return new ChangeSecretResult(success: false, error: ChangeSecretResultError.CredentialNotFound);

            byte[] salt = SecurityConfigrationManager.GenerateRandomSalt();
            string hash = SecurityConfigrationManager.ComputeHash(secret, salt);

            credential.Secret = hash;
            credential.Extra = Convert.ToBase64String(salt);
            SecurityConfigrationManager.SecuritySettings.Save(credential);
            return new ChangeSecretResult(success: true);
        }

        public ValidateResult Validate(string credentialTypeCode, string identifier)
        {
            return this.Validate(credentialTypeCode, identifier, null);
        }

        public ValidateResult Validate(string credentialTypeCode, string identifier, string secret)
        {
            CredentialType credentialType = SecurityConfigrationManager.SecuritySettings.Get<CredentialType>(ct => ct.Code.Contains(credentialTypeCode)).FirstOrDefault();

            if (credentialType == null)
                return new ValidateResult(success: false, error: ValidateResultError.CredentialTypeNotFound);

            Credential credential = SecurityConfigrationManager.SecuritySettings.Get<Credential>(c => c.CredentialType_Id == credentialType.Id && c.Identifier == identifier).FirstOrDefault();

            if (credential == null)
                return new ValidateResult(success: false, error: ValidateResultError.CredentialNotFound);

            if (!string.IsNullOrEmpty(secret))
            {
                byte[] salt = Convert.FromBase64String(credential.Extra);
                string hash = SecurityConfigrationManager.ComputeHash(secret, salt);

                if (credential.Secret != hash)
                    return new ValidateResult(success: false, error: ValidateResultError.SecretNotValid);
            }

            return new ValidateResult(user: SecurityConfigrationManager.SecuritySettings.Get<User>(x => x.Id == credential.User_Id).FirstOrDefault(), success: true);
        }

        public async void SignIn(HttpContext httpContext, User user, bool isPersistent = false)
        {
            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
              CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = isPersistent }
            );
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public Guid? GetCurrentUserId(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            Claim claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;


            if (!Guid.TryParse(claim.Value, out Guid currentUserId))
                return null;

            return currentUserId;
        }

        public User GetCurrentUser(HttpContext httpContext)
        {
            var currentUserId = this.GetCurrentUserId(httpContext);

            if (currentUserId == null)
                return null;
            return SecurityConfigrationManager.SecuritySettings.Get<User>(x => x.Id == currentUserId).FirstOrDefault();
        }

        private IEnumerable<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.AddRange(this.GetUserRoleClaims(user));
            return claims;
        }

        private IEnumerable<Claim> GetUserRoleClaims(User user)
        {
            List<Claim> claims = new List<Claim>();
            if (user != null)
            {
                foreach (var item in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item.Role.Position.ToString()));
                }
            }

            return claims;
        }
    }
}
using Microsoft.AspNetCore.Http;
using OAuth.Security.Library;
using OAuth.Security.Modules.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Security.Interface
{
    public interface IUserManager
    {
        SignUpResult SignUp(string name, string credentialTypeCode, string identifier);
        SignUpResult SignUp(string name, string credentialTypeCode, string identifier, string secret);
        ChangeSecretResult ChangeSecret(string credentialTypeCode, string identifier, string secret);
        ValidateResult Validate(string credentialTypeCode, string identifier);
        ValidateResult Validate(string credentialTypeCode, string identifier, string secret);
        void SignIn(HttpContext httpContext, User user, bool isPersistent = false);
        void SignOut(HttpContext httpContext);
        Guid? GetCurrentUserId(HttpContext httpContext);
        User GetCurrentUser(HttpContext httpContext);
    }
}

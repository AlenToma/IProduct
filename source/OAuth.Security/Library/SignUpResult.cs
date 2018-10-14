using OAuth.Security.Modules.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Security.Library
{
    public class SignUpResult
    {
        public User User { get; set; }
        public bool Success { get; set; }
        public SignUpResultError? Error { get; set; }

        public SignUpResult(User user = null, bool success = false, SignUpResultError? error = null)
        {
            this.User = user;
            this.Success = success;
            this.Error = error;
        }
    }
}

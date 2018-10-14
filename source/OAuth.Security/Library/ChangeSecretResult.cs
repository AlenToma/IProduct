using System;
using System.Collections.Generic;
using System.Text;

namespace OAuth.Security.Library
{
    public class ChangeSecretResult
    {
        public bool Success { get; set; }
        public ChangeSecretResultError? Error { get; set; }

        public ChangeSecretResult(bool success = false, ChangeSecretResultError? error = null)
        {
            this.Success = success;
            this.Error = error;
        }
    }
}

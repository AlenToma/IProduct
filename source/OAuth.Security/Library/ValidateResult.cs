
using OAuth.Security.Modules.Library;

namespace OAuth.Security.Library
{
    public class ValidateResult
    {
        public User User { get; set; }
        public bool Success { get; set; }
        public ValidateResultError? Error { get; set; }

        public ValidateResult(User user = null, bool success = false, ValidateResultError? error = null)
        {
            this.User = user;
            this.Success = success;
            this.Error = error;
        }
    }
}

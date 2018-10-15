using EntityWorker.Core.Helper;
using IProduct.Controllers.Shared;
using IProduct.Models;
using IProduct.Modules;
using System.Web.Mvc;

namespace IProduct.Controllers
{
    public class AccountController : SharedController
    {
        public ActionResult Index()
        {
            if(Request.IsAuthenticated)
               return Redirect("~/Home");
            return View();
        }

        [AllowAnonymous]
        public void SignIn(string ReturnUrl = "/", string type = "")
        {
            using(var manager = new UserManager())
            {
                if(!Request.IsAuthenticated)
                {
                    manager.SignIn(type.ConvertValue<SignInApplication>());
                }
            }
        }

        #region Google
        // we may need to add some changes here later as if now, the google provider take care of the login
        [AllowAnonymous]
        public ActionResult Google(string error)
        {
            if(Request.IsAuthenticated)
                return Redirect("~/Home");

            return Redirect("Index");
        }
        #endregion

    }
}
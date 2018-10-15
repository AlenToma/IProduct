using EntityWorker.Core.Helper;
using IProduct.Controllers.Shared;
using IProduct.Models;
using IProduct.Modules;
using IProduct.Modules.Library.Custom;
using System;
using System.Web.Mvc;

namespace IProduct.Controllers
{
    public class AccountController : SharedController
    {
        public ActionResult Index(string type = "")
        {
            if(Request.IsAuthenticated)
                return Redirect("~/Home");
            else if (type.ConvertValue<SignInApplication?>().HasValue)
            {
                using(var manager = new UserManager())
                {
                    if(!Request.IsAuthenticated)
                    {
                        manager.SignIn(type.ConvertValue<SignInApplication>());
                    }
                }

                if(type.ConvertValue<SignInApplication>() == SignInApplication.Cookie && !Request.IsAuthenticated)
                    return View(new JsonData { Success = false, Data = "Email or Password could not be found in our system" });
            }
            return View();
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

        #region Facebook
        // we may need to add some changes here later as if now, the google provider take care of the login
        [AllowAnonymous]
        public ActionResult Facebook(string error)
        {
            if(Request.IsAuthenticated)
                return Redirect("~/Home");

            return Redirect("Index");
        }
        #endregion

    }
}
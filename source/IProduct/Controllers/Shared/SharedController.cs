using EntityWorker.Core.Helper;
using IProduct.Modules.Data;
using IProduct.Modules.Library;
using IProduct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IProduct.Controllers.Shared
{
 

    public class SharedController : Controller
    {
        private DbContext dbContext;

        protected DbContext DbContext
        {
            get
            {
                if (dbContext == null)
                    dbContext = new DbContext();
                return dbContext;
            }
        }

        [HttpGet]
        public ActionResult GetModule(string partialName)
        {
            return PartialView("~/Views/" + partialName);
        }

        [AllowAnonymous]
        [HttpPost]
        public string SignIn(string email, string password, bool rememberMe)
        {
            var user = DbContext.Get<User>().Where(x => x.Email == email && x.Password == password).LoadChildren().ExecuteFirstOrDefault();
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Email, rememberMe);
                return user.ToJson();
            }

            return null;

        }

        [AllowAnonymous]
        [HttpPost]
        public string GetcurrentUser()
        {
            
            var email = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
            //var email = System.Web.HttpContext.Current.User.Identity.Name;
            var user= DbContext.Get<User>().Where(x => x.Email == email).LoadChildren().IgnoreChildren(x=> x.Invoices).Execute();
            if (user.Count > 0 && SessionHelper.Cart._user == null)
                SessionHelper.Cart.ApplyUser(user.First().Id.Value);
            return user.ToJson();
        }

        [AllowAnonymous]
        [HttpPost]
        public void SignOut()
        {
            SessionHelper.Cart = null;
            FormsAuthentication.SignOut();
        }

    }
}
using EntityWorker.Core.Helper;
using IProduct.Modules.Data;
using IProduct.Modules.Library;
using IProduct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IProduct.Controllers.Shared
{
    
    public class SharedController : Controller
    {
        private DbContext dbContext;

        public DbContext DbContext
        {
            get
            {
                if(dbContext == null)
                    dbContext = new DbContext();
                return dbContext;
            }
        }

        [HttpGet]
        public ActionResult GetModule(string partialName)
        {
            return PartialView("~/Views/" + partialName);
        }


        [HttpPost]
        public string GetcurrentUser()
        {
            using(var manager = new UserManager())
            {
                var user = manager.GetCurrentUser();
                if(user == null)
                    return "[]";
                if(SessionHelper.Cart._user == null)
                    SessionHelper.Cart.ApplyUser(user.Id.Value);
                return user.ToJson();
            }
        }

        [HttpPost]
        public void SignOut()
        {
            using(var manager = new UserManager())
                manager.SignOut();
            SessionHelper.Cart = null;
        }

    }
}
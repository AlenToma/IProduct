using IProduct.Modules.Data;
using IProduct.Models;
using System;
using System.Web.Mvc;
using IProduct.Modules;
using System.IO;

namespace IProduct.Controllers.Shared
{

    public class SharedController : Controller, IProduct.Modules.Interface.IController
    {
        private DbContext dbContext;
        public bool ExceptionHandled { get; set; }
        protected DbContext DbContext
        {
            get
            {
                if (dbContext == null)
                    dbContext = new DbContext();
                return dbContext;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetModule(string partialName)
        {
            partialName = "~/Views/" + partialName;
            return PartialView(partialName);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Template(string path)
        {
            return Content(System.IO.File.ReadAllText(Server.MapPath(path)));
        }

        [HttpPost]
        public ActionResult GetcurrentUser()
        {
            using (var manager = new UserManager())
            {
                var user = manager.GetCurrentUser();
                if (user == null)
                    return Json(new { });
                if (SessionHelper.Cart._user == null)
                    SessionHelper.Cart.ApplyUser(user.Id.Value);
                return user.ViewResult();
            }
        }

        [HttpPost]
        public void SignOut()
        {
            using (var manager = new UserManager())
                manager.SignOut();
            SessionHelper.Cart = null;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || ExceptionHandled)
            {
                ExceptionHandled = false;
                return;
            }
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
            {
                Data = new { success = false, error = $"Exception: {filterContext.Exception.Message + Environment.NewLine} InnerException: {filterContext.Exception.InnerException}" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
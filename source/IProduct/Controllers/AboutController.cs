using IProduct.Controllers.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IProduct.Controllers
{
    public class AboutController : SharedController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Privacy_Policy()
        {
            return View();
        }
    }
}
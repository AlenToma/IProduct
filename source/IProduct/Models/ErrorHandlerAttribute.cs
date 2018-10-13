using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IProduct.Models
{
    public class ErrorHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
            {
                Data = new { success = false, error = $"Exception: {filterContext.Exception.Message + Environment.NewLine} InnerException: {filterContext.Exception.InnerException}" },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
               
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace IProduct.Models
{
    public class PAuthorize : AuthorizeAttribute
    {
        public PAuthorize()
        {
            base.Roles = IProduct.Modules.Roles.Customers.ToString();
        }

        public PAuthorize(params IProduct.Modules.Roles[] roles)
        {
            if (roles != null && roles.Any())
                base.Roles = string.Join(",", roles.Select(x => x.ToString()));
            else
                base.Roles = IProduct.Modules.Roles.Customers.ToString();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext ctx)
        {
            if (!ctx.HttpContext.User.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(ctx);
            else
            {
                var role = HttpContext.Current.GetOwinContext().Authentication.User.Claims.FirstOrDefault(x => x.Type == "role" || x.Type == ClaimTypes.Role)?.Value;
                if (!(role == IProduct.Modules.Roles.Administrator.ToString() || Roles.Contains(role))) // Role check IsAuthenticated
                {
                    ctx.Result = new ViewResult { ViewName = "Unauthorized" };
                    ctx.HttpContext.Response.StatusCode = 403;
                }
            }
            //base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
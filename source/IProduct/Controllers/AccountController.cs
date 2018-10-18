using EntityWorker.Core.Helper;
using IProduct.Controllers.Shared;
using IProduct.Models;
using IProduct.Modules;
using IProduct.Modules.Library;
using IProduct.Modules.Library.Custom;
using System;
using System.Web.Mvc;

namespace IProduct.Controllers
{
    public class AccountController : ProtectedController
    {
        #region None Authorize Methods
        [AllowAnonymous]
        public ActionResult Index(GenericView<User> user, string type = "")
        {
            if (Request.IsAuthenticated)
                return Redirect("~/Home");
            else if (type.ConvertValue<SignInApplication?>().HasValue)
            {
                using (var manager = new UserManager())
                {
                    if (!Request.IsAuthenticated)
                    {
                        manager.SignIn(type.ConvertValue<SignInApplication>(), user);
                    }
                }

                if (type.ConvertValue<SignInApplication>() == SignInApplication.Cookie && !Request.IsAuthenticated)
                    return View(user.Error("Email or Password could not be found."));
            }
            return View(user ?? new GenericView<User>());
        }

        [AllowAnonymous]
        public ActionResult SignUp(GenericView<User> user)
        {
            try
            {
                if (Request.IsAuthenticated)
                    return Redirect("~/Home");

                if (user == null)
                    user = new GenericView<User>();
                else
                {
                    user.Validate();
                    if (user.View.Password != user.Option_1)
                        user.Error("Password and Confirm password must match");

                    if (user.Success)
                        using (var manager = new UserManager())
                        {
                            DbContext.Save(user.View).SaveChanges();
                            manager.Authorize(user.View, false);
                        }
                }

            }
            catch (Exception e)
            {
                user.Error(e, this);
            }

            return PartialView(user);
        }


        #region Google
        // we may need to add some changes here later as if now, the google provider take care of the login
        [AllowAnonymous]
        public ActionResult Google(string error)
        {
            if (Request.IsAuthenticated)
                return Redirect("~/Home");

            return Redirect("Index");
        }
        #endregion

        #region Facebook
        // we may need to add some changes here later as if now, the google provider take care of the login
        public ActionResult Facebook(string error)
        {
            if (Request.IsAuthenticated)
                return Redirect("~/Home");

            return Redirect("Index");
        }
        #endregion
        #endregion

        #region Authorize Methods
        [AllowAnonymous]
        public new PartialViewResult Profile(GenericView<User> user)
        {
            try
            {
                user.Validate();
                if (user.View.Password != user.Option_1)
                    user.Error("Password and Confirm password must match");

                if (user.Success)
                {
                    var item = user.Join(DbContext.Get<User>().Where(x => x.Id == user.View.Id).LoadChildren().ExecuteFirstOrDefault());
                    using (var manager = new UserManager())
                    {
                        DbContext.Save(item).SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                user.Error(e, this);
            }
            return PartialView(user);
        }

        public ActionResult UserProfile()
        {
            var userId = Request.QueryString["id"].ConvertValue<Guid?>();

            if (userId.HasValue)
            {
                return View(new GenericView<User>(DbContext.Get<User>().Where(x => x.Id == userId).LoadChildren().ExecuteFirstOrDefault()));
            }

            return View();
        }
        #endregion

    }
}
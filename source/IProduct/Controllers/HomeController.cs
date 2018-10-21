using EntityWorker.Core.Helper;
using IProduct.Modules.Library;
using IProduct.Controllers.Shared;
using IProduct.Models;
using System;
using IProduct.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using IProduct.Modules.Library.Custom;

namespace IProduct.Controllers
{
    public class HomeController : SharedController
    {
        #region Action

        public ActionResult LogInView()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Index(Guid? page = null)
        {
            /// test data
            //var products = DbContext.Get<IProduct.Modules.Library.Product>().LoadChildren().Execute();
            //foreach (var p in products)
            //{
            //    for (var i = 1; i <= 100; i++)
            //    {
            //        var clonedProduct = p.ClearAllIdsHierarchy<IProduct.Modules.Library.Product>();
            //        DbContext.Save(clonedProduct);
            //    }
            //}
            //DbContext.SaveChanges();

            if (page.HasValue)
                return View(new GenericView<Pages>(DbContext.Get<Pages>().Where(x => x.Id == page && x.IsActive).LoadChildren().ExecuteFirstOrDefault()));
            else
                return View(new GenericView<Pages>(DbContext.Get<Pages>().Where(x => x.IsActive).OrderBy(x => x.Order).LoadChildren().ExecuteFirstOrDefault()));
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult ShoppingCart()
        {
            return View();
        }

        #endregion

        #region Pages eg Menus 

        [HttpPost]
        public async Task<string> Pages()
        {
            return await DbContext.Get<Pages>().Where(x => x.IsActive && !x.Parent_Id.HasValue).LoadChildren().OrderBy(x => x.Order).JsonAsync();
        }

        [HttpPost]
        public async Task<string> GetPages(Guid? id)
        {
            if (id.HasValue)
                return await DbContext.Get<Pages>().Where(x => x.Id == id).LoadChildren().JsonAsync();
            else
                return await DbContext.Get<Pages>().Where(x => x.IsActive).OrderBy(x => x.Order).LoadChildren().JsonAsync();
        }

        #endregion

        #region Products

        [HttpPost]
        public ActionResult GetProducts(TableTreeSettings tbSettings, List<Guid> categoriesId)
        {
            return DbContext.Search<Product>(tbSettings, x => x.ProductCategories.Any(a => categoriesId.Contains(a.Category_Id)), x => x.Images, x => x.Images.Select(a => a.Images)).ViewResult();
        }

        [HttpGet]
        public ActionResult Product(Guid id)
        {
            return View(DbContext.Get<Product>().Where(x => x.Id == id).LoadChildren().ExecuteFirstOrDefault());
        }

        #endregion

        #region Cart

        [HttpPost]
        public string GetCart()
        {
            return SessionHelper.Cart.Get().ToJson();
        }

        [HttpPost]
        public void UpdateField(string field, string value)
        {
            SessionHelper.Cart.Update(field, value);
        }


        [HttpPost]
        public void AddCart(Guid productid, int count)
        {
            var product = DbContext.Get<Product>().Where(x => x.Id == productid).LoadChildren(x => x.Images, x => x.Images.Select(a => a.Images)).ExecuteFirstOrDefault();
            SessionHelper.Cart.Add(product, count);
        }

        #endregion
    }
}
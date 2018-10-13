using EntityWorker.Core.Helper;
using IProduct.Modules.Library;
using IProduct.Controllers.Shared;
using IProduct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IProduct.Controllers
{
    public class HomeController : SharedController
    {
        #region Action

        public ActionResult LogInView()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
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
            else return await DbContext.Get<Pages>().Where(x => x.IsActive).OrderBy(x=> x.Order).LoadChildren().JsonAsync();
        }

        #endregion

        #region Products

        [HttpPost]
        public async Task<string> GetProducts(List<Guid> categoriesId, int pageNr)
        {
            if (pageNr <= 0)
                pageNr = 1;
            return await DbContext.Get<Product>().Where(x => x.ProductCategories.Any(a => categoriesId.Contains(a.Category_Id))).LoadChildren(x=> x.Images, x=> x.Images.Select(a=> a.Images)).Skip((pageNr -1) * 20).Take(20).JsonAsync();
        }

        [HttpPost]
        public async Task<string> GetProduct(Guid id)
        {
            return await DbContext.Get<Product>().Where(x => x.Id == id).LoadChildren().JsonAsync();
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
            var product = DbContext.Get<Product>().Where(x => x.Id == productid).LoadChildren(x=> x.Images, x=> x.Images.Select(a=> a.Images)).ExecuteFirstOrDefault();
            SessionHelper.Cart.Add(product, count);
        }

        #endregion
    }
}
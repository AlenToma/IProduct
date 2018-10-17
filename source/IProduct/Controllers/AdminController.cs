using EntityWorker.Core.Helper;
using IProduct.Modules.Library;
using IProduct.Controllers.Shared;
using IProduct.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using IProduct.Modules.Library.Custom;
using IProduct.Modules;

namespace IProduct.Controllers
{
    public class AdminController : ProtectedController
    {
        #region Action

        public ActionResult Pages()
        {
            return View();
        }
        public ActionResult Language()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FileBrowser()
        {
            return View();
        }

        #endregion

        #region Category

        [HttpPost]
        public ActionResult GetCategoriesComboBoxItems(string value)
        {
            var data = DbContext.Get<Category>();
            if (!value.ConvertValue<Guid?>().HasValue)
                data.Where(x => (x.Name.Contains(value) || x.Categories.Any(a => x.Name.Contains(value))) && !x.Parent_Id.HasValue).LoadChildren();
            else
            {
                var guid = value.ConvertValue<Guid>();
                data.Where(x => x.Id == guid);
            }
            return data.Execute().ViewResult();
        }
        
        [HttpPost]
        public void SaveCategories(Category category)
        {
            DbContext.Save(category).SaveChanges();
        }

        
        [HttpPost]
        public void DeleteCategories(Guid itemId)
        {
            DbContext.Get<Category>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        
        [HttpPost]
        public ActionResult GetCategories(TableTreeSettings settings)
        {
            return DbContext.Search<Category>(settings, x => (x.Name.Contains(settings.SearchText) || x.Categories.Any(a => x.Name.Contains(settings.SearchText))) && !x.Parent_Id.HasValue).ViewResult();
        }

        #endregion

        #region Products
        
        [HttpPost]
        public ActionResult GetProductsComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Product>().Where(x => x.Name.Contains(value)).LoadChildren().Execute().ViewResult();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Product>().Where(x => x.Id == guid).Execute().ViewResult();
            }
        }
        
        [HttpPost]
        public void SaveProduct(Product product)
        {
            product.Images?.ForEach(x =>
            {
                var id = x.Images.Id;
                var image = DbContext.Get<Files>().Where(a => a.Id == id).LoadChildren().ExecuteFirstOrDefault();
                x.Images = image;
            });
            DbContext.Save(product);
            DbContext.SaveChanges();
        }

        
        [HttpPost]
        public void DeleteProduct(Guid itemId)
        {
            DbContext.Get<Product>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        
        [HttpPost]
        public void DeleteProductImage(Guid id)
        {
            DbContext.Get<ProductImages>().Where(x => x.Id == id).LoadChildren().Remove().SaveChanges();
        }
        
        [HttpPost]
        public ActionResult GetProduct(TableTreeSettings settings)
        {
            return DbContext.Search<Product>(settings, x => x.Name.Contains(settings.SearchText)).ViewResult();
        }

        #endregion

        #region Users
        
        [HttpPost]
        public ActionResult GetUsersComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<User>().Where(x => x.Email.Contains(value) || x.Person.FirstName.Contains(value) || x.Person.LastName.Contains(value)).LoadChildren().Execute().ViewResult();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<User>().Where(x => x.Id == guid).Execute().ViewResult();
            }
        }
        
        [HttpPost]
        public void SaveUser(User user)
        {

            DbContext.Save(user).SaveChanges();
        }

        
        [HttpPost]
        public void DeleteUser(Guid itemId)
        {
            DbContext.Get<User>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        
        [HttpPost]
        public ActionResult GetUser(TableTreeSettings settings)
        {
            return DbContext.Search<User>(settings, x => x.Email.Contains(settings.SearchText) || x.Person.FirstName.Contains(settings.SearchText) || x.Person.LastName.Contains(settings.SearchText)).ViewResult();
        }


        #endregion

        #region Country

        
        [HttpPost]
        public ActionResult GetCountryComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Country>().Where(x => x.Name.Contains(value)).LoadChildren().Execute().ViewResult();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Country>().Where(x => x.Id == guid).Execute().ViewResult();
            }
        }
        
        [HttpPost]
        public void SaveCountry(Country country)
        {
            DbContext.Save(country).SaveChanges();
        }


        [HttpPost]
        public ActionResult GetCountry(TableTreeSettings settings)
        {
            return DbContext.Search<Country>(settings, x => x.Name.Contains(settings.SearchText) || x.CountryCode.Contains(settings.SearchText)).ViewResult();
        }

        #endregion

        #region Column
        
        [HttpPost]
        public ActionResult GetColumnComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Column>().Where(x => x.Key.Contains(value)).LoadChildren().Execute().ViewResult();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Country>().Where(x => x.Id == guid).Execute().ViewResult();
            }
        }
        
        [HttpPost]
        public void SaveColumn(Column column)
        {
            DbContext.Save(column).SaveChanges();
        }
        
        [HttpPost]
        public void DeleteColumn(Guid itemId)
        {
            DbContext.Get<Column>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        
        [HttpPost]
        public ActionResult GetColumn(TableTreeSettings settings)
        {
            return DbContext.Search<Column>(settings, x => x.Key.Contains(settings.SearchText)).ViewResult();
        }

        #endregion

        #region Role

        
        [HttpPost]
        public ActionResult GetRoleComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Role>().Where(x => x.Name.Contains(value)).LoadChildren().Execute().ViewResult();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Role>().Where(x => x.Id == guid).Execute().ViewResult();
            }
        }

        #endregion

        #region Pages

        
        [HttpPost]
        public ActionResult GetPagesComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Pages>().Where(x => !x.Parent_Id.HasValue && (x.Name.Contains(value) || x.Children.Any(a => a.Name.Contains(value)))).LoadChildren().Execute().ViewResult();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Pages>().Where(x => x.Id == guid).ViewResult();
            }
        }
        
        [HttpPost]
        public void SavePages(Pages page)
        {
            DbContext.Save(page).SaveChanges();
        }

        
        [HttpPost]
        public void DeletePages(Guid itemId)
        {
            DbContext.Get<Pages>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }

        
        [HttpPost]
        public ActionResult GetPages(TableTreeSettings settings)
        {
            return DbContext.Search<Pages>(settings, x => x.Name.Contains(settings.SearchText) || x.Children.Any(a => a.Name.Contains(settings.SearchText))).ViewResult();
        }

        #endregion
    }
}
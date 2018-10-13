using EntityWorker.Core.Helper;
using IProduct.Modules.Library;
using IProduct.Controllers.Shared;
using IProduct.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IProduct.Controllers
{
    public class AdminController : SharedController
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
        [ErrorHandler]
        public CallbackJsonResult GetCategoriesComboBoxItems(string value)
        {
            var data = DbContext.Get<Category>();
            if (!value.ConvertValue<Guid?>().HasValue)
                data.Where(x => (x.Name.Contains(value) || x.Categories.Any(a => x.Name.Contains(value))) && !x.Parent_Id.HasValue).LoadChildren();
            else
            {
                var guid = value.ConvertValue<Guid>();
                data.Where(x => x.Id == guid);
            }
            return new CallbackJsonResult(data.Execute());
        }
        [ErrorHandler]
        [HttpPost]
        public void SaveCategories(Category category)
        {
            DbContext.Save(category).SaveChanges();
        }

        [ErrorHandler]
        [HttpPost]
        public void DeleteCategories(Guid itemId)
        {
            DbContext.Get<Category>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetCategories(TableTreeSettings settings)
        {

            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<Category>().Where(x => (x.Name.Contains(text) || x.Categories.Any(a => x.Name.Contains(text))) && !x.Parent_Id.HasValue).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return new CallbackJsonResult(settings);

        }

        #endregion

        #region Products
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetProductsComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return new CallbackJsonResult(DbContext.Get<Product>().Where(x => x.Name.Contains(value)).LoadChildren().Execute());
            else
            {
                var guid = value.ConvertValue<Guid>();
                return new CallbackJsonResult(DbContext.Get<Product>().Where(x => x.Id == guid).Execute());
            }
        }
        [ErrorHandler]
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

        [ErrorHandler]
        [HttpPost]
        public void DeleteProduct(Guid itemId)
        {
            DbContext.Get<Product>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        [ErrorHandler]
        [HttpPost]
        public void DeleteProductImage(Guid id)
        {
            DbContext.Get<ProductImages>().Where(x => x.Id == id).LoadChildren().Remove().SaveChanges();
        }
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetProduct(TableTreeSettings settings)
        {

            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<Product>().Where(x => x.Name.Contains(text)).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return new CallbackJsonResult(settings);

        }

        #endregion

        #region Users
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetUsersComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return new CallbackJsonResult(DbContext.Get<User>().Where(x => x.Email.Contains(value) || x.Person.FirstName.Contains(value) || x.Person.LastName.Contains(value)).LoadChildren().Execute());
            else
            {
                var guid = value.ConvertValue<Guid>();
                return new CallbackJsonResult(DbContext.Get<User>().Where(x => x.Id == guid).Execute());
            }
        }
        [ErrorHandler]
        [HttpPost]
        public void SaveUser(User user)
        {
            DbContext.Save(user).SaveChanges();
        }

        [ErrorHandler]
        [HttpPost]
        public void DeleteUser(Guid itemId)
        {
            DbContext.Get<User>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetUser(TableTreeSettings settings)
        {

            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<User>().Where(x => x.Email.Contains(text) || x.Person.FirstName.Contains(text) || x.Person.LastName.Contains(text)).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return new CallbackJsonResult(settings);

        }


        #endregion

        #region Country

        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetCountryComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return new CallbackJsonResult(DbContext.Get<Country>().Where(x => x.Name.Contains(value)).LoadChildren().Execute());
            else
            {
                var guid = value.ConvertValue<Guid>();
                return new CallbackJsonResult(DbContext.Get<Country>().Where(x => x.Id == guid).Execute());
            }
        }
        [ErrorHandler]
        [HttpPost]
        public void SaveCountry(Country country)
        {
            DbContext.Save(country).SaveChanges();
        }


        [HttpPost]
        public CallbackJsonResult GetCountry(TableTreeSettings settings)
        {

            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<Country>().Where(x => x.Name.Contains(text) || x.CountryCode.Contains(text)).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return new CallbackJsonResult(settings);
        }

        #endregion

        #region Column
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetColumnComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return new CallbackJsonResult(DbContext.Get<Column>().Where(x => x.Key.Contains(value)).LoadChildren().Execute());
            else
            {
                var guid = value.ConvertValue<Guid>();
                return new CallbackJsonResult(DbContext.Get<Country>().Where(x => x.Id == guid).Execute());
            }
        }
        [ErrorHandler]
        [HttpPost]
        public void SaveColumn(Column column)
        {
            DbContext.Save(column).SaveChanges();
        }
        [ErrorHandler]
        [HttpPost]
        public void DeleteColumn(Guid itemId)
        {
            DbContext.Get<Column>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }
        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetColumn(TableTreeSettings settings)
        {
            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<Column>().Where(x => x.Key.Contains(text)).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return new CallbackJsonResult(settings);
        }

        #endregion

        #region Role

        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetRoleComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return new CallbackJsonResult(DbContext.Get<Role>().Where(x => x.Name.Contains(value)).LoadChildren().Execute());
            else
            {
                var guid = value.ConvertValue<Guid>();
                return new CallbackJsonResult(DbContext.Get<Role>().Where(x => x.Id == guid).Execute());
            }
        }

        #endregion

        #region Pages

        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetPagesComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return new CallbackJsonResult(DbContext.Get<Pages>().Where(x => !x.Parent_Id.HasValue && (x.Name.Contains(value) || x.Children.Any(a => a.Name.Contains(value)))).LoadChildren().Execute());
            else
            {
                var guid = value.ConvertValue<Guid>();
                return new CallbackJsonResult(DbContext.Get<Pages>().Where(x => x.Id == guid));
            }
        }
        [ErrorHandler]
        [HttpPost]
        public void SavePages(Pages page)
        {
            DbContext.Save(page).SaveChanges();
        }

        [ErrorHandler]
        [HttpPost]
        public void DeletePages(Guid itemId)
        {
            DbContext.Get<Pages>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }

        [ErrorHandler]
        [HttpPost]
        public CallbackJsonResult GetPages(TableTreeSettings settings)
        {

            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<Pages>().Where(x => x.Name.Contains(text) || x.Children.Any(a => a.Name.Contains(text))).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return new CallbackJsonResult(settings);

        }

        #endregion
    }
}
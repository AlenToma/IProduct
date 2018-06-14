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
        public string GetCategoriesComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Category>().Where(x => (x.Name.Contains(value) || x.Categories.Any(a => x.Name.Contains(value))) && !x.Parent_Id.HasValue).LoadChildren().Json();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Category>().Where(x => x.Id == guid).Json();
            }
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
        public string GetCategories(TableTreeSettings settings)
        {

            var text = settings.SearchText ?? "";
            if (settings.SelectedPage <= 0)
                settings.SelectedPage = 1;
            if (settings.PageSize <= 0)
                settings.PageSize = 20;
            var data = DbContext.Get<Category>().Where(x => (x.Name.Contains(text) || x.Categories.Any(a => x.Name.Contains(text)))).LoadChildren();
            if (!string.IsNullOrEmpty(settings.SortColumn))
            {
                if (settings.Sort != "desc")
                    data = data.OrderBy(settings.SortColumn);
                else data = data.OrderByDescending(settings.SortColumn);
            }
            settings.TotalPages = Math.Ceiling(data.ExecuteCount().ConvertValue<decimal>() / settings.PageSize).ConvertValue<int>();
            data = data.Skip(settings.SelectedPage / settings.PageSize).Take(settings.PageSize);
            settings.Result = data.Execute();
            return settings.ToJson();

        }

        #endregion

        #region Products

        [HttpPost]
        public string GetProductsComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Product>().Where(x => x.Name.Contains(value)).LoadChildren().Json();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Product>().Where(x => x.Id == guid).Json();
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
        public string GetProduct(TableTreeSettings settings)
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
            return settings.ToJson();

        }

        #endregion

        #region Users
        [HttpPost]
        public string GetUsersComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<User>().Where(x => x.Email.Contains(value) || x.Person.FirstName.Contains(value) || x.Person.LastName.Contains(value)).LoadChildren().Json();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<User>().Where(x => x.Id == guid).Json();
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
        public string GetUser(TableTreeSettings settings)
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
            return settings.ToJson();

        }


        #endregion

        #region Country


        [HttpPost]
        public string GetCountryComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Country>().Where(x => x.Name.Contains(value)).LoadChildren().Json();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Country>().Where(x => x.Id == guid).Json();
            }
        }

        #endregion

        #region Role


        [HttpPost]
        public string GetRoleComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Role>().Where(x => x.Name.Contains(value)).LoadChildren().Json();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Role>().Where(x => x.Id == guid).Json();
            }
        }

        #endregion

        #region Pages
        [HttpPost]
        public string GetPagesComboBoxItems(string value)
        {
            if (!value.ConvertValue<Guid?>().HasValue)
                return DbContext.Get<Pages>().Where(x => !x.Parent_Id.HasValue && (x.Name.Contains(value) || x.Children.Any(a => a.Name.Contains(value)))).LoadChildren().Json();
            else
            {
                var guid = value.ConvertValue<Guid>();
                return DbContext.Get<Pages>().Where(x => x.Id == guid).Json();
            }
        }

        [HttpPost]
        public void SavePages(Pages page)
        {
            page.PagesSliders?.ForEach(x =>
            {
                var id = x.Files.Id;
                x.Files.File = DbContext.Get<Files>().Where(a => a.Id == id).ExecuteFirstOrDefault().File;
            });

            DbContext.Save(page).SaveChanges();
        }


        [HttpPost]
        public void DeletePages(Guid itemId)
        {
            DbContext.Get<Pages>().Where(x => x.Id == itemId).LoadChildren().Remove().SaveChanges();
        }

        [HttpPost]
        public string GetPages(TableTreeSettings settings)
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
            return settings.ToJson();

        }

        #endregion
    }
}
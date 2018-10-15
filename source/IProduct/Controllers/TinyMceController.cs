using EntityWorker.Core.Helper;
using IProduct.Modules;
using IProduct.Modules.Library;
using IProduct.Controllers.Shared;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IProduct.Models;

namespace IProduct.Controllers
{
    public class TinyMceController : SharedController
    {
        // GET: TinyMce
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string data)
        {
            var message = "Hello " + data;
            return Content(message);
        }

        #region Mapps
        [HttpPost]
        public async Task<string> GetMaps(string value)
        {
            value = value ?? "";
            var id = value.ConvertValue<Guid?>();
            if (!id.HasValue)
                return await DbContext.Get<Mapps>().Where(x => !x.Parent_Id.HasValue && (x.Name.Contains(value) || x.Children.Any(a => a.Name.Contains(value)))).IgnoreChildren(x => x.Files).LoadChildren().JsonAsync();
            else return await DbContext.Get<Mapps>().Where(x => x.Id == id).IgnoreChildren(x => x.Files).JsonAsync();
        }

        [HttpPost]
        public void SaveMapps(Mapps mapps)
        {
            DbContext.Save(mapps);
            DbContext.SaveChanges();
        }

        [HttpPost]
        public void DeleteMapps(Guid id)
        {
            DbContext.Get<Mapps>().Where(x => x.Id == id && !x.System).LoadChildren().Remove().Save().SaveChanges();
        }

        #endregion
        #region Files 
        [ErrorHandler]
        [HttpPost]
        public void DeleteFile(Guid id)
        {
            var file = DbContext.Get<Files>().Where(x => x.Id == id).ExecuteFirstOrDefault();
            var mapp = DbContext.Get<Mapps>().Where(x => x.Id == file.Mapp_Id).ExecuteFirstOrDefault();
            var path = Path.Combine(GlobalConfigration.FileBasePath, mapp.Name, file.FilePath);
            var thumpPath = Path.Combine(GlobalConfigration.FileBasePath, "Thumps", file.FilePath);
            DbContext.Get<Files>().Where(x => x.Id == id).Remove().SaveChanges();
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            if (System.IO.File.Exists(thumpPath))
                System.IO.File.Delete(thumpPath);
        }

        [ErrorHandler]
        [HttpPost]
        public async Task<string> GetFiles(Guid mappId)
        {
            return await DbContext.Get<Files>().Where(x => x.Mapp_Id == mappId).JsonAsync();
        }

        [ErrorHandler]
        [HttpGet]
        public ActionResult GetImageFile(string uuid)
        {
            ActionResult actionResult = null;
            var imageId = uuid.ConvertValue<Guid>();
            var fileImage = DbContext.Get<Files>().Where(x => x.Id == imageId).ExecuteFirstOrDefault();
            if (fileImage != null)
            {
                actionResult = new FileContentResult(fileImage.FileBytes, fileImage.Type);
            }
            return actionResult;
        }


        [HttpGet]
        [ErrorHandler]
        public ActionResult GetIcon(string uuid, int width, int height)
        {
            ActionResult actionResult = null;
            var imageId = uuid.ConvertValue<Guid>();
            var fileImage = DbContext.Get<Files>().Where(x => x.Id == imageId).ExecuteFirstOrDefault();
            if (fileImage != null)
                actionResult = new FileContentResult(Actions.GenerateThumbImage(fileImage.FileBytes, width, height), fileImage.Type);

            return actionResult;
        }

        [ErrorHandler]
        [HttpPost]
        public string TinyMceUpload(HttpPostedFileBase file, Guid mappId)
        {

            var location = SaveFile(file, mappId);

            return location;

        }

        /// <summary>
        /// Saves the contents of an uploaded image file.
        /// </summary>
        /// <param name="targetFolder">Location where to save the image file.</param>
        /// <param name="file">The uploaded image file.</param>
        /// <exception cref="InvalidOperationException">Invalid MIME content type.</exception>
        /// <exception cref="InvalidOperationException">Invalid file extension.</exception>
        /// <exception cref="InvalidOperationException">File size limit exceeded.</exception>
        /// <returns>The relative path where the file is stored.</returns>
        private string SaveFile(HttpPostedFileBase file, Guid mappId)
        {
            UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
            const int megabyte = 1024 * 1024;

            if (!file.ContentType.StartsWith("image/"))
            {
                throw new InvalidOperationException("Invalid MIME content type.");
            }
            var name = file.FileName.Split('\\').Last().Split('.').First();
            var extension = Path.GetExtension(file.FileName.ToLowerInvariant());
            string[] extensions = { ".gif", ".jpg", ".png", ".svg", ".webp" };
            //if (!extensions.Contains(extension))
            //{
            //    throw new InvalidOperationException("Invalid file extension.");
            //}

            if (file.ContentLength > (8 * megabyte))
            {
                throw new InvalidOperationException("File size limit exceeded.");
            }

            byte[] image = new byte[file.ContentLength];
            file.InputStream.Read(image, 0, image.Length);
            var fileName = $"{name}.{extension}";
            var img = DbContext.Get<Files>().Where(x => x.FriendlyName == fileName && x.Mapp_Id == mappId).ExecuteFirstOrDefault();
            if (img == null)
            {

                img = new Files()
                {
                    FileBytes = image,
                    Type = extension,
                    FriendlyName = fileName,
                    FilePath = fileName,
                    Mapp_Id = mappId
                };
            }
            else img.FileBytes = image;
            DbContext.Save(img);
            DbContext.SaveChanges();


            return u.Action("GetImageFile", "TinyMce", new { uuid = img.Id });
        }

        #endregion
    }
}
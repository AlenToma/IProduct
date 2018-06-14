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

        [HttpPost]
        public string DeleteFile(Guid id)
        {
            try
            {
                DbContext.Get<Files>().Where(x => x.Id == id).Remove().SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToJson();
            }
        }

        [HttpPost]
        public async Task<string> GetFiles(Guid mappId)
        {
            return await DbContext.Get<Files>().Where(x => x.Mapp_Id == mappId).JsonAsync();
        }


        [HttpGet]
        public ActionResult GetImageFile(string uuid)
        {
            ActionResult actionResult = null;
            var imageId = uuid.ConvertValue<Guid>();
            var fileImage = DbContext.Get<Files>().Where(x => x.Id == imageId).ExecuteFirstOrDefault();
            if (fileImage != null)
            {
                actionResult = new FileContentResult(fileImage.File, fileImage.Type);
            }
            return actionResult;
        }


        [HttpGet]
        public ActionResult GetIcon(string uuid, int width, int height)
        {
            ActionResult actionResult = null;
            var imageId = uuid.ConvertValue<Guid>();
            var fileImage = DbContext.Get<Files>().Where(x => x.Id == imageId).ExecuteFirstOrDefault();
            if (fileImage != null)
                actionResult = new FileContentResult(Actions.GenerateThumbImage(fileImage.File, width, height), fileImage.Type);

            return actionResult;
        }

        [HttpPost]
        public ActionResult TinyMceUpload(HttpPostedFileBase file, Guid mappId)
        {
            //Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var location = SaveFile(file, mappId);

            return Json(new { location }, JsonRequestBehavior.AllowGet);
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
                    File = image,
                    Type = extension,
                    FriendlyName = fileName,
                    Mapp_Id = mappId
                };
            }
            else img.File = image;
            DbContext.Save(img);
            DbContext.SaveChanges();


            return u.Action("GetImageFile", "TinyMce", new { uuid = img.Id });
        }

        #endregion
    }
}
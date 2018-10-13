using EntityWorker.Core.LiteDB;
using IProduct.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IProduct
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            EntityWorker.Core.GlobalConfiguration.DataEncode_Key = "IProduct default Encoder";

            EntityWorker.Core.GlobalConfiguration.JSONParameters.JsonFormatting = EntityWorker.Core.Helper.JsonFormatting.LowerCase;
            EntityWorker.Core.GlobalConfiguration.JSONParameters.UseFastGuid = false;
            EntityWorker.Core.GlobalConfiguration.Log = new IProduct.Modules.Library.Base_Entity.Logger();
            GlobalConfigration.FileBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ImagePath"]);
            GlobalConfigration.ImageMapp = ConfigurationManager.AppSettings["ImagePath"];
            GlobalConfigration.LoadSettings(new IProduct.Modules.Data.DbContext());
           
        }

        protected void Application_End()
        {
            EntityWorker.Core.GlobalConfiguration.Log.Dispose();
        }
    }
}

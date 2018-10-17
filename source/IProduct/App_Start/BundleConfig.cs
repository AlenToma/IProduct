using System.Web;
using System.Web.Optimization;

namespace IProduct
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                         "~/Scripts/jqUi.js",
                         "~/Scripts/tinymce/tinymce.js",
                         "~/Scripts/tinymce/jquery.tinymce.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                      "~/Content/custom/tabs.css",
                      "~/Content/custom/dialog.css",
                      "~/Content/custom/autofill.css",
                      "~/Content/custom/verticalMenu.css",
                      "~/Content/custom/filebrowser.css",
                       "~/Content/custom/ToolTip.css",
                       "~/Content/custom/Menu.css",
                       "~/Content/slick/slick.css",
                       "~/Content/slick/slick-theme.css",
                       "~/Content/custom/ContextMenu.css",
                       "~/Content/custom/Treeview.css",
                       "~/Content/custom/login.css",
                       "~/Content/custom/checkbox.css",
                       "~/Content/custom/portalMenu.css",
                       "~/Content/HomePage.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include(
                      "~/Content/site/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/My").Include(
                        "~/Scripts/My/actions.js",
                        "~/Scripts/My/tabs.js",
                        "~/Scripts/My/dialog.js",
                        "~/Scripts/My/autofill.js",
                        "~/Scripts/My/ajax.js",
                        "~/Scripts/My/verticalMenu.js",
                        "~/Scripts/My/ToolTip.js",
                        "~/Scripts/My/lightslider.js",
                        "~/Scripts/My/Cart.js",
                        "~/Scripts/My/ContextMenu.js",
                        "~/Scripts/My/Login.js",
                        "~/Scripts/My/Treeview.js",
                        "~/Scripts/My/checkbox.js",
                        "~/Scripts/My/jquery.fittext.js",
                        "~/Content/slick/slick.js",
                        "~/Scripts/My/portalMenu.js"));
        }
    }
}

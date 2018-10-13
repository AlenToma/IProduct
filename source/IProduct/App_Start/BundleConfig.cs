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
                        "~/Scripts/jquery-{version}.js").Include(
                        "~/Scripts/jqUi.js").Include(
                        "~/Scripts/tinymce/tinymce.js").Include(
                        "~/Scripts/tinymce/jquery.tinymce.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery-ui.css"));

            bundles.Add(new StyleBundle("~/Content/My").Include(
                      "~/Content/My/tabs.css",
                      "~/Content/My/dialog.css",
                      "~/Content/My/autofill.css",
                      "~/Content/My/verticalMenu.css",
                      "~/Content/My/filebrowser.css",
                       "~/Content/My/ToolTip.css",
                       "~/Content/My/Menu.css",
                       "~/Content/My/lightslider.css",
                       "~/Content/My/ContextMenu.css",
                       "~/Content/My/Treeview.css"));


            bundles.Add(new StyleBundle("~/Navigation Menu/css").Include(
                      "~/Navigation Menu/css/normalize.css",
                       "~/Navigation Menu/css/defaults.css",
                       "~/Navigation Menu/css/nav-core.css",
                       "~/Navigation Menu/css/nav-layout.css"));

            bundles.Add(new ScriptBundle("~/Navigation Menu/js").Include(
                "~/Navigation Menu/js/rem.min.js",
                "~/Navigation Menu/js/nav.jquery.min.js"));

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
                        "~/Scripts/My/jquery.fittext.js"));
        }
    }
}

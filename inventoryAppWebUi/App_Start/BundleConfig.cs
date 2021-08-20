using System.Web;
using System.Web.Optimization;

namespace inventoryAppWebUi
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-3.4.1.min.js",
                "~/Content/assets/js/backend-bundle.min.js",
                "~/Content/assets/js/customizer.js",
                "~/Content/assets/js/chart-custom.js",
                "~/Content/datatable/datatables.min.js",
                "~/Content/assets/js/app.js"));


            bundles.Add(new StyleBundle("~/Content/template/css").Include(
                      "~/Content/assets/css/backend-plugin.min.css",
                      "~/Content/datatable/datatables.min.css",
                      "~/Content/assets/css/backende209.css?v=1.0.0",
                      "~/Content/assets/vendor/%40fortawesome/fontawesome-free/css/all.min.css",
                      "~/Content/assets/vendor/line-awesome/dist/line-awesome/css/line-awesome.min.css",
                      "~/Content/assets/vendor/remixicon/fonts/remixicon.css"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxUnobtrusive").Include(
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                        "~/Scripts/jquery-3.4.1.min.js",
                        "~/Content/datatable/datatables.min.js",
                        "~/Content/datatable/datatables.min.css"));

        }
    }
}

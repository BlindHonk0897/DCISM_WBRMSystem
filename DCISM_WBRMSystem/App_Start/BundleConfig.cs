using System.Web;
using System.Web.Optimization;

namespace DCISM_WBRMSystem
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/AllStyling").Include(
                      "~/Content/assets/fonts/fontawesome/css/fontawesome-all.min.css",
                      "~/Content/assets/plugins/animation/css/animate.min.css",
                      "~/Content/assets/css/style.css",
                      "~/Content/chart-morris/css/morris.css",
                      "~/Content/demo/demo.css",
                      "~/Content/forSelect2/select2.min.css",
                      "~/Content/assets/css/datepicker3.css",
                      "~/Content/forCalendar/assets/css/fullcalendar.css",
                      "~/Content/forCalendar/assets/css/fullcalendar.print.css",
                      "~/Content/assets/css/daterangepicker.css",
                      "~/Content/toastr/toastr.min.css",
                      "~/Content/pnotify/pnotify.css",
                      "~/Content/pnotify/pnotifycustom.min.css",
                      "~/Content/charts/canvas.min.js",
                      "~/Content/tour/enjoyhint.css",
                      "~/Content/datepicker/materialdatepicker.css",
                      "~/Content/modal/md-modal.css",
                      "~/Content/lightbox/ekko/ekko-lightbox.min.css",
                      "~/Content/lightbox/lightbox2/lightbox2.min.css",
                      "~/Content/wizard/css/smart-wizard.min.css",
                      "~/Content/wizard/css/smart_arrow.min.css",
                      "~/Content/jstree/Style.min.css",
                      "~/Content/jstree/32px.png",
                      "~/Content/jstree/throbber.gif",
                      "~/Content/footable/footable.standalone.css",
                      "~/Content/footable/datatable.min.css",
                      "~/Content/notification/notification.min.css",
                      "~/Content/assets/plugins/jquery-scrollbar/css/jquery.scrollbar.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/AllScripting").Include(
                      "~/Content/assets/js/vendor-all.min.js",
                      "~/Content/assets/plugins/bootstrap/js/bootstrap.min.js",
                      "~/Content/assets/js/pcoded.min.js",
                      "~/Content/forSelect2/select2.min.js",
                      "~/Content/assets/js/bootstrap-datepicker3.js",
                      "~/Content/assets/js/promise.min.js",
                      "~/Content/assets/js/sweet-alert.js",
                      "~/Content/forCalendar/assets/js/fullcalendar.js",
                      "~/Content/assets/js/moment.min.js",
                      "~/Content/assets/js/daterangepicker.js",
                      "~/Content/toastr/toastr.min.js",
                      "~/Content/pnotify/pnotifycustom.min.js",
                      "~/Content/pnotify/notify-event.js",
                      "~/Content/datepicker/materialdatepicker.js",
                      "~/Content/datepicker/datepicker.js",
                      "~/Content/modal/classie.js",
                      "~/Content/modal/modalEffect.js",
                      "~/Content/assets/js/pages/custom.js",
                      "~/Content/lightbox/ekko/ekko-lightbox.min.js",
                      "~/Content/lightbox/lightbox2/lightbox2.min.js",
                      "~/Content/jstree/jstree.min.js",
                      "~/Content/wizard/js/jquery.smartWizard.min.js",
                      "~/Content/notification/bootstrap-growl.min.js",
                      "~/Content/assets/plugins/bootstrap/js/bootstrap.min.js",
                      "~/Content/assets/js/perfectScrollbar.js",
                      "~/Scripts/jquery.signalR-2.4.1.min.js",
                      "~/Scripts/Chart.js"));

        }
    }
}

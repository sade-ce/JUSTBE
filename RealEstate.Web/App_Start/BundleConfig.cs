using System.Web;
using System.Web.Optimization;

namespace RealEstate.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/Content/backend").Include(
                "~/Content/backendcss/font-awesome.css",
                     "~/Content/Plugins/Select2/select2.min.css"
                    ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/skin-blue.css",
                      "~/Content/ns-pager-style.css",
                      "~/Content/backendcss/font-awesome.css",
                     "~/Content/Plugins/Select2/select2.min.css",
                     "~/Content/Site.css")
                     );

            bundles.Add(new StyleBundle("~/Content/cssnew").Include(
                      "~/Content/ns-pager-style.css"
                    )
                     );

            bundles.Add(new StyleBundle("~/Content/JayBauerFrontEndCSS").Include(
                  "~/Content/CssFront/style.css",
                     "~/Content/CssFront/Cryan.css",
                 "~/Content/CssFront/icons.css",
                "~/Content/CssFront/bootstrap.css"


                  )
                 );

            bundles.Add(new StyleBundle("~/Content/Home").Include(
                  "~/Content/slider/demo.css",
                  "~/Content/slider/style1.css",
                  "~/Content/PacContainer.css",
                  "~/Content/popup.css"
                )
               );

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                    "~/Content/adminjs/app.min.js",
                    "~/Scripts/CustomAjax.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/ClientJs").Include(
                    "~/Content/Plugins/swipebox/lib/jquery-2.1.0.min.js",
                    "~/Content/Plugins/swipebox/jquery.swipebox.min.js",
                    "~/Content/Plugins/highcharts/highcharts.js",
                    "~/Content/Plugins/aslider/jssor.slider-26.1.5.min.js"
                    //"~/Content/Plugins/clientcustomjs/custom.js"
                    ));


            bundles.Add(new ScriptBundle("~/bundles/RealEstateFrontEndJS").Include(

                 "~/Content/FrontEndJS/custom.js",
                 "~/Content/FrontEndJS/chosen.min.js",
                 "~/Content/FrontEndJS/jquery.jpanelmenu.js",
                 "~/Content/FrontEndJS/magnific-popup.min.js",
                 "~/Content/FrontEndJS/owl.carousel.min.js",
                 "~/Content/FrontEndJS/slick.min.js",
                  "~/Content/FrontEndJS/tooltips.min.js"
                   ));
            //dropzone
            bundles.Add(new StyleBundle("~/Content/dropzonescss").Include(
                     "~/Scripts/dropzone/css/basic.css",
                     "~/Scripts/dropzone/css/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                     "~/Scripts/dropzone/dropzone.js"));


            bundles.Add(new StyleBundle("~/201Murray/JBGHomeCss").Include(
                 "~/201Murray/font/demo-files/demo.css",
                 "~/201Murray/plugins/fancybox/jquery.fancybox.css",
                 "~/201Murray/plugins/revolution/css/settings.css",
                 "~/201Murray/plugins/revolution/css/layers.css",
                  "~/201Murray/plugins/revolution/css/navigation.css",
                 "~/201Murray/plugins/revolution/css/layers.css"
               )
              );
            bundles.Add(new StyleBundle("~/201Murray/JBGCommonCss").Include(
               "~/201Murray/css/bootstrap.min.css",
               "~/201Murray/css/fontello.css",
               "~/201Murray/css/owl.carousel.css",
               "~/201Murray/css/style.css",
                "~/201Murray/css/responsive.css",
                "~/201Murray/TempFIles/dropnav.css"


             )
            );


            bundles.Add(new ScriptBundle("~/bundles/JBGHomeJs").Include(
               "~/201Murray/js/libs/jquery.modernizr.js",
               "~/201Murray/js/libs/jquery-2.2.4.min.js",
               "~/201Murray/js/libs/jquery-ui.min.js",
               "~/201Murray/js/libs/retina.min.js",
               "~/201Murray/plugins/fancybox/jquery.fancybox.min.js",
               "~/201Murray/plugins/classie.js",
               "~/201Murray/plugins/pathLoader.js",
               "~/201Murray/plugins/owl.carousel.min.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/JBGCommonJs").Include(
            "~/201Murray/js/plugins.js",
            "~/201Murray/js/script.js"
             ));

            //Client Dashboard



            bundles.Add(new StyleBundle("~/Content/Dashboard").Include(
                 "~/Content/AppDashboard/assets/fonts/font-awesome/css/font-awesome.min.css",
                 "~/Content/AppDashboard/assets/libraries/owl-carousel/owl.carousel.css",
                 "~/Content/AppDashboard/assets/libraries/chartist/chartist.min.css",
                 "~/Content/AppDashboard/assets/css/leaflet.css",
                 "~/Content/AppDashboard/assets/css/leaflet.markercluster.css",
                 "~/Content/AppDashboard/assets/css/leaflet.markercluster.default.css",
                 "~/Content/AppDashboard/assets/css/villareal-turquoise.css"
               )
              );

            bundles.Add(new ScriptBundle("~/bundles/ClientDash").Include(
                          //"~/Content/AppDashboard/assets/js/jquery.js",
                          "~/201Murray/js/libs/jquery-2.2.4.min.js",
                        "~/Content/AppDashboard/assets/js/jquery.ezmark.min.js",
                        "~/Content/AppDashboard/assets/js/tether.min.js",
                        "~/Content/AppDashboard/assets/js/bootstrap.min.js",
                        "~/Content/AppDashboard/assets/js/gmap3.min.js",
                        "~/Content/AppDashboard/assets/js/leaflet.js",
                        "~/Content/AppDashboard/assets/js/leaflet.markercluster.js",
                        "~/Content/AppDashboard/assets/libraries/owl-carousel/owl.carousel.min.js",
                        "~/Content/AppDashboard/assets/libraries/chartist/chartist.min.js",
                        //"~/Content/AppDashboard/assets/js/scrollPosStyler.js",
                         "~/Content/AppDashboard/assets/js/villareal.js"
    ));

            //BundleTable.EnableOptimizations = true;
        }
    }
}

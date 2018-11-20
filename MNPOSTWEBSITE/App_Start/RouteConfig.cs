using MNPOSTWEBSITE.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MNPOSTWEBSITE
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
             name: "Login",
             url: "tai-khoan/dang-nhap",
             defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
         );

            routes.MapRoute(
           name: "Register",
           url: "tai-khoan/dang-ky",
           defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional }
       );

            routes.MapRoute(
          name: "Forget",
          url: "tai-khoan/quen-mat-khau",
          defaults: new { controller = "Account", action = "Forget", id = UrlParameter.Optional }
      );

            routes.MapRoute(
            name: "Home",
            url: "trang-chu/index",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "AboutUs",
               url: "gioi-thieu/ve-chung-toi",
               defaults: new { controller = "AboutUs", action = "Introduce", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Contact",
              url: "gioi-thieu/lien-he",
              defaults: new { controller = "AboutUs", action = "Contact", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "Service",
               url: "van-chuyen/dich-vu",
               defaults: new { controller = "Transport", action = "Service", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Recruitment",
               url: "van-chuyen/tuyen-dung",
               defaults: new { controller = "Transport", action = "Recruitment", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "Post",
               url: "bai-viet/danh-sach-bai-viet",
               defaults: new { controller = "Post", action = "PostList", id = UrlParameter.Optional }
           );

            routes.Add("PostDetails", new SeoFriendlyRoute("bai-viet/chi-tiet/{id}",
           new RouteValueDictionary(new { controller = "Post", action = "DetailPost" }),
           new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

﻿using System.Web.Http;

namespace M3.Website
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "GetGalleryDetailApi",
               routeTemplate: "api/{controller}/Detail/{id}",
               defaults: new { action = "Detail", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "GetGalleryPreviewApi",
               routeTemplate: "api/{controller}/Preview/{id}",
               defaults: new { action = "Preview", id = RouteParameter.Optional }
            );
        }
    }
}

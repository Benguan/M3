using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace M3.Website
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "GalleryDetailApi",
               routeTemplate: "api/{controller}/Detail/{id}/{page}",
               defaults: new { action = "Detail", page = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "GalleryDetailsApi",
               routeTemplate: "api/{controller}/Details/{id}/{pages}",
               defaults: new { action = "Details", pages = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "GalleryPreviewApi",
               routeTemplate: "api/{controller}/Preview/{id}",
               defaults: new { action = "Preview", id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}

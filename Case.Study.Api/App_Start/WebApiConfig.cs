using Case.Study.Api.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Case.Study.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //  config.EnableCors();
            // Web API configuration and services
            config.MessageHandlers.Add(new LocalizationMessageHandler());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

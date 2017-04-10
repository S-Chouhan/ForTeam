using CustomerCareAppDemo.BaseCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CustomerCareAppDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
           

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

           
            //config.EnableSystemDiagnosticsTracing();

            config.Filters.Add(new CustomExceptionFilter());
        }
    }
}

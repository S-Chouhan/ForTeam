using CustomerCareAppDemo.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CustomerCareAppDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.Register(GlobalConfiguration.Configuration);
            
            
            //Define Formatters
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            //For XML
            //var appXmlType = formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            //formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //var appJsonType = formatters.JsonFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/json");
            //formatters.JsonFormatter.SupportedMediaTypes.Remove(appJsonType);

            //Add CORS Handler
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());

           
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using System.Web.Mvc;
using CustomerCareBusinessServices;



namespace CustomerCareAppDemo
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

           // DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // register dependency resolver for WebAPI RC
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            //Component initialization via MEF
            ComponentLoader.LoadContainer(container, ".\\bin", "CustomerCareAppDemo.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "CustomerCareBusinessServices.dll");
            ComponentLoader.LoadContainer(container, ".\\bin", "DataModel.dll");          
        }
    }
}